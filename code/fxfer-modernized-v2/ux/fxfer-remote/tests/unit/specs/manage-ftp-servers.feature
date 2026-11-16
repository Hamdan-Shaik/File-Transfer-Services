# ---
# type: Acceptance Scenarios
# title: Manage FTP servers and credentials
# description: Support staff maintain the list of FTP servers and their login details for file transfer.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.FileTransferSvcQueryService
# tags:
# - api
# - flow
# - providerfiletransfer
# - scenarios
# - wcf
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-04T08:48:57Z
# sources:
# - id: rules
#   resource: /technical/flows/manage-ftp-servers/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-controller-get-ftpserver-credentials
#   resource: components/ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L26
#   title: GetFTPServerCredentials (api door)
# - id: door-file-transfer-controller-add-ftpserver-credentials
#   resource: components/ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L43
#   title: AddFTPServerCredentials (api door)
# - id: door-file-transfer-controller-create-ftpserver-credentials
#   resource: components/ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L52
#   title: CreateFTPServerCredentials (api door)
# - id: door-file-transfer-controller-update-ftpserver-credentials
#   resource: components/ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.FTPServerCredentials.cs#L68
#   title: UpdateFTPServerCredentials (api door)
# - id: footprint
#   resource: /technical/flows/manage-ftp-servers/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: ProviderFileTransfer
#   item: flow:manage-ftp-servers
#   generation:
#     status: done
#     attempts: 2
#   rule_prefix: F01
#   rules: 56
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
#   - backend-data-file-transfer
#   - backend-manage-ftp-servers-api
#   - backend-manage-source-locations-api
#   - data-data-access-entities
#   - data-data-access-entities-3
#   - shared-contracts-commands
#   - shared-contracts-utilities
#   - ui-manage-ftp-servers-screens
#   - ui-manage-source-locations-screens
#   body_sha256: 5b4969ca16c416eca6337318f5abe6c23aa86efeaabbed7a0271c4460e95f3c1
# ---

