# ---
# type: Acceptance Scenarios
# title: Manage FTP servers and credentials
# description: Staff maintain the FTP servers used for file transfers with partners.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
# tags:
# - api
# - flow
# - provider-file-transfer-v2
# - scenarios
# - wcf
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-15T08:47:43Z
# sources:
# - id: rules
#   resource: /technical/flows/manage-ftp-servers-and-credentials/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-controller-get-ftpserver-credentials
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L26
#   title: GetFTPServerCredentials (api door)
# - id: door-file-transfer-controller-add-ftpserver-credentials
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L43
#   title: AddFTPServerCredentials (api door)
# - id: door-file-transfer-controller-create-ftpserver-credentials
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L52
#   title: CreateFTPServerCredentials (api door)
# - id: door-file-transfer-controller-update-ftpserver-credentials
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L68
#   title: UpdateFTPServerCredentials (api door)
# - id: footprint
#   resource: /technical/flows/manage-ftp-servers-and-credentials/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: provider-file-transfer-v2
#   item: flow:manage-ftp-servers-and-credentials
#   generation:
#     status: done
#     attempts: 2
#   rule_prefix: F02
#   rules: 63
#   doors:
#   - file-transfer-svc-query-service-get-ftp-servers
#   - file-transfer-svc-query-service-get-ftp-servers-2
#   - file-transfer-svc-query-service-get-ftp-servers-3
#   - file-transfer-svc-query-service-controller-get-ftp-servers
#   - file-transfer-svc-service-add-ftp-server
#   - file-transfer-svc-service-add-ftp-server-2
#   - file-transfer-svc-service-add-ftp-server-3
#   - file-transfer-svc-service-controller-add-ftp-server
#   - file-transfer-svc-service-update-ftp-server
#   - file-transfer-svc-service-update-ftp-server-2
#   - file-transfer-svc-service-update-ftp-server-3
#   - file-transfer-svc-service-controller-update-ftp-server
#   - file-transfer-svc-service-invalidate-ftp-server
#   - file-transfer-svc-service-invalidate-ftp-server-2
#   - file-transfer-svc-service-invalidate-ftp-server-3
#   - file-transfer-svc-service-controller-invalidate-ftp-server
#   - file-transfer-controller-get-ftp-server-details
#   - file-transfer-controller-get-ftpserver-credentials
#   - file-transfer-controller-add-ftpserver-credentials
#   - file-transfer-controller-create-ftpserver-credentials
#   - file-transfer-controller-update-ftpserver-credentials
#   tables: 1
#   procedures: 0
#   components:
#   - backend-business-command-handlers
#   - backend-business-command-handlers-2
#   - backend-configure-source-locations-and-destinations-api
#   - backend-data-access-event-handlers
#   - backend-data-access-event-handlers-2
#   - backend-data-file-transfer
#   - data-data-access-entities
#   - data-data-access-entities-3
#   - shared-contracts-commands
#   - shared-contracts-utilities
#   - ui-configure-source-locations-and-destinations-screens
#   - ui-manage-ftp-servers-and-credentials-screens
#   body_sha256: 0a4107c3d175e480998beaf9a2611020685770efa6bb89d4af9958b6a32141ce
# ---

