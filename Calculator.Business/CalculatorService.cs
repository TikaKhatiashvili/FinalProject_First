using Calculator.Models;

namespace Calculator.Business;
public class CalculatorService
{
    public double Calculate(OperationModel operation)
    {
        return operation.Operator switch
        {
            '+' => Add(operation.Number1, operation.Number2),
            '-' => Subtract(operation.Number1, operation.Number2),
            '*' => Multiply(operation.Number1, operation.Number2),
            '/' => Divide(operation.Number1, operation.Number2),
            _ => throw new InvalidOperationException("Invalid operation")
        };
    }

    private double Add(double a, double b) => a + b;
    private double Subtract(double a, double b) => a - b;
    private double Multiply(double a, double b) => a * b;
    private double Divide(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Division by zero is not allowed.");
        return a / b;
    }
}