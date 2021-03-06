---
description: >-
  This document outlines the types of data sets that agent collects/generates
  and how to enable/disable their ingestion to Edge Delta backend.
---

# Ingestion to ED Backend Preferences

Edge Delta agent collects/extracts generates a few different type of observability data such as logs, metrics, patterns etc. Some of this data is sent to Edge Delta backend for further processing and visualization purposes as well as user's configured destinations.

Below table lists the data types and their ingestion status to Edge Delta backend. The defaults might be different for your account which can be viewed on [Global Settings](https://admin.edgedelta.com/global-settings) page on your account.

| Data Type | Description | Ingested by default |
| :--- | :--- | :--- |
| Metric | Metric can be either collected from the environment \(e.g. system\_stats\) or generated by [processors](../configuration/processors.md). | Yes |
| Log Cluster Patterns | Log clustering can be turned on by adding a [Cluster processor](../configuration/processors.md#cluster) to the config. Patterns are the trimmed down version of original log line where the variant parts are replaced with stars '\*'. Usually doesn't contain sensitive data. By default sent to Edge Delta Backend. | Yes |
| Log Cluster Samples | Cluster samples are raw logs that matches a specific pattern. For each distinct cluster pattern found by the agent, it also sends a few samples of that cluster. | Yes |
| Contextual Logs | These are the logs happened around an issue. The "issue" can be an anomaly detected by an individual agent \(a.k.a. signal\) or by the backend processing jobs \(a.k.a finding\). Contextual logs are very useful to investigate an issue to find root cause. [Insights page](https://admin.edgedelta.com/insights) lists all signals/findings and you can right click on them to drill down into Contextual logs and metrics around that time. | Yes |
| Raw Log Forwarding | Log is the raw line collected by agent from configured inputs. Raw logs are discarded by default and Edge Delta backend never collects raw logs. See Archive below for ad-hoc log query needs. | No |
| Archive | Archive type represents the storage of raw logs in a compressed manner on a cloud storage such as S3. Edge Delta maintans separate S3 buckets per customer organization and has integration with Snowflake which powers the Log Search page. You can use the top-right filter component to narrow the source/time-range, and run a simple "contains" query on your logs. Note that logs in last few minutes might now show up due to ingestion delay of archived files. | Yes |
| Alerts | Based on the thresholds in the agent configuration, agents can detect signals about unexpected things and this information including its metadata is sent to Edge Delta backend by default. | Yes |
| Events | Edge Delta agent and the backend system can collect/generate some events such as new container version deployments, k8s events etc. These events can be seen on the timeline chart on [Insights page](https://admin.edgedelta.com/insights). | Yes |
| Agent Heartbeats | Agents send heartbeat to the backend every minute. This is useful for us to make sure they are alive and health. This data powers the [Pipeline Status page](https://admin.edgedelta.com/pipeline-status). | Yes |
| Agent Health Data | Agent runs internal pipelines based on the workflow configuration given and has many critical internal components working independently of each other. In order to make sure everything is working as expected we collect health information from the critical components. This data includes success/failure count, last error, start time etc. | Yes |

**Important note:** If you believe any of these data set types contain sensitive data and must NOT be sent to Edge Delta backend then please reach out to us for turning them off. Edge Delta agent supports completely offline mode where the agent can be run with local yaml file and never communicates with Edge Delta backend. If this is the desired mode \(super sensitive data processing use-cases\) then let us know so we can guide you on specifics of this type of deployment.