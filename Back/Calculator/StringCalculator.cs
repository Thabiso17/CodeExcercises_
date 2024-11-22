using CodeExcercises.Interfaces.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeExcercises.Back.Calculator
{
    public class StringCalculator : IStringCalculator
    {
        public (bool Success, int Result, string ErrorMessage) Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
            {
                return (true, 0, null);
            }

            var delimiters = new List<string> { ",", "\n" };
            string numbersWithoutDelimiter = numbers;

            if (numbers.StartsWith("//"))
            {
                var delimiterEndIndex = numbers.IndexOf('\n');
                if (delimiterEndIndex == -1 || delimiterEndIndex == numbers.Length - 1)
                {
                    return (false, 0, "Invalid input: Delimiter declaration found but no numbers provided.");
                }

                var delimiter = numbers.Substring(2, delimiterEndIndex - 2);
                delimiters.Add(delimiter);
                numbersWithoutDelimiter = numbers.Substring(delimiterEndIndex + 1);
            }

            try
            {
                var numberList = SplitNumbers(numbersWithoutDelimiter, delimiters);
                var validationResult = ValidateNumbers(numberList);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                return (true, numberList.Sum(), null);
            }
            catch (FormatException ex)
            {
                // Log the error using telemetry or another logging mechanism
                // TelemetryClient.TrackException(ex);
                return (false, 0, "Input string was not in a correct format.");
            }
            catch (Exception ex)
            {
                // Log the error using telemetry or another logging mechanism
                // TelemetryClient.TrackException(ex);
                return (false, 0, "An error occurred while processing the numbers.");
            }
        }

        private List<int> SplitNumbers(string numbers, List<string> delimiters)
        {
            var delimiterPattern = string.Join("|", delimiters.Select(Regex.Escape));
            var numberStrings = Regex.Split(numbers, delimiterPattern);
            return numberStrings.Select(int.Parse).ToList();
        }

        private (bool Success, int Result, string ErrorMessage) ValidateNumbers(List<int> numbers)
        {
            var negativeNumbers = numbers.Where(n => n < 0).ToList();
            if (negativeNumbers.Any())
            {
                return (false, 0, $"Negatives not allowed: {string.Join(", ", negativeNumbers)}");
            }

            return (true, 0, null);
        }
    }
}