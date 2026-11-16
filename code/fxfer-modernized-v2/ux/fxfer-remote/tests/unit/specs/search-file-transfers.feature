# ---
# type: Acceptance Scenarios
# title: Search and track file transfers
# description: Support staff and other internal systems search saved file transfer configurations and see matching results with a total count.
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
#   at: 2026-11-04T10:44:44Z
# sources:
# - id: rules
#   resource: /technical/flows/search-file-transfers/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-svc-query-service-search-inbound-target
#   resource: components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L45
#   title: SearchInboundTarget (wcf door)
# - id: door-file-transfer-svc-query-service-search-inbound-target-2
#   resource: components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L36
#   title: SearchInboundTarget (wcf door)
# - id: door-file-transfer-svc-query-service-search-inbound-target-3
#   resource: components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L36
#   title: SearchInboundTarget (wcf door)
# - id: door-file-transfer-svc-query-service-controller-search-inbound-target
#   resource: components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L45
#   title: SearchInboundTarget (api door)
# - id: footprint
#   resource: /technical/flows/search-file-transfers/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: ProviderFileTransfer
#   item: flow:search-file-transfers
#   generation:
#     status: done
#     attempts: 2
#   rule_prefix: F04
#   rules: 41
#   doors:
#   - file-transfer-controller-search-file-transfer
#   - file-transfer-controller-search-file-transfer-tracking
#   - file-transfer-controller-get-data-list-data
#   - file-transfer-controller-get-data-list-data-2
#   - file-transfer-controller-get-ftemetadatas
#   - file-transfer-svc-query-service-search-file-transfer-movement
#   - file-transfer-svc-query-service-search-file-transfer-movement-2
#   - file-transfer-svc-query-service-search-file-transfer-movement-3
#   - file-transfer-svc-query-service-controller-search-file-transfer-movement
#   - file-transfer-svc-query-service-search-inbound-target
#   - file-transfer-svc-query-service-search-inbound-target-2
#   - file-transfer-svc-query-service-search-inbound-target-3
#   - file-transfer-svc-query-service-controller-search-inbound-target
#   tables: 0
#   procedures: 0
#   components:
#   - backend-manage-source-locations-api
#   - ui-manage-ftp-servers-screens
#   - ui-manage-source-locations-screens
#   body_sha256: 5cd99f1207b8bca99a201829db4ab71d84dc95f33a1d4152b9582082cf9a1d27
# ---

