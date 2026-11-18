# ---
# type: Acceptance Scenarios
# title: Search and track file transfers
# description: Staff search for file transfers and see what happened to a transfer over time.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.UX.Presentation.Controllers.FileTransferController
# tags:
# - api
# - flow
# - provider-file-transfer-v2
# - scenarios
# - wcf
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-15T09:12:32Z
# sources:
# - id: rules
#   resource: /technical/flows/search-and-track-file-transfers/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-svc-query-service-controller-search-inbound-target
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L45
#   title: SearchInboundTarget (api door)
# - id: door-file-transfer-controller-get-ftemetadatas
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L160
#   title: GetFTEMetadatas (api door)
# - id: door-file-transfer-controller-get-data-list-data
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L63
#   title: GetDataListData (api door)
# - id: door-file-transfer-controller-get-data-list-data-2
#   resource: ux/HP.HSP.UA3.FileTransfer.UX.Services/Controller/FileTransferController.SearchFileTransfer.cs#L90
#   title: GetDataListData (api door)
# - id: footprint
#   resource: /technical/flows/search-and-track-file-transfers/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: provider-file-transfer-v2
#   item: flow:search-and-track-file-transfers
#   generation:
#     status: done
#     attempts: 1
#   rule_prefix: F05
#   rules: 36
#   doors:
#   - file-transfer-controller-search-file-transfer-tracking
#   - file-transfer-controller-search-file-transfer-tracking-2
#   - file-transfer-controller-search-file-transfer
#   - file-transfer-svc-query-service-search-file-transfer-movement
#   - file-transfer-svc-query-service-search-file-transfer-movement-2
#   - file-transfer-svc-query-service-search-file-transfer-movement-3
#   - file-transfer-svc-query-service-controller-search-file-transfer-movement
#   - file-transfer-svc-query-service-search-inbound-target
#   - file-transfer-svc-query-service-search-inbound-target-2
#   - file-transfer-svc-query-service-search-inbound-target-3
#   - file-transfer-svc-query-service-controller-search-inbound-target
#   - file-transfer-controller-get-ftemetadatas
#   - file-transfer-controller-get-data-list-data
#   - file-transfer-controller-get-data-list-data-2
#   tables: 0
#   procedures: 0
#   components:
#   - backend-configure-source-locations-and-destinations-api
#   - ui-configure-source-locations-and-destinations-screens
#   - ui-manage-ftp-servers-and-credentials-screens
#   body_sha256: f1d3cdae4d92f9e45fef20f4c27a48dedda4e7b44837e5444b411790f56da338
# ---

