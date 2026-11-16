# ---
# type: Acceptance Scenarios
# title: Run file transfer batch processing
# description: Operations staff run a batch that moves files from agreed source locations to agreed destination locations.
# resource: urn:csharp-doc-agent:HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.BatchProcess
# tags:
# - batch
# - flow
# - provider-file-transfer-v2
# - scenarios
# status: draft
# generated:
#   by: csharp-doc-agent/0.1.0
#   at: 2026-11-15T07:15:42Z
# sources:
# - id: rules
#   resource: /technical/flows/run-file-transfer-batch/RULES.md
#   title: The rules these scenarios are tagged with
# - id: door-main-batch-process
#   resource: bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main/BatchProcess.cs#L17
#   title: BatchProcess (batch door)
# - id: door-main-batch-process-2
#   resource: batch/FileTransferManagement/HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Main/BatchProcessProgram.cs#L21
#   title: BatchProcess (batch door)
# - id: footprint
#   resource: /technical/flows/run-file-transfer-batch/FOOTPRINT.yaml
#   title: Flow footprint (every method, table and procedure)
# cef:
#   pack: provider-file-transfer-v2
#   item: flow:run-file-transfer-batch
#   generation:
#     status: done
#     attempts: 2
#   rule_prefix: F04
#   rules: 14
#   doors:
#   - main-batch-process
#   - main-batch-process-2
#   tables: 0
#   procedures: 0
#   components:
#   - backend-run-file-transfer-batch-api
#   body_sha256: f2efd259e7ec7e9cfce1469ba4515434d6fd7b6befa61a61ecf09e4ed10b45dc
# ---

Feature: Run file transfer batch processing
  Operations staff run a batch that moves files from agreed source locations to agreed destination locations.
  They can run it for one tenant or as a selective run, and they get back one number to record the result.

  @rule:F04-001 @keep
  Scenario: A basic batch run starts the file transfer work
    Given operations start a file transfer batch run in the basic way
    When the run begins
    Then the file transfer work starts

  @rule:F04-002 @keep
  Scenario: A basic batch run reports failure as -1 when it cannot start
    Given operations start a file transfer batch run in the basic way
    When the run cannot start because of an unexpected problem
    Then they are told the run finished with -1

  @rule:F04-003 @rule:F04-014 @keep
  Scenario: The managed run always records an end record
    Given operations start a file transfer batch run in the managed way
    When the run fails unexpectedly
    Then an end record is still recorded for the support team
    And they are told the run finished with -1

  @rule:F04-004 @keep
  Scenario Outline: The managed run rejects start-up values that do not match an accepted pattern
    Given operations start a file transfer batch run in the managed way
    When they start the run with <count> start-up values
    Then the run is rejected
    And they are shown instructions on how to start the run
    And they are told the run finished with -1

    Examples:
      | count |
      | 0 |
      | 2 |
      | 4 |

  @rule:F04-005 @rule:F04-006 @keep
  Scenario Outline: A one-value managed run processes all files for one tenant
    Given operations start a file transfer batch run in the managed way
    When they start the run for <tenant>
    Then <outcome>

    Examples:
      | tenant | outcome |
      | a tenant reference in the standard tenant reference style | the run is accepted and continues |
      | a tenant reference not in the standard tenant reference style | the run is rejected, they are shown instructions, and they are told the run finished with -1 |

  @rule:F04-007 @rule:F04-008 @rule:F04-009 @keep
  Scenario Outline: A three-value managed run checks the chosen value and the tenant reference
    Given operations start a file transfer batch run in the managed way
    When they start the run with the label <label>, the chosen value <chosen>, and the tenant reference <tenant>
    Then <outcome>

    Examples:
      | label | chosen | tenant | outcome |
      | any label | 10 | a tenant reference in the standard tenant reference style | the run is accepted and continues |
      | any label | a long reference in the standard tenant reference style | a tenant reference in the standard tenant reference style | the run is accepted and continues |
      | any label | not a number and not a long reference | a tenant reference in the standard tenant reference style | the run is rejected, they are shown instructions, and they are told the run finished with -1 |
      | any label | 10 | a tenant reference not in the standard tenant reference style | the run is rejected, they are shown instructions, and they are told the run finished with -1 |

  @rule:F04-010 @flag
  Scenario: A three-value managed run does not reject an unrecognised label
    Given operations start a file transfer batch run in the managed way
    When they start the run with the label made-up-option, the chosen value 10, and a tenant reference in the standard tenant reference style
    Then the run is accepted and continues

  @rule:F04-011 @keep
  Scenario: The managed run makes tenant settings available when they are not already available
    Given operations start a file transfer batch run in the managed way
    And the tenant settings are not already available
    When they start the run for a tenant reference in the standard tenant reference style
    Then the tenant settings are made available for that tenant before the work begins

  @rule:F04-012 @keep
  Scenario Outline: The managed run stops when a required run setting is missing
    Given operations start a file transfer batch run in the managed way
    And a required run setting is missing
    And the missing setting affects <missing area>
    When they start the run for a tenant reference in the standard tenant reference style
    Then they are told the run cannot proceed because a required setting is missing
    And they are told the run finished with -1

    Examples:
      | missing area |
      | where files are picked up |
      | where working files are placed |

  @rule:F04-013 @keep
  Scenario: The managed run starts the file transfer work after checks pass
    Given operations start a file transfer batch run in the managed way
    And the start-up values are accepted
    And required run settings are present
    When the run begins
    Then the file transfer work starts
