# ATM Project

A console-based ATM (Automated Teller Machine) simulation written in C# that provides basic banking operations with persistent data storage using JSON. 

## Features

- **User Authentication**:  Secure login using card number, expiration date, and PIN code
- **Balance Inquiry**: Check current account balance
- **Withdraw Money**: Withdraw funds from your account
- **Deposit Money**:  Add funds to your account
- **PIN Change**: Update your PIN code securely
- **Transaction History**: View the last 5 transactions
- **Persistent Storage**: All data is saved to a JSON file for persistence between sessions

## Technologies

- **Language**: C# (. NET)
- **Data Storage**: JSON (using System.Text.Json)
- **Architecture**: Console Application

## Project Structure

```
Atm-project/
├── Atm project/
│   ├── Program.cs              # Main application logic
│   ├── File.json               # User data storage
│   ├── Atm_project. csproj      # Project configuration
│   └── cardinfo/
│       ├── CardInfo.cs         # Card information model
│       ├── UserInfo.cs         # User information model
│       └── TransactionInfo.cs  # Transaction record model
└── Atm project. sln             # Solution file
```
