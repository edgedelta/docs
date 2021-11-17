---
description: >-
  This document is the landing page for Outputs with links to the Output types (Streaming, Triggers and
  Archives) 
---

# Outputs

## Overview

You can use this document to learn about the output types available to manage in a configuration file.

An output tells the Edge Delta agent where to send collected and generated data, such as as metrics, patterns, alerts, etc.

> **Note**
> 
> The terms **output**, **integration**, and **destination** are used frequently in these documents. At Edge Delta:
>   * **Output** refers to the high-level concept of sending data to another software platform via a configuration.
>   * **Integration** refers to an organization-level output. 
>   * **Destination** refers to the software platform that will receive the data, such as Splunk or AWS. 

There are 3 types of outputs:

  * Stream
  * Trigger
  * Archive

***

## Output - Stream 

This output type focuses on centralized monitoring platforms.

Edge Delta supports the following platforms: 

  * Sumo Logic 
  * AWS CloudWatch
  * Datadog
  * New Relic
  * InfluxDB
  * Wavefront
  * Scalyr
  * Elastic Search
  * Azure AppInsight
  * Kafka
  * SignalFx
  * Humio
  * Loggly
  * Logz.io
  * Loki

To learn more, see [Outputs-Streams](outputs-streams.md).

***

## Output - Trigger

This output type focuses on alerting and automation systems. Specifically, this output type tells the Edge Delta agent to send alerts and notifications when an anomaly is detected or when various conditions are met. 

Edge Delta supports the following platforms: 

  * Slack
  * Microsoft Teams
  * Pagerduty
  * Jira
  * Service Now
  * Webhook
  * AWS Lambda
  * Azure Functions

To learn more, see [Outputs-Triggers](outputs-triggers.md).

***

## Output - Archive

This output type focuses on storage solutions where the Edge Delta agent can periodically send compressed raw data logs.

Edge Delta supports the following platforms: 

  * AWS S3
  * Azure Blob Storage
  * Google Cloud Storage
  * DigitalOcean Spaces
  * IBM Object Storage
  * Minio
  * Zenko CloudServer
  * Moogsoft
  * Remedy
  * Azure Event Hub Trigger

To learn more, see [Outputs-Archives](outputs-archives.md).

***

## Create an Output

There are 2 ways to create an output. You can create an output at the organization level or for an individual configuration. 

  * An output at the organization level is called an integration. When you create an integration, you must add the integration to an existing configuration. 

### Option 1: Create an Output at the Organization Level

1. In the Edge Delta Admin portal, on the left-side navigation, click **Integrations**.
2. Under **Add Integrations**, select the desired destination and output type.
3. Complete the missing fields, and then click **Save**.

    * To learn more about the parameters for each destination, see [Outputs-Streams](outputs-streams.md), [Outputs-Triggers](outputs-triggers.md), or [Outputs-Archives](outputs-archives.md).
  
4. Select the desired configuration to add the integration, and then click **Add To Configuration**.

   * If you have not yet created the desired configuration, then click **Skip**. You can add the integration later when you create the configuration. Specifically, when you create a configuration, in the YAML file, enter: integration_name: <name of existing integration>  

5. The newly created integration will be listed under **Existing Integrations**. 

#### Option 2: Create an Output for a New Configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Click **Visual**.
4. On the right-side, select **Streams**, **Triggers**, or **Archives**.
5. Select the desired destination, and then complete the missing fields. 

    * To learn more about the parameters for each destination, see [Outputs-Streams](outputs-streams.md), [Outputs-Triggers](outputs-triggers.md), or [Outputs-Archives](outputs-archives.md).

6. To make additional configurations to the configuration file, click the back button, and then select a new configuration parameter to manage. 
7. To save the configuration and exit the visual editor, click **Save**. 
8. Refresh the screen to view the newly created configuration in the table. 

> **Note**
> 
> To add an output to an existing configuration, you must access the configuration's YAML file. Additionally, to make changes outside of the visual editor, you can access the configuration's YAML file.
> 
>  1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
>  2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
>  3. Review the YAML file, make your changes, and then click **Save**.  

***
