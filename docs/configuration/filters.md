---
description: >-
  This document outlines the various Filters types supported by the Edge Delta
  agent, and how the filters are configured for inputs.
---

# Filters

## Overview

You can use this document to learn about the configuration parameters available in a configuration file, specifically for **Filters**.

You can use a filter to refine and transform collected logs before additional processing takes place. 

  * In other words, the **Filter** step takes place before the **Processor** step. 

You can use a filter to discard unncesary logs or protect sensitive data. As a result, filters can help reduce the agent's resource load because of the reduced log ingestion.

At a high level, to create a filter, you must: 

Step 1: Review filter types
Step 2: Access the visual editor or YAML file
Step 3: Define a filter

You can create the following filter types: 

| Filter Type | Description | 
| regex | This filter type passes all log lines that match the specified regular expression. | 
| mask | This filter type hides (or masks) specific data, based on the configured regex pattern. | 
| buffered-trace | This filter type handles trace logs. | 
| extract-json-field | This filter type extracts a field's value and replaces the whole JSON content with the field's value. | 

***

## Step 1: Access Filters

At a high level, there are 2 ways to manage **Workflows**:

  * If you need to create a new configuration, then you can use the visual editor to populate a YAML file, as well as make changes directly in the YAML file.
  * If you already have an existing configuration, then you can update the configuration in the YAML file. 

> **Note**
> 
> While all parameters can be updated in the YAML file, most **Workflows** parameters are available in the visual editor. 

***

### Option 1: Access the visual editor for a new configuration

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

### Option 2: Access the YAML file for an existing configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
3. Review the YAML file, make your changes, and then click **Save**. 

***

## Step 2: Understand Filter Options

You can create the following filter types:

  * regex
  * mask
  * buffered trace
  * JSON field extractor

***

### Option 1: Regex Filters

This filter type passes all log lines that match the specified regular expression. As a result, all unmatched logs are discarded.  

In the Edge Delta Admin portal, in the visual editor, when you select **regex** as the filter type, the following fields will appear:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| Name | Enter a descriptive name for this filter. When you create an input, processor, or workflow, this name will appear in the list of filters to select. | Required |
| Type | Select **regex**. | Optional |
| Pattern | Enter a regular expression pattern to define which strings to match on. | Required |
| Negate | To reverse the effect of the filter, set this parameter to **true**. | Optional |

***

#### Review Examples for Regex Filters

The following example obtains the log lines that are error-related, and then discards other lines:

```yaml
  - name: error
    type: regex
    pattern: "error|ERROR|ERR|Err"
```

Negative filters are also possibla via the **negate** parameter. The following example discards DEBUG logs, then and passes through other logs:

```yaml
  - name: not_debug
    pattern: "DEBUG"
    negate: true
```

***

### Option 2: Mask Filters

This filter type hides (or masks) specific data, based on the configured regex pattern.

This filter type can be useful to hide sensitive data, such as phone numbers, social security numbers, credit card numbers, etc. Specifically, based on the configured regex pattern, this filter type changes the log lines to hide the content that matched the configured regex pattern.

In the Edge Delta Admin portal, in the visual editor, when you select **mask** as the filter type, the following fields will appear:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for this filter. When you create an input, processor, or workflow, this name will appear in the list of filters to select. | Required |
| type | Select **mask**. | Required |
| pattern | Enter a regular expression pattern to define which strings to match for. You must set **pattern** or **predefined\_pattern**. | Optional |
| predefined\_pattern | Instead of a custom pattern, you can use a common, predefined pattern, such as **credit\_card** and **us\_phone\_dash**. | Optional |
| mask | Enter the string to be used as the replacement for the matched part of the log. The default mask is **\*\*\*\*\*\***. If you specify an empty mask ( **""** ), then the filter will remove the matched pattern from the log line. | Optional |
| mask_captured | This parameter support capture groups for regex masks. In other words, you can replace any match of a capture group with a given map. To replace all match (not submatch), you can use the **all** keyword. | Optional |

***

#### Review Examples for Mask Filters

In the following example, the filter replaces **password: SOME\_PASSWORD** with **password: \*\*\*\*\*\***:

```yaml
  - name: mask_password
    type: mask
    pattern: "password:\s*\w+"
    mask: "password: ******"

  - name: mask_email
    type: mask
    pattern: "email:\s*(?P<email>\w+"
    mask_captured:
      email: '******'
```

Instead of a custom pattern, you can also use a common, predefined pattern, such as **credit\_card** and **us\_phone\_dash**. In the following example, the filter masks US phone numbers: 

```yaml
  - name: mask_phone
    type: mask
    predefined_pattern: us_phone_dash
```

***

## Option 3: Buffered Trace Filters

This filter type handles trace logs. 

  * Edge Delta defines trace log as a set of logs that can be tied together with an ID, such as a trace ID or request ID.

At a high level, this filter type:

  * Groups logs by a specified ID, then
  * Verifies that all relavant events of that trace (or request) is collected, and then
  * Discards or passes the trace logs, based on the configuration.

This filter type offers the following options: 

* Pass through failed operation events
* Pass through high-latency operation events
* Pass through certain percentage of successful events

