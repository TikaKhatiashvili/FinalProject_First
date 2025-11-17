using Calculator.Models;

namespace Calculator.Data;

public class InputService
{
    public OperationModel GetOperationFromUser()
    {
        var operation = new OperationModel
        {
            Number1 = GetNumber("Enter first number: "),
            Number2 = GetNumber("Enter second number: "),
            Operator = GetOperator()
        };
        return operation;
    }

    private double GetNumber(string prompt)
    {
        double number;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (double.TryParse(input, out number))
                return number;
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private char GetOperator()
    {
        while (true)
        {
            Console.Write("Enter operation (+, -, *, /): ");
            string input = Console.ReadLine();
            if (input.Length == 1 && "+-*/".Contains(input))
                return input[0];
            Console.WriteLine("Invalid operation. Please enter one of +, -, *, /.");
        }
    }
}