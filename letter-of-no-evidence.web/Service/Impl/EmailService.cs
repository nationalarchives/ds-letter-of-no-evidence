using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Text;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml;
using letter_of_no_evidence.model;
using System.Xml.Serialization;
using letter_of_no_evidence.web.Models;

namespace letter_of_no_evidence.web.Service
{
    public class EmailService : IEmailService
    {
        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;
        private readonly IConfiguration _configuration;

        public EmailService(IAmazonSimpleEmailService amazonSimpleEmailService, IConfiguration configuration)
        {
            _amazonSimpleEmailService = amazonSimpleEmailService;
            _configuration = configuration;
        }
        public async Task SendCustomerEmailAsync(RequestModel requestModel)
        {
            var payment = requestModel.Payments?.FirstOrDefault();
            var fromAddress = _configuration.GetValue<string>("EmailSettings:EmailFrom");
            var subject = $"{_configuration.GetValue<string>("EmailSettings:EmailSubject")}{requestModel.RequestNumber}";

            var rootElement = new XElement("Root");
            rootElement.Add(new XElement("ContactullName", $"{requestModel.ContactFirstName} {requestModel.ContactLastName}"));
            rootElement.Add(new XElement("RequestNumber", requestModel.RequestNumber));
            rootElement.Add(new XElement("SessionId", payment.SessionId));
            rootElement.Add(new XElement("Amount", payment.Amount));
            rootElement.Add(new XElement("CreatedDate", $"{payment.TransactionDate:dddd dd MMMM yyyy}"));

            var xDocument = new XDocument(rootElement);

            var htmlBody = GetHtmlBody(xDocument);

            var sb = new StringBuilder(File.ReadAllText(@"EmailTemplate/Text/RequestConfirmation.txt"));

            sb = sb.Replace("{ContactullName}", $"{requestModel.ContactFirstName} {requestModel.ContactLastName}");
            sb = sb.Replace("{RequestNumber}", requestModel.RequestNumber);
            sb = sb.Replace("{SessionId}", payment.SessionId);
            sb = sb.Replace("{Amount}", payment.Amount.ToString());
            sb = sb.Replace("{CreatedDate}", $"{payment.TransactionDate:dddd dd MMMM yyyy}");

            var textBody = sb.ToString();

            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = requestModel.ContactEmail.Split(',').ToList()
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlBody
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = textBody
                        }
                    }

                }
            };
            await _amazonSimpleEmailService.SendEmailAsync(sendRequest);
        }

        public async Task SendD365EmailAsync(RequestModel requestModel)
        {
            var fromAddress = _configuration.GetValue<string>("EmailSettings:D365EmailFrom");
            var toAddress = _configuration.GetValue<string>("EmailSettings:D365Emailto");
            var subject = _configuration.GetValue<string>("EmailSettings:D365Subject");
            var payment = requestModel.Payments?.FirstOrDefault();

            var mailObject = new D365EmailModel
            {
                enquiry_id = requestModel.RequestNumber,
                payment_reference = payment.SessionId,
                amount_received = payment.Amount,
                subject_firstname = requestModel.SubjectFirstName,
                subject_lastname = requestModel.SubjectLastName,
                alternative_firstname = requestModel.AlternativeFirstName,
                alternative_lastname = requestModel.AlternativeLastName,
                birth_date = requestModel.DateOfBirth,
                death_date = requestModel.DateOfDeath,
                country_of_birth = requestModel.CountryOfBirth,
                contact_title = requestModel.ContactTitle,
                contact_firstname = requestModel.ContactFirstName,
                contact_lastname = requestModel.ContactLastName,
                contact_email = requestModel.ContactEmail,
                contact_address1 = requestModel.ContactAddress1,
                contact_address2 = requestModel.ContactAddress2,
                contact_town_city = requestModel.ContactCity,
                contact_county = requestModel.ContactCounty,
                contact_postcode = requestModel.ContactPostCode,
                contact_country = requestModel.ContactCountry,
                agent_companyname = requestModel.AgentCompanyName,
                agent_fullname = $"{requestModel.AgentFirstName} {requestModel.AgentLastName}",
                agent_address1 = requestModel.AgentAddress1,
                agent_address2 = requestModel.AgentAddress2,
                agent_town_city = requestModel.AgentCity,
                agent_county = requestModel.AgentCounty,
                agent_postcode = requestModel.AgentPostCode,
                agent_country = requestModel.AgentCountry
            };

            var textBody = "";

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true
            };
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serialiser = new XmlSerializer(typeof(D365EmailModel));
            using (StringWriter textwriter = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(textwriter, settings))
            {
                serialiser.Serialize(xmlWriter, mailObject, emptyNamespaces);
                textBody = textwriter.ToString();
            }

            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = toAddress.Split(',').ToList()
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = textBody
                        }
                    }

                }
            };
            await _amazonSimpleEmailService.SendEmailAsync(sendRequest);
        }

        private string GetHtmlBody(XDocument xDocument)
        {
            var fileName = $"EmailTemplate/RequestConfirmation.xslt";
            var filePath = Path.GetFullPath(fileName);
            XslCompiledTransform xslTransform = new XslCompiledTransform();
            xslTransform.Load(filePath);

            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                xslTransform.Transform(new XmlNodeReader(xmlDocument), null, memoryStream);
                memoryStream.Position = 0;
                string content = null;
                using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                }
                return content;
            }
        }
    }
}
