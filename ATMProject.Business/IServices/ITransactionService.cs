using ATMProject.Models;

namespace ATMProject.Business.IServices;
public interface ITransactionService
{
    Task<decimal> CheckBalanceAsync(User user);
    Task DepositAsync(User user, decimal amount);
    Task WithdrawAsync(User user, decimal amount);
    Task<IEnumerable<TransactionLog>> GetTransactionHistoryAsync();
}
