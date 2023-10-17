using DataAccess.Repository;
using Domain.Entity;

namespace DataAccess.Abstract;

public interface IAccountActivitiesRepository
{
    Task<List<AccountActivities>> GetAccountActivitiesAsync(string accountNumber);
    
    Task<List<AccountActivities>> GetLastThreeActivitiesAsync(string accountNumber);
}