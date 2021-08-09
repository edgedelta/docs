---
description: >-
  This document provides an overview of the Edge Delta agent configuration
  process, and the various pieces that go into it.
---

# Configuration

The Edge Delta agents utilize a configuration file to manage various settings, such as: **Inputs** \(Sources\), **Processors** \(Monitors and Analytics\) that it performs against the data, as well as **Outputs** \(Streaming destinations and Triggers\).

The configuration file is automatically loaded into memory at runtime. Updates to the configuration file are typically made via the CCB \(Central Configuration Backend\), or locally against the file itself.

If configuration updates are made in the UI \(via the CCB\), these updates are automatically propagated down to the agent without requiring a restart.

## Configuration File Overview

An Edge Delta configuration file contains five main components:

* [Agent Settings](https://docs.edgedelta.com/configuration/agent-settings)
* [Inputs](https://docs.edgedelta.com/configuration/inputs)
* [Processors](https://docs.edgedelta.com/configuration/outputs)
* [Outputs](https://docs.edgedelta.com/configuration/processors)
* [Workflows](https://docs.edgedelta.com/configuration/workflows)
* [Variables](https://docs.edgedelta.com/configuration/variables)

These pieces work together to tell the agent which datasets should be analyzed, what analytics to perform against the datasets, and where to send the generated analytics, insights, anomalies and notifications.

**See diagram below for more details**

![The Federated Learning Edge \(FLE\) applies distributed machine learning, statistical analysis, and stream-processing algorithms to incoming data, resulting in dynamically generated outputs \(streams and triggers\)](.././assets/image%20%2812%29.png)

## Details

| Name | Description |
| :--- | :--- |
| [Agent settings](https://docs.edgedelta.com/configuration/agent-settings) | Agent Settings are global settings for the configuration and agent |
| [Inputs](inputs.md) | Inputs define what type of data sources you monitor \(files, syslog ports, docker containers, etc.\), and the location or configuration settings of these inputs. |
| [Outputs](outputs.md) | Outputs are either are either Streaming destinations, or Trigger destinations. Streaming destinations are typically Centralized Monitoring Platforms \(i.e. Splunk, Sumo Logic, Datadog, Snowflake, New Relic, Elastic, etc.\). As the Edge Delta service is running,  analyics and insights are continuously generated and forwarded on to Streaming destinations. Trigger destinations are alerting and automation systems \(i.e. PagerDuty, Slack, ServiceNow, OpsGenie, Runbook, etc.\) that Edge Delta can be configured to send alerts and notifications to when anomalies are detected or various conditions are met. |
| [Processors](processors.md) | Processors are user-defined patterns, analytics, and processing tools to analyze the incoming data streams. Processors can be defined using regular expressions, keyword matching, ratio analytics, tracing logic, as well as clustering and statistical analysis. |
| [Workflows](https://docs.edgedelta.com/configuration/workflows) | Workflows are the mapping of Inputs, Outputs and Processors, logically grouped together based on the underlying use-cases and analytics being performed. |
| [Variables](https://docs.edgedelta.com/configuration/variables) | Variables are environment variables that can be referred in configuration. |

