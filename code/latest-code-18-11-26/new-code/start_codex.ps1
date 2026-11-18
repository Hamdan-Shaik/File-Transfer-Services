param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$CodexArgs
)

# Always start from this project's folder
Set-Location $PSScriptRoot
 
# Load environment variables from .env
$envFile = Join-Path $PSScriptRoot ".env"
 
if (!(Test-Path $envFile)) {
    throw ".env file not found: $envFile"
}
 
Get-Content $envFile | ForEach-Object {
    if ($_ -match '^\s*([^#\s][^=]*?)=(.*)$') {
        $key = $matches[1].Trim()
        $value = $matches[2].Trim().Trim('"').Trim("'")
        Set-Item "Env:$key" $value
    }
}
 
# Validate required settings
if (!$env:LLM_MODEL -or !$env:OPENAI_BASE_URL) {
    throw "LLM_MODEL or OPENAI_BASE_URL is missing in .env"
}
 
# Start Codex
$baseArgs = @(
    "--model", "$env:LLM_MODEL",
    "-c", "openai_base_url=`"$env:OPENAI_BASE_URL`"",
    "--dangerously-bypass-approvals-and-sandbox"
)

if ($CodexArgs -and $CodexArgs.Count -gt 0) {
    codex @baseArgs @CodexArgs
}
else {
    codex @baseArgs
}
