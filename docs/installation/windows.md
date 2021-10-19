---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on a Windows-based Operating System.
---

# Windows

## Overview

You can use this document to learn how to install the Edge Delta Agent for your Windows-based operating system.

Edge Delta uses 64-bit or 32-bit MSI installation process.

> **Note**

> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](/docs/basic-onboarding.md).

***

## Step 1: Obtain Your API Key

1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
2. In **Configurations**, locate the **Windows** tag, and then copy the corresponding key. You will need this key in a later step.

***

## Step 2: Download the Agent

1. Visit [release.edgedelta.com](https://release.edgedelta.com/), and then select the appropriate package.

***

## Step 3: Install the Agent

There are 2 ways to install the agent:

  * Via the wizard
  * Via the command line (silent mode)

### Option 1: Via the Wizard

1. Double-click the downloaded package, and then follow the on-screen instructions.
2. During the installation process, you can change the installation directory where the agent will install. Based on your selected platform, the default path is **Program Files** or **Program Files \(x86\)**.
3. Enter the API key you copied earlier, and then follow the on-screen instructions to complete the installation.


### Option 2: Via the Command Line (Silent Mode)


1. For the downloaded package, start **cmd.exe** as the administrator.
2. Navigate to the appropriate download directory.
3. In the following command, replace &lt;YOUR API KEY&gt; with the key you copied earlier, and then run command:

```text
start /wait msiexec /qn /i edgedelta-version_64bit.msi APIKEY="<YOUR_API_KEY>"
```
> **Note**

> Since the service is running in silent mode, there will not be an output.

<br>

> **Note**

> As another option, you can automate the installation process.

> To automate:

> 1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.

> 2. In the list of configurations, locate the **Windows** tag, and then click the corresponding deploy icon (green rocket).

> 3. In the window that appears, select **Windows**, and then copy the command.

> 4. In a command prompt, run the command on the host where you want to deploy Edge Delta, and then the download and installation process will begin.

***

## Step 4: Configure the Agent

To configure the agent, you must access the installation directory and locate the following files:

  * The configuration file \(config.yml\)
  * The agent log file \(edgedelta.log\)

***

## Troubleshoot the Agent

To troubleshoot and check the status of the agent, use the **Windows Services UI \(services.msc\)** and the **&lt;installation\_directory&gt;\edgedelta.log** file.


***

## Uninstall the Agent

To uninstall the agent via the command line (silent uninstall), run the following command in Powershell:

```text
(Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name like 'Edge Delta%'").uninstall()
```

***
