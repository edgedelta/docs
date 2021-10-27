---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on a Linux-based Operating System.
---

# Linux

## Overview

You can use this document to learn how to install the Edge Delta Agent for your Linux-based operating system.

> **Note**
>
> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](/docs/basic-onboarding.md).


***

## Step 1: Create and Download the Agent 

1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**. 
3. Select **Linux**.
4. Click **Save**.  
5. In the table, locate the newly created agent, and then click the corresponding green rocket to deploy additional instructions. 
6. Click **Linux**. 
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

```
sudo ED_API_KEY=<your api key> \
ED_ENV_VARS="MY_VAR1=MY_VALUE_1,MY_VAR2=MY_VALUE_2" \
bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"
```

> **Note**
>
> To view a full list of varaibles supported the agent, see [Environment Variables](environment-variables.md). 


***

## Troubleshoot the Agent

To check the status of the agent, run one of the following commands: 

  * For systems with systemd, run:

  ```
  sudo systemctl status edgedelta
  ```

  * For older systems with init, run:

  ```
  sudo /etc/init.d/edgedelta status
  ```

  * For certain older versions of Ubuntu, run:

  ```
  sudo service edgedelta status
  ```

To check the agent's log file for any errors that may indicate an issue with the agent, configuration, or deployment settings, run the following command:

  ```
  cat /opt/edgedelta/agent/edgedelta.log
  ```

To check the agent's configuration file to ensure that the configuration does not contain any issues, run the following command:

  ```
  cat /opt/edgedelta/agent/config.yml
  ```

## Uninstall the Agent

To uninstall the agent, run the following command: 

```
sudo bash -c "$(curl -L https://release.edgedelta.com/uninstall.sh)"
```

***
