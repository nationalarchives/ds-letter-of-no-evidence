<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:ms="urn:schemas-microsoft-com:xslt" 
                xmlns:dt="urn:schemas-microsoft-com:datatypes">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="Root">
    <body style="font-family: Open Sans;font-size: 16px;line-height: 24px;">
      <p>
        Dear <xsl:value-of select="ContactFirstName" /> <xsl:value-of select="ContactLastName" />,
      </p>

      <p>
        Your payment was successfully received.
      </p>
      
      <h3 style="margin-top: 2em;">Your payment summary </h3>

      <p>
        Enquiry reference number: <xsl:value-of select="RequestNumber" /><br/>
        Transaction reference: <xsl:value-of select="SessionId" /><br/>
        Amount received: <xsl:value-of select="Amount" /> GBP (no VAT added)<br/>
        Date received: <xsl:value-of select="CreatedDate" />
      </p>

      <p>
        This email address is not monitored. If you have any questions about your request for confirmation of no evidence of British naturalisation, contact ceerequests@nationalarchives.gov.uk.
      </p>
    </body>
  </xsl:template>
</xsl:stylesheet>
