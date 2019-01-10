<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes" />

	<xsl:template match="/*">

        <!-- Visually Hidden Preheader Text : BEGIN -->
        <div style="display: none; font-size: 1px; line-height: 1px; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;">
          <xsl:value-of select="Name" /><xsl:text> </xsl:text>Weekly Status Report, ? escalations and ? change requests.
        </div>
        <!-- Visually Hidden Preheader Text : END -->

        <!-- Email Header : BEGIN -->
        <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: auto;" class="email-container">
          <tr>
            <td>
              <br />
            </td>
          </tr>
          <tr>
            <td bgcolor="99c2ff" style="padding: 0px 0; text-align: center">
              <img src="" width="200" height="17" alt="Company" border="0" style="height: auto; background: #dddddd; font-family: sans-serif; font-size: 15px; line-height: 140%; color: #555555;" />
            </td>
          </tr>
        </table>
        <!-- Email Header : END -->

        <!-- Email Body : BEGIN -->
        <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" width="1200" style="margin: auto;" class="email-container">
          <!-- 1 Column Text : BEGIN -->
          <tr>
            <td>
              <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" width="1200" style="margin: auto;" class="email-container">
                <tr>
                  <td bgcolor="#ffffff" style="padding: 0px 0px 0px; text-align: left;">
                    <h1 style="margin: 0; font-family: sans-serif; font-size: 12px; color: #333333; font-weight: normal;">
                      Business Owner: <xsl:text> </xsl:text><xsl:value-of select="BusinessOwner" />
                    </h1>
                  </td>
                  <td bgcolor="#ffffff" style="padding: 0px 0px 0px; text-align: right;">
                    <h1 style="margin: 0; font-family: sans-serif; font-size: 12px; color: #333333; font-weight: normal;">
                      <xsl:value-of select="Date" />
                    </h1>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td bgcolor="#ffffff" style="padding: 0px 10px 10px; text-align: center;">
              <h1 style="margin: 0; font-family: sans-serif; font-size: 22px; line-height: 125%; color: #333333; font-weight: normal;">
                <strong>
                  <xsl:value-of select="Project/Name" />
                </strong>
                <xsl:text> </xsl:text><span style="font-size: 20px">Weekly Status Report</span>
              </h1>
            </td>
          </tr>
          <tr>
            <td bgcolor="#ffffff" style="padding: 0 10px 10px; font-family: sans-serif; font-size: 13px; line-height: 140%; color: #555555; text-align: center;">
              <p style="margin: 0;">
                <xsl:value-of select="Project/Description" />
              </p>
            </td>
          </tr>
          <tr>
            <td>
              <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" width="1200" style="margin: auto;" class="email-container">
                <tr>
                  <td bgcolor="#ffffff" style="padding: 0px 0px 0px; text-align: left;">
                    <h1 style="margin: 0; font-family: sans-serif; font-size: 12px; color: #333333; font-weight: normal;">
                      Project Manager: <xsl:text> </xsl:text><xsl:value-of select="ProjectManager" />
                    </h1>
                  </td>
                  <td bgcolor="#ffffff" style="padding: 0px 0px 0px; text-align: right;">
                    <h1 style="margin: 0; font-family: sans-serif; font-size: 12px; color: #333333; font-weight: normal;">
                      Target Go-Live: <xsl:text> </xsl:text><xsl:value-of select="TargetGoLive" />
                    </h1>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <!-- 1 Column Text : END -->
          <!-- 1 Column Text : BEGIN -->
          <tr>
            <td bgcolor="#ccffee" style="padding: 5px 0px 5px; font-family: sans-serif; font-size: 15px; line-height: 140%; color: #333333;">
              <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: auto">
                <tr>
                  <td>
                    <xsl:if test="Escalation = 'true'">
                      <xsl:if test="ChangeRequest = 'true'">
                        There is an escalation and a change request.
                      </xsl:if>
                      <xsl:if test="ChangeRequest = 'false'">
                        There is an escalation but no change request.
                      </xsl:if>
                    </xsl:if>
                    <xsl:if test="Escalation = 'false'">
                      <xsl:if test="ChangeRequest = 'true'">
                        There is no escalation but there is a change request
                      </xsl:if>
                      <xsl:if test="ChangeRequest = 'false'">
                        There is no escalation and no change request
                      </xsl:if>
                    </xsl:if>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <!-- 1 Column Text : END -->
          <!-- 2 Even Columns : BEGIN -->
          <tr>
            <td bgcolor="#ffffff" align="center" valign="top" style="padding: 10px 0px 0px;">
              <table role="presentation" cellspacing="0" cellpadding="0" border="box" width="100%">
                <tr>
                  <!-- Column : BEGIN -->
                  <td class="stack-column-center" valign="top" width="60%">
                    <table border="1" role="presentation" cellspacing="0" cellpadding="0">
                      <tr>
                        <td bgcolor="#262626" style="padding: 5px 0px 5px; text-align: center;">
                          <h1 style="margin: 0; font-family: sans-serif; font-size: 18px; color: #ffffff; font-weight: normal;">
                            Summary of Progress
                          </h1>
                        </td>
                      </tr>
                      <tr>
                        <td bgcolor="#ffffff" style="padding: 0 5px 0px; font-family: sans-serif; font-size: 15px; line-height: 140%; color: #000000; text-align: left;">
                          <p style="margin: 0;">
                            <xsl:value-of select="Progress" disable-output-escaping="yes" />
                          </p>
                        </td>
                      </tr>
                    </table>
                  </td>
                  <!-- Column : END -->
                  <!-- Column : BEGIN -->
                  <td class="stack-column-center" valign="top">
                    <table border="1" role="presentation" cellspacing="0" cellpadding="0">
                      <tr>
                        <td bgcolor="#262626" style="padding: 0px 0px 0px; text-align: center;">
                          <h1 style="margin: 0; font-family: sans-serif; font-size: 18px; color: #ffffff; font-weight: normal;">
                            Upcoming Milestones
                          </h1>
                        </td>
                      </tr>
                      <tr>
                        <td bgcolor="#ffffff" style="padding: 0 0px 0px; font-family: sans-serif; font-size: 15px; line-height: 140%; color: #000000; text-align: left;">
                          <table>
                            <tr style="background-color: #595959; color: #ffffff">
                              <th>Milestone</th>
                              <th>Target Date</th>
                              <th>Actual Date</th>
                            </tr>
                            <xsl:for-each select="Milestones/MilestonesItem">
                              <tr>
                                <td>
                                  <a target="_blank">
                                    <xsl:attribute name="href">
                                      <xsl:value-of select="current()/Url" disable-output-escaping="yes" />
                                    </xsl:attribute>
                                    <xsl:value-of select="current()/Title" disable-output-escaping="yes" />
                                  </a>
                                </td>
                                <td><xsl:value-of select="current()/TargetDate" /></td>
                                <td><xsl:value-of select="current()/ActualDate" /></td>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                  <!-- Column : END -->
                </tr>
              </table>
            </td>
          </tr>
          <!-- 2 Even Columns : END -->
          <!-- 1 Column Text : BEGIN -->
          <tr>
            <td bgcolor="#262626" style="padding: 5px 0px 5px; text-align: center;">
              <h1 style="margin: 0; font-family: sans-serif; font-size: 18px; color: #ffffff; font-weight: normal;">
                Risks and Issues
              </h1>
            </td>
          </tr>
          <tr>
            <td bgcolor="#ffffff" style="font-size: 12px;">
              <table border="1" role="presentation" cellspacing="0" cellpadding="0">
                <tr style="background-color: #595959; color: #ffffff;">
                  <th>ID</th>
                  <th>Title</th>
                  <th>Description and Impact</th>
                  <th align="center">Severity</th>
                  <th align="center">Priority</th>
                  <th>Mitigation</th>
                  <th>Action Owner</th>
                </tr>
                <xsl:for-each select="RisksAndIssues/RisksAndIssuesItem">
                  <tr>
                    <td>
                      <a target="_blank">
                        <xsl:attribute name="href">
                          <xsl:value-of select="current()/Url" disable-output-escaping="yes" />
                        </xsl:attribute>
                        <xsl:value-of select="current()/Id" disable-output-escaping="yes" />
                      </a>
                    </td>
                    <td>
                      <a target="_blank">
                        <xsl:attribute name="href">
                          <xsl:value-of select="current()/Url" disable-output-escaping="yes" />
                        </xsl:attribute>
                        <xsl:value-of select="current()/Title" disable-output-escaping="yes" />
                      </a>
                    </td>
                    <td><xsl:value-of select="current()/Description" disable-output-escaping="yes" /></td>
                    <td align="center"><xsl:value-of select="current()/Severity" /></td>
                    <td align="center"><xsl:value-of select="current()/Priority" /></td>
                    <td><xsl:value-of select="current()/Mitigation" disable-output-escaping="yes" /></td>
                    <td><xsl:value-of select="current()/Owner" disable-output-escaping="yes" /></td>
                  </tr>
                </xsl:for-each>
              </table>
            </td>
          </tr>
          <!-- 1 Column Text : END -->
        </table>
        <!-- Email Body : END -->

  </xsl:template>
</xsl:stylesheet>