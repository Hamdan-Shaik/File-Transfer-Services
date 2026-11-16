- Updated xfer-service POST /api/v1/file-transfers/search request model to match the flow OpenAPI (page/pageSize/orderBy/orderDir/filters).
- Updated xfer-bff /api/v1/file-transfers/search to pass paging/sort/filter details through to xfer-service.
- Added xfer-bff GET /api/v1/modules/selection backed by ReferenceData:Modules configuration.
