# ---
# type: Acceptance Scenarios
# title: Configure file transfer source locations and destinations
# description: Staff can review and maintain where files are picked up from and where they are delivered.
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
#   at: 2026-11-15T08:09:56Z
# sources:
# - id: rules
#   resource: /technical/flows/configure-source-locations-and-destinations/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-file-transfer-svc-query-service-search-source-locations
#   resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/FileTransferSvcQueryService.svc.cs#L25
#   title: SearchSourceLocations (wcf door)
# - id: door-file-transfer-svc-query-service-search-source-locations-2
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Queries/FileTransferSvcQueryService.cs#L16
#   title: SearchSourceLocations (wcf door)
# - id: door-file-transfer-svc-query-service-search-source-locations-3
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvcCore/FileTransferSvcQueryService.cs#L16
#   title: SearchSourceLocations (wcf door)
# - id: door-file-transfer-svc-query-service-controller-search-source-locations
#   resource: bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferApi/Controllers/FileTransferSvcQueryServiceController.cs#L21
#   title: SearchSourceLocations (api door)
# - id: footprint
#   resource: /technical/flows/configure-source-locations-and-destinations/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: provider-file-transfer-v2
#   item: flow:configure-source-locations-and-destinations
#   generation:
#     status: done
#     attempts: 1
#   rule_prefix: F01
#   rules: 108
#   doors:
#   - file-transfer-controller-file-transfer-configuration-view
#   - file-transfer-controller-file-transfer-configuration-edit
#   - file-transfer-controller-add-file-base-configuration
#   - file-transfer-controller-add-file-transfer-base-configuration
#   - file-transfer-controller-destination-configuration-edit
#   - file-transfer-controller-destination-configuration-edit-panel
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
#   - file-transfer-svc-query-service-get-source-locations
#   - file-transfer-svc-query-service-get-source-locations-2
#   - file-transfer-svc-query-service-get-source-locations-3
#   - file-transfer-svc-query-service-controller-get-source-locations
#   - file-transfer-controller-get-source-locations
#   - file-transfer-svc-query-service-search-source-locations
#   - file-transfer-svc-query-service-search-source-locations-2
#   - file-transfer-svc-query-service-search-source-locations-3
#   - file-transfer-svc-query-service-controller-search-source-locations
#   tables: 3
#   procedures: 0
#   components:
#   - backend-business-command-handlers
#   - backend-business-command-handlers-2
#   - backend-configure-source-locations-and-destinations-api
#   - backend-data-access-event-handlers
#   - backend-data-access-event-handlers-2
#   - shared-contracts-commands
#   - ui-configure-source-locations-and-destinations-screens
#   - ui-manage-ftp-servers-and-credentials-screens
#   body_sha256: 324f2448569550bf8dd27fd1891118a39425f68dabe1c07bc25dd61beb71fbd7
# ---

