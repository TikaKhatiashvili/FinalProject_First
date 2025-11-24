using ATMProject.Business.IServices;
using ATMProject.Data;
using ATMProject.Models;

namespace ATMProject.Business.ServicesImplementations;
public class TransactionService : ITransactionService
{
    private readonly IUserRepository _userRepo;
    private readonly ILogRepository _logRepo;

    public TransactionService(IUserRepository userRepo, ILogRepository logRepo)
    {
        _userRepo = userRepo;
        _logRepo = logRepo;
    }

    public async Task<decimal> CheckBalanceAsync(User user)
    {
        var balance = user.Balance;

        await _logRepo.AddLogAsync(new TransactionLog
        {
            Date = DateTime.Now,
            UserFullName = $"{user.FirstName} {user.LastName}",
            Action = "Check the balance.",
            BalanceAfter = balance
        });

        return balance;
    }

    public async Task DepositAsync(User user, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("The amount must be positive.");

        user.Balance += amount;
        await _userRepo.UpdateAsync(user);

        await _logRepo.AddLogAsync(new TransactionLog
        {
            Date = DateTime.Now,
            UserFullName = $"{user.FirstName} {user.LastName}",
            Action = $"Deposited  {amount} GEL",
            Amount = amount,
            BalanceAfter = user.Balance
        });
    }

    public async Task WithdrawAsync(User user, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("The amount must be positive..");
        if (user.Balance < amount) throw new InvalidOperationException("The balance is insufficient.");

        user.Balance -= amount;
        await _userRepo.UpdateAsync(user);

        await _logRepo.AddLogAsync(new TransactionLog
        {
            Date = DateTime.Now,
            UserFullName = $"{user.FirstName} {user.LastName}",
            Action = $"Withdrew  {amount} Gel",
            Amount = amount,
            BalanceAfter = user.Balance
        });
    }

    public Task<IEnumerable<TransactionLog>> GetTransactionHistoryAsync()
        => _logRepo.GetAllLogsAsync();
}