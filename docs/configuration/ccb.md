---
description: >-
  This document outlines how the Edge Delta SaaS Cloud Configuration Backend
  works, and how it is designed to be used.
---

# Cloud Configuration Backend \(CCB\)

## Overview

You can use this document to learn how to create and manage the Edge Delta Cloud Configuration Backend \(CCB\).

The CCB is a service provided by Edge Delta to help generate and deploy configuration files used by the Edge Delta service.

With the CCB, you can use the Edge Delta Admin portal to create and manage configuration files. The CCB also automatically generates configuration API keys that are used to pre-configure agents during deployment.

Additionally, CCB allows you to update and modify configuration files directly through the portal, which automatically propagates changes down to running agents.

***

## Create a Configuration

There are 2 ways to create a configuration:

  * Use a template with default settings
    * After you save a template, you can view and update the template's configurations.

  * Use a visual editor to populate a YAML file
    * Before you save, you can view and make changes directly in the YAML file.  

**Option 1: Use a template with default settings**

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Select a template, and then click **Save**.
4. Refresh the screen to view the newly created configuration in the table.

  * This entry will also list the API key associated with the configuration.
  * The API key is utilized as part of the agent deployment process, which allows new agents to install and deploy based on this configuration.

As an optional step, you can view and update the configurations in the template.

1. In the **Configurations** table, under **Actions**, click the edit icon for the newly created configuration.
2. Click **YAML**.
3. Make your desired changes, and then click **Save**.

**Option 2: Use a visual editor to populate a YAML file**

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Click **Visual**.
4. On the right-side, select a configuration option. In the empty field, enter your configurations. When you are done, click the back button on top of the form to return to the list of configuration options. **Do not click Save.**  
5. Add additional configurations.
6. As an optional step, you can click **YAML** to view and change your configuraitons in a YAML file.
7. When you are done with the configuration file, click **Save**.
8. Refresh the screen to view the newly created configuration in the table.
  * This entry will also list the API key associated with the configuration.
  * The API Key is utilized as part of the agent deployment process, which allows new agents to install and deploy based on this configuration.

***

## View Configuration History

Any change made to a configuration will be tracked and displayed in the Edge Delta Admin portal.

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Review the listed entries.
3. To filter for specific entry types, click **Filter** and then mark a filter option.
4. To view a diff, locate the desired configuration, and then under **Actions**, click the corresponding diff icon.
5. In the window that appears, click **Show Change**.
6. In **Left Side** and **Ride Side**, select the 2 versions of the config file that you want to see.
7. You can also click **Diff From Current** to see how the latest version differs from the previous version.

***

## Update an Existing Configuration

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
3. Make your changes, and then click **Save**.
4. A new entry will appear in the table, with the date and time of the update.

After you update a configuration via the portal, the updated configuration version will be automatically propagated to any active agent that uses the same API key. The agents communicate with the CCB and retrieve any update. Typically, the agent will be updated within a minute or less.

***

## Configure File Management Locally

While you can use the Cloud Configuration Backend \(CCB\) in the Edge Delta Admin portal to update a configuration, you can also manage and deploy configuration files locally, with tools such as Chef, Puppet, Ansible, Salt, Terraform, etc.  

To configure locally, a flag must be provided during agent deployment to let the system know that Local Configuration File Management is in place.


 **Run Parameter:**

-c path/to/config.yml

**Linux Example:**

```
./edge_delta.sh -c /opt/configs/edge_delta.yml
```

**Windows Example:**

```
start /wait msiexec /qn /i edgedelta-version_64bit.msi -c /opt/configs/edge_delta.yml
```

***
