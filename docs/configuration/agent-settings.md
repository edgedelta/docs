---
description: >-
  This document outlines the agent level parameters that can be defined as part
  of the Edge Delta agent's configuration file.
---

# Agent Settings

## Configuration Parameters

| Key | Description | Default | Required |
| :--- | :--- | :--- | :--- |
| tag | User defined tag to describe the environment. e.g. _prod\_us\_west\_2\_cluster_.  Default value is _Edge_ but it's highly recommended to set this value. | Edge | No |
| _**log**_ | The _log_ section contains subfields to configure agent's own log settings. Agent logs to standard output when run inside a container \(e.g. when run in kubernetes\). When installed as a linux/windows/macos service the agent logs to a file named edgedelta.log next to the installed service location. | N/A | No |
|   level | Logging level for the edge delta agent's self logs. | info | No |
| _**persisting_cursor_settings**_ | The _persisting_cursor_settings_ section contains subfields to configure agent's persisting cursor provider for tailers. This provider will store in-memory cursors into a file, that includes latest cursor positions for a given source, periodically. | N/A | No |
|   path | Path of the folder that cursor file will be saved into. This folder should exist before agent start. | `/var/lib/edgedelta` (Unix) `C:\var\lib\edgedelta` (Windows) | No |
|   file_name | File name of the cursor file that cursors will be saved into | `cursor.json` | No |
|   flush_interval | Flush period of cursor tailer. | 3m | No |
| anomaly\_capture\_size | Anomaly Capture buffer size represents the number of log lines to capture during an anomaly capture | 125 | No |
| anomaly\_capture\_bytesize | Anomaly Capture buffer byte size represents the maximum buffer size in terms of bytes during an anomaly capture | 0 B \(disabled\) | No |
| anomaly\_capture\_duration | Anomaly Capture buffer duration represents the maximum span that the logs of an anomaly capture can belong to | 0s \(disabled\) | No |
| anomaly\_tolerance | When it is non-zero, anomaly scores handle edge cases better when standard deviation is too small. Can be set at rule level for some rule types. | 0.01 | No |
| anomaly\_confidence\_period | Anomaly scores will not be calculated for the first this period after a source is found. Can be set at rule level for some rule types. | 30m | No |
| anomaly\_coefficient | Anomaly coefficient is used to multiple final score to \[0, 100\] range. The higher the coefficient the higher the anomaly score will be. Can be set at rule level for some rule types. | 10 | No |
| skip\_empty\_intervals | Skips empty intervals when rolling so the anomaly scores are calculated based on history of non-zero intervals. Can be set at rule level for some rule types. | false | No |
| only\_report\_nonzeros | Only report non zero stats. Can be set at rule level for some rule types. | false | No |
| capture\_flush\_mode | Sets the behavior of flushing captured contextual log buffers. Supported modes are listed below:   **local\_per\_source**: This is the default mode. captured buffer of a source is flushed when there's a local alert being triggered from same source.   **local\_all**: This is the mode where all captured buffers are flushed when there's a local alert being triggered \(not necessarily from same source\). So in this mode whenever an alert is triggered from agent all capture buffers from all active sources will be flushed   **tag\_per\_source**: This is the mode where captured buffer of a source is flushed when there's an alert from same source and tag \(from any agent within current tag\)   **tag\_all**: This is the mode where all captured buffers on all agents within the same tag is flushed whenever any of the agents trigger an alert | local\_per\_source | No |
| grace\_period | Time the agent waits before triggering alerts. Can be in seconds \(s\) or minutes \(m\) | 0s | No |
| ephemeral | It marks the agent as can be down temporarily due to scale down scenarios and it will be used for capturing down agents. | true | No |
| soft\_cpu\_limit | It is only honored by clustering processor at the moment. 0.5 means 50% of a core. It can be enabled by setting cpu\_friendly=true in clustering rule. | 0.0 | No |

Example:

```yaml
agent_settings:
  tag: prod_payments
  log:
    level: info
  persisting_cursor_settings:
    path: /var/lib/edgedelta/cursor_provider
    file_name: cursor_provider.json
    flush_interval: 5m
  soft_cpu_limit: 0.5
  anomaly_tolerance: 0.1
  anomaly_confidence_period: 1m
  skip_empty_intervals: false
  only_report_nonzeros: false
  anomaly_capture_size: 1000
  anomaly_capture_bytesize: "10 KB"
  anomaly_capture_duration: 1m
  anomaly_coefficient: 10.0
  grace_period: 30s
```

