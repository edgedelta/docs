---
description: >-
  This document outlines the Thresholds supported by the Edge Delta
  agent, and how they are configured for inputs.
---

# Thresholds

## Overview

You can use this document to learn about the configuration parameters available in a configuration file, specifically for **Thresholds**.

Thresholds define alerting conditions at the agent level. Each agent locally evaluates the thresholds and then triggers an alert if the threshold is met. Alert destinations, such as Slack, PageDuty, and email, can be added to the same workflow to receive these alerts. 

> **Note**
> 
> By default, alerts are saved in the Edge Delta SaaS backend, which powers the [insights](https://admin.edgedelta.com/insights) page.

<br>

> **Note**
> 
> While there are 2 ways to define a threshold, this document applies to workflow-level thresholds.

***

## Define a Threshold

There are 2 ways to define a threshold:

  * Processor-level thresholds
    *  Most processors support the **trigger_thresholds** parameter to define thresholds. 
    *  To learn more, see [Processors](./processors.md).
  * Workflow-level thresholds
    * This method is the newer and more flexible way to define thresholds with various operators and regex-based metric name matching. 
    * This document focuses on this method to explain how to configure thresholds. 

***

## Review Parameters 

| Key | Description | Required |
| :--- | :--- | :--- |
| name | This key is the user-defined name for the threshold. When you create a workflow, this name will appear in the list of available thresholds to add to the workflow. | Yes |
| metric_name | This key is the exact name of the metric to be evaluated. Metric names are generated based on processor names. To learn more, see [Processors](./processors.md). | No |
| metric_name_pattern | This key is the regular expression to match with the metric names. You must provide **metric_name** or **metric_name_pattern**, but not both. | No |
| operator | This key includes valid operators, such as ==, >, >=, <, and <=. | Yes |
| value | This key is the numeric value to compare with the metric value with the operator. | Yes |
| consecutive | This key is the number of times in a row that a threshold condition must be met to trigger an alert. For example, the default value is **0**, which means that any threshold condition met will cause an alert to trigger. | No |

***

## Review and Understand YAML Example 

> **Note**
> 
> The following example has been shortened.


```yaml
thresholds: # Thresholds are defined here and referred in workflows at the bottom.
  - name: max-threshold
    metric_name: response_time_latency.max
    operator: ">="
    value: 300

processors: # Processors are defined here and referred in the workflows at the bottom.
  regexes: 
    - name: "response_time"   # this processor converts logs to metrics based on below pattern. 
      pattern: "completed in (?P<latency>\\d+)ms"
      interval: 1m
  
outputs:
 triggers: # triggered alert destinations are defined here and referred in the workflows at the bottom.
    - name: my-slack-channel
      type: slack
      endpoint: https://hooks.slack.com/services/abc/def/ghi
      suppression_window: 1h
  streams:
    - name: my-splunk
      type: splunk
      token: "******"
      endpoint: "https://xyz.splunkcloud.com:8088/services/collector/event"

workflows:
  sample-workflow:
    inputs:
      ...
    processors:
      - response_time
    thresholds:
      - max-threshold
    destinations:
      - my-slack-channel
      - my-splunk
```

In the above sample configuration, the **response_time** processor generates the following metrics every minute:

| Metric | Description | 
| :--- | :--- | 
| _response\_time\_latency.count_ | This metric represents the total number of matches in the current interval. | 
| _response\_time\_latency.avg_ | This metric represents the average response time for the current interval. | 
| _response\_time\_latency.min_ | This metric represents the minimum response time for the current interval. | 
| _response\_time\_latency.max_ | This metric represents the maximum response time for the current interval. | 
| _response\_time\_latency.anomaly1_ | This metric represents the anomaly score based on the history of the average response times. | 


Based on the above sample configuration, review the overall process:

1. Generated metrics are sent to a streaming destination (**my-splunk**) based on the interval (**1 minute**).
2. The thresholds are evaluated based on the interval (**1 minute**). 
    * In the above example, **sample-workflow** has **max-threshold**, which checks whether **response\_time\_latency.max** is greater than or equal to **300**. When the threshold is met, an alert is triggered and sent to **my-slack-channel**. 
3. When the agent triggers an alert, the agent also sends the most recent relevant logs to the destination (**my-splunk**). 
    * The amount of logs sent with the alert is configurable in [agent_settings](./agent-settings.md). 
    * By default, only the logs of the specific source whose metric exceeded the threshold are sent. 
    * In some cases, logs from all sources might be needed to assist in debugging. This behavior can be configured via the **capture_flush_mode** parameter in [agent_settings](./agent-settings.md).
    * If a destination is not configured, then the alerts and relevant logs are still sent to the Edge Delta SaaS backend. You can disable this behavior via [data preferences](../appendices/data_preferences.md).
4. By default, alerts are suppressed for 30 minutes. Suppression takes places at the individual source and processor level. Some processors can send multiple alerts for different sources within 30 minutes.
    * The suppression window for custom trigger destinations can be configured via the **suppression_window** parameter under trigger destination. In the above example, this parameter is set to 1 hour (**1hr**).


***
