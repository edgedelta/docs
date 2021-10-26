---
description: >-
  This document describes usage of environment variables in Edge Delta
  configuration.
---

# Variables

## Overview

You can use this document to learn how environment variables are used with Edge Delta.

Environment variables are used as values in the configuration files for the Edge Delta Agent. Variables are used to pass secrets to the agent in a secure manner.

During runtime, the agent's local operating system environment obtains the values for the environment variables.

***

## Learn About Variables in Agent Configuration Files

There are 2 ways to reference variables in the agent configuration file:

```text
'{{ Env "MY_VARIABLE_NAME" }}'
'{{ Env "MY_VARIABLE_NAME" "my default value" }}'
```

If the default value is **not** provided, then the variable in the agent execution environment must exist. If not, the agent will stop with an error.  

If the default value is provided, but the variable does not exist in the agent execution environment, then the default value will be used.

> **Note**
> 
> Environment variables can only be used with string-typed inputs. In other words, if a configuration parameter expects anything besides a string, then the environment variable substitution cannot be used.

***

## Review Example

In the following example, the Slack endpoint is a secret that allows posts to be directly made into a Slack channel.

Instead of explicitly putting the variable into the configuration file, the variable can be referred from the agent execution environment. 

Review the following example: 

```text
  triggers:
      - name: slack-integration
        type: slack
        endpoint: '{{ Env "MY_SLACK_ENDPOINT" }}'
```

***

## Learn About Global Configuration Variables

In certain cases, it is useful to set a variable in a simple way. For example, on a production environment with a large number of agents, it is not practical to update opearting system variables to toggle certain features, change destinations, or update parts of the processing rules.

> **Note**
> 
> To learn how to set or override variables, review the **Configuration Variables** table in the [Global Settings](https://admin.edgedelta.com/global-settings) page in the Edge Delta Admin portal. The agent will automatically detect variable updates.

Review the following examples of configuration variables:

| Name | Value |
| :--- | :--- |
| AGENT\_LOG\_LEVEL | error |
| API\_FILTER\_PATTERN | INFO |

***

## Review Additional Documentation

To learn more, see [Environment Variables](../installation/environment-variables.md).

***
