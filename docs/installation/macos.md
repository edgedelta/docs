---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on the MacOS Operating System.
---

# MacOS

## Overview

You can use this document to learn how to install the Edge Delta Agent for your Mac-based operating system.

> **Note**
>
> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](../basic-onboarding.md).

***

## Step 1: Create a Configuration and Download the Agent

1. In the Edge Delta App, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Select **MacOS**.
4. Click **Save**.  
5. In the table, locate the newly created configuration, and then click the corresponding green rocket to deploy additional instructions.
6. Click **MacOS**.
7. In the window that appears, copy the command.
  - This window also displays your API key. Copy this key for a later step.

***

## Step 2: Install the Agent

1. Open a terminal, and paste the copied command.
  - If you are not running as **root**, then you may be asked to enter the sudo password.
2. The installation process will deploy Edge Delta into the `/opt/edgedelta/agent/` path. Additionally, the `edgedelta` system service will start automatically with default configurations.


> **Note**
>
> The ED\_ENV\_VARS special variable is used in the installation command to pass one or more persistent environment variables to the agent, which will run as the system service.


```bash
sudo ED_API_KEY=<your api key> \
ED_ENV_VARS="MY_VAR1=MY_VALUE_1,MY_VAR2=MY_VALUE_2" \
bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"
```

***

## Troubleshoot the Agent

To check the status of the agent, run the following command:

```
sudo su
launchctl list edgedelta
```

To check the agent's log file for any errors that may indicate an issue with the agent, configuration, or deployment settings, run the following command on the Edge Delta service log file path:

```
cat /opt/edgedelta/agent/edgedelta.log
```

To check the agent's configuration file to ensure the configuration does not contain any issue, run the following command on the configuration file path:

```
cat /opt/edgedelta/agent/config.yml
```

***

## Uninstall the Agent

To uninstall the agent, run the following command as the **root** user:

```
sudo bash -c "$(curl -L https://release.edgedelta.com/uninstall.sh)"
```

***
