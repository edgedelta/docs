---
description: >-
  The following document covers the process for deploying the Edge Delta service
  on a Linux-based Operating System.
---

# Linux

Edge Delta provides a convenient self extracting installation package supporting common Linux distributions such as Ubuntu, Debian, CentOS, RHEL.

## Download

Go to [admin.edgedelta.com](https://admin.edgedelta.com) \(or contact the Edge Delta team at [info@edgedelta.com](mailto:info@edgedelta.com)\) to create an account and get access to the agent deployment portal.

## Installation

_**Note**: The following steps below can be automated by selecting the 'Deploy' button on the right-hand side of a given configuration in the Edge Delta Admin Portal. After hitting the 'Deploy' button, a dialog box will appear with a pre-configured Linux command containing the appropriate API Key for deployment. Simply run that command on the host you want to deploy Edge Delta on, and the installation / deployment process will begin._

Replace the &lt;YOUR\_API\_KEY&gt; field from the command below with your configuration API Key from the administration portal:

![](../assets/screen-shot-2020-03-31-at-1.16.15-pm.png)

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

See [Environment Variables](environment-variables.md) page for full list of variables supported by agent.

## Troubleshooting

* Check the agent's service status using one of the following commands depending on your Linux.

  Systems with systemd\(most distributions\):

  ```text
  sudo systemctl status edgedelta
  ```

  Older systems with init:

  ```text
  sudo /etc/init.d/edgedelta status
  ```

  Some older versions of Ubuntu:

  ```text
  sudo service edgedelta status
  ```

* Check the agent's log file for any errors that may indicate an issue with the agent, configuration, or deployment settings.

  ```text
  cat /opt/edgedelta/agent/edgedelta.log
  ```

* Check the agent's configuration file to ensure the configuration doesn't contain issues.

  ```text
  cat /opt/edgedelta/agent/config.yml
  ```

## Uninstallation

```text
sudo bash -c "$(curl -L https://release.edgedelta.com/uninstall.sh)"
```