Feature: Search and track file transfers
  Staff search for file transfers and see what happened to a transfer over time.
  They can also look up supporting lists such as modules and inbound identifiers for a contract.
  Other systems can request movement and inbound target details to support reporting and troubleshooting.

  @rule:F05-001 @rule:F05-002 @rule:F05-003 @keep
  Scenario: Staff open the file transfer tracking search screen
    Given staff are signed in
    When they open the file transfer tracking search screen
    Then they see an empty search form for file transfers
    And the main action is labelled Search in their language
    And choosing Cancel returns them to My Home

  @rule:F05-004 @rule:F05-005 @rule:F05-035 @rule:F05-006 @rule:F05-007 @rule:F05-008 @keep
  Scenario: Staff search the tracking list of file transfers
    Given staff are signed in
    And there are file transfers recorded
    When they search for file transfers and ask for a specific page of results
    Then they see the matching transfers and the total number of matches
    And when there are no matches they see an empty list and total 0
    And a date or size written in an unexpected way is ignored
    And a search only counts when it comes from the staff member's own signed-in session
    And if the search cannot be completed then staff are told the reason

  @rule:F05-009 @rule:F05-010 @rule:F05-011 @keep
  Scenario: Staff search saved file transfer configurations
    Given staff are signed in
    And there are saved file transfer configurations recorded
    When they search for configurations and ask for a specific page of results
    Then they see the matching configurations and the total number of matches
    And a search only counts when it comes from the staff member's own signed-in session
    And if the search cannot be completed then staff are told the reason

  @rule:F05-011 @flag
  Scenario: A configuration search is treated as a failure when no list is returned
    Given staff are signed in
    When the system says the search succeeded but provides no list of configurations
    Then staff are told the request could not be completed

  @rule:F05-036
  Scenario: Staff can start a configuration search without choosing a page
    Given staff are signed in
    And there are saved file transfer configurations recorded
    When they search for configurations without asking for a specific page of results
    Then the system still tries to find matching configurations
    And if the search cannot be completed then staff are told the reason

  @rule:F05-012 @rule:F05-013 @rule:F05-014 @keep
  Scenario: Staff request pick list options
    Given staff are signed in
    When they ask for a pick list of options for a screen field
    Then they receive the matching options
    And a request for options only counts when it comes from the staff member's own signed-in session
    And if the options cannot be provided then staff are told the reason

  @rule:F05-013 @flag
  Scenario: Pick list options are returned without a separate total count
    Given staff are signed in
    When they ask for pick list options
    Then they receive only the list of options
    And no separate total count is provided

  @rule:F05-015 @rule:F05-016 @keep
  Scenario: Staff see a pick list of active modules
    Given staff are signed in
    And there are active and inactive modules recorded
    When staff open the module pick list
    Then they see only the active modules
    And the modules are ordered by name from A to Z

  @rule:F05-015 @flag
  Scenario: The module pick list ignores the provided screen and language settings
    Given staff are signed in
    When staff open the module pick list with different screen and language settings
    Then they see the same set of modules

  @rule:F05-017 @rule:F05-018 @keep
  Scenario Outline: Staff look up inbound identifiers for a contract
    Given staff are signed in
    When staff look up inbound identifiers for <contract>
    Then they see <outcome>
    And a lookup only counts when it comes from the staff member's own signed-in session

    Examples:
      | contract | outcome |
      | ACME-CONTRACT | a list of inbound identifiers |
      | UNKNOWN-CONTRACT | an empty list |

  @rule:F05-019 @rule:F05-020 @rule:F05-023 @rule:F05-024 @rule:F05-025 @rule:F05-026 @rule:F05-027 @rule:F05-028 @rule:F05-029 @rule:F05-030 @keep
  Scenario Outline: Another system searches file transfer movements
    Given there are file transfers recorded with different names, partners, dates, sizes, and statuses
    When another system searches file transfer movements with <filter>
    Then it receives only transfers that match <expected>
    And the transfers are ordered by transfer name from A to Z when no ordering is requested
    And for a shared folder source location, the originating server name is blank
    And for an FTP server source location, the originating server name is the FTP server name
    And each transfer includes its activity entries and any recorded problems
    And activity entries are ordered by activity reference number, then by activity code
    And recorded problems are ordered by problem reference number, then by problem type

    Examples:
      | filter | expected |
      | a transfer reference | only that one transfer |
      | a transfer name | transfers with that name |
      | a status | transfers with that status |
      | a direction | transfers with that direction |
      | a transport type | transfers with that transport type |
      | a source location reference | transfers from that source location |
      | a partner name | transfers for that partner |
      | a date range | transfers created within that range |
      | a size range | transfers whose size is within that range |

  @rule:F05-023 @flag
  Scenario: An invalid transfer or source location reference is treated as the system default
    Given there are file transfers recorded
    When another system searches file transfer movements using a transfer or source location reference that is not in the expected format
    Then the search does not stop for that reason
    And the search behaves as if the reference were the system default

  @rule:F05-021 @rule:F05-022 @rule:F05-031 @rule:F05-032 @rule:F05-033 @rule:F05-034 @keep
  Scenario Outline: Another system searches inbound targets
    Given there are inbound target records, including invalid ones and duplicates for the same inbound identifier
    When another system searches inbound targets with <filter>
    Then it receives only valid inbound targets that match <expected>
    And it receives at most one result for each inbound identifier
    And leaving the inbound identifier blank is accepted

    Examples:
      | filter | expected |
      | an inbound identifier | only targets for that inbound identifier |
      | a contract name | only targets for that contract name |
      | no filters | inbound targets for all contracts |
