---
description: >-
  The following document outlines the high level steps for an initial onboarding
  and deployment of the Edge Delta service.
---

# Overview

You can use this document to learn how to:

  * Create an Edge Delta account
  * Download the Edge Delta Agent
  * Create an integration for an existing streaming data service

>  **Before you begin**
>  
> To ensure an easy installation process, review the [Pre-Installation Agent Requirements](agent-requirements.md) document.

<br>

>  **Before you begin**
>  
> To use this document, you must have relevant account information for your existing streaming data service, such as endpoints, tokens, keys, containers, buckets, etc. For example, to create an integration with AWS S3, you must have the Name, Bucket, Region, AWS Key, and AWS Secret Key.

***

## Step 1: Access Edge Delta

1. Navigate to [app.edgedelta.com](https://app.edgedelta.com/), and then click **Sign up**.
2. Complete the missing fields, and then click **Register**. You will be redirected to the **Welcome to Edge Delta** screen in the Edge Delta App.

***

## Step 2: Deploy the Agent

1. In the Edge Delta App, select your operating platform.
2. In **Enter Environment Tag**, enter a descriptive tag to explain where the agent will be deployed.
3. Click **Continue**.
4. Select a destination type to create an organization-level output to send data from Edge Delta.
5. Complete the missing fields, and then click **Continue**.
    * To learn more about outputs, including parameters, see [Outputs](configuration/outputs.md). 
6. Copy the pre-populated agent command.
7. Open a terminal or command line prompt, then paste and run the command.  
8. In the app, click **I Ran Deploy Commands**.
9. The agent will take a few minutes to deploy and authenticate with Edge Delta.
10. After a successful deployment, click **Go To Status Page**.
    * In the window that appears, you can click **Go To Demo Environment** to see a pre-populated account where you can view and test data.
    * To view your own account, click **X** to close the window.
    * If you access the demo environment, but you want to access your own account, in the top, click **Return To Your Own Account**

***

## Step 3: Verify Agent Deployment

There are 2 ways to verify agent deployment:

  * In the app
  * In the target software platform

***

### Option 1: Verify Agent Deployment in the App

1. In the Edge Delta App, on the left-side navigation, under **Data Pipeline**, click **Pipeline Status**.
2. Review the **Active Nodes** section.
3. If the agent was successfully installed, then there will be at least 1 active node.

>  **Note**
>
> After an initial agent deployment, the app may only display non-zero values.

***

### Option 2: Verify Agent Deployment in the Target Software Platform

1. Log in to the configured streaming destination \(Output\) platform, such as Splunk, Sumo Logic, Datadog, New Relic, etc.
2. Identify the appropriate source metadata to query for incoming Edge Delta data. This source should match the source configuration details provided in the configuration file, such as HTTPs Endpoint, HEC endpoint, API URL, etc.
3. Query the source that is configured to receive incoming Edge Delta data.
4. Review query results. Incoming data should contain metadata tags that match the labels and tags defined in the configuration file

***

## Next Steps

After you create an account and install the agent, you can configure your account.
    * To learn more, see [Configuration](configuration/index.md).

***

## Troubleshoot Agent Deployment

If you experience any of the following issues:

* Run command in terminal failed with output message
* No agent appeared in the Edge Delta UI
* No background service is running on target host
* No data is being reported to streaming destination

Then review the appropriate [Installation](installation/index.md) document, specifically the **Troubleshooting**  section.

***
