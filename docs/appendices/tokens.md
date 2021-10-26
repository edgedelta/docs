## Overview

You can use this document to learn how to create a token.

You can create a token to give your users specific read / write access to the Edge Delta API system. 

Additionally, you can use a token to programtically access the Edge Delay API system. 

> **Note** 
>
> Once a token is created, the token will not expire. 
> <p>Also, used tokens are cached. 
> <p>Additionally, Edge Delta does not store tokens. As a result, if you or your users lose a token, the token cannot be retrieved.

***

## Review list of Permissions

When you create a token, you can add the following permissions for read / write access to the Edge Delta API system:




***

## Create a Token

After you create the token, the unique token key will only display once. Afterwards, you will not be able to retrieve the token. As a result, copy and store the key in a secure location. 

1. In the Edge Delta Admin portal, on the left-side navigation, click **Global Settings**. 
2. In the Tokens sections, click  **Create Token**. 
3. Under **Name**, enter a descriptive name for the token, such as **john-smith-integrations-token**. 
4. Click **Add Permissions**.
5. Under **Select An Access**, click the drop-down menu, and then select the desired access type. 
6. Under **Select Resources**, mark **All and Current XXXX**, and then click **Done**.
7. For **Access Type**, mark **Read** or **Write**. 
8. Click **Add To Token**. 
9. (Optional) You can add multiple permissions to a token. Repeat
10. Click **Token Details**. 
11. Review the list of permissions, and then click **Done**.
12. In the window that appears, copy and save the token.
  - Remember, Edge Delta does not store tokens. As a result, a lost or missplaced token cannot be retrieved. 
13. Click **Close**. 
14. The newly created token will be listed in the **Tokens** table. 

***

## Understand the Tokens Table

Review the following table to understand the information listed in the **Tokens** table:

| Column    | Description                                                                |
|-----------|----------------------------------------------------------------------------|
| Name      | This column displays the name of the token.                                |
| Author    | This column displays the email address of the user who created the token.  |
| Created   | This column displays the date and time when the token was created.         |
| Last Used | This column displays the date and time when the token was last used.       |
| Secret    | This column displays the final digits of the token key.                    |

***

## Troubleshoot Token Issues

You cannot retrieve a lost token. As a result, if you lose a token, you must delete the old token, and create a new token with the same permissions. 

***