Feature: Search and track file transfers
  Support staff and other internal systems search saved file transfer configurations and see matching results with a total count.
  They can also look up tracked files to see their current state, recorded history entries, and any recorded problems.

  @rule:F04-001 @keep
  Scenario: A configuration search starts from a staff request
    Given support staff want to search saved file transfer configurations
    When they start a configuration search
    Then the file transfer system uses their signed-in identity and the provided search details

  @rule:F04-039 @keep
  Scenario: A configuration search is accepted only from a signed-in session
    Given support staff are signed in
    When they start a configuration search from the web-facing screens
    Then the configuration search is accepted

  @rule:F04-002 @keep
  Scenario: A successful configuration search returns a list and a total count
    Given support staff search saved file transfer configurations
    When the file transfer system can complete the search
    Then they see the matching configurations
    And they see the total number of matching configurations
    But when the file transfer system cannot complete the search
    Then they are told why the search could not be completed

  @rule:F04-003 @keep
  Scenario: A configuration search can use pages, ordering, and filters
    Given support staff search saved file transfer configurations
    When they specify a page number, results per page, ordering, and filters
    Then the returned results reflect the requested page
    And the returned results follow the requested ordering
    And the returned results reflect the requested filters

  @rule:F04-004 @keep
  Scenario: A configuration search can return no matches without failing
    Given support staff search saved file transfer configurations
    When no saved configuration matches the search
    Then they see an empty list of configurations
    And they see a total count of 0

  @rule:F04-005 @keep
  Scenario: A configuration search supports a defined set of filters
    Given saved file transfer configurations exist with different configuration numbers, names, owners, subsystems, transfer methods, and active status
    When support staff filter the search by one or more of those values
    Then only matching configurations are returned
    But when they supply other filter fields
    Then those other filter fields do not affect the results

  @rule:F04-006 @keep
  Scenario: A configuration search supports ordering by known columns
    Given saved file transfer configurations exist with different configuration numbers and names
    When support staff choose an ordering for the search results
    Then the results follow the chosen ordering
    But when no usable ordering is chosen
    Then the results are ordered by configuration number from low to high

  @rule:F04-007 @keep
  Scenario: The default configuration ordering shows active items first within the same configuration number
    Given saved file transfer configurations exist with the same configuration number and different active status
    When support staff search configurations without choosing an ordering
    Then active configurations appear before inactive configurations for the same configuration number

  @rule:F04-008 @flag
  Scenario: Configuration search filtering is applied only when values are present, and some identifiers must be well formed
    Given saved file transfer configurations exist with different configuration numbers, names, owners, subsystems, transfer methods, and active status
    When support staff supply only some filter values
    Then only those supplied values restrict the results
    But when support staff supply a subsystem identifier that is not in the expected identifier format
    Then the search can fail instead of returning no matches
    And when support staff ask for inactive-only results
    Then inactive-only results are not offered as a filter option

  @rule:F04-009 @keep
  Scenario: Each configuration search result includes the saved configuration details
    Given at least one file transfer configuration exists
    When support staff search file transfer configurations
    Then each returned configuration includes its configuration number and name
    And each returned configuration includes the owner and subsystem reference
    And each returned configuration includes the transfer method, source address, and source mask
    And each returned configuration includes whether it is active

  @rule:F04-010 @keep
  Scenario: A configuration search result shows coded choices and an active label
    Given at least one file transfer configuration exists
    When support staff search file transfer configurations
    Then each returned configuration shows the subsystem, transfer method, run day, and time window as coded choices
    And each returned configuration shows an Active or Inactive label based on the saved active status

  @rule:F04-011 @keep
  Scenario: The configuration result includes the subsystem name only when it can be resolved
    Given a configuration exists with a subsystem reference
    When support staff search file transfer configurations
    Then the subsystem name is shown when it can be matched to an active subsystem list
    But when the subsystem reference does not match an active subsystem
    Then the subsystem name is left blank

  @rule:F04-040 @keep
  Scenario: A selection list lookup is accepted only from a signed-in session
    Given support staff are signed in
    When they request a selection list from the web-facing screens
    Then the selection list lookup is accepted

  @rule:F04-012 @keep
  Scenario: A selection list request succeeds only when it is considered valid
    Given support staff request a selection list
    When the file transfer system considers the request valid
    Then they receive the list items
    But when the file transfer system considers the request invalid
    Then they are told why the list cannot be shown

  @rule:F04-013 @keep
  Scenario: Only the subsystem selection list is supported by name
    Given support staff request a selection list
    When they do not specify which list type they want
    Then they receive an empty list
    But when they request a list type other than the subsystem list
    Then they receive an empty list
    And when they request the subsystem list
    Then they receive subsystem list items

  @rule:F04-014 @keep
  Scenario: The subsystem selection list contains only active subsystems in name order
    Given some subsystems are active and some are inactive
    When support staff request the subsystem selection list
    Then only active subsystems appear in the list
    And the subsystems are listed in name order from A to Z
    And each list item includes the subsystem name and identifier

  @rule:F04-041 @keep
  Scenario: Inbound target reference lookup is accepted only from a signed-in session
    Given support staff are signed in
    When they look up inbound target references for a contract
    Then the inbound target reference lookup is accepted

  @rule:F04-015 @keep
  Scenario: A caller can look up inbound target references by contract name
    Given another system supplies a contract name
    When it asks to look up inbound target references for that contract
    Then it receives the inbound target references for that contract name

  @rule:F04-016 @keep
  Scenario: Inbound target reference lookup returns the import job name and inbound reference identifier
    Given inbound targets exist for a contract name
    When another system looks up inbound target references for that contract name
    Then each returned reference includes the import job name
    And each returned reference includes the inbound reference identifier

  @rule:F04-017 @flag
  Scenario: Inbound target reference lookup returns an empty list rather than a not found response
    Given no inbound targets exist for a contract name
    When another system looks up inbound target references for that contract name
    Then it receives an empty list
    And it is not told that the contract name was not found

  @rule:F04-018 @keep
  Scenario: Tracked-file search requests are passed through without extra checks
    Given another system asks to search tracked files through any available entry point
    When the file transfer system processes the request
    Then the request is passed through without adding extra validation or transformations

  @rule:F04-019 @keep
  Scenario: Tracked-file results can come from shared-folder locations and FTP-server locations
    Given tracked files exist for shared-folder collection locations
    And tracked files exist for FTP-server collection locations
    When another system searches tracked files
    Then the results can include files from both kinds of collection locations
    And files from FTP-server collection locations include the originating server name

  @rule:F04-020 @flag
  Scenario: Tracked-file filtering is optional and uses exact matches and inclusive ranges
    Given tracked files exist with different identifiers, names, status values, directions, transfer methods, dates, sizes, trading partner identifiers, and source locations
    When another system supplies one or more filter values
    Then only matching tracked files are returned
    And date and size ranges include the boundary values
    But when the file identifier or source location identifier is not in the expected identifier format
    Then the filter value can be treated as an all-zero identifier value

  @rule:F04-021 @keep
  Scenario: Tracked-file results support ordering and default to ordering by the name of the file
    Given tracked files exist with different identifiers, names, status values, directions, sizes, dates, and trading partner identifiers
    When another system chooses an ordering for the tracked-file results
    Then the results follow the chosen ordering
    But when no ordering is chosen
    Then the results are ordered by the name of the file from A to Z

  @rule:F04-022 @keep
  Scenario: Each tracked file includes history entries and recorded problems
    Given at least one tracked file exists
    When another system searches tracked files
    Then each returned tracked file includes its history entries
    And each returned tracked file includes its recorded problems

  @rule:F04-023 @keep
  Scenario: Tracked-file history entries are listed in a stable order
    Given a tracked file has more than one history entry
    When another system views the tracked file history
    Then the entries are listed by history entry number from low to high
    And within the same history entry number, entries are listed by history entry code from A to Z

  @rule:F04-024 @keep
  Scenario: Tracked-file recorded problems are listed in a stable order
    Given a tracked file has more than one recorded problem
    When another system views the tracked file problems
    Then the problems are listed by problem entry number from low to high
    And within the same problem entry number, problems are listed by problem type code from A to Z

  @rule:F04-025 @keep
  Scenario: The web-facing tracked-file search entry point returns the search result
    Given another system searches tracked files through the web-facing search entry point
    When the file transfer system can complete the search
    Then it receives the search result

  @rule:F04-026 @keep
  Scenario: A tracking search is accepted only from a signed-in session
    Given support staff are signed in
    When they start a tracking search from the web-facing screens
    Then the tracking search is accepted

  @rule:F04-027 @keep
  Scenario: A tracking search uses the staff identity and provided search details
    Given support staff want to search tracked files
    When they start a tracking search
    Then the file transfer system uses their signed-in identity
    And the file transfer system uses the provided search details

  @rule:F04-028 @keep
  Scenario: A successful tracking search returns a list and a total count
    Given support staff search tracked files
    When the file transfer system can complete the search
    Then they see the matching tracked files
    And they see the total number of matching tracked files

  @rule:F04-029 @keep
  Scenario: An unsuccessful tracking search returns a rejection
    Given support staff search tracked files
    When the file transfer system cannot complete the search
    Then they are told why the search could not be completed

  @rule:F04-030 @keep
  Scenario: A tracking search passes paging, ordering, and filters into the tracked-file search
    Given support staff search tracked files
    When they specify a page number, results per page, ordering, and filters
    Then the tracked-file search uses those paging details
    And the tracked-file search uses those ordering details
    And the tracked-file search uses the supported filters

  @rule:F04-031 @keep
  Scenario: Tracking search ignores date and size filters that are not in the expected format
    Given support staff search tracked files
    When they supply a date or size filter value that is not in the expected format
    Then the search ignores that filter value
    And the search continues using the remaining filter values

  @rule:F04-032 @flag
  Scenario: An empty tracked-file result can be treated as an unsuccessful search
    Given no tracked files match the search criteria
    When support staff start a tracking search
    Then the search can be treated as unsuccessful instead of returning an empty list

  @rule:F04-033 @keep
  Scenario: Tracking search results show coded direction, transfer method, and status values
    Given at least one tracked file matches a tracking search
    When support staff view the tracking results
    Then each tracked file shows its direction as a coded choice
    And each tracked file shows its transfer method as a coded choice
    And each tracked file shows its status as a coded choice

  @rule:F04-034 @keep
  Scenario: A remote error during tracking search is recorded and returned as a rejection
    Given support staff search tracked files
    When a remote error occurs during the search
    Then the problem is recorded for the support team
    And support staff are told why the search could not be completed

  @rule:F04-035 @keep
  Scenario: Inbound target searches are passed through without extra checks
    Given another system asks to search inbound targets through any available entry point
    When the file transfer system processes the request
    Then the request is passed through without adding extra validation or transformations

  @rule:F04-036 @keep
  Scenario: The web-facing inbound target search entry point returns the search result
    Given another system searches inbound targets through the web-facing search entry point
    When the file transfer system can complete the search
    Then it receives the search result

  @rule:F04-037 @keep
  Scenario: A new inbound target search starts with an empty inbound reference filter
    Given another system starts an inbound target search
    When it does not provide an inbound reference filter
    Then the inbound reference filter is treated as empty

  @rule:F04-038 @keep
  Scenario: Inbound target search returns only valid targets and removes duplicates by inbound reference
    Given inbound targets exist with different contract names and inbound reference identifiers
    And some inbound targets are marked invalid
    And more than one inbound target exists for the same inbound reference identifier
    When another system searches inbound targets
    Then only valid inbound targets are returned
    And the caller can restrict results by contract name and inbound reference identifier
    And only one inbound target is returned per inbound reference identifier