Feature: Configure file transfer source locations and destinations
  Staff can review and maintain where files are picked up from and where they are delivered.
  They can add new setups, change existing ones, stop using a setup, and find an existing setup when they need it.

  @rule:F01-001 @rule:F01-002 @rule:F01-003 @rule:F01-004 @rule:F01-005 @rule:F01-006 @rule:F01-007
  @rule:F01-009 @rule:F01-010 @rule:F01-011 @rule:F01-015 @rule:F01-016 @rule:F01-017 @rule:F01-018 @rule:F01-019 @rule:F01-020
  @keep
  Scenario: A staff member opens the setup for an existing pickup location
    Given a staff member is signed in
    And there is a pickup location called "Daily Claims" at the pickup address "Claims Inbox" with two delivery destinations
    When the staff member opens the setup for that pickup location
    Then they see the saved pickup location details
    And the delivery destinations are shown from newest to oldest
    And the setup shows whether it is active or inactive
    And the schedule is shown as weekly or monthly based on the saved day selections
    And opening the destination edit screen shows the destination details as the staff member provided them

  @rule:F01-008 @rule:F01-012 @rule:F01-013 @rule:F01-014
  @keep
  Scenario: A staff member opens the setup for a pickup location that cannot be loaded
    Given a staff member is signed in
    And there is no pickup location for the reference they entered
    When the staff member opens the setup for that reference
    Then they see a setup screen that is not filled with saved details

  @rule:F01-021 @rule:F01-022
  @keep
  Scenario: A staff member starts adding a new pickup location
    Given a staff member is signed in
    When the staff member starts adding a new pickup location
    Then the setup starts out as active

  @rule:F01-023 @rule:F01-024 @rule:F01-025 @rule:F01-026 @rule:F01-027 @rule:F01-028 @rule:F01-029
  @rule:F01-030 @rule:F01-031 @rule:F01-032 @rule:F01-033 @rule:F01-034 @rule:F01-035 @rule:F01-036 @rule:F01-037
  @rule:F01-046 @rule:F01-047 @rule:F01-048 @rule:F01-049 @rule:F01-050 @rule:F01-051 @rule:F01-052 @rule:F01-053 @rule:F01-054
  @keep
  Scenario: A staff member saves a new pickup location with delivery destinations
    Given a staff member is signed in
    And the staff member enters a new pickup location called "Daily Claims" at the pickup address "Claims Inbox"
    And the staff member enters an owner contact phone number "555-010-1234"
    And the staff member adds at least one delivery destination with a contact phone number "555-010-2222"
    And the staff member selects at least one day the setup should run
    When the staff member saves the setup
    Then the setup is saved
    And the saved owner phone number does not include hyphens
    And the saved destination contact phone number does not include hyphens
    And the setup receives a new configuration number

  @rule:F01-038 @rule:F01-039 @rule:F01-040 @rule:F01-041 @rule:F01-042 @rule:F01-043 @rule:F01-044 @rule:F01-045
  @keep
  Scenario Outline: Saving a pickup location is rejected when required details are missing or not allowed
    Given a staff member is signed in
    And the staff member is entering a pickup location
    When the staff member tries to save with <problem>
    Then the setup is not saved
    And the staff member is told what needs to be corrected

    Examples:
      | problem |
      | missing required details |
      | no delivery destination marked as usable |
      | no run schedule selection |
      | a file mask ending in an excluded extension |
      | a server-based transfer method with no server chosen |

  @rule:F01-055 @rule:F01-056
  @rule:F01-059 @rule:F01-060 @rule:F01-061 @rule:F01-062 @rule:F01-063 @rule:F01-064 @rule:F01-065
  @rule:F01-069 @rule:F01-071
  @rule:F01-073 @rule:F01-074 @rule:F01-075
  @keep
  Scenario: A staff member updates an existing pickup location
    Given a staff member is signed in
    And there is an active pickup location called "Daily Claims" at the pickup address "Claims Inbox" with one delivery destination
    When the staff member changes the pickup location details and saves
    Then the changed details are saved
    And any delivery destination changes are applied

  @rule:F01-107
  @keep
  Scenario Outline: A pickup location change is not applied when its reference cannot be read
    Given a staff member is signed in
    And there is an active pickup location called Daily Claims at the pickup address Claims Inbox with one delivery destination
    When another internal system tries to change that pickup location using <reference>
    Then the pickup location is not changed
    And the change is not accepted

    Examples:
      | reference |
      | a missing pickup location reference |
      | a pickup location reference that is not recognized |

  @rule:F01-057 @rule:F01-058
  @rule:F01-066 @rule:F01-067 @rule:F01-068
  @rule:F01-070 @rule:F01-072
  @rule:F01-076 @rule:F01-077 @rule:F01-078
  @keep
  Scenario: A staff member stops using a pickup location
    Given a staff member is signed in
    And there is an active pickup location called "Daily Claims" at the pickup address "Claims Inbox" with two delivery destinations
    When the staff member stops using that pickup location
    Then the pickup location becomes inactive
    And the delivery destinations for that pickup location become inactive

  @rule:F01-108
  @keep
  Scenario Outline: A pickup location cannot be stopped when its reference cannot be read
    Given a staff member is signed in
    And there is an active pickup location called Daily Claims at the pickup address Claims Inbox with two delivery destinations
    When another internal system tries to stop using that pickup location using <reference>
    Then the pickup location stays active
    And the delivery destinations for that pickup location stay active
    And the change is not accepted

    Examples:
      | reference |
      | a missing pickup location reference |
      | a pickup location reference that is not recognized |

  @rule:F01-083 @rule:F01-084 @rule:F01-085 @rule:F01-086 @rule:F01-087 @rule:F01-088 @rule:F01-089 @rule:F01-090
  @keep
  Scenario: A staff member selects a pickup location from a list
    Given a staff member is signed in
    And there are two active pickup locations called "Daily Claims" at the pickup address "Claims Inbox" and "Monthly Reports" at the pickup address "Reports Inbox"
    When the staff member opens the pickup location list
    Then the list shows each entry as "name - address"
    And the entries are ordered by name from A to Z

  @rule:F01-079 @rule:F01-080 @rule:F01-081 @rule:F01-082
  @rule:F01-091 @rule:F01-092 @rule:F01-093 @rule:F01-094 @rule:F01-095 @rule:F01-096 @rule:F01-097 @rule:F01-098 @rule:F01-099
  @keep
  Scenario Outline: A staff member finds pickup locations using exact-match filters
    Given a staff member is signed in
    And there are several pickup locations already set up
    When the staff member looks for pickup locations using <filter>
    Then only pickup locations that exactly match the filter are shown

    Examples:
      | filter |
      | the configuration number |
      | the pickup location name |
      | the owner name |
      | the owning business area |
      | the transfer method |
      | whether it is active |

  @rule:F01-100 @rule:F01-101 @rule:F01-102 @rule:F01-103 @rule:F01-104 @rule:F01-105 @rule:F01-106
  @keep
  Scenario: A staff member opens one pickup location and sees its delivery destinations
    Given a staff member is signed in
    And there is a pickup location called "Daily Claims" at the pickup address "Claims Inbox" with two delivery destinations
    When the staff member opens the details for that pickup location
    Then they see the pickup location details
    And they see the delivery destinations from newest to oldest

