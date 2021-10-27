## Overview

You can use this document to learn how to create and manage tokens.

With tokens, you can specify read / write access for specific backend functionality for your users. In other words, you can create a token to give your users specific read / write access to the Edge Delta API system. 

> **Notes** 
>
> - Once a token is created, the token will not expire. 
> - Used tokens are cached. 
> - Edge Delta does not store tokens. As a result, if you or your users lose a token, the token cannot be retrieved.

***

## Review Available Permissions

When you create a token, you can add the following permissions for read / write access to the Edge Delta API system.

> **Note**
>
> Currently, there are no permissions to give users complete access to the API system. As a result, to give a user admin-level access, you must add each available permission to the token.    

| Permissions          | Description                                                                                                         |
|----------------------|---------------------------------------------------------------------------------------------------------------------|
| Accesses             | This permission gives you read / write access to manage the user permissions in your account.                       |
| Rehydrations         | This permission gives you read / write access to manage rehydrations.                                               |
| Integrations         | This permission gives you read / write access to manage integrations with third-party services.                     |
| Monitors             | This permission gives you read / write access to manage monitors (alert definitions).                               |
| Agent Configurations | This permission gives you read / write access to manage agent configurations                                        |
| Hosted Agents        | This permission gives you read / write access to manage the agents hosted by Edge Delta.                            |
| Tokens               | This permission gives you read / write access to manage tokens.                                                     |


***

## Create a Token

After you create the token, the unique token key will only display once. Afterwards, you will not be able to retrieve the token. As a result, copy and store the key in a secure location.   

> **Note**
> 
> You can only create a token based on the permisssions that you already have. For example, if you have read-only access for **Integrations**, then you can create a token with read-only access for **Integrations**; however, you cannot create a token with write-only access for **Integrations** because you do not currently have that permission. 

1. In the Edge Delta Admin portal, on the left-side navigation, click **Global Settings**. 
2. In the Tokens sections, click  **Create Token**. 
3. Under **Name**, enter a descriptive name for the token, such as **john-smith-integrations-token**. 
4. Click **Add Permissions**.
5. Under **Select An Access**, click the drop-down menu, and then select the desired access type. 
6. Under **Select Resources**, mark **All and Current XXXX**, and then click **Done**.
7. For **Access Type**, mark **Read** or **Write**. 
  - **Read** gives users the ability to view resource information, such as obtain a list of existing integrations. 
  - **Write** gives users the ability to create, edit, and remove resources, such as create a new intergration or delete an existing monitor. 
8. Click **Add To Token**. 
9. (Optional) You can add multiple permissions to a token. Repeat steps 5 - 8. 
10. Click **Token Details**. 
11. Review the list of permissions, and then click **Done**.
12. In the window that appears, copy and save the token.
  - Remember, Edge Delta does not store tokens. As a result, a lost or missplaced token cannot be retrieved. 
13. Click **Close**. 
14. The newly created token will be listed in the **Tokens** table. 
15. You can use the newly created token to make API calls to the Edge Delta backend system. Tokens should be sent in the **X-ED-API-Token** request header.

***

## Understand the Tokens Table

Review the following table to understand the information listed in the **Tokens** table:

| Column    | Description                                                                |
|-----------|----------------------------------------------------------------------------|
| Name      | This column displays the descriptive name of the token.                    |
| Author    | This column displays the email address of the user who created the token.  |
| Created   | This column displays the date and time when the token was created.         |
| Last Used | This column displays the date and time when the token was last used.       |
| Secret    | This column displays the final digits of the token key.                    |

***

## Delete a Token 
  
1. In the Edge Delta Admin portal, on the left-side navigation, click **Global Settings**.   
2. Navigate to **Tokens**.
3. Under **Actions**, click the trash icon for the corresponding token. 
4. Click **Yes** to confirm your action. 
  - The table will refresh and remove the deleted token. 

*** 
  
  
## Troubleshoot Token Issues
  
You cannot retrieve a lost token. As a result, if you lose or misplace a token, then you must delete the old token, and then create a new token with the same permissions. 

***
