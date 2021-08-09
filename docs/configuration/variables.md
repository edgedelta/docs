---
description: >-
  This document describes usage of environment variables in Edge Delta
  configuration.
---

# Variables

## Overview

Edge Delta supports the usage of environment variables as values in configuration file. Variables are especially useful to pass secrets to agent in a secure manner.

Environment variables values are acquired from the local operating system environment of the Edge Delta agent process during runtime. Values of environment variables are passed in different ways depending on the agent deployment method and the target environment, see [Environment Variables](https://docs.edgedelta.com/installation/environment-variables).

## Using Variables in Agent Configuration Files

Variables can be referred in one of the following formats in the agent configuration:

```text
{{ Env "MY_VARIABLE_NAME" }}
{{ Env "MY_VARIABLE_NAME" "my default value" }}
```

If no default value is provided existence of the variable in agent execution environment is expected. Otherwise agent will stop with error.

If default value is provided and variable does not exists on agent execution environment default value will be used.

### Example

Slack endpoint is a secret allowing posting directly into a slack channel.

Instead of explicitly putting it into configuration it can be referred from agent execution environment as below:

```text
  triggers:
      - name: slack-integration
        type: slack
        endpoint: {{ Env "MY_SLACK_ENDPOINT" }}
```

## Global Configuration Variables

In certain scenarios it is useful to set a variable in a simple way. On production environment with large number of agents it is impractical to toggle certain features, change destinations or update parts of the processing rules by updating operating system variables.

Configuration Variables table under [Global Settings](https://admin.edgedelta.com/global-settings) provides an easy way to set or override such variables. Variable updates will be detected automatically by the agents and will be in use within a minute.

Example Configuration Variables:

| Name | Value |
| :--- | :--- |
| AGENT\_LOG\_LEVEL | error |
| API\_FILTER\_PATTERN | INFO |

