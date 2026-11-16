# ---
# type: Acceptance Scenarios
# title: Manage source locations and transfer configuration
# description: Providers and support staff maintain where inbound transfers are collected and where they are delivered.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Services.Controller.FileTransferController
# tags:
# - api
# - flow
# - providerfiletransfer
# - scenarios
# - wcf
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-04T09:54:18Z
# sources:
# - id: rules
#   resource: /technical/flows/manage-source-locations/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-svc-query-service-get-integration-file
#   resource: components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L50
#   title: GetIntegrationFile (wcf door)
# - id: door-file-transfer-svc-query-service-get-integration-file-2
#   resource: components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L41
#   title: GetIntegrationFile (wcf door)
# - id: door-file-transfer-svc-service-update-integration-file
#   resource: components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcService.svc.cs#L55
#   title: UpdateIntegrationFile (wcf door)
# - id: door-file-transfer-svc-service-update-integration-file-2
#   resource: components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcService.cs#L55
#   title: UpdateIntegrationFile (wcf door)
# - id: footprint
#   resource: /technical/flows/manage-source-locations/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: ProviderFileTransfer
#   item: flow:manage-source-locations
#   generation:
#     status: done
#     attempts: 1
#   rule_prefix: F02
#   rules: 69
#   doors:
#   - file-transfer-controller-add-file-transfer-base-configuration
#   - file-transfer-controller-get-source-locations
#   - file-transfer-svc-query-service-get-source-locations
#   - file-transfer-svc-query-service-get-source-locations-2
#   - file-transfer-svc-query-service-get-source-locations-3
#   - file-transfer-svc-query-service-controller-get-source-locations
#   - file-transfer-svc-query-service-search-source-locations
#   - file-transfer-svc-query-service-search-source-locations-2
#   - file-transfer-svc-query-service-search-source-locations-3
#   - file-transfer-svc-query-service-controller-search-source-locations
#   - file-transfer-svc-service-file-transfer-add
#   - file-transfer-svc-service-file-transfer-add-2
#   - file-transfer-svc-service-file-transfer-add-3
#   - file-transfer-svc-service-controller-file-transfer-add
#   - file-transfer-svc-service-update-file-transfer-source-location
#   - file-transfer-svc-service-update-file-transfer-source-location-2
#   - file-transfer-svc-service-update-file-transfer-source-location-3
#   - file-transfer-svc-service-controller-update-file-transfer-source-location
#   - file-transfer-svc-service-invalidate-file-transfer-source-location
#   - file-transfer-svc-service-invalidate-file-transfer-source-location-2
#   - file-transfer-svc-service-invalidate-file-transfer-source-location-3
#   - file-transfer-svc-service-controller-invalidate-file-transfer-source-location
#   - file-transfer-svc-query-service-get-integration-file
#   - file-transfer-svc-query-service-get-integration-file-2
#   - file-transfer-svc-service-update-integration-file
#   - file-transfer-svc-service-update-integration-file-2
#   tables: 4
#   procedures: 0
#   components:
#   - backend-business-command-handlers
#   - backend-business-command-handlers-2
#   - backend-data-access-event-handlers
#   - backend-data-access-event-handlers-2
#   - backend-manage-ftp-servers-api
#   - backend-manage-source-locations-api
#   - shared-contracts-commands
#   - ui-manage-ftp-servers-screens
#   - ui-manage-source-locations-screens
#   body_sha256: b7c082a406c37ef2376fee2de9ab6899b50fd2cca7b04661af0f9c0990dd1ace
# ---

