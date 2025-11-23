using ATMProject.Business.IServices;
using ATMProject.Business.ServicesImplementations;
using ATMProject.Data;
using ATMProject.Models;

namespace ATMProject.Presentation;

internal class Program
{
    static async Task Main()
    {
        var userRepo = await JsonUserRepository.CreateAsync("users.json");
        var logRepo = await JsonLogRepository.CreateAsync("logs.json");

        IUserService userService = new UserService(userRepo);
        ITransactionService transactionService = new TransactionService(userRepo, logRepo);

        Console.WriteLine("Welcome to the Console ATM System!");

        User? currentUser = null;

        while (true)
        {
            if (currentUser == null)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("First Name: ");
                    var firstName = Console.ReadLine() ?? "";
                    Console.Write("Last Name: ");
                    var lastName = Console.ReadLine() ?? "";
                    Console.Write("Personal ID: ");
                    var personalId = Console.ReadLine() ?? "";

                    try
                    {
                        currentUser = await userService.RegisterAsync(firstName, lastName, personalId);
                        Console.WriteLine($"User registered successfully. Your PIN: {currentUser.PIN}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Personal ID: ");
                    var personalId = Console.ReadLine() ?? "";
                    Console.Write("PIN: ");
                    var pin = Console.ReadLine() ?? "";

                    currentUser = await userService.AuthenticateAsync(personalId, pin);
                    if (currentUser != null)
                        Console.WriteLine($"Login successful. Welcome {currentUser.FirstName}!");
                    else
                        Console.WriteLine("Invalid Personal ID or PIN.");
                }
            }
            else
            {
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Transaction History");
                Console.WriteLine("5. Logout");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                try
                {
                    if (choice == "1")
                    {
                        var balance = await transactionService.CheckBalanceAsync(currentUser);
                        Console.WriteLine($"Your balance: {balance} GEL");
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Amount: ");
                        var amount = decimal.Parse(Console.ReadLine() ?? "0");
                        await transactionService.DepositAsync(currentUser, amount);
                        Console.WriteLine($"Deposit successful. Current balance: {currentUser.Balance} GEL");
                    }
                    else if (choice == "3")
                    {
                        Console.Write("Amount: ");
                        var amount = decimal.Parse(Console.ReadLine() ?? "0");
                        await transactionService.WithdrawAsync(currentUser, amount);
                        Console.WriteLine($"Withdrawal of {amount} GEL successful. Current balance: {currentUser.Balance} GEL");
                    }
                    else if (choice == "4")
                    {
                        var logs = await transactionService.GetTransactionHistoryAsync();
                        foreach (var log in logs)
                        {
                            Console.WriteLine($"{log.Date}: {log.UserFullName} - {log.Action} | Balance: {log.BalanceAfter}");
                        }
                    }
                    else if (choice == "5")
                    {
                        currentUser = null;
                        Console.WriteLine("Logged out successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}