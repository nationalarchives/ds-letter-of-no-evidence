<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:ms="urn:schemas-microsoft-com:xslt" 
                xmlns:dt="urn:schemas-microsoft-com:datatypes">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="Root">
    <body style="font-family: Open Sans;font-size: 16px;line-height: 24px;">
      <p>
        Dear <xsl:value-of select="ContactullName" />,
      </p>

      <p>
        Your payment was successfully received.
      </p>
      
      <h3 style="margin-top: 2em;">Your payment summary </h3>

      <p>
        Enquiry reference number: <xsl:value-of select="RequestNumber" /><br/>
        Payment reference: <xsl:value-of select="SessionId" /><br/>
        Service cost: <xsl:value-of select="ServiceCost" /> GBP (no VAT added)<br/>
        Postage cost: <xsl:value-of select="PostalCost" /> GBP<br/>
        Total amount received: <xsl:value-of select="TotalCost" /> GBP<br/>
        Date received: <xsl:value-of select="CreatedDate" />
      </p>

      <p>You will receive a confirmation email with the outcome of your request within 16 working days.</p>

      <p>
        This email address is not monitored. If you have any questions about your request for confirmation of no evidence of British naturalisation, contact PaidSearchTeam@nationalarchives.gov.uk.      
      </p>
    </body>
  </xsl:template>
</xsl:stylesheet>