Feature: Manage source locations and transfer configuration
  Providers and support staff maintain where inbound transfers are collected and where they are delivered.
  Other internal systems can search these locations, retrieve details, and mark integration files as processed.

  @rule:F02-001 @keep
  Scenario: A caller can search source locations and receive the result
    Given another system asks to search source locations
    When the file transfer system processes the request
    Then the system receives the matching source locations

  @rule:F02-002 @keep
  Scenario: A caller can ask for details of one source location and receive the result
    Given another system asks for the details of one source location
    When the file transfer system processes the request
    Then the system receives the source location details when a match exists

  @rule:F02-003 @keep
  Scenario: A caller can search source locations through the web-facing entry point
    Given another system asks to search source locations through the web-facing entry point
    When the file transfer system processes the request
    Then the system receives the matching source locations

  @rule:F02-004 @keep
  Scenario: A caller can ask for source location details through the web-facing entry point
    Given another system asks for the details of one source location through the web-facing entry point
    When the file transfer system processes the request
    Then the system receives the source location details when a match exists

  @rule:F02-005 @keep
  Scenario: Providers can load a selection list of source locations
    Given a provider opens a screen that needs a list of source locations
    When the file transfer system can retrieve the list
    Then the provider sees a list of items named as the configuration name followed by a dash and the source address
    And each item includes the source location identifier
    But when the file transfer system cannot retrieve the list
    Then the provider is told why the list cannot be shown

  @rule:F02-006 @keep
  Scenario: The selection list shows only active source locations
    Given some source locations are active and some are inactive
    When a provider loads the source location selection list
    Then only active source locations appear in the list

  @rule:F02-007 @keep
  Scenario: The selection list uses the saved source location identifier
    Given a source location exists with a saved identifier
    When a provider loads the source location selection list
    Then the list item identifier matches the saved identifier

  @rule:F02-008 @keep
  Scenario: The selection list is shown in configuration name order
    Given some source locations exist with different configuration names
    When a provider loads the source location selection list
    Then the items are listed in configuration name order from A to Z

  @rule:F02-009 @keep
  Scenario: Searching source locations returns the key saved details for each match
    Given source locations exist with different owners, addresses, and transfer settings
    When another system searches source locations
    Then the results include each matching location's configuration number and name
    And the results include the owner details
    And the results include the transfer method, source address, and source mask
    And the results include whether the location is active

  @rule:F02-010 @flag
  Scenario: Searching source locations supports filtering, but some identifiers must be well formed
    Given source locations exist with different configuration numbers, owner names, configuration names, subsystems, and transfer methods
    When another system filters the search by one or more of those values
    Then only matching source locations are returned
    But when the subsystem identifier filter is not in the expected identifier format
    Then the search can fail instead of returning no matches

  @rule:F02-011 @keep
  Scenario: Search results can be ordered by different business columns
    Given source locations exist with different configuration numbers, names, and active status
    When another system chooses an ordering for the search results
    Then the results follow the chosen ordering
    But when no ordering is chosen
    Then the results are ordered by configuration number from low to high
    And within the same configuration number, active locations are shown before inactive ones

  @rule:F02-012 @keep
  Scenario: A source location detail request must include a source location identifier
    Given another system asks for the details of one source location
    When the request does not include a source location identifier
    Then the request is rejected as missing required information

  @rule:F02-013 @flag
  Scenario: A malformed source location identifier fails the detail lookup
    Given another system asks for the details of one source location
    When the provided source location identifier is not in the expected identifier format
    Then the lookup can fail instead of returning an empty result

  @rule:F02-014 @keep
  Scenario: Source location detail lookup considers all destinations and job definitions
    Given a source location exists with one or more destination locations
    When another system asks for the details of that source location
    Then the returned details include destination locations and their related job names

  @rule:F02-015 @keep
  Scenario: Source location details are returned only when exactly one match exists
    Given no source location exists for a given identifier
    When another system asks for details using that identifier
    Then no source location details are returned
    But given two source locations exist with the same identifier
    When another system asks for details using that identifier
    Then no source location details are returned
    But given exactly one source location exists for a given identifier
    When another system asks for details using that identifier
    Then that source location is returned

  @rule:F02-016 @keep
  Scenario: Source location details include FTP server name when one is linked
    Given a source location exists that is linked to an FTP server
    When another system asks for the details of that source location
    Then the returned details include the FTP server name
    But given a source location exists with no linked FTP server
    When another system asks for the details of that source location
    Then no FTP server name is included in the returned details

  @rule:F02-017 @flag
  Scenario: Destination locations are shown newest first and include job names, but missing job definitions can fail the lookup
    Given a source location exists with two destination locations created at different times
    When another system asks for the details of that source location
    Then the newest destination is shown first
    And each destination includes the related job name when it can be matched
    But when a destination refers to a job definition that does not exist
    Then the lookup can fail instead of returning the details

  @rule:F02-018 @flag
  Scenario: When no source location exists, destination attachment can fail instead of returning an empty result
    Given no source location exists for a given identifier
    When another system asks for the details of that source location
    Then the lookup can fail instead of returning an empty result

  @rule:F02-019 @keep
  Scenario: Invalid portal submissions do not save and return error details
    Given a provider enters transfer configuration details
    When the submitted details are invalid
    Then the configuration is not saved
    And the provider is told what needs to be corrected

  @rule:F02-020 @keep
  Scenario: Dashes in the owner phone number are ignored when saving
    Given a provider enters an owner phone number that includes dashes
    When the provider saves the transfer configuration
    Then the saved phone number keeps the same digits but without dashes

  @rule:F02-021 @keep
  Scenario: Leaving the source location identifier blank creates a new configuration
    Given a provider enters transfer configuration details
    When the provider saves with a blank source location identifier
    Then a new configuration is created
    But when the provider saves with an existing source location identifier
    Then the existing configuration is changed

  @rule:F02-022 @keep
  Scenario: Portal saves succeed only when the back-end save succeeds
    Given a provider saves transfer configuration details
    When the back-end save succeeds
    Then the provider sees the save succeed
    But when the back-end save is rejected
    Then the provider sees the save rejected and is shown the rejection details

  @rule:F02-023 @keep
  Scenario: Destination phone numbers ignore dashes and some destination addresses are forced to a shared folder
    Given a provider enters destination contact phone numbers that include dashes
    And the configuration includes a destination that delivers into events
    When the provider saves the transfer configuration
    Then the destination phone numbers keep the same digits but without dashes
    And the event destination address is forced to the tenant's shared folder setting

  @rule:F02-024 @keep
  Scenario: Destinations inherit their active choice and can omit FTP server details
    Given a provider enters one active destination and one inactive destination
    And one destination leaves its FTP server choice blank
    When the provider saves the transfer configuration
    Then each destination is saved with the same active choice
    And the destination that left the FTP server choice blank is saved without an FTP server identifier

  @rule:F02-025 @keep
  Scenario: Day-of-month scheduling defaults to no chosen day and suspension records the current time
    Given a provider saves a configuration without choosing a day of month
    When the configuration is saved
    Then the day of month choice is treated as not set
    But when a provider marks a configuration as suspended
    Then the suspension change time is set to the current time

  @rule:F02-026 @keep
  Scenario: Unexpected failures during saving are returned as errors
    Given a provider saves transfer configuration details
    When an unexpected failure occurs during the back-end save
    Then the provider receives an error result

  @rule:F02-027 @keep
  Scenario: A back-end add operation returns a confirmation result
    Given another system submits a new transfer configuration for saving
    When the file transfer system saves the configuration
    Then the system receives a confirmation of the saved configuration

  @rule:F02-028 @keep
  Scenario: A caller can save a new transfer configuration through the web-facing entry point
    Given another system submits a new transfer configuration through the web-facing entry point
    When the file transfer system saves the configuration
    Then the system receives the save confirmation

  @rule:F02-029 @keep
  Scenario: Saving is rejected when any rule checks fail
    Given a provider attempts to save transfer configuration details
    When one or more rule checks fail
    Then the save is rejected as a business error

  @rule:F02-030 @keep
  Scenario: Required transfer configuration details must be provided
    Given a provider attempts to save transfer configuration details
    When the configuration name, transfer method, source address, owner name, owner module, owner phone, owner email, start time, or end time is missing
    Then the save is rejected and the provider is told what is missing

  @rule:F02-031 @keep
  Scenario: At least one destination must be active
    Given a provider attempts to save a configuration with no destinations
    When the provider saves the configuration
    Then the save is rejected
    But given a provider attempts to save a configuration where all destinations are inactive
    When the provider saves the configuration
    Then the save is rejected

  @rule:F02-032 @keep
  Scenario: A run schedule must be selected
    Given a provider attempts to save a configuration
    When no run schedule choice is selected
    Then the save is rejected

  @rule:F02-033 @keep
  Scenario: FTP transfers require an FTP server
    Given a provider attempts to save a configuration that uses FTP
    When no FTP server is provided
    Then the save fails

  @rule:F02-034 @keep
  Scenario: Source masks ending in excluded extensions are rejected
    Given a provider attempts to save a configuration with a source mask ending in exe
    When the provider saves the configuration
    Then the save is rejected

  @rule:F02-035 @keep
  Scenario: Adding a configuration assigns the next configuration number
    Given a provider saves a new transfer configuration
    When the save succeeds
    Then the saved configuration includes the next available configuration number

  @rule:F02-036 @flag
  Scenario: In one implementation, newly added configurations are always saved as active
    Given a provider saves a new transfer configuration marked inactive
    When the configuration is saved through the implementation that forces new records active
    Then the saved configuration is active

  @rule:F02-037 @keep
  Scenario: In another implementation, newly added configurations keep the chosen active status
    Given a provider saves a new transfer configuration marked inactive
    When the configuration is saved through the implementation that preserves the chosen status
    Then the saved configuration is inactive

  @rule:F02-038 @keep
  Scenario: Destinations are saved under the new source location and can be forced inactive
    Given a provider saves a new transfer configuration with destination locations
    When the save succeeds
    Then each destination is saved under the new source location
    But when the new source location is saved as inactive
    Then all destinations under it are saved as inactive

  @rule:F02-039 @keep
  Scenario: A caller can change an existing source location and receive a change confirmation
    Given another system asks to change an existing source location
    When the file transfer system processes the change
    Then the system receives a confirmation of the change

  @rule:F02-040 @keep
  Scenario: A caller can change a source location through the web-facing entry point
    Given another system asks to change an existing source location through the web-facing entry point
    When the file transfer system processes the change
    Then the system receives a confirmation of the change

  @rule:F02-041 @keep
  Scenario: A caller can retire an existing source location and receive a retirement confirmation
    Given another system asks to retire an existing source location
    When the file transfer system processes the retirement
    Then the system receives a confirmation of the retirement

  @rule:F02-042 @keep
  Scenario: A caller can retire a source location through the web-facing entry point
    Given another system asks to retire an existing source location through the web-facing entry point
    When the file transfer system processes the retirement
    Then the system receives a confirmation of the retirement

  @rule:F02-043 @keep
  Scenario: A caller can ask for integration file records and receive the result
    Given another system asks for integration file records
    When the file transfer system processes the request
    Then the system receives the matching integration file records

  @rule:F02-044 @keep
  Scenario: A caller can update an integration file record and receive a confirmation
    Given another system asks to update an integration file record
    When the file transfer system processes the update
    Then the system receives a confirmation of the update

  @rule:F02-045 @keep
  Scenario: Source location changes are rejected when any rule checks fail
    Given another system asks to change an existing source location
    When one or more rule checks fail
    Then the change is rejected as a business error

  @rule:F02-046 @keep
  Scenario: Changing a source location requires the key configuration details
    Given another system asks to change an existing source location
    When the configuration name, transfer method, source address, owner name, owner module, owner phone, owner email, start time, or end time is missing
    Then the change is rejected and the caller is told what is missing

  @rule:F02-047 @keep
  Scenario: Changing a source location requires at least one schedule choice
    Given another system asks to change an existing source location
    When no schedule choice is selected
    Then the change is rejected

  @rule:F02-048 @keep
  Scenario: Changing an FTP transfer requires an FTP server
    Given another system asks to change a source location that uses FTP
    When no FTP server is provided
    Then the change is rejected

  @rule:F02-049 @keep
  Scenario: Changing a source location requires both identifiers
    Given another system asks to change an existing source location
    When the source location identifier is missing
    Then the change is rejected
    But when the configuration number is missing
    Then the change is rejected

  @rule:F02-050 @keep
  Scenario: Marking a source location suspended requires a suspension change time
    Given another system changes a source location to be suspended
    When no suspension change time is provided
    Then the change is rejected

  @rule:F02-051 @keep
  Scenario: Source masks with unsafe extensions are rejected on change
    Given another system changes a source location to use a source mask ending in ps1
    When the change is submitted
    Then the change is rejected

  @rule:F02-052 @keep
  Scenario: Retiring a source location requires operator and configuration identifiers
    Given another system asks to retire an existing source location
    When the operator identifier is missing
    Then the retirement is rejected
    But when the source location identifier is missing
    Then the retirement is rejected
    But when the configuration number is missing
    Then the retirement is rejected

  @rule:F02-053 @keep
  Scenario: A change must reference an existing configuration number
    Given no configuration exists for a given configuration number
    When another system asks to change a source location using that configuration number
    Then the change is rejected as a business error

  @rule:F02-054 @flag
  Scenario: Changes choose what to update by configuration number, even when a different source location identifier is supplied
    Given a configuration exists under one source location identifier
    When another system submits a change using that configuration number
    And the submission includes a different source location identifier
    Then the saved change still applies to the configuration number

  @rule:F02-055 @keep
  Scenario: Retirement must reference an existing configuration number
    Given no configuration exists for a given configuration number
    When another system asks to retire a source location using that configuration number
    Then the retirement is rejected as a business error

  @rule:F02-056 @flag
  Scenario: Retirement chooses what to retire by configuration number, even when a different source location identifier is supplied
    Given a configuration exists under one source location identifier
    When another system submits a retirement using that configuration number
    And the submission includes a different source location identifier
    Then the retirement still applies to the configuration number

  @rule:F02-057 @keep
  Scenario: A successful change updates the saved record time
    Given a source location exists
    When another system successfully changes that source location
    Then the saved record reflects the change as of the current time

  @rule:F02-058 @flag
  Scenario: FTP server identifiers must be well formed when changing a source location
    Given another system changes a source location and supplies an FTP server identifier
    When the FTP server identifier is not in the expected identifier format
    Then the change can fail

  @rule:F02-059 @flag
  Scenario: Owner module identifiers must be well formed when changing a source location
    Given another system changes a source location and supplies an owner module identifier
    When the owner module identifier is not in the expected identifier format
    Then the change can fail

  @rule:F02-060 @flag
  Scenario: Destination updates are applied by destination identifier
    Given a source location exists with one destination
    When another system changes the destination using its destination identifier
    Then the destination details are updated
    But when another system adds a destination without an existing destination identifier
    Then a new destination is created

  @rule:F02-061 @flag
  Scenario: Changing a source location always attempts to apply the destination list
    Given another system changes a source location
    When the change includes a destination list
    Then the destination list is applied
    But when the change does not include a destination list
    Then the change can fail

  @rule:F02-062 @keep
  Scenario: Making a source location inactive also makes its destinations inactive
    Given a source location exists with one or more destinations
    When another system changes the source location to be inactive
    Then all destinations under that source location become inactive
    And the destinations record the operator identifier of the person making the change

  @rule:F02-063 @keep
  Scenario: Retiring a source location marks it inactive and records the operator
    Given a source location exists
    When another system retires that source location
    Then the source location becomes inactive
    And the source location records the operator identifier

  @rule:F02-064 @keep
  Scenario: Retiring a source location also retires all its destinations
    Given a source location exists with one or more destinations
    When another system retires that source location
    Then all destinations under that source location become inactive
    And the destinations record the operator identifier

  @rule:F02-065 @keep
  Scenario: Integration file lookups return only active records and support filtering
    Given some integration file records are active and some are inactive
    When another system looks up integration file records
    Then only active records are returned
    But when the caller filters by integration file identifier
    Then only matching records are returned
    But when the caller filters by part of the file's name
    Then only records whose file's name includes that text are returned

  @rule:F02-066 @keep
  Scenario: Integration file lookup returns the expected fields
    Given at least one integration file record exists
    When another system looks up integration file records
    Then each returned record includes the group identifier
    And each returned record includes the outbound definition identifier
    And each returned record includes the configuration number
    And each returned record includes the file's name
    And each returned record includes whether it has been processed
    And each returned record includes the integration file identifier

  @rule:F02-067 @keep
  Scenario: Updating an integration file requires a configuration number
    Given another system asks to update an integration file record
    When the configuration number is missing
    Then the update is rejected

  @rule:F02-068 @keep
  Scenario: Integration file updates record who requested the update
    Given another system asks to update an integration file record
    When the update succeeds
    Then the saved record reflects the identifier of the requestor

  @rule:F02-069 @keep
  Scenario: Updating an integration file marks it processed and records the change
    Given an integration file record exists
    When another system successfully updates that integration file record
    Then the record is marked processed
    And the record reflects the updated configuration number
    And the record reflects whether it is active
    And the record records the operator identifier
    And the record reflects the change as of the current time