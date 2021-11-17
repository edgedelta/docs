---
description: >-
  This document focuses on archiving outputs.  
---

# Outputs - Archives 

## Overview

You can use this document to learn about the configuration parameters available in a configuration file, specifically for **Outputs - Archives**.

An **Output - Archive** focuses on storage solutions. Specifically, this output type tells the Edge Delta agent where the agent can periodically send compressed raw data logs. 

> **Note**
> 
> Edge Delta offers additional output types, specifically **Stream** and **Triggers**. 
>   * To learn more, see [Outputs-Streams](outputs-streams.md) and [Outputs-Triggers](outputs-triggers.md).

***

## Step 1: Access Outputs

At a high level, there are 2 ways to manage **Outputs**:

  * If you need to create a new configuration, then you can use the visual editor to populate a YAML file, as well as make changes directly in the YAML file.
  * If you already have an existing configuration, then you can update the configuration in the YAML file.  

***

### Option 1: Access the visual editor for a new configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Click **Visual**.
4. On the right-side, select **Archive**.
5. Select the desired destination, and then complete the missing fields. 

    * To learn more about the parameters for each destination, see [Step 2: Review Parameters for Archive Destinations](##step-2-review-parameters-for-archive-destinations).

6. To make additional configurations to the configuration file, click the back button, and then select a new configuration parameter to manage. 
7. To save the configuraiton and exit the visual editor, click **Save**. 
8. Refresh the screen to view the newly created configuration in the table. 

***

### Option 2: Access the YAML file for an existing configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
3. Review the YAML file, make your changes, and then click **Save**. 

  * To learn more about the parameters for each destination, see [Step 2: Review Parameters for Archive Destinations](##step-2-review-parameters-for-archive-destinations).

***

## Step 2: Review Parameters for Archive Destinations

Edge Delta supports the following archive destinations: 

***

### AWS S3

The **AWS S3** output will stream logs to an AWS S3 endpoint.

Before you configure your Edge Delta account to sends logs to an AWS S3 endpoint, you must first access the AWS console to:

  * Create an IAM user to access the AWS S3 bucket
    * To learn how to create an IAM user, review this [document from AWS](https://docs.aws.amazon.com/IAM/latest/UserGuide/id_users_create.html).
  * Attach the following custom policy to the newly created IAM user
    * To learn how to create and add a custom policy, review this [document from AWS](https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies_create.html). 

```
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "VisualEditor0",
            "Effect": "Allow",
            "Action": [
                "s3:PutObject",
                "s3:GetObject",
                "s3:ListBucket"
            ],
            "Resource": [
                "arn:aws:s3:::bucket-name",
                "arn:aws:s3:::bucket-name/*"
            ]
        }
    ]
}
```

After you attach the policy in the AWS console, review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **s3**. | Required |
| bucket | Enter the target S3 bucket to send the archived logs. | Required |
| region | Enter the specified S3 bucket's region. | Required |
| aws\_key\_id | Enter the AWS key ID that has the PutObject permission to target the bucket. If you use role-based AWS authentication where keys are not provided, then you should keep this field empty; however, you must still attach the custom policy listed above. | Optional |
| aws\_sec\_key | Enter the AWS secret key ID that has the PutObject permission to target the bucket. If you use role-based AWS authentication where keys are not provided, then you should keep this field empty; however, you must still attach the custom policy listed above.| Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-s3
        type: s3
        bucket: testbucket
        region: us-east-2
        aws_key_id: "<add aws key id>"
        aws_sec_key: "<add aws secure key>"
```

***

### Azure Blob Storage

The **Azure Blob Storage** output will stream logs to an Azure Blob Storage endpoint.

> **Before you begin**
> 
> Before you can create an output, you must have an account key. 
>   * To learn more, review this [document from Microsoft](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage?tabs=azure-portal). 

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow.  | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows.  | Optional |
| type | You must set this parameter to **blob**. | Required |
| account\_name | Enter the account name for the Azure account. | Required |
| account\_key | Enter the account key for Azure account. | Required |
| container | Enter the container name to upload. | Required |
| auto_create_container | Create the container on the service, with no metadata and no public access. | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-blob
        type: blob
        account_name: "<add account name>"
        account_key: "<add account key>"
        container: testcontainer
```

***

### Google Cloud Storage

The **Google Cloud Storage** output will stream logs to a GCS endpoint.

> **Before you begin**
> 
> Before you can create an output, you must have a GCS HMAC Access key. 
>   * To learn how to create a new key, review this [document from Google](https://cloud.google.com/storage/docs/authentication/managing-hmackeys). 

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **gcs**. | Required |
| bucket | Enter the target GCS bucket to send the archived logs. | Required |
| hmac\_access\_key | Enter the GCS HMAC Access key that has permissions to upload files to specified bucket. | Required |
| hmac\_secret | GCS HMAC secret associated with the access key specified. | Required |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-gcs
        type: gcs
        bucket: ed-test-bucket
        hmac_access_key: my_hmac_access_key_123
        hmac_secret: my_hmac_secret_123
```

***

### DigitalOcean Spaces

The **DigitalOcean Spaces** output will stream logs to a DigitalOcean Spaces endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **dos**.| Required |
| endpoint | Enter the DigitalOcean Spaces endpoint. | Required |
| bucket | Enter the target DOS bucket to send the archived logs. | Required |
| access\_key | Enter the access key that has permissions to upload files to the specified bucket. | Required |
| secret\_key | Enter the secret key associated with the specified access key. | Required |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-digitalocean-spaces
        type: dos
        endpoint: nyc3.digitaloceanspaces.com
        bucket: ed-test-bucket
        access_key: my_access_key_123
        secret_key: my_secret_key_123
```

***

### IBM Object Storage

The **IBM Object Storage** output will stream logs to an IBM Object Storage endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows.  | Optional |
| type | You must set this parameter to **ibmos**. | Required |
| endpoint | Enter the IBM Object Storage endpoint | Required |
| bucket | Enter the desired IBM Object Storage bucket to send the archived logs. | Required |
| access\_key | Enter the access key that has permission to upload files to the specified bucket. | Required |
| secret\_key | Enter the secret key associated with the specified access key. | Required |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-ibm-object-storage
        type: ibmos
        endpoint: s3-api.us-geo.objectstorage.softlayer.net
        bucket: ed-test-bucket
        access_key: my_access_key_123
        secret_key: my_secret_key_123
```

***

### **Minio**

The **Minio** output will stream logs to a Minio endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **minio**. | Required |
| endpoint | Enter the Minio endpoint. | Required |
| bucket | Enter the Minio bucket to send the archived logs. | Required |
| access\_key | Enter the access key that has permissions to upload files to the specified bucket. | Yes |
| secret\_key | Enter the secret key associated with the specified access key. | Required |
| disable\_ssl | You can disable the SSL requirement when logs are pushed to the Minio endpoint. | Optional |
| s3\_force\_path\_style | You can force the archive destination to use the `{endpoint}/{bucket}` format instead of the `{bucket}.{endpoint}/` format when reaching buckets. | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-minio
        type: minio
        endpoint: play.min.io:9000
        bucket: ed-test-bucket
        access_key: my_access_key_123
        secret_key: my_secret_key_123
        disable_ssl: true
        s3_force_path_style: true
```

***

### Zenko CloudServer

The **Zenko CloudServer** output will stream logs to a CloudServer endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows.  | Optional |
| type | You must set this parameter to **zenko**. | Required |
| endpoint | Enter the Zenko endpoint. | Required |
| bucket | Enter the desired Zenko bucket to send the archived logs. | Required |
| access\_key | Enter the access key that has permissions to upload files to the specified bucket. | Required |
| secret\_key | Enter the secret key associated with the specified access key. | Required |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: my-zenko-cloudserver
        type: zenko
        endpoint: https://XXXXXXXXXX.sandbox.zenko.io
        bucket: ed-test-bucket
        access_key: my_access_key_123
        secret_key: my_secret_key_123
```

***

### Moogsoft

The **Moogsoft** output will stream notifications and alerts to a specified Moogsoft URL.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **moogsoft**. | Required |
| endpoint | Enter the Moogsoft API endpoint. | Required |
| api_key | Enter the Moogsoft API key. You must enter an API key or a username/password. | Optional |
| username | Enter the username for Moogsoft basic authentication. You must enter an API key or a username/password. | Optional |
| password | Enter the password for Moogsoft basic authentication. You must enter an API key or a username/password. | Optional |
| notify\_content | You can use this parameter to customize the notification content. This parameter supports templating. Moogsoft only supports the **custom_fields** subfield. | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: moogsoft-default
        type: moogsoft
        endpoint: "https://example.moogsoftaiops.com/events/webhook_webhook1"
        api_key: "moogsoft-apikey"
        notify_content:
          custom_fields:
            "jira-ticket": "ticket"
```

***

### Remedy

The **Remedy** output will stream notifications and alerts to a specified Remedy URL.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **remedy**. | Required |
| endpoint | Enter the Remedy API endpoint. | Required |
| token | Enter the Remedy token. You must enter a token or a username/password. | Optional |
| username | Enter the username for Remedy basic authentication. You must enter a token or a username/password. | Optional |
| password | Enter the password for Remedy basic authentication. You must enter a token or a username/password. | Optional |
| custom\_headers | This parameter is used to append custom headers, such as Authorization, to requests from the integration. | Optional |
| notify\_content | You can use this parameter to customize the notification content. This parameter supports templating. Remedy only supports the **custom_fields** subfield. | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: remedy-default
        type: remedy
        endpoint: "localhost"
        token: remedy-token
        notify_content:
          custom_fields:
            "test-field": "test"
        custom_headers: 
          X-header1: "test-header"
```

***

### **Azure Event Hub Trigger**

The **Azure Event Hub Trigger** output will stream notifications and alerts to a specified Remedy URL.


> **Before you begin**
> 
> To enable this integration, you must have an Azure AD token. 
>   * To learn how to create an Azure AD token, review this [document from Microsoft](https://docs.microsoft.com/en-us/rest/api/eventhub/get-azure-active-directory-token).

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **eventhub**. | Required |
| endpoint | Enter the Event Hub endpoint. | Required |
| token | Enter the Azure AD token. | Required |
| custom\_headers | This parameter is used to append custom headers, such as Authorization, to requests from the integration. | Optional |
| notify\_content | You can use this parameter to customize the notification content. This parameter supports templating. Event Hub only supports the **custom_fields** subfield. | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
       - name: eventhub-test
        type: eventhub
        endpoint: https://eventshub-test.servicebus.windows.net/test/messages
        token: "test-token"
        notify_content:
          custom_fields:
            "test-field": "test"
        custom_headers: 
          X-header1: "test-header"
```

***
