---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on a Windows-based Operating System.
---

# Windows

## Windows

Edge Delta has a very simple 64 or 32 bit MSI installation process.

Once complete, the agent will run as a background service for Windows-based operating systems.

### Download

Go to [admin.edgedelta.com](https://admin.edgedelta.com) \(or contact the Edge Delta team at [info@edgedelta.com](mailto:info@edgedelta.com)\) to create an account and get access to the agent deployment portal.

### UI Installation

Please download the appropriate package at [release.edgedelta.com](https://release.edgedelta.com/)

After downloading the appropriate installation package, simply double click and follow the wizard.

You can change the installation directory where the Edge Delta agent will install during installation wizard, default path is "Program Files" or "Program Files \(x86\)" depending on your chosen platform.

When prompted, provide the appropriate configuration API Key from the administration portal, and complete installation.

![](../.gitbook/assets/image%20%282%29.png)

## Command Line Silent Installation

_**Note**: The following steps below can be automated by selecting the 'Deploy' button on the right-hand side of a given configuration in the Edge Delta Admin Portal. After hitting the 'Deploy' button, a dialog box will appear with a pre-configured Windows command containing the appropriate API Key for deployment. Simply run the provided Powershell command on the host you want to deploy Edge Delta on, and the download + installation will begin._

After downloading the package start cmd.exe as Administrator. Navigate to the appropriate download directory. Replace &lt;YOUR API KEY&gt; with the appropriate configuration API Key from the administration portal, and run following command:

```text
start /wait msiexec /qn /i edgedelta-version_64bit.msi APIKEY="<YOUR_API_KEY>"
```

It will not output anything to the terminal due to the service running in silent mode.

## Configuration

Once installation completed, the configuration file \(config.yml\) and the agent log file \(edgedelta.log\) can be found in the installation directory.

## Command Line Silent Uninstallation via Powershell

```text
(Get-WmiObject -Query "SELECT * FROM Win32_Product WHERE Name like 'Edge Delta%'").uninstall()
```

### Troubleshooting

After installation, the Windows Services UI \(services.msc\) and the &lt;installation\_directory&gt;\edgedelta.log file can be used to troubleshoot and to check the status of the agent.

