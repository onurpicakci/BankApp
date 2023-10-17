using DataAccess.Repository;
using Domain.Entity;

namespace Application.Interfaces;

public interface IAccountActivitiesService
{
    Task<List<AccountActivities>> GetAccountActivitiesAsync(string accountNumber);
    
    Task<List<AccountActivities>> GetLastThreeActivitiesAsync(string accountNumber);
}