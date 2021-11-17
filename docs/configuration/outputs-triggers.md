---
description: >-
  This document focuses on triggering outputs.  
---

# Outputs - Triggers 

## Overview

You can use this document to learn about the configuration parameters available in a configuration file, specifically for **Outputs - Triggers**.

An **Output - Trigger** focuses on alerting and automation systems. Specifically, this output type tells the Edge Delta agent to send alerts and notifications when an anomaly is detected or when various conditions are met.

> **Note**
> 
> Edge Delta offers additional output types, specifically **Streams** and **Archives**. 
>   * To learn more, see [Outputs-Streams](outputs-streams.md) and [Outputs-Archives](outputs-archives.md).

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
4. On the right-side, select **Triggers**.
5. Select the desired destination, and then complete the missing fields. 

   * To learn more about the parameters for each destination, see [Step 2: Review Parameters for Trigger Destinations](#step-2-review-parameters-for-trigger-destinations).

6. To make additional configurations to the configuration file, click the back button, and then select a new configuration parameter to manage. 
7. To save the configuraiton and exit the visual editor, click **Save**. 
8. Refresh the screen to view the newly created configuration in the table. 

***

### Option 2: Access the YAML file for an existing configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
3. Review the YAML file, make your changes, and then click **Save**. 

    * To learn more about the parameters for each destination, see [Step 2: Review Parameters for Trigger Destinations](#step-2-review-parameters-for-trigger-destinations).
    * To add an existing integration, access the YAML file, and then enter ``integration_name: <name of existing integration>``.    

***

## Step 2: Review Parameters for Trigger Destinations

Edge Delta supports the following trigger destinations: 

### Slack

The **Slack** output will stream notifications and alerts to a specified Slack channel.

> **Before you begin**
> 
> To use this output, you must provide a Slack webhook or endpoint URL. 
>   * To learn more about webhooks, review [this document from Slack](https://api.slack.com/messaging/webhooks).

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **slack**.| Required |
| endpoint | Enter the Slack Webhook or APP endpoint URL. | Required |
| suppression\_window | Enter a [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents the suppression window. Once the agent detects an issue and notifies the Slack endpoint, the agent will suppress any new issues for this time period. The default setting is **20m**. | Optional |
| suppression\_mode | Enter a supression mode, which can be **local** or **global**. The default mode is **local**, which indicates that an individual agent suppresses an issue if the agent has already made a local notification for a similar issue in the last suppresson window. **Global** mode indicates that an individual agent checks with the Edge Delta backend to see if there were similar alerts from other sibling agents \(agents that share the same tag in the configuration\). | Optional |
| notify\_content | You can use this parameter to customize the notification content. This parameter supports templating. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: slack-integration
        type: slack
        endpoint: "https://hooks.slack.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX"
        notify_content:
          title: "Anomaly Detected: {{.ProcessorDescription}}"
          disable_default_fields: false
          custom_fields:
            "Dashboard": "https://admin.edgedelta.com/investigation?edac={{.EDAC}}&timestamp={{.Timestamp}}"
            "Current Value": "{{.CurrentValue}}"
            "Threshold Value": "{{.ThresholdValue}}"
            "Custom Message": "{{.CurrentValue}} exceeds {{.ThresholdValue}}"
            "Matched Term": "{{.MatchedTerm}}"
```

***

### Microsoft Teams

The **Microsoft Teams** output will stream notifications and alerts to a specified Teams channel.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | Select **teams**. | Required |
| endpoint | Enter the Microsoft Teams webhook URL. | Required |
| suppression\_window | Enter a [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents the suppression window. Once the agent detects an issue and notifies the Slack endpoint, the agent will suppress any new issues for this time period. The default setting is **20m**.| Optional |
| suppression\_mode | Enter a supression mode, which can be **local** or **global**. The default mode is **local**, which indicates that an individual agent suppresses an issue if the agent has already made a local notification for a similar issue in the last suppresson window. **Global** mode indicates that an individual agent checks with the Edge Delta backend to see if there were similar alerts from other sibling agents \(agents that share the same tag in the configuration\).  | Optional |
| notify\_content | You can use this parameter to customize the notification content. This parameter supports templating. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters).| Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: microsoft-teams-integration
        type: teams
        endpoint: "https://outlook.office.com/webhookb2/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX@XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/IncomingWebhook/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
        notify_content:
          title: "Anomaly Detected: {{.ProcessorDescription}}"
          disable_default_fields: false
          custom_fields:
            "Dashboard": "https://admin.edgedelta.com/investigation?edac={{.EDAC}}&timestamp={{.Timestamp}}"
            "Current Value": "{{.CurrentValue}}"
            "Threshold Value": "{{.ThresholdValue}}"
            "Custom Message": "{{.CurrentValue}} exceeds {{.ThresholdValue}}"
            "Matched Term": "{{.MatchedTerm}}"
```

***

### Pagerduty

The **Pagerduty** output will stream notifications and alerts to a specified Pagerduty API endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **pagerduty**. | Required |
| endpoint | Enter the Pagerduty API endpoint URL. | Required |
| custom\_headers | This parameter is used to append custom headers, such as Authorization, to requests from the integration. | Optional |
| notify\_content | This parameter is used to customize the notification content. This parameter supports templating. While this parameter is optional, Edge Delta recommends that you use the **advanced_content** subfield. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: pagerduty-integration
        type: pagerduty
        endpoint: "https://api.pagerduty.com/XXXXX"
        notify_content:
          advanced_content: |
            {
              "incident": {
                "type": "incident",
                "title": "{{ .Title }}",
                "service": {
                  "id": "<ID of the pagerduty service which can be fetched via services rest API>",
                  "type": "service_reference"
                },
                "body": {
                  "type": "incident_body",
                  "details": "<Message for your incident>"
                }
              }
            }
```

***

### Jira

The **Jira** output will stream notifications and alerts to a specified Jira webhook URL.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **jira**. | Required |
| endpoint | Enter the Jira webhook URL. | Required |
| notify\_content | This parameter is used to customize the notification content. This parameter supports templating. While this parameter is optional, Edge Delta recommends that you use the **advanced_content** subfield. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: jira-integration
        type: jira
        endpoint: "https://automation.codebarrel.io/pro/hooks/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
        notify_content:
          advanced_content: |
            {
              "data": {
                "title": "{{ .Title }}",
                "message": "{{ .Message }}"
              }
            }
```

***

### Service Now

The **Service Now** output will stream notifications and alerts to a specified Service Now API endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **servicenow**. | Required |
| endpoint | Enter the Service Now URL. | Required |
| username | Enter the username for Service Now basic authentication. | Optional |
| password | Enter the password for Service Now basic authentication. | Optional |
| custom\_headers | This parameter is used to append custom headers, such as Authorization, to requests from the integration. | Optional |
| notify\_content | This parameter is used to customize the notification content. This parameter supports templating. While this parameter is optional, Edge Delta recommends that you use the **advanced_content** subfield. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: service-now-integration
        type: servicenow
        endpoint: "https://instance.service-now.com/api/now/table/incident"
        notify_content:
          advanced_content: |
            {
              'short_description': 'Raw POST Anomaly Detected: {{.ProcessorDescription}}',
              'assignment_group':'287ebd7da9fe198100f92cc8d1d2154e',
              'urgency':'2',
              'impact':'2'
            }
```

***

### Webhook

The **Webhook** output will stream notifications and alerts to the specified Webhook URL.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **webhook**. | Required |
| endpoint | Enter the Webhook API endpoint. | Required |
| username | Enter the username for Webhook basic authentication. | Optional |
| password | Enter the password for Webhook basic authentication. | Optional |
| notify\_content | This parameter is used to customize the notification content. This parameter supports templating. While this parameter is optional, Edge Delta recommends that you use the **advanced_content** subfield. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: webhook-integration
        type: webhook
        endpoint: "localhost"
        payload:
          signature: "{{.MetricName}}"
          source_id: "{{.Host}}"
          external_id: "{{.EDAC}}"
          manager: "edgedelta"
          source: "{{.Host}}"
          class: "application"
          agent_location: "{{.Host}}"
          type: "{{.SourceType}}"
          agent_time: "{{.Epoch}}"
        notify_content:
          advanced_content: |
            {
              "foo": {
                "title": "{{ .Title }}",
                "message": "{{ .Message }}",
                "foo2": "bar2"
              }
            }
```

***

### AWS Lambda

The **AWS Lambda** output will stream notifications and alerts to  the specified AWS Lambda FaaS endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **awslambda**. | Required |
| endpoint | Enter the AWS Lambda FaaS endpoint. | Required |
| notify\_content | This parameter is used to customize the notification content. This parameter supports templating. While this parameter is optional, Edge Delta recommends that you use the **advanced_content** subfield. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: aws-lambda-integration
        type: awslambda
        endpoint: "https://XXXXXXXXXX.execute-api.XXXXXXXXX.amazonaws.com/XXXX/XXXXXX"
        notify_content:
          advanced_content: |
            {
              "foo": "bar",
              "title": "{{ .Title }}",
              "message": "{{ .Message }}"
            }
```

***

### Azure Functions

The **Azure Functions** output will stream notifications and alerts to Azure endpoint.

Review the following parameters that you can configure in the Edge Delta Admin portal:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for the output, which will be used to map this destination to a workflow. | Optional |
| integration\_name | This parameter refers to the organization-level integration created in the **Integrations** page. If you enter this name, then the rest of the fields will be automatically populated. If you need to add multiple instances of the same integration into the config, then you can add a custom name to each instance via the **name** field. In this situation, the name should be used to refer to the specific instance of the destination in the workflows. | Optional |
| type | You must set this parameter to **azurefunctions**. | Required |
| endpoint | Enter the Azure Functions FaaS endpoint. | Required |
| notify\_content | This parameter is used to customize the notification content. This parameter supports templating. While this parameter is optional, Edge Delta recommends that you use the **advanced_content** subfield. To learn more, see [(Optional) Step 3: Review Notify Content Parameters](#optional-step-3-review-notify-content-parameters). | Optional |

The following example displays an output without the name of the organization-level integration:

```yaml
      - name: azure-functions-integration
        type: azurefunctions
        endpoint: "https://XXXXXXXXXX.azurewebsites.net/XXXX/XXXXXX"
        notify_content:
          advanced_content: |
            {
              "foo": "bar",
              "title": "{{ .Title }}",
              "message": "{{ .Message }}"
            }
```

***

### (Optional) Step 3: Review Notify Content Parameters 

The **Notify Content** parameter is an optional way to customize the notification content for specific Outputs - Triggers. 

Review the following fields:  

* **Title**: This field is the title text for the webhook message. This field can be customized with custom template fields.
* **Disable default fields**: This field is used to disable default fields in notification messages. By default, the value is false. 
* **Custom Fields**: You can extend the notification content by adding name-value pairs, which you can build with the template fields listed below.
* **Advanced Content**: This field provides full flexibility to define the payload in Slack notification post requests.
  
    * **Advanced Content** overrides other settings, such as title, custom fields, etc. 
    * Custom templates are also supported in **Advanced Content**.
    * You can use block kit builder tool provided by Slack to test your notification content. To learn more, see [Block Kit Builder](https://app.slack.com/block-kit-builder). 


> **Note**
> 
> While this parameter supports templating, consider the following statements: 
> 
>   * If the value is empty, then the item will not be sent to Slack.
>   * Keys are sorted alphabetically before being sent to Slack. As a result, keys will not appear in the order specified in the configuration. 


Review the following template fields: 

* **Tag**: This field is a tag to describe the environment, such as **prod\_us\_west\_2\_cluster**.
* **EDAC**: This field is the Edge Delta Anomaly Context ID.
* **Host**: This field is the hostname of the environment where the agent runs on.
* **ConfigID**: This field is the configuration ID for the agent. 
* **MetricName**: This field is the metric name that causes the anomaly.
* **Source**: This field is the identifier name of the source, such as docker container id or file name.
* **SourceType**: This field is the source type, such as **Docker** or **system**.  
* **SourceAttributes**: This field is list of source attributes.
* **Timestamp**: This field is the timestamp for when an event is triggered.
* **Epoch**: This field is the timestamp (in epoch format) for when an event triggered. 
* **CurrentValue**: This field is the metric current value.
* **ThresholdValue**: This field is the threshold value.
* **ThresholdDescription**: This field is a detailed threshold description that includes the threshold type, value, etc.
* **MatchedTerm**: This field is a sample log message to describe the anomaly event.
* **ThresholdType**: This field is the threshold type.
* **FileGlobPath**: This field is the file global path.
* **K8sPodName**: This field is the Kubernetes pod name.
* **K8sNamespace**: This field is the Kubernetes namespace.
* **K8sControllerKind**: This field is the Kubernetes controller kind.
* **K8sContainerName**: This field is the Kubernetes container name.
* **K8sContainerImage**: This field is the Kubernetes container image.
* **K8sControllerLogicalName**: This field is the Kubernetes controller logical name.
* **ECSCluster**: This field is the ECS cluster name.
* **ECSContainerName**: This field is the ECS container name.
* **ECSTaskVersion**: This field is the ECS task version.
* **ECSTaskFamily**: This field is the ECS task family.
* **DockerContainerName**: This field is the Docker container name.



The following example dispalys the use of custom fields:

```yaml
       notify_content:
         title: "Anomaly Detected: {{.ProcessorDescription}}"
         disable_default_fields: false
         custom_fields:
           "Dashboard": "https://admin.edgedelta.com/investigation?edac={{.EDAC}}&timestamp={{.Timestamp}}"
           "Current Value": "{{.CurrentValue}}"
           "Threshold Value": "{{.ThresholdValue}}"
           "Custom Message": "{{.CurrentValue}} exceeds {{.ThresholdValue}}"
           "Built-in Threshold Description": "{{.ThresholdDescription}}"
           "Matched Term": "{{.MatchedTerm}}"
           "Threshold Type": "{{.ThresholdType}}"
           "File Path": "{{.FileGlobPath}}"
           "K8s PodName": "{{.K8sPodName}}"
           "K8s Namespace": "{{.K8sNamespace}}"
           "K8s ControllerKind": "{{.K8sControllerKind}}"
           "K8s ContainerName": "{{.K8sContainerName}}"
           "K8s ContainerImage": "{{.K8sContainerImage}}"
           "K8s ControllerLogicalName": "{{.K8sControllerLogicalName}}"
           "ECSCluster": "{{.ECSCluster}}"
           "ECSContainerName": "{{.ECSContainerName}}"
           "ECSTaskVersion": "{{.ECSTaskVersion}}"
           "ECSTaskFamily": "{{.ECSTaskFamily}}"
           "DockerContainerName": "{{.DockerContainerName}}"
           "SourceAttributes": "{{.SourceAttributes}}"
           "ConfigID": "{{.ConfigID}}"
           "EDAC": "{{.EDAC}}"
           "Epoch": "{{.Epoch}}"
           "Host": "{{.Host}}"
           "MetricName": "{{.MetricName}}"
           "Source": "{{.Source}}"
           "SourceType": "{{.SourceType}}"
           "Tag": "{{.Tag}}"
```

The following example displays the uses of advanced content:  

```yaml
       notify_content:
         title: "Anomaly Detected: {{.ProcessorDescription}}"
         advanced_content: |
           {
             "blocks": [
               {
                 "type": "section",
                 "text": {
                   "type": "mrkdwn",
                   "text": "*Raw POST Anomaly Detected: {{.ProcessorDescription}}*"
                 }
               },
               {
                 "type": "section",
                 "text": {
                   "type": "mrkdwn",
                   "text": "*MatchedTerm* {{.MatchedTerm}}\n*ConfigID* {{.ConfigID}}"
                 }
               }
             ]
           }
```

***


### Example of Stream and Trigger Outputs

The following example displays a configuration with a stream output and a trigger output: 

```yaml
outputs:
  streams:
      - name: sumo-logic-integration
        type: sumologic
        endpoint: "https://[SumoEndpoint]/receiver/v1/http/[UniqueHTTPCollectorCode]"
  triggers:
      - name: slack-integration
        type: slack
        endpoint: "https://hooks.slack.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX"
```

***

