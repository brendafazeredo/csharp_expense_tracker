using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class ExpenseTracker
{
    private const string DataFilePath = "expenses.txt";
    private ExpenseRepository expenseRepository = new ExpenseRepository();

    public void Run()
    {
        expenseRepository.LoadExpensesFromFile(DataFilePath);

        while (true)
        {
            ShowMainMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddExpense();
                    break;
                case "2":
                    RemoveExpense();
                    break;
                case "3":
                    ShowAllExpenses();
                    break;
                case "0":
                    expenseRepository.SaveExpensesToFile(DataFilePath);
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("Expense Tracker");
        Console.WriteLine("1. Add Expense");
        Console.WriteLine("2. Remove Expense");
        Console.WriteLine("3. Show All Expenses");
        Console.WriteLine("0. Exit");
        Console.Write("Enter your choice: ");
    }

    private string GetUserInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    private decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            string input = GetUserInput(prompt);
            if (decimal.TryParse(input, out decimal result) && result >= 0)
            {
                return result;
            }
            Console.WriteLine("Invalid input. Please enter a non-negative decimal.");
        }
    }

    private DateTime GetDateInput(string prompt)
    {
        while (true)
        {
            string input = GetUserInput(prompt);
            if (DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            Console.WriteLine("Invalid input. Please enter a valid date in the format MM/dd/yyyy.");
        }
    }

    private void AddExpense()
    {
        Console.WriteLine("Enter expense description:");
        string description = GetUserInput("");

        decimal amount = GetDecimalInput("Enter expense amount:");

        DateTime date = GetDateInput("Enter expense date (MM/dd/yyyy):");

        Expense newExpense = new Expense
        {
            Description = description,
            Amount = amount,
            Date = date
        };

        expenseRepository.AddExpense(newExpense);
        Console.WriteLine("Expense added successfully!");
    }

    private void RemoveExpense()
    {
        Console.WriteLine("Enter the ID of the expense to remove:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            bool removed = expenseRepository.RemoveExpense(id);
            if (removed)
            {
                Console.WriteLine("Expense removed successfully!");
            }
            else
            {
                Console.WriteLine("Expense not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid expense ID.");
        }
    }

    private void ShowAllExpenses()
    {
        List<Expense> expenses = expenseRepository.GetExpenses();

        if (expenses.Count > 0)
        {
            Console.WriteLine("All Expenses:");
            foreach (Expense expense in expenses)
            {
                Console.WriteLine($"ID: {expense.Id}, Description: {expense.Description}, Amount: {expense.Amount}, Date: {expense.Date.ToString("MM/dd/yyyy")}");
            }
        }
        else
        {
            Console.WriteLine("No expenses found.");
        }
    }
}