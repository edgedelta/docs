---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on the MacOS Operating System.
---

# MacOS

Edge Delta provides a convenient self extracting installer package for MacOS.

## Download

Go to [admin.edgedelta.com](https://admin.edgedelta.com) \(or contact the Edge Delta team at [info@edgedelta.com](mailto:info@edgedelta.com)\) to create an account and get access to the agent deployment portal.

## Installation

Replace the &lt;YOUR\_API\_KEY&gt; field from the command below with your configuration API Key from the administration portal:

![](../.gitbook/assets/screen-shot-2020-03-31-at-1.16.15-pm.png)

Replace the &lt;DOWNLOAD\_URL&gt; field from the command below with the installation endpoint URL you received from the Edge Delta team.

```text
sudo ED_API_KEY=<YOUR_API_KEY> bash -c "$(curl -L <DOWNLOAD_URL>/install.sh)"
```

The installation process may prompt for the sudo password if you are not running as root.

The installation process deploys Edge Delta into the path`/opt/edgedelta/agent/` and system service `edgedelta` starts automatically with default configuration.

ED\_ENV\_VARS special variable is used as part of the installation command to pass one or more persistent environment variables to the agent which will run as the system service.

```bash
sudo ED_API_KEY=<your api key> \
ED_ENV_VARS="MY_VAR1=MY_VALUE_1,MY_VAR2=MY_VALUE_2" \
bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"
```

## Troubleshooting

Check the service status using the following command

```text
sudo su
launchctl list edgedelta
```

Check the agent's log file for any errors that may indicate an issue with the agent, configuration, or deployment settings.

Edge Delta's Service Log file path: `/opt/edgedelta/agent/edgedelta.log`

```text
cat /opt/edgedelta/agent/edgedelta.log
```

Check the agent's configuration file to ensure the configuration doesn't contain issues.

Configuration File path: `/opt/edgedelta/agent/config.yml`

```text
cat /opt/edgedelta/agent/config.yml
```

## Uninstallation

Make sure to run uninstallation process as root.

```text
sudo bash -c "$(curl -L https://release.edgedelta.com/uninstall.sh)"
```

