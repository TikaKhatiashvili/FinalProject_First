using Calculator.Business;
using Calculator.Data;
using Calculator.Models;

namespace CalculatorProject.Calculator.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputService = new InputService();
            var calculatorService = new CalculatorService();

            while (true)
            {
                try
                {
                    OperationModel operation = inputService.GetOperationFromUser();
                    double result = calculatorService.Calculate(operation);
                    Console.WriteLine($"Result: {result}\n");
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}\n");
                }
            }
        }
    }
}