namespace ATMProject.Models;

public class TransactionLog
{
    public DateTime Date { get; set; }
    public string UserFullName { get; set; } 
    public string Action { get; set; } 
    public decimal? Amount { get; set; } 
    public decimal? BalanceAfter { get; set; }
}