Feature: Manage FTP servers and credentials
  Staff maintain the FTP servers used for file transfers with partners.
  They can review the available servers, add new servers, change existing servers, and deactivate servers that should no longer be used.

  @rule:F02-001 @rule:F02-002 @rule:F02-003 @rule:F02-004 @keep
  Scenario: Another system receives a list of FTP servers
    Given there are FTP servers recorded for the organisation
    When another system asks for the list of FTP servers
    Then it receives the matching servers

  @rule:F02-005 @rule:F02-006 @rule:F02-007 @rule:F02-008
  Scenario Outline: The original server list can be filtered and ordered
    Given there are FTP servers recorded for the organisation
    When another system asks for FTP servers with <filter> using the original list request
    Then it receives servers that match <expected>
    And the list is ordered by server name from A to Z when no ordering is requested

    Examples:
      | filter | expected |
      | only active servers | active servers |
      | only inactive servers | inactive servers |
      | a specific server reference number | that one server |
      | a server name match rule such as contains or starts with | servers whose name matches |
      | a server description match rule such as contains or starts with | servers whose description matches |
      | a server user name match rule such as contains or starts with | servers whose user name matches |

  @rule:F02-005 @rule:F02-006 @rule:F02-007 @rule:F02-008
  Scenario Outline: The newer server list has limited filtering and no default ordering
    Given there are FTP servers recorded for the organisation
    When another system asks for FTP servers with <filter> using the newer list request
    Then it receives servers that match <expected>
    And the list is not guaranteed to be ordered by server name

    Examples:
      | filter | expected |
      | only active servers | active servers |
      | only inactive servers | inactive servers |
      | an exact server name | servers with that exact name |
      | a server name match rule such as contains or starts with | servers with a name exactly equal to the provided name |
      | a server description filter | all servers |
      | a server user name filter | all servers |
      | a specific server reference number | all servers |

  @rule:F02-009 @flag
  Scenario: The server list includes saved passwords
    Given there are FTP servers recorded for the organisation
    When another system asks for the list of FTP servers
    Then the returned server details include the saved password for each server

  @rule:F02-010 @rule:F02-011 @rule:F02-012 @rule:F02-013 @rule:F02-014 @rule:F02-015 @rule:F02-016 @keep
  Scenario: Staff see a pick list of active FTP servers
    Given staff are signed in
    And there are active and inactive FTP servers recorded
    When staff open the list of FTP servers to choose from
    Then they see only the active servers
    And each option shows the server name and its reference number
    And if the list cannot be loaded then staff are told the request could not be completed

  @rule:F02-017 @rule:F02-018 @rule:F02-032 @rule:F02-035 @keep
  Scenario: A new FTP server is saved as active
    Given there are no active FTP servers named ACME-FTP
    When staff save a new FTP server named ACME-FTP
    Then the new server is recorded as active
    And the save records who saved it
    And the save records when it was created and last changed

  @rule:F02-019 @rule:F02-020 @rule:F02-024 @rule:F02-033 @rule:F02-036 @keep
  Scenario: An existing FTP server can be changed
    Given there is an active FTP server named ACME-FTP with address 10.20.30.40
    When staff change that server to address 10.20.30.41
    Then the server details are updated
    And the save records who changed it
    And the save records when it was last changed
    And the same required details apply when adding and when changing a server

  @rule:F02-021 @rule:F02-022 @rule:F02-034 @rule:F02-037 @keep
  Scenario: An FTP server can be deactivated
    Given there is an active FTP server named ACME-FTP
    When staff deactivate the server
    Then the server is recorded as inactive
    And the save records who changed it
    And the save records when it was last changed

  @rule:F02-023 @rule:F02-025 @rule:F02-026 @rule:F02-027 @rule:F02-028 @rule:F02-029 @rule:F02-030 @keep
  Scenario Outline: Saving a server requires key details
    Given staff are saving an FTP server
    When they try to save it with <problem>
    Then the save does not complete
    And they are told what is missing or invalid
    And each missing piece is reported as a separate message starting with Missing field:
    And the failure is labelled Execution Error with reference E

    Examples:
      | problem |
      | a missing server name |
      | a missing description |
      | a missing address |
      | a missing transfer mode |
      | a missing user name |
      | a missing password |
      | a missing server reference number |
      | an address that is not four numbers separated by dots |
      | a proxy address is given but the proxy port is missing or not greater than zero |

  @rule:F02-062
  Scenario: A server can be saved with a port number of zero
    Given staff are saving an FTP server
    And they provide all required details for a new FTP server
    When they set the server port number to 0 and save
    Then the save succeeds

  @rule:F02-031 @flag
  Scenario: A new active server cannot reuse an existing active server name
    Given there is an active FTP server named ACME-FTP
    When staff try to save another active FTP server named acme-ftp
    Then the save does not complete
    And staff are told the server name is already in use
    And upper and lower case differences are not treated as meaningful for this check
    And in one part of the system, extra spaces at the start or end of the name are also ignored

  @rule:F02-038 @keep
  Scenario Outline: Saving sign-in details creates a new server or changes an existing one
    Given staff are saving sign-in details for an FTP server
    When they save sign-in details <reference>
    Then <outcome>

    Examples:
      | reference | outcome |
      | without a server reference number | a brand new server is created |
      | with an existing server reference number | the existing server is changed |

  @rule:F02-039 @rule:F02-040 @rule:F02-041 @rule:F02-042 @rule:F02-044 @rule:F02-045 @rule:F02-046 @rule:F02-047 @rule:F02-048 @keep
  Scenario: Staff can browse FTP server sign-in details with filtering and paging
    Given there are FTP servers recorded
    When staff browse the server sign-in details
    Then they see a list of matching servers and the total number of matches
    And if there are no results then they see an empty list
    And leaving any filter blank is treated as blank
    And if the same filter is provided more than once then only the first is used
    And saved passwords are not shown by default
    And when staff request a specific page then the page size and page number are used
    And when staff choose multiple orderings then only the last chosen ordering is applied
    And the match rule can be contains, starts with, ends with, or exact match, with exact match used by default

  @rule:F02-043 @flag
  Scenario: Any non-empty value in the active-only filter forces active servers
    Given there are active and inactive FTP servers recorded
    When staff provide any non-empty value for the active-only filter
    Then they see only the active servers
    And there is no way on this screen to explicitly ask for inactive servers

  @rule:F02-049 @rule:F02-050 @rule:F02-051 @rule:F02-052 @rule:F02-061 @keep
  Scenario: Creating sign-in details shows a success message in the staff member's language
    Given staff are signed in
    And they provide all required sign-in details for a new FTP server
    When they save the sign-in details
    Then the save succeeds
    And the new server is given a new reference number
    And staff see a success message in their language

  @rule:F02-053 @rule:F02-054 @rule:F02-056 @rule:F02-057 @rule:F02-058 @rule:F02-061 @keep
  Scenario: Changing sign-in details can keep the existing password
    Given there is an FTP server with sign-in details already saved
    When staff change the server details without entering a new password
    Then the existing password is kept
    And staff see a success message in their language for changing sign-in details

  @rule:F02-063
  Scenario: Reusing the saved password can fail when the server reference number is not recorded
    Given staff are signed in
    When staff try to change sign-in details for a server reference number that is not recorded
    And they do not enter a new password and save
    Then the save does not complete
    And staff see an error

  @rule:F02-054 @rule:F02-057 @rule:F02-059 @rule:F02-060 @rule:F02-061 @keep
  Scenario: Deactivating a server from the sign-in details screen shows a different success message
    Given there is an active FTP server
    When staff mark the server as inactive and save
    Then the server becomes inactive
    And staff see a success message in their language for deactivating a server

  @rule:F02-055 @keep
  Scenario: A save fails when the system cannot provide the saved server details
    Given staff try to change an FTP server
    When the system cannot provide the saved server details after the change
    Then the save does not complete
    And staff see the first error message the system received
