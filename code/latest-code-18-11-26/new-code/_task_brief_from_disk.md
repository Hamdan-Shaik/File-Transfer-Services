# Brief: port part 1 of 1 of component `shared-queries-parameters-2`

- Deployable: `fxfer-domain-api` at `services/fxfer-domain-api`; build: `dotnet build services/fxfer-domain-api`; tests: `dotnet test services/fxfer-domain-api`
- The component's folder (architecture.yaml): `services/fxfer-domain-api/src/Contracts/Queries`
- The component's spec: `technical/components/shared-queries-parameters-2/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 4 legacy methods or classes, 77 legacy lines
- Playbook: `inputs/playbook/port.md`

## What to port

### `GroupInfo`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Domain\GroupInfo.cs`)

#### `GroupInfo` (lines 11-36) â†’ `GroupInfo`

```csharp
    [DataContract]
    public class GroupInfo
    {
        #region Properties
        /// <summary>
        /// <para>System generated unique identifier for the group record</para>
        /// <list type="bullet">
        /// <item><term>Format</term><description>Guid</description></item>
        /// <item><term>MaxLength</term><description>36</description></item>
        /// <item><term> Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public Guid? GroupId { get; set; }

        /// <summary>
        /// <para>Number of Outbound Records in Extract File</para>
        /// <list type="bullet">
        /// <item><term>Format</term><description>int</description></item>
        /// <item><term>MaxLength</term><description>-2,147,483,648 to 2,147,483,647</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public int NumberOfRecords { get; set; }
        #endregion
    }
```

### `GetIntegrationFileQuery`: role **application**, target file(s) `GetIntegrationFileQuery.cs` (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\GetIntegrationFileQuery.cs`)

#### `GetIntegrationFileQuery.HandleErrors()` (lines 56-65) â†’ `drop: legacy no-op scaffolding`

```csharp
        private void HandleErrors()
        {
            if (this.messages.Count > 0)
            {
                throw new BusinessException(
                                    this.messages,
                                    CommonConstants.Validation.ErrorType.InvalidParameters,
                                    CommonConstants.Validation.ProcessingSystem);
            }
        }
```

#### `GetIntegrationFileQuery.Validate()` (lines 48-52) â†’ `GetIntegrationFileQuery.Validate()`

```csharp
        public void Validate()
        {
            this.messages = new List<BusinessExceptionMessage>();
            this.HandleErrors();
        }
```

### `IntegrationFileQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\IntegrationFileQueryParms.cs`)

#### `IntegrationFileQueryParms` (lines 14-49) â†’ `IntegrationFileQueryParms`

```csharp
    [DataContract]
    public class IntegrationFileQueryParms
    {
        #region Constructor
        public IntegrationFileQueryParms()
        {
            this.FTSIntegrationFileId = Guid.Empty;
        }
        #endregion

        #region Properties

        /// <summary>
        /// <para>key that identify an Integration File record</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>Guid</description></item>  
        /// <item><term>MaxLength</term><description>36</description></item>
        /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public Guid FTSIntegrationFileId { get; set; }

        /// <summary>
        /// <para>File Name</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>AlphaNumeric</description></item>  
        /// <item><term>MaxLength</term><description>200</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
        #endregion
    }
```
