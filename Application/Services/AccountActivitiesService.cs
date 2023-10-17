using Application.Interfaces;
using DataAccess.Abstract;
using DataAccess.Repository;
using Domain.Entity;

namespace Application.Services;

public class AccountActivitiesService : IAccountActivitiesService
{
    private readonly IAccountActivitiesRepository _accountActivitiesRepository;

    public AccountActivitiesService(IAccountActivitiesRepository accountActivitiesRepository)
    {
        _accountActivitiesRepository = accountActivitiesRepository;
    }

    public async Task<List<AccountActivities>> GetAccountActivitiesAsync(string accountNumber)
    {
        return await _accountActivitiesRepository.GetAccountActivitiesAsync(accountNumber);
    }

    public Task<List<AccountActivities>> GetLastThreeActivitiesAsync(string accountNumber)
    {
        return _accountActivitiesRepository.GetLastThreeActivitiesAsync(accountNumber);
    }
}