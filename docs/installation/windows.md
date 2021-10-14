---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on a Windows-based Operating System.
---

# Windows

## Overview

You can use this document to learn how to install the Edge Delta Agent for your Windows-based operating system.

Edge Delta uses 64-bit or 32-bit MSI installation process.

> Note:
> This document is designed for existing users. If you have not created an account with Edge Delta, then see see [Basic Onboarding](/basic-onboarding.md).

***

## Step 1: Obtain Your API Key

1. In the Edge Delta App, on the left-side navigation, click

***

## Step 2: Download the Agent

1. To download the agent, visit [release.edgedelta.com](https://release.edgedelta.com/), and then select the appropriate package.

## Step 2: Install the Agent

There are 2 ways to install the agent:

  * Via the Edge Delta App
  * Via the command line (silent install)

### Option 1: Via the Edge Delta App

1. Double-click the downloaded package, and then follow the on-screen instructions.
2. During the installation process, you can change the installation directory where the agent will install. Based on your selected platform, the default path is **Program Files** or **Program Files \(x86\)**.
3. Enter the API key you copied earlier, and then follow the on-screen instructions to complete the installation.


### Option 2: Via the Command Line  (silent install)

> **Note**
> You can automate the steps below.
> To automate:
> 1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
> 2. In the list of configurations, based on the tag, click the deploy icon (green rocket).
> 3. In the window that appears, select **Windows**, and then copy the command.
> 2. In a command prompt, run the command on the host where you want to deploy Edge Delta. The download and installation process will begin.


1. After you download the package, start cmd.exe as Administrator.
2. Navigate to the appropriate download directory.
3. Replace &lt;YOUR API KEY&gt; with the appropriate configuration API Key from the administration portal, and then run following command:

```text
start /wait msiexec /qn /i edgedelta-version_64bit.msi APIKEY="<YOUR_API_KEY>"
```
> **Note**
> Since the service is running in silent mode, nothing will output.

***

## Step 3: Configure the Agent

To configure the agent, you must access the installation directory and locate the following files:

  * The configuration file \(config.yml\)
  * The agent log file \(edgedelta.log\)

***

## Command Line Silent Uninstallation via Powershell

```text
(Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name like 'Edge Delta%'").uninstall()
```

***

### Troubleshoot the Agent

To troubleshoot and check the status of the agent, use the Windows Services UI \(services.msc\) and the &lt;installation\_directory&gt;\edgedelta.log file.


***
