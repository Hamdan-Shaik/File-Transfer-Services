# Task `port:shared-contracts-exceptions/1` (port) keeps failing

- The task's brief, with the legacy source of its methods: `technical/target/tasks/port-shared-contracts-exceptions-1.md`
- The component's spec: `technical/components/shared-contracts-exceptions/`
- Deployable: `fxfer-domain-api`
- The target-state document: `inputs/target-state.md`

## The problems the checks report

- fxfer-domain-api: `dotnet build services/fxfer-domain-api` failed (exit 1):
est\Fxfer.DomainApi.Tests\Contracts\Exceptions\FileTransferExceptionConstantsRuleTests.cs(45,38): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\FileTransferExceptionConstantsRuleTests.cs(46,73): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(123,32): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(124,40): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(125,30): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(127,22): error CS0246: The type or namespace name 'FileTransferValidationException' could not be found (are you missing a using directive or an assembly reference?) [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(137,32): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(138,40): error CS0103: The name 'FileTransferExceptionConstants' does not exist in the current context [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Contracts\Exceptions\SharedContractsExceptionsRuleTests.cs(140,23): error CS0246: The type or namespace name 'FileTransfertBASValidationException' could not be found (are you missing a using directive or an assembly reference?) [C:\Users\gt154165\work-r30.2609\provider-file-transfer-v2\new-code\services\fxfer-domain-api\test\Fxfer.DomainApi.Tests\Fxfer.DomainApi.Tests.csproj]
    134 Warning(s)
    61 Error(s)

Time Elapsed 00:00:11.12


## What earlier tasks in this run came to

- `port:backend-business-aggregates-2/1`: plan. The task brief `technical/target/tasks/port-backend-business-aggregates-2-1.md` requires `CommandDispatcher.cs` and `QueryDispatcher.cs`, but the component placement spec `technical/components/backend-business-aggregates-2/TARGET.md` explicitly says not to create these legacy empty subclasses.

