---
description: >-
  This document outlines the Role Based Access Control for Edge Delta.
---

# Role Based Access Control

Edge Delta SaaS offering provides isolation between customers via organizations. Any user must be part of an organization within Edge Delta system to access the system in the first place. 

Within an organization, more granular access control to various resources is possible. 

**Resources** are the things that are protected with RBAC system. For example, configurations, logs, rehydration, etc.

**Permissions** specify an access type (read or write) to a resource. For example, read configurations, read/write integrations etc.

**Users** are the individuals who signed up with their email address to admin.edgedelta.com. A user is part of an organization that they created or got invited to. 
Users have a set of permissions attached directly to them and also inherit premissions from the groups they are member of.

**Groups** provide a way to bulk manage user permissions. A group has a set of permissions attached to it. Users can be assigned to one or more groups and the user inherits permissions from all the groups they are member of. By default every organization has 2 builtin groups: Admin and Analyst. Admin has permissions to do everything and Analyst has readonly permissions to most parts of the system. 

### Granular resource types

- **Organization resource:** All users of an organization has read permission on the organization resource by default. Only the super admins has organization level write permission which means they can take any action on any resource.

- **Configuration resource**: Configurations are used by agents to collect/process/stream data. These are managed on [Agent Settings](https://admin.edgedelta.com/agent-settings) page. Granular access per configuration is supported. A user or group can have read or write access to selected configurations. 

- **Access resource**: Access resource represent the permissions. If a user has read permission on access resource then they can only view the users/groups. If a user has write permission on access resource then they can add/remove users/groups.

- **Integration resource**: Integrations are the 3rd party services that Edge Delta supports such as Splunk, AWS, Slack etc. 
A user with read permission on integrations resource can only view the integrations. A user with write permission on integrations resource can add/edit/remove integrations.

- **Monitor resource**: Monitors are the custom alert definitions created by users via [Monitors](https://admin.edgedelta.com/monitors) or [Metrics](https://admin.edgedelta.com/metrics). Only users with write permission on monitor resource can add/edit/remove monitors.

- **Rehydration resource**: Rehydration is the process of unpacking selected range of archive logs and sending them to a streaming destination such as Splunk. Only users with write permission on rehydration resource can add/edit/remove rehydrations.


Organization admins can manage access to these resources via [Organization Management](https://admin.edgedelta.com/organization) page by creating custom groups and assigning users to them based on their access needs.