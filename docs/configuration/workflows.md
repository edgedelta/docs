---
description: >-
  This document outlines the process of defining Workflows within an Edge Delta
  configuration.
---

# Workflows

## Overview

You can use this document to learn about the configuration parameters available in a configuration file, specifically for **Workflows**.

A workflow maps inputs, processors, and outputs that are logically grouped, based on the underlying use-cases and analytics being performed.

***

## Access Parameters

At a high level, there are 2 ways to manage **Workflows**:

  * If you need to create a new configuration, then you can use the visual editor to populate a YAML file, as well as make changes directly in the YAML file.
  * If you already have an existing configuration, then you can update the configuration in the YAML file. 

> **Note**
> 
> While all parameters can be updated in the YAML file, most **Workflows** parameters are available in the visual editor. 

***

### Access the visual editor for a new configuration

> **Note**
> 
> To create a new workflow, you must have existing inputs, processors, destinations, thresholds, and filters to add to the new workflow. You cannot create a workflow without these existing components.  

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Click **Visual**.
4. On the right-side, select **Workflows**. 
5. Enter a **Name** and **Description**.
6. Select the desired inputs, processors, destinations, thresholds, and filters to add to the workflow. 
7. To make additional configurations to the configuration file, click the back button, and then select a new configuration parameter to manage. 
8. To save the configuraiton and exit the visual editor, click **Save**. 
9. Refresh the screen to view the newly created configuration in the table. 

***

### Access the YAML file for an existing configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
3. Review the YAML file, make your changes, and then click **Save**. 

***

## Review Parameters

| Key | Description | Required |
| :--- | :--- | :--- |
| name | This key is a user-defined name for the specific workflow. Workflow names are only used for labeling and organizing workflows within a configuration. Workflow names are not reported to any destination. | Yes |
| input\_labels | This key represents a list of input labels to feed to the workflow. Input labels are defined as part of the input configuration. To learn more about available inputs, see [Inputs](./inputs.md). | Yes |
| filters | This key represents a list of filter names that apply to the workflow before incoming logs are passed to the processors. To learn more about filters, see [Filters](./filters.md). | No |
| processors | This key represents a list of processor names that apply to the workflow. To learn about available processors, see [Processors](./processors.md). | Yes |
| destinations | This key represents a list of output names that apply to the workflow. To learn about available integrations, see [Outputs](./outputs.md). | No |

***

## Review YAML Example

```yaml
workflows:
  application_workflow:
    input_labels:
      - system_stats
      - agent_stats
      - application_logs
    processors:
      - error-check
      - fail-check
      - success-check
    destinations:
      - sumo-logic-devops-integration
      - slack-devops-integration

  security_workflow:
    input_labels:
      - syslog_traffic
      - windows_events
      - auth_logs
    filters:
      - not_debug
    processors:
      - traffic-patterns
      - authentication-monitoring
      - system-patterns
    destinations:
      - sumo-logic-security-integration
      - slack-security-integration
```

***

## Configure an Expiring Workflow

To define an expiring workflow, set an expiration time in the workflow definition with the **expires_in** parameter.

With an expiring workflow, you can also enable log forwarding for a specified time with the **log_forward_workflow** parameter.  

> **Note**
> 
> The "expires_in" time format must be in [RFC3339](https://datatracker.ietf.org/doc/html/rfc3339) format.

Review the following example: 

```yaml
workflows:
  log_forward_workflow:
    input_labels:
      - system_stats
      - agent_stats
      - application_logs
    filters:
      - not_debug
    destinations:
      - sumo-logic-devops-integration
    expires_in: 2021-06-01T12:00:00.000Z
```

***
