$ErrorActionPreference = 'Stop'

# T-009: Ensure migration discipline guardrails stay in place.

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..\..')

$flywayConfig = Join-Path $repoRoot 'schema/fxfer-db/flyway.conf'
$baselineSql = Join-Path $repoRoot 'schema/fxfer-db/sql/V1__baseline.sql'

$requiredFiles = @(
  'schema/fxfer-db/MIGRATIONS.md'
  'schema/fxfer-db/sql/V3__migration_observability.sql'
  'schema/fxfer-db/sql/beforeMigrate__t009_audit_start.sql'
  'schema/fxfer-db/sql/afterMigrate__t009_audit_success.sql'
  'schema/fxfer-db/sql/afterMigrateError__t009_audit_failure.sql'
)

foreach ($relativePath in $requiredFiles) {
  $path = Join-Path $repoRoot $relativePath
  if (-not (Test-Path -LiteralPath $path)) {
    throw ('Missing required T-009 artifact: ' + $relativePath)
  }
}

$flywayConfText = Get-Content -Raw -LiteralPath $flywayConfig
if ($flywayConfText -notmatch '(?m)^flyway\.placeholders\.tenant_id=') {
  throw 'flyway.conf must define flyway.placeholders.tenant_id (T-009)'
}

$baselineText = Get-Content -Raw -LiteralPath $baselineSql
$createTableCount = ([regex]::Matches($baselineText, '(?im)^CREATE\s+TABLE\s+IF\s+NOT\s+EXISTS\s+')).Count
if ($createTableCount -lt 1) {
  throw 'V1__baseline.sql must use CREATE TABLE IF NOT EXISTS for idempotency (T-009)'
}

$auditCallbackText = Get-Content -Raw -LiteralPath (Join-Path $repoRoot 'schema/fxfer-db/sql/beforeMigrate__t009_audit_start.sql')
if ($auditCallbackText -notmatch '\$\{tenant_id\}') {
  throw 'Expected callbacks to reference ${tenant_id} (T-009)'
}

# Validate naming/consistency in the default, unattended build configuration.
& flyway ('-configFiles=' + $flywayConfig) validate
if ($LASTEXITCODE -ne 0) {
  throw 'flyway validate failed'
}

Write-Host 'T-009 migration discipline checks passed.'
