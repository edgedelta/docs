---
description: >-
  This document describes how to set up an Okta SAML integration with Edge Delta.
---

# Create an Okta SAML Integration 

## Overview

You can use this document to learn how to set up an Okta SAML integration with Edge Delta. 

> **Note**
>
> To use this document, you must have administrative access to your Okta account. 

***

## Create an Okta SAML integration

1. In the Okta dashboard, in the top menu, click **Applications**. 
2. Click **Create New App**.
3. In the window that appears, for **Platform**, select **Web**. For **Sign on method**, mark **SAML 2.0**. Click **Create**. 
4. Enter a descriptive name for the app, such as **Edge Delta SAML**.
5. Click **Next**. 
6. Complete the missing fields with the following information: 
    * For **Single sign on URL**, enter **https://api.edgedelta.com/saml/acs**. 
    * Mark **Use this for Recipient URL and Destination URL**. 
    * For **Audience URI (SP Entity ID)**, enter **https://api.edgedelta.com/saml/metadata**. 
    * For **Default RelayState**, enter **https://admin.edgedelta.com/saml**. 
    * For **Name ID format**, select **EmailAddress**.
    * For **Application username**, select **Email**. 
    * For **Update application username on**, select **Create and update**.
    * For **Encryption Certificate**, navigate to **https://api.edgedelta.com/saml/metadata**, and then in the downloaded file, copy and paste the text within the **X509Certificate** and **/X509Certificate** brackets. 
7. Click **Next**. 
8. Select **I'm an Okta customer adding an internal app**.
9. Mark **This is an internal app that we have created**.
10. Click **Finish**.
11. From the **Settings** section of the **Sign On** menu for the new application, locate and copy the metadata URL for the Identity Provider metadata.
12. Send the metadata URL to Edge Delta. 
    * You can contact [support@edgedelta.com](mailto:support@edgedelta.com) to share this information. 
    * Edge Delta will use this URL to make backend configurations. 
13. In Okta, register your users to use SAML authentication. 
    * To learn more, review this [article from Okta](https://help.okta.com/en/prod/Content/Topics/users-groups-profiles/usgp-add-users.htm). 
14. Visit https://admin.edgedelta.com/saml , and then enter your Okta credentials. You will be redirected to Okta. 
15. Enter your Okta credentials, and then you will be redirected to the Edge Delta App. 

> **Note**
> 
> When you use the same browser to access the Edge Delta App, by default SAML authentication will take place. 

***
