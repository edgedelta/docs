---
description: >-
  This document outlines the Thresholds supported by the Edge Delta
  agent, and how they are configured for inputs.
---

# Thresholds

## Overview

Thresholds can be used to define alerting conditions at the agent level. Each agent evaluates the thresholds locally and trigger an alert if the threshold is met. Alert destinations such as slack, pager duty, email, etc. can be added to the same workflow to receive these alerts. By default, alerts are saved in the Edge Delta SaaS backend which powers the [insights](https://admin.edgedelta.com/insights) page.

There are two ways to define thresholds.
1. Processor level thresholds: Most processors support `trigger_thresholds` section to define thresholds. For details see [Processors](./processors.md).
2. Workflow level thresholds: This is the newer and more flexible way to define thresholds with various operators and regex based metric name matching. Rest of this document explains how to configure these thresholds.


Example config (trimmed down):

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

In the above example config, the response_time processor generates following metrics every minute:
* _response\_time\_latency.count_: total count of matches in current interval
* _response\_time\_latency.avg_: average response time in current interval (1 minute)
* _response\_time\_latency.min_: minimum response time in current interval (1 minute)
* _response\_time\_latency.max_: maximum response time in current interval (1 minute)
* _response\_time\_latency.anomaly1_: anomaly score based on history of average response times

**How it works?**
- Generated metrics are sent to the streaming destination `my-splunk` at every interval (1 minute). 
- The thresholds are evaluated at every interval (1 minute). In the above example, `sample-workflow` has `max-threshold` which checks whether _response\_time\_latency.max_ is greater than or equal to 300. When threshold is met, an alert is triggered which would be sent to `my-slack-channel`. 
- When agent triggers an alert, it also sends the most recent relevant logs to the destination which is `my-splunk` in above example. The amount of logs to send with the alerts is configurable in [agent_settings](./agent-settings.md). 
- By default only the logs of the specific source whose metric exceeded the threshold are sent. In some cases, logs from all sources might be needed to assist in debugging. This can be configured via `capture_flush_mode` in [agent_settings](./agent-settings.md).
- Even if there is no destination configured, the alerts and relevant logs are still sent to Edge Delta SaaS backend. This can be turned off if needed via [data preferences](../appendices/data_preferences.md).
- The alerts are suppressed for 30 minutes by default. Suppression is done per source and per processor level. Same processor can fire multiple alerts for different sources within 30 minutes.
- Suppression window for custom trigger destinations can be configured via `suppression_window` specified in the trigger destination. In the above example, it is set to 1 hour.


## Threshold configuration

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific threshold, used for mapping this threshold to a workflow. | Yes |
| metric_name | Exact name of the metric to be evaluated. Metric names are generated based on processor names. Details can be found at [Processors](./processors.md). | No |
| metric_name_pattern | Regular expression to match with metric names to be evaluated. Either metric_name or metric_name_pattern must be provided but not both. | No |
| operator | Valid operators are "==", ">", ">=", "<", "<=". | Yes |
| value | Numeric value to compare with metric value with given operator. | Yes |
| consecutive | The threshold must condition must be met this many times in a row to trigger an alert. Default is 0 so the any condition hit would cause alert | No |