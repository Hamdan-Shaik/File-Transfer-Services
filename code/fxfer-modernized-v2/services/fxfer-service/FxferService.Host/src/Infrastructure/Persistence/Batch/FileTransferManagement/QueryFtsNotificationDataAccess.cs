using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

using FxferService.Application.Persistence.Batch;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class QueryFtsNotificationDataAccess : DataAccessBase<List<FtsNotificationModel>>
{
    private readonly GetNotificationSearchCriteria _query;

    public QueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery)
    {
        _query = searchQuery;
    }

    protected override List<FtsNotificationModel> ExecuteQuery(IDbContextBase dbcontext)
    {
        List<FtsNotificationModel>? notificationResult;
        if (_query.NotifyOnErrorInd == true && _query.NotifyOnSuccessInd == true)
        {
            notificationResult = NotifyErrorSuccess(dbcontext);
        }
        else if (_query.NotifyOnErrorInd == true)
        {
            notificationResult = NotifyErrorInd(dbcontext);
        }
        else
        {
            notificationResult = NotifyOnSuccessInd(dbcontext);
        }

        return notificationResult;
    }

    private List<FtsNotificationModel> NotifyErrorSuccess(IDbContextBase dbcontext)
    {
        var notificationResult = (from notify in dbcontext.Set<FtsNotificationEntity>()
                                  where notify.NotifyOnError == true && notify.NotifyOnSuccess == true
                                        && notify.FtsSourceLocationId == _query.ConfigId
                                  select new FtsNotificationModel
                                  {
                                      ContactName = notify.ContactName,
                                      EmailAddress = notify.EmailAddress,
                                      NotifyOnErrorInd = notify.NotifyOnError,
                                      NotifyOnSuccessInd = notify.NotifyOnSuccess,
                                      ConfigId = notify.FtsSourceLocationId,
                                  }).ToList();
        return notificationResult;
    }

    private List<FtsNotificationModel> NotifyOnSuccessInd(IDbContextBase dbcontext)
    {
        var notificationResult = (from notify in dbcontext.Set<FtsNotificationEntity>()
                                  where notify.NotifyOnSuccess == true
                                        && notify.FtsSourceLocationId == _query.ConfigId
                                  select new FtsNotificationModel
                                  {
                                      ContactName = notify.ContactName,
                                      EmailAddress = notify.EmailAddress,
                                      NotifyOnErrorInd = notify.NotifyOnError,
                                      NotifyOnSuccessInd = notify.NotifyOnSuccess,
                                      ConfigId = notify.FtsSourceLocationId,
                                  }).ToList();
        return notificationResult;
    }

    private List<FtsNotificationModel> NotifyErrorInd(IDbContextBase dbcontext)
    {
        var notificationResult = (from notify in dbcontext.Set<FtsNotificationEntity>()
                                  where notify.NotifyOnError == true
                                        && notify.FtsSourceLocationId == _query.ConfigId
                                  select new FtsNotificationModel
                                  {
                                      ContactName = notify.ContactName,
                                      EmailAddress = notify.EmailAddress,
                                      NotifyOnErrorInd = notify.NotifyOnError,
                                      NotifyOnSuccessInd = notify.NotifyOnSuccess,
                                      ConfigId = notify.FtsSourceLocationId,
                                  }).ToList();
        return notificationResult;
    }
}