Feature: Manage FTP servers and credentials
  Support staff maintain the list of FTP servers and their login details for file transfer.
  Other internal systems can retrieve that list, filter it, and use the results to connect and move files.

  @rule:F01-001 @keep
  Scenario: A caller can ask for the list of FTP servers and receive the result
    Given another system asks for the list of FTP servers
    When the file transfer system processes the request
    Then the system receives the matching list of FTP servers

  @rule:F01-002 @keep
  Scenario: A caller can ask for the list of FTP servers through the web-facing entry point
    Given another system asks for the list of FTP servers through the web-facing entry point
    When the file transfer system processes the request
    Then the system receives the matching list of FTP servers

  @rule:F01-003 @keep
  Scenario: Support staff can load a list of FTP server names for selection
    Given support staff ask to load a selection list of FTP servers
    When the file transfer system can retrieve the server list
    Then support staff see the server names and their identifiers
    But when the file transfer system cannot retrieve the server list
    Then support staff are told why the list cannot be shown

  @rule:F01-004 @keep
  Scenario: The FTP server list reflects the servers saved in the system
    Given at least one FTP server has been saved in the system
    When someone views the FTP server list
    Then the saved FTP servers appear in the list

  @rule:F01-005 @flag
  Scenario: A full FTP server list can include stored passwords
    Given another system retrieves FTP server records with full connection details
    When the list is returned
    Then the stored passwords are included in the returned details

  @rule:F01-006 @keep
  Scenario: The FTP server list can be limited to only active servers
    Given some FTP servers are active and some are inactive
    When someone asks for only active FTP servers
    Then only active FTP servers are shown
    But when they do not ask for only active servers
    Then both active and inactive FTP servers may be shown

  @rule:F01-007 @keep
  Scenario: The FTP server list is shown in name order by default in the legacy entry point
    Given some FTP servers exist with different names
    When another system views the FTP server list through the legacy entry point without choosing an ordering
    Then the FTP servers are listed in name order from A to Z

  @rule:F01-007 @keep
  Scenario: The FTP server list has no default order in the web-facing entry point
    Given some FTP servers exist with different names
    When another system views the FTP server list through the web-facing entry point without choosing an ordering
    Then the FTP servers may appear in any order

  @rule:F01-008 @keep
  Scenario: Ordering by name, description, and user treats upper and lower case the same in the legacy entry point
    Given FTP servers exist with names, descriptions, and user names that differ only by letter case
    When another system orders the list by server name, description, or user name through the legacy entry point
    Then the order does not change based only on letter case

  @rule:F01-008 @keep
  Scenario: Choosing an ordering does not change the server list order in the web-facing entry point
    Given some FTP servers exist with different names
    When another system views the FTP server list through the web-facing entry point without choosing an ordering
    Then the FTP servers may appear in any order
    But when the same system views the FTP server list through the web-facing entry point and does choose an ordering
    Then the FTP servers may still appear in the same order

  @rule:F01-009 @keep
  Scenario: The FTP server list can be limited to one specific server identifier in the legacy entry point
    Given an FTP server exists with a known identifier
    When another system asks for that specific identifier through the legacy entry point
    Then only that FTP server is returned

  @rule:F01-009 @keep
  Scenario: Asking for a specific server identifier does not limit the list in the web-facing entry point
    Given at least two FTP servers exist with different identifiers
    When another system asks for one of those identifiers through the web-facing entry point
    Then the returned list may still include FTP servers with other identifiers

  @rule:F01-010 @keep
  Scenario Outline: Filtering by server name supports different ways of matching in the legacy entry point
    Given FTP servers exist with server names that include the text ALPHA
    When another system filters by server name using the <matching method> through the legacy entry point
    Then the resulting list follows that same matching method

    Examples:
      | matching method |
      | contains |
      | starts with |
      | ends with |
      | exact |

  @rule:F01-010 @keep
  Scenario: Filtering by server name uses an exact match in the web-facing entry point
    Given an FTP server exists with name "ACME"
    When another system filters the server list by name "acme" through the web-facing entry point
    Then that FTP server is returned
    But when another system filters the server list by name "  ACME  " through the web-facing entry point
    Then that FTP server is not returned

  @rule:F01-011 @keep
  Scenario Outline: Filtering by server description supports different ways of matching in the legacy entry point
    Given FTP servers exist with descriptions that include the text BACKUP
    When another system filters by description using the <matching method> through the legacy entry point
    Then the resulting list follows that same matching method

    Examples:
      | matching method |
      | contains |
      | starts with |
      | ends with |
      | exact |

  @rule:F01-011 @keep
  Scenario: Filtering by description does not limit the list in the web-facing entry point
    Given FTP servers exist with different descriptions
    When another system filters the server list by description BACKUP through the web-facing entry point
    Then the returned list may still include servers whose description does not include BACKUP

  @rule:F01-012 @keep
  Scenario Outline: Filtering by user name supports different ways of matching in the legacy entry point
    Given FTP servers exist with user names that include the text TRANSFER
    When another system filters by user name using the <matching method> through the legacy entry point
    Then the resulting list follows that same matching method

    Examples:
      | matching method |
      | contains |
      | starts with |
      | ends with |
      | exact |

  @rule:F01-012 @keep
  Scenario: Filtering by user name does not limit the list in the web-facing entry point
    Given FTP servers exist with different user names
    When another system filters the server list by user name TRANSFER through the web-facing entry point
    Then the returned list may still include servers whose user name does not include TRANSFER

  @rule:F01-013 @flag
  Scenario: The selection list always shows only active FTP servers
    Given support staff ask to load a selection list of FTP servers
    And support staff provide other filtering choices
    When the selection list is shown
    Then only active FTP servers appear in the list
    And the other filtering choices are ignored

  @rule:F01-014 @keep
  Scenario: The selection list follows the same ordering as the main server list
    Given support staff ask to load a selection list of FTP servers
    When the selection list is shown
    Then the order of servers matches the order used for the main server list

  @rule:F01-015 @keep
  Scenario: A caller can add a new FTP server through the file transfer entry points
    Given another system provides new FTP server connection details
    When the system records the new FTP server
    Then the caller receives confirmation that the FTP server was added

  @rule:F01-016 @keep
  Scenario: A caller can add a new FTP server through the web-facing entry point
    Given another system provides new FTP server connection details through the web-facing entry point
    When the system records the new FTP server
    Then the caller receives confirmation that the FTP server was added

  @rule:F01-017 @keep
  Scenario: A caller can change an existing FTP server through the file transfer entry points
    Given an FTP server already exists
    When another system changes that FTP server's saved details
    Then the caller receives confirmation that the FTP server was changed

  @rule:F01-018 @keep
  Scenario: A caller can change an existing FTP server through the web-facing entry point
    Given an FTP server already exists
    When another system changes that FTP server's saved details through the web-facing entry point
    Then the caller receives confirmation that the FTP server was changed

  @rule:F01-019 @keep
  Scenario: A caller can retire an FTP server through the file transfer entry points
    Given an FTP server is currently active
    When another system retires that FTP server
    Then the caller receives confirmation that the FTP server was retired

  @rule:F01-020 @keep
  Scenario: A caller can retire an FTP server through the web-facing entry point
    Given an FTP server is currently active
    When another system retires that FTP server through the web-facing entry point
    Then the caller receives confirmation that the FTP server was retired

  @rule:F01-021 @keep
  Scenario: Adding an FTP server checks the details before accepting the save
    Given support staff enter details for a new FTP server
    When they try to save the new FTP server
    Then the system checks the details and either accepts the save or reports problems

  @rule:F01-022 @keep
  Scenario: Changing an FTP server checks the details before accepting the save
    Given support staff edit details for an existing FTP server
    When they try to save the change
    Then the system checks the details and either accepts the save or reports problems

  @rule:F01-023 @keep
  Scenario: A save with validation problems is rejected with a standard error message
    Given support staff try to save an FTP server with at least one validation problem
    When the system rejects the save
    Then the save is rejected with a standard error message
    And the error code is "E"

  @rule:F01-024 @keep
  Scenario: Missing required information is reported as "Missing field: ..."
    Given support staff leave out required information when saving an FTP server
    When the system checks the details
    Then each missing required item is reported using text that starts with "Missing field: "

  @rule:F01-025 @keep
  Scenario: Saving an FTP server checks the identifier, address format, and proxy port rules
    Given support staff try to save an FTP server
    When the system checks the details
    Then the system checks the server identifier
    And the system checks the server address format
    And the system checks the proxy port rules

  @rule:F01-026 @keep
  Scenario: A save is rejected when the server identifier is missing
    Given support staff try to save an FTP server without a server identifier
    When the system checks the details
    Then the save is rejected because the server identifier is not valid

  @rule:F01-027 @keep
  Scenario: A save is rejected when the server address is not in the required format
    Given support staff enter a server address that is not in the required format
    When they try to save the FTP server
    Then the save is rejected because the server address is not in the required format

  @rule:F01-028 @keep
  Scenario: Proxy settings require a positive proxy port number when a proxy address is provided
    Given support staff provide a proxy address for an FTP server
    When they leave the proxy port number blank or set it to 0
    Then the save is rejected because the proxy port number is not valid

  @rule:F01-029 @keep
  Scenario: The system rejects a duplicate FTP server name
    Given an active FTP server already exists with name ACME
    When support staff try to save another active FTP server also named ACME
    Then the save is rejected because the server name is already in use

  @rule:F01-030 @keep
  Scenario: Duplicate name checks ignore letter case
    Given an active FTP server already exists with name "Acme"
    When support staff try to save another active FTP server with name "ACME"
    Then the save is rejected because the server name is already in use

  @rule:F01-030 @keep
  Scenario: Duplicate name checks ignore extra spaces in the legacy entry point
    Given an active FTP server already exists with name "Acme"
    When support staff try to save another active FTP server with name "  ACME  " through the legacy entry point
    Then the save is rejected because the server name is already in use

  @rule:F01-030 @keep
  Scenario: Duplicate name checks treat extra spaces as part of the name in the web-facing entry point
    Given an active FTP server already exists with name "Acme"
    When support staff try to save another active FTP server with name "  ACME  " through the web-facing entry point
    Then the save is not rejected because the server name is already in use

  @rule:F01-031 @keep
  Scenario: A new FTP server is created as active and is assigned a new identifier
    Given support staff are adding a new FTP server
    When the system prepares the new record
    Then the new FTP server is marked as active
    And the new FTP server is assigned a new identifier

  @rule:F01-032 @keep
  Scenario: Adding an FTP server records who made the change and keeps it active
    Given support staff add a new FTP server
    When the system records the new server
    Then the saved record shows the server is active
    And the saved record shows who made the change

  @rule:F01-033 @keep
  Scenario: Changing an FTP server records who made the change
    Given support staff change an existing FTP server
    When the system records the change
    Then the saved record shows who made the change

  @rule:F01-034 @keep
  Scenario: Retiring an FTP server always marks it inactive
    Given an FTP server is currently active
    When support staff retire the FTP server
    Then the FTP server becomes inactive

  @rule:F01-035 @keep
  Scenario: Adding an FTP server records when it was created and last changed
    Given support staff add a new FTP server
    When the system records the new server
    Then the saved record includes when it was created
    And the saved record includes when it was last changed

  @rule:F01-036 @keep
  Scenario: Changing an FTP server updates the last-changed information
    Given an FTP server already exists
    When support staff change the FTP server
    Then the saved record includes an updated last-changed time
    And the saved record includes who made the change

  @rule:F01-037 @keep
  Scenario: Retiring an FTP server updates the last-changed information
    Given an FTP server already exists
    When support staff retire the FTP server
    Then the saved record shows the server is inactive
    And the saved record includes an updated last-changed time
    And the saved record includes who made the change

  @rule:F01-038 @keep
  Scenario: Support staff can view the FTP server credential list from their signed-in screen
    Given support staff are signed in
    When they view the FTP server credential list
    Then they see a list of matching FTP servers
    And they see the total number of matching FTP servers
    But when the system cannot provide the list due to a problem
    Then support staff are told the list cannot be shown
    But when no list data is available at all
    Then support staff see an empty list

  @rule:F01-039 @keep
  Scenario: Saving credentials creates a new server when no server identifier is provided
    Given support staff enter FTP server credentials without a server identifier
    When they save the credentials
    Then the system treats the save as adding a new FTP server

  @rule:F01-040 @keep
  Scenario: Creating FTP server credentials saves a new server when the information is valid
    Given support staff are signed in
    And support staff enter valid details for a new FTP server
    When they save the new FTP server
    Then the FTP server is saved
    And support staff see the saved FTP server details
    But when the provided information is not valid
    Then the save is rejected and support staff are told what is wrong

  @rule:F01-041 @flag
  Scenario: Updating FTP server credentials saves changes and can retire a server
    Given support staff are signed in
    And an FTP server already exists
    When support staff save changes for that FTP server
    Then the change is saved and support staff see the saved details
    But when support staff mark the FTP server as inactive and save
    Then the FTP server is retired
    And support staff see the saved details
    But when the save is rejected and no explanation is provided
    Then the screen may not be able to show an error reason

  @rule:F01-042 @keep
  Scenario: Credential searching uses the first provided value for each search item
    Given support staff provide search text for server name, description, or user name
    When they view the FTP server credential list
    Then only the first value provided for each search item is used

  @rule:F01-043 @flag
  Scenario: Credential searching cannot be used to request only inactive servers
    Given support staff view the FTP server credential list
    When they provide any value at all for the validity search item
    Then the list is limited to only active FTP servers
    And there is no way to request only inactive FTP servers through this search

  @rule:F01-044 @keep
  Scenario: A credential list failure is shown as an error with messages
    Given support staff view the FTP server credential list
    When the system hits a fault while retrieving the list
    Then the list is not shown
    And support staff see one or more error messages

  @rule:F01-045 @keep
  Scenario: The credential list hides stored passwords by default
    Given FTP servers exist with stored passwords
    When support staff view the FTP server credential list in the usual way
    Then the password values in the list are blank
    But when the list is requested with password removal turned off
    Then the stored passwords are included in the list

  @rule:F01-046 @keep
  Scenario: Credential list filtering passes through the entered search choices
    Given support staff provide search choices for server name, description, user name, and validity
    When they view the FTP server credential list
    Then the list reflects those search choices

  @rule:F01-047 @keep
  Scenario: Only the last chosen ordering is used when multiple orderings are supplied
    Given support staff provide more than one ordering choice for the credential list
    When they view the FTP server credential list
    Then only the last ordering choice is used
    And any earlier ordering choices are ignored

  @rule:F01-048 @keep
  Scenario: Credential list paging follows the chosen page number and page size
    Given support staff choose a page number and a page size for the credential list
    When they view the credential list
    Then they see that page of results

  @rule:F01-049 @keep
  Scenario: After a successful save, the screen shows the values returned by the system
    Given support staff save a new or changed FTP server
    When the save succeeds
    Then the screen shows the saved values returned by the system

  @rule:F01-050 @keep
  Scenario: A successful save shows "Success" and a confirmation message
    Given support staff save a new, changed, or retired FTP server
    When the save succeeds
    Then the screen shows save action text "Success"
    And the screen shows a confirmation message suitable for the action

  @rule:F01-051 @keep
  Scenario: Saving an inactive server retires it instead of changing its details
    Given an FTP server already exists
    When support staff save changes with the server marked as inactive
    Then the FTP server is retired

  @rule:F01-052 @keep
  Scenario: Adding a server assigns a new identifier and sends the connection details for saving
    Given support staff enter details for a new FTP server
    When they save the new FTP server
    Then the system assigns a new server identifier
    And the system sends the connection details for saving

  @rule:F01-053 @keep
  Scenario: After adding a server, the returned record includes only the new identifier
    Given support staff add a new FTP server
    When the save succeeds
    Then the returned record includes the new server identifier
    And the returned record may omit other connection details

  @rule:F01-054 @keep
  Scenario: Updating a server without a new password keeps the stored password unchanged
    Given an FTP server already exists with a stored password
    When support staff change other server details without entering a new password
    Then the stored password remains unchanged

  @rule:F01-055 @keep
  Scenario: After changing or retiring a server, the returned record includes the identifier and validity
    Given an FTP server already exists
    When support staff save a change or retire the FTP server
    Then the returned record includes the server identifier
    And the returned record includes whether the server is active

  @rule:F01-056 @keep
  Scenario: Save rejections show localized messages for known problems
    Given support staff try to save an FTP server
    When the save is rejected due to a duplicate name
    Then support staff see a localized message explaining the duplicate name
    But when the save is rejected due to an invalid server address format
    Then support staff see a localized message explaining the server address format
    But when the save is rejected due to an invalid proxy port number
    Then support staff see a localized message explaining the proxy port number

