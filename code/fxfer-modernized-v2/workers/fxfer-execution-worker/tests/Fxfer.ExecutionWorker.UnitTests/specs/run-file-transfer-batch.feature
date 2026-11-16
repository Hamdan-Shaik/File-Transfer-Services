# ---
# type: Acceptance Scenarios
# title: Run file transfer batch processing
# description: Operations staff run scheduled delivery processing for configured collection locations.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess
# tags:
# - batch
# - flow
# - providerfiletransfer
# - scenarios
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-06T08:21:36Z
# sources:
# - id: rules
#   resource: /technical/flows/run-file-transfer-batch/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-main-batch-process
#   resource: components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main/BatchProcess.cs#L17
#   title: BatchProcess (batch door)
# - id: door-main-batch-process-2
#   resource: components/batch/FileTransferManagement/HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main/BatchProcessProgram.cs#L21
#   title: BatchProcess (batch door)
# - id: footprint
#   resource: /technical/flows/run-file-transfer-batch/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: ProviderFileTransfer
#   item: flow:run-file-transfer-batch
#   generation:
#     status: done
#     attempts: 2
#   rule_prefix: F03
#   rules: 30
#   doors:
#   - main-batch-process
#   - main-batch-process-2
#   tables: 0
#   procedures: 0
#   components:
#   - backend-run-file-transfer-batch-api
#   body_sha256: 11d1c42c63c8e8ea8e9ffa506cc61565bd2193b6cb2899d819a85d5a5bfb2ec6
# ---

