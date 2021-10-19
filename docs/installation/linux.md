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

## Step 1: Obtain Your Endpoint URL

1. Contact [info@edgedelta.com](mailto:info@edgedelta.com) to obtain your installation endpoint URL. You will need this URL in a later step.

***

## Step 2: Obtain Your API Key

1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
2. In **Configurations**, locate the **Linux** tag, and then copy the corresponding key. You will need this key in a later step.

***

## Step 3: Download the Agent 

1. Visit [release.edgedelta.com](https://release.edgedelta.com/), and then select the appropriate package.

***

## Step 4: Install the Agent 

1. In the following command, replace &lt;YOUR\_API\_KEY&gt; with the key you copied earlier. Additionally, replace &lt;DOWNLOAD\_URL&gt; with the installation endpoint URL you received from Edge Delta. 

```text
sudo ED_API_KEY=<YOUR_API_KEY> bash -c "$(curl -L <DOWNLOAD_URL>/install.sh)"
```

2. If you are not running as **root**, then you may be asked to enter the sudo password. 

3. The installation process will deploy Edge Delta into the `/opt/edgedelta/agent/` path. Additionally, the `edgedelta` system service will start automatically with default configurations.

> **Note**
> 
> The ED\_ENV\_VARS special variable is used in the installation command to pass one or more persistent environment variables to the agent, which will run as the system service.

```bash
sudo ED_API_KEY=<your api key> \
ED_ENV_VARS="MY_VAR1=MY_VALUE_1,MY_VAR2=MY_VALUE_2" \
bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"
```

> **Note**
>
> To view a full list of varaibles supported the agent, see [Environment Variables](environment-variables.md). 



> **Note**
>
> As another option, you can automate the installation process.
>
> To automate:
>
> 1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
>
> 2. In the list of configurations, locate the **Windows** tag, and then click the corresponding deploy icon (green rocket).
>
> 3. In the window that appears, select **Windows**, and then copy the command.
>
> 4. In a command prompt, run the command on the host where you want to deploy Edge Delta, and then the download and installation process will begin.

***

## Troubleshoot the Agent

To check the status of the agent, run one of the following commands: 

  * For systems with systemd\(most distributions\), run:

  ```text
  sudo systemctl status edgedelta
  ```

  * For older systems with init, run:

  ```text
  sudo /etc/init.d/edgedelta status
  ```

  * For certain older versions of Ubuntu, run:

  ```text
  sudo service edgedelta status
  ```

To check the agent's log file for any errors that may indicate an issue with the agent, configuration, or deployment settings, run the following command:

  ```text
  cat /opt/edgedelta/agent/edgedelta.log
  ```

To check the agent's configuration file to ensure that the configuration does not contain any issues, run the following command:

  ```text
  cat /opt/edgedelta/agent/config.yml
  ```

## Uninstall the Agent

To uninstall the agent, run the following command: 

```text
sudo bash -c "$(curl -L https://release.edgedelta.com/uninstall.sh)"
```

***
