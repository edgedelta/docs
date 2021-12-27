---
description: >-
  This document provides an overview of the Edge Delta agent configuration
  process, and the various pieces that go into it.
---

# Configuration

## Overview

You can use this document to learn about basic configuration information for the Edge Delta Agent.

The agent uses a configuration file to manage various settings, such as:

  * **Inputs** \(Sources\)
  * **Processors** \(Monitors and Analytics\)
  * **Outputs** \(Streaming destinations and Triggers\)

These components work together to tell the agent:

  * Which datasets should be analyzed
  * What analytics to perform against the datasets    
  * Where to send the generated analytics, insights, anomalies, and notifications

The configuration file is automatically loaded into memory at runtime.

Updates to the configuration file are typically made via the CCB \(Central Configuration Backend\), or locally against the file itself.

  * If configuration updates are made in the Edge Delta App \(via the Cloud Configuration Backend (CCB)\), then the updates are automatically propagated to the agent without a restart.

***

## Review Configuration File Components

An Edge Delta configuration file contains the following components:

| Component | Description |
| :--- | :--- |
| [Agent Settings](./agent-settings.md) | Agent settings are global settings for the configuration and agent. |
| [Inputs](inputs.md) | Inputs define the type of data sources to monitor, such as files, syslog ports, docker containers, etc., and the location or configuration settings of these inputs. |
| [Outputs](outputs.md) | Outputs are streaming destinations or trigger destinations. <p>Streaming destinations are typically Centralized Monitoring Platforms, such as Splunk, Sumo Logic, Datadog, Snowflake, New Relic, Elastic, etc. As the Edge Delta service runs, analytics and insights are continuously generated and forwarded to a streaming destination. <p>Trigger destinations are alerting and automation systems, such as PagerDuty, Slack, ServiceNow, OpsGenie, Runbook, etc. Edge Delta can be configured to send alerts and notifications an alerting and automation system when an anomaly is detected or when certain conditions are met.
| [Processors](processors.md) | Processors are user-defined patterns, analytics, and processing tools to analyze the incoming data streams. Processors can be defined with regular expressions, keyword matching, ratio analytics, tracing logic, as well as clustering and statistical analysis. |
| [Workflows](./workflows.md) | Workflows are the mapping of inputs, outputs and processors, logically grouped together based on the underlying use cases and the analytics being performed. |
| [Variables](./variables.md) | Variables are environment variables that can be referred in configuration. |

> To better understand how these components work, review the following diagram:

> ![The Federated Learning Edge \(FLE\) applies distributed machine learning, statistical analysis, and stream-processing algorithms to incoming data, resulting in dynamically generated outputs \(streams and triggers\)](.././assets/image%20%2812%29.png)

***

## Related Documentation

To learn more, review the following documents:

  * [Agent Settings](./agent-settings.md)
  * [Inputs](inputs.md)
  * [Outputs](outputs.md)
  * [Processors](processors.md)
  * [Workflows](./workflows.md)
  * [Variables](./variables.md)

***