In the Edge Delta Admin portal, in the visual editor, when you select **buffered-trace** as the filter type, the following fields will appear:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for this filter. When you create an input, processor, or workflow, this name will appear in the list of filters to select. | Required |
| trace\_id\_pattern | Enter a regular expression pattern to extract the trace ID values from logs. Enter a regex with single capture group. | Required |
| failure\_pattern | Enter a regular expression pattern whose match indicates that the trace event \(group of logs sharing same ID\) is a failure. Failures are passed through from this filter. | Required |
| trace\_deadline | Enter a [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents the max duration of a trace. Once the specified trace deadline is reached, the buffered trace filter will take all events belonging to the same trace, apply the filters/sampling \(based on configuration\), and then if passed, the events are passed through | Required |
| success\_sample\_rate | The sample rate \[0,1\] of successfull traces. Default is zero which means successfull traces are discarded. If it's set to 0.2 then 20% of successfull traces will pass thru this filter. _Note:_ Any trace event without a failure\_pattern match indicates successful trace. | Optional |
| latency\_pattern | Regular expression pattern which is used to extract the latency value \(if applicable\) from the trace logs. Must be a regex with single numeric capture group. Only one of the logs belonging to the same trace \(sharing same id\) should have such latency information or the last one will be picked to represent the latency of the trace. Once the latency value is extracted and converted to a number it can be used in conjunction with _latency\_threshold_ to pass thru high latency traces. This is useful to collect the high latency traces in addition to the failed ones which are already pass thru as described in _failure\_pattern_. | Optional |
| latency\_threshold | Enter a numeric value to represent the threshold for high-latency limit. Latency of a trace is extracted with **latency\_pattern**. | Optional |

***

#### Review Examples for Buffered Trace Filters 

Review the following example:

```yaml
filters:
- name: trace_filter
  type: buffered-trace
  trace_id_pattern: \"trace_id\":\"(?P<id>\w+)"
  failure_pattern: \"status\":\"Failed\"
  trace_deadline: 1m
  success_sample_rate: 0.0
  latency_pattern: \"duration\":\"(?P<latency>\d+)\"
  latency_threshold: 8000
```

***

### JSON Field Extractor Filters

This filter type extracts a field's value and replaces the whole JSON content with the field's value. 

When this filter is applied, the original JSON is discarded. As a result, if the original JSON needs to be fed into another workflow or processor, then Edge Delta recommends that you attach this filter to the processor. 

In the Edge Delta Admin portal, in the visual editor, when you select **extract-json-field** as the filter type, the following fields will appear:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| name | Enter a descriptive name for this filter. When you create an input, processor, or workflow, this name will appear in the list of filters to select. | Required |
| type | Select **extract-json-field**. | Required |
| field_path | This parameter is a dot-separated path of the field, such as **log.message**. This value will be extracted, and then the original JSON content will be discarded. | Required |

***

#### Review Examples for JSON Field Extractor Filters

The following example extracts the message field:

  * If the field was nested, then we would set field_path to its path.

```yaml
  - name: extract_message
    type: extract-json-field
    field_path: "message"
```

Review the following example log:

**`{"timestamp":1623793757, "level": "info", "message": "hello world"}`**

Review the following example log after extractor filter is applied:
**`hello world`**

***

## Step 3: Define a Filter and Understand Workflow

In a YAML file, filters are defined at the top level. Review the following example: 

```yaml
filters:
  - name: error
    type: regex
    pattern: "error"
```

After you define a filter, filters can be referred at different places in the YAML file:

* Input filters apply right after the data ingestion from the input and before running workflows associated with the input.
* Workflow filters apply before running processors within the workflow.
* Processor filters apply before the processor runs, regardless of which workflow the processor is running within.


***

## Filter Specific Inputs, Processors, and Workflows

When you create a filter, you must add a name to describe the filter. This name will appear in the list of filters to select when you create an input, processor, or workflow.

When multiple filters are provided, the names are applied in given order from top to bottom.

The following example displays a file **input** with **error** and **mask\_card** filters:

  ```yaml
    inputs:
      files:
        - labels: "billing"
          path: "/var/log/billing/*.log"
          filters:
            - error
            - mask_card
  ```

To learn how inputs can be filtered, see [Inputs](./inputs.md).

The following example displays a **workflow** with the **error** filter:

  ```yaml
  workflows:
    application_workflow:
      input_labels:
        - system_stats
        - agent_stats
        - application_logs
      filters:
        - error
      processors:
        - error-check
        - fail-check
        - success-check
      destinations:
        - sumo-logic-devops-integration
        - slack-devops-integration
  ```

To learn how workflows can be filtered, see [Workflows](./workflows.md).

The following example displays the **Dimension Counter Processor** with the **not\_debug** filter.

  ```yaml
  regexes:
    - name: "log"
      pattern: "level=(?P<level>\\w+) "
      dimensions: ["level"]
      trigger_thresholds:
        anomaly_probability_percentage: 90
      filters:
        - not_debug
  ```

To learn how processors can be filtered, see [Processors](./processors.md).

***
