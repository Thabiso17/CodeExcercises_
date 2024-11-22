namespace CodeExcercises.Interfaces.Calculator
{
    public interface IStringCalculator
    {
        (bool Success, int Result, string ErrorMessage) Add(string numbers);
    }
}
