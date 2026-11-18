using System.Net;
using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class QueryFtsNotificationDataAccess : DataAccessBase<List<FtsNotificationModel>>
{
    private readonly GetNotificationSearchCriteria _searchQuery;

    public QueryFtsNotificationDataAccess(IFxferDomainApiClient domainApiClient, IRequestContextAccessor requestContextAccessor, GetNotificationSearchCriteria searchQuery)
        : base(domainApiClient, requestContextAccessor)
    {
        _searchQuery = searchQuery;
    }

    protected override async Task<List<FtsNotificationModel>> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/fxfer-domain/v1/transfer-configurations/{_searchQuery.ConfigId}/notifications");
        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return [];
        }

        response.EnsureSuccessStatusCode();

        var notifications = await response.Content.ReadFromJsonAsync<List<FtsNotificationModel>>(cancellationToken) ?? [];

        List<FtsNotificationModel> notificationResult;
        if (_searchQuery.NotifyOnErrorInd == true && _searchQuery.NotifyOnSuccessInd == true)
        {
            notificationResult = NotifyErrorSuccess(notifications);
        }
        else if (_searchQuery.NotifyOnErrorInd == true)
        {
            notificationResult = NotifyErrorInd(notifications);
        }
        else
        {
            notificationResult = NotifyOnSuccessInd(notifications);
        }

        return notificationResult;
    }

    private List<FtsNotificationModel> NotifyErrorSuccess(List<FtsNotificationModel> notifications)
    {
        return notifications
            .Where(x => x.NotifyOnErrorInd == true && x.NotifyOnSuccessInd == true)
            .Select(x => new FtsNotificationModel
            {
                ContactName = x.ContactName,
                EmailAddress = x.EmailAddress,
                NotifyOnErrorInd = x.NotifyOnErrorInd,
                NotifyOnSuccessInd = x.NotifyOnSuccessInd,
                ConfigId = _searchQuery.ConfigId,
            })
            .ToList();
    }

    private List<FtsNotificationModel> NotifyOnSuccessInd(List<FtsNotificationModel> notifications)
    {
        return notifications
            .Where(x => x.NotifyOnSuccessInd == true)
            .Select(x => new FtsNotificationModel
            {
                ContactName = x.ContactName,
                EmailAddress = x.EmailAddress,
                NotifyOnErrorInd = x.NotifyOnErrorInd,
                NotifyOnSuccessInd = x.NotifyOnSuccessInd,
                ConfigId = _searchQuery.ConfigId,
            })
            .ToList();
    }

    private List<FtsNotificationModel> NotifyErrorInd(List<FtsNotificationModel> notifications)
    {
        return notifications
            .Where(x => x.NotifyOnErrorInd == true)
            .Select(x => new FtsNotificationModel
            {
                ContactName = x.ContactName,
                EmailAddress = x.EmailAddress,
                NotifyOnErrorInd = x.NotifyOnErrorInd,
                NotifyOnSuccessInd = x.NotifyOnSuccessInd,
                ConfigId = _searchQuery.ConfigId,
            })
            .ToList();
    }
}