Feature: Run file transfer batch processing
  Operations staff run scheduled delivery processing for configured collection locations.
  They get a result showing whether the run completed, and support records when deliveries fail.

  @rule:F03-001 @rule:F03-012 @keep
  Scenario: A standard scheduled run starts batch delivery processing
    Given operations schedule a delivery run with some run information
    When the standard scheduled run starts
    Then delivery processing is carried out using that run information
    And operations are told the run completed

  @rule:F03-002 @keep
  Scenario: A standard scheduled run reports failure when something goes wrong early
    Given operations schedule a delivery run with some run information
    When the run hits an unexpected problem before it can complete
    Then the run is reported as failed
    And a support record is created describing the problem

  @rule:F03-021 @keep
  Scenario: The standard scheduled run stops when a required setting is missing
    Given at least one required setting is missing for delivery processing
    When the standard scheduled run starts
    Then the run is reported as failed
    And support staff are told which setting is missing

  @rule:F03-003 @keep
  Scenario: The management scheduled run records a start and end record
    Given operations schedule a delivery run for a tenant
    When the management scheduled run starts
    Then a support record shows the run has started
    And a support record shows the run has finished

  @rule:F03-004 @keep
  Scenario Outline: The management scheduled run refuses to start when given the wrong amount of run information
    Given operations provide <count> pieces of run information
    When the management scheduled run starts
    Then the run does not start
    And support staff are given guidance on how to start it

    Examples:
      | count |
      | 0 |
      | 2 |
      | 4 |

  @rule:F03-005 @keep
  Scenario: The management scheduled run starts for a tenant when given only a tenant identifier
    Given operations provide a tenant identifier as the only run information
    When the management scheduled run starts
    Then delivery processing is carried out for that tenant
    And operations are told the run completed

  @rule:F03-005 @keep
  Scenario: The management scheduled run refuses to start when the tenant identifier is not in the required format
    Given operations provide tenant identifier NOT-A-TENANT as the only run information
    When the management scheduled run starts
    Then the run does not start
    And support staff are given guidance on how to start it

  @rule:F03-006 @keep
  Scenario: The management scheduled run can be started with a selection for a chosen subset
    Given operations provide a selection choice
    And operations provide selection value 42
    And operations provide a tenant identifier in the required format
    When the management scheduled run starts
    Then delivery processing is carried out using those three pieces of run information
    And operations are told the run completed

  @rule:F03-006 @keep
  Scenario Outline: The management scheduled run refuses to start when selection values are not well formed
    Given operations provide a selection choice
    And operations provide selection value <value>
    And operations provide tenant identifier <tenant>
    When the management scheduled run starts
    Then the run does not start
    And support staff are given guidance on how to start it

    Examples:
      | value | tenant |
      | not-a-number | a tenant identifier in the required format |
      | 42 | NOT-A-TENANT |

  @rule:F03-007 @rule:F03-014 @flag
  Scenario: An unexpected selection choice is not rejected and can trigger a full run
    Given operations provide a selection choice that is not recognised
    And operations provide selection value 42
    And operations provide a tenant identifier in the required format
    When the management scheduled run starts
    Then the run still starts
    And delivery processing is carried out for all locations for that tenant

  @rule:F03-022 @keep
  Scenario: A re-run selection treats a tracking reference as zero in the standard run
    Given operations request a re-run for the oldest failed deliveries
    And they provide a tracking reference instead of a number of deliveries
    When the standard scheduled run starts
    Then the run still switches into a re-run mode
    And the number of deliveries selected is treated as 0

  @rule:F03-023 @keep
  Scenario: A re-run selection treats a tracking reference as zero in the management run
    Given operations schedule the management scheduled run for a tenant
    And they request a re-run for the oldest failed deliveries
    And they provide a tracking reference instead of a number of deliveries
    When the management scheduled run starts
    Then the run still switches into a re-run mode for that tenant
    And the number of deliveries selected is treated as 0

  @rule:F03-008 @keep
  Scenario: The management scheduled run uses the tenant's saved settings when starting
    Given a tenant has delivery settings saved for them
    And no settings have been loaded yet for this run
    When operations schedule the management scheduled run for that tenant
    Then the run uses that tenant's saved settings

  @rule:F03-009 @rule:F03-011 @keep
  Scenario: The management scheduled run stops when a required setting is missing
    Given a tenant is missing at least one required setting for delivery processing
    When operations schedule the management scheduled run for that tenant
    Then the run is reported as failed
    And support staff are told which required setting is missing

  @rule:F03-010 @keep
  Scenario: The management scheduled run starts delivery processing after checking required settings
    Given operations schedule the management scheduled run for a tenant
    And all required settings are present for that tenant
    When the management scheduled run starts
    Then delivery processing is carried out
    And operations are told the run completed

  @rule:F03-013 @keep
  Scenario Outline: The standard scheduled run refuses malformed run information
    Given operations schedule a delivery run with <count> pieces of run information
    When the standard scheduled run starts
    Then the run is reported as failed
    And support staff are given guidance on how to start it

    Examples:
      | count |
      | 1 |
      | 3 |

  @rule:F03-015 @keep
  Scenario: Only eligible and scheduled locations are processed in an all-locations run
    Given some locations are active and scheduled to run
    And some locations are inactive, suspended, or not scheduled to run
    When operations run delivery processing for all locations
    Then only the active and scheduled locations are processed

  @rule:F03-016 @keep
  Scenario: Duplicate-marker files and excluded extensions can be skipped during scanning
    Given a location contains files including duplicate-marker files
    And the management batch has some excluded file extensions
    When delivery processing scans the location
    Then duplicate-marker files are ignored
    And files with excluded extensions are ignored in the management batch

  @rule:F03-024 @keep
  Scenario: A location is suspended when it cannot be scanned in the standard run
    Given delivery processing is scanning locations
    And one location cannot be scanned
    And notifications are enabled
    When the standard scheduled run continues
    Then the location is suspended
    And a notice is sent to the location owner about the suspension

  @rule:F03-025 @keep
  Scenario: A location is suspended when it cannot be scanned in the management run
    Given delivery processing is scanning locations for a tenant
    And one location cannot be scanned
    And notifications are enabled
    When the management scheduled run continues
    Then the location is suspended
    And a notice is sent to the location owner about the suspension for that tenant

  @rule:F03-026 @rule:F03-027 @keep
  Scenario: A new file can carry forward a previous tracking number from its label
    Given a location was previously configured under a different setup
    And a new file begins with tracking number 123
    When delivery processing queues that file for delivery
    Then the file is queued with tracking number 123
    And the management run also records the file size and file kind and groups the file for delivery

  @rule:F03-017 @keep
  Scenario Outline: A previously failed tracked file is only retried after the configured wait time
    Given a tracked delivery failed <age> minutes ago
    When delivery processing scans the source location
    Then the file is <result>

    Examples:
      | age | result |
      | 1 | not retried |
      | 999 | retried |

  @rule:F03-018 @keep
  Scenario: A re-run of failed deliveries can be limited to a specific subset
    Given some previously failed deliveries exist
    When operations start a re-run using a specific identifier, a specific location identifier, or a top count
    Then only the matching previously failed deliveries are selected for re-run

  @rule:F03-019 @flag
  Scenario: The run can be configured with a worker count that results in no work being processed
    Given the worker count setting is missing or not a whole number
    When operations start delivery processing
    Then the run can complete without processing any queued work

  @rule:F03-020 @keep
  Scenario: Delivering a file records delivery history and completes the tracked item
    Given a file is selected for delivery from a collection location
    And that location has at least one destination
    When delivery processing delivers the file
    Then the system records delivery history for each destination
    And the file is removed from the collection location
    And the tracked item is marked as completed

  @rule:F03-020 @rule:F03-028 @rule:F03-029 @keep
  Scenario: A duplicate delivery can be handled by renaming the source file when that option is enabled
    Given a selected file cannot be delivered because the destination already has a file with that name
    And duplicate-rename handling is enabled
    When delivery processing attempts the delivery
    Then the source file is renamed with the configured duplicate marker
    And the tracked item is treated as completed
    And a support record is created describing the duplicate

  @rule:F03-028 @rule:F03-029 @keep
  Scenario: A duplicate delivery is treated as an error when renaming is not enabled
    Given a selected file cannot be delivered because the destination already has a file with that name
    And duplicate-rename handling is not enabled
    When delivery processing attempts the delivery
    Then the tracked item is marked as an error
    And a support record is created describing the duplicate

  @rule:F03-030 @keep
  Scenario: The original file is only removed when every destination receives it in the management run
    Given a file is selected for delivery from a collection location in the management scheduled run
    And that location has more than one destination
    And at least one destination does not receive the file successfully
    When delivery processing completes for that file
    Then the original file is not removed from the collection location
