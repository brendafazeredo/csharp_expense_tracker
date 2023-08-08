using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

public class ExpenseRepository
{
    private List<Expense> expenses = new List<Expense>();
    private int nextId = 1;

    public void AddExpense(Expense expense)
    {
        expense.Id = nextId++;
        expenses.Add(expense);
    }

    public bool RemoveExpense(int id)
    {
        Expense expenseToRemove = expenses.FirstOrDefault(e => e.Id == id);
        if (expenseToRemove != null)
        {
            expenses.Remove(expenseToRemove);
            return true;
        }
        return false;
    }

    public List<Expense> GetExpenses()
    {
        return expenses;
    }

    public void SaveExpensesToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Expense expense in expenses)
            {
                writer.WriteLine($"{expense.Id},{expense.Description},{expense.Amount},{expense.Date.ToString("yyyy-MM-dd")}");
            }
        }
    }

    public void LoadExpensesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            expenses.Clear();
            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4 && int.TryParse(parts[0], out int id)
                    && decimal.TryParse(parts[2], out decimal amount)
                    && DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    expenses.Add(new Expense
                    {
                        Id = id,
                        Description = parts[1],
                        Amount = amount,
                        Date = date
                    });
                    nextId = Math.Max(nextId, id + 1);
                }
            }
        }
    }
}
