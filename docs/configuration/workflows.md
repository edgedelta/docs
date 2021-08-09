---
description: >-
  This document outlines the process of defining Workflows within an Edge Delta
  configuration.
---

# Workflows

## Overview

Workflows are the mapping of Inputs, Processors and Outputs, logically grouped together based on the underlying use-cases and analytics being performed.

## Workflow Definition

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific workflow. The workflow names are strictly used for labeling and organizing workflows within a configuration, thus they are not reported to any destinations. | Yes |
| input\_labels | A list of input labels to feed into the workflow. Input labels are defined as part of the input configuration. See [Inputs](https://docs.edgedelta.com/configuration/inputs) documentation for more details about available processors. | Yes |
| filters | List of filter names to be applied before passing incoming logs to the processors in this workflow. See [Filters](https://docs.edgedelta.com/configuration/filters) documentation for more details about filters. | No |
| processors | A list of processor names to apply to the given workflow. See [Processors](https://docs.edgedelta.com/configuration/processors) documentation for more details about available processors. | Yes |
| destinations | A list of Output names to apply to the given workflow. See [Outputs](https://docs.edgedelta.com/configuration/outputs) documentation for details about available integrations and how to configure them. | No |
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

### Expiring Workflow

User can define an expiring workflow and set the expiration time in workflow definition. 
By using expiring workflow you can also enable log forwarding until a specific time.
*Note* The "expires_in" time format must be in [RFC3339](https://datatracker.ietf.org/doc/html/rfc3339) format.
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
