using Atm_project.Info;
using System.Text.Json;

namespace Atm_project
{
    internal class Program
    {
        private List<UserInfo> _users = new List<UserInfo>();

        static void Main(string[] args)
        {
            Program program = new Program();
            program.InitializeUsers();
            program.Start();
        }

        public void InitializeUsers()
        {
            try
            {
                string filePath = @"C:\Users\elene\source\repos\Atm project\Atm project\File.json";
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    _users = JsonSerializer.Deserialize<List<UserInfo>>(jsonString, options);
                }
                else
                {
                    Console.WriteLine("JSON file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the ATM!\n");

                Console.Write("Enter Card Number: ");
                string cardNumber = Console.ReadLine();

                Console.Write("Enter Expiration Date (MM/YY): ");
                string expDate = Console.ReadLine();

                UserInfo user = null;
                foreach (var u in _users)
                {
                    if (u.CardInfo.CardNumber == cardNumber && u.CardInfo.ExpirationDate == expDate)
                    {
                        user = u;
                        break;
                    }
                }

                if (user == null)
                {
                    Console.WriteLine("Card details are not correct");
                    continue;
                }

                Console.Write("Enter PIN Code: ");
                string pin = Console.ReadLine();

                if (user.CardInfo.PinCode != pin)
                {
                    Console.WriteLine("PIN is not correct! ");
                    continue;
                }

                ShowMainMenu(user);
            }
        }

        private void ShowMainMenu(UserInfo user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine($"Welcome, {user.FirstName} {user.LastName}");
                Console.WriteLine($"Current Balance: {user.CardInfo.Balance:C}");
                Console.WriteLine();
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Withdraw Money");
                Console.WriteLine("3. View Transaction History");
                Console.WriteLine("4. Deposit Money");
                Console.WriteLine("5. Change PIN");
                Console.WriteLine("6. Logout");
                Console.WriteLine();
                Console.Write("Select an option (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CheckBalance(user);
                        break;
                    case "2":
                        WithdrawMoney(user);
                        break;
                    case "3":
                        ViewTransactionHistory(user);
                        break;
                    case "4":
                        DepositMoney(user);
                        break;
                    case "5":
                        ChangePin(user);
                        break;
                    case "6":
                        Console.WriteLine("Logging out... ");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }

                Console.ReadKey();
            }
        }

        private void CheckBalance(UserInfo user)
        {
            Console.Clear();
            Console.WriteLine("Account Balance");
            Console.WriteLine($"Current Balance: {user.CardInfo.Balance:C}");

            user.CardInfo.TransactionHistory.Add(new TransactionInfo
            {
                TransactionDate = DateTime.UtcNow,
                TransactionType = "Balance Inquiry",
                AmountGEL = 0,
                AmountUSD = 0,
                AmountEUR = 0
            });
            SaveUsersToFile();
        }

        private void WithdrawMoney(UserInfo user)
        {
            Console.Clear();
            Console.WriteLine("Withdraw Money");
            Console.Write("Enter amount to withdraw: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (amount <= user.CardInfo.Balance)
                {
                    user.CardInfo.Balance -= amount;
                    user.CardInfo.TransactionHistory.Add(new TransactionInfo
                    {
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "Withdrawal",
                        AmountGEL = amount,
                        AmountUSD = 0,
                        AmountEUR = 0
                    });
                    SaveUsersToFile();
                    Console.WriteLine($"Successfully withdrew {amount:C}. New balance: {user.CardInfo.Balance:C}");
                }
                else
                {
                    Console.WriteLine("not enough funds.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        private void DepositMoney(UserInfo user)
        {
            Console.Clear();
            Console.WriteLine("Deposit Money");
            Console.Write("Enter amount to deposit: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                user.CardInfo.Balance += amount;
                user.CardInfo.TransactionHistory.Add(new TransactionInfo
                {
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "Deposit",
                    AmountGEL = amount,
                    AmountUSD = 0,
                    AmountEUR = 0
                });
                SaveUsersToFile();
                Console.WriteLine($"Successfully deposited {amount:C}. New balance: {user.CardInfo.Balance:C}");
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        private void ChangePin(UserInfo user)
        {
            Console.Clear();
            Console.WriteLine("Change PIN ");
            Console.Write("Enter new PIN (4 digits): ");

            string newPin = Console.ReadLine();

            if (newPin.Length == 4 && int.TryParse(newPin, out _))
            {
                user.CardInfo.PinCode = newPin;
                user.CardInfo.TransactionHistory.Add(new TransactionInfo
                {
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "PIN Change",
                    AmountGEL = 0,
                    AmountUSD = 0,
                    AmountEUR = 0
                });
                SaveUsersToFile();
                Console.WriteLine("PIN successfully changed.");
            }
            else
            {
                Console.WriteLine("Invalid PIN. Must be 4 digits.");
            }
        }

        private void ViewTransactionHistory(UserInfo user)
        {
            Console.Clear();
            Console.WriteLine("Transaction History");

            if (user.CardInfo.TransactionHistory.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            var recentTransactions = user.CardInfo.TransactionHistory
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToList();

            foreach (var transaction in recentTransactions)
            {
                Console.WriteLine($"{transaction.TransactionDate:yyyy-MM-dd HH:mm:ss} - {transaction.TransactionType}");
                if (transaction.AmountGEL > 0)
                    Console.WriteLine($" Amount: {transaction.AmountGEL:C} GEL");
                Console.WriteLine();
            }
        }

        private void SaveUsersToFile()
        {
            try
            {
                string filePath = @"C:\Users\elene\source\repos\Atm_project\Atm_project\File.json";
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string jsonString = JsonSerializer.Serialize(_users, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
    }
}
