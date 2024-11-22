using CodeExcercises.Back.Calculator;
using CodeExcercises.Interfaces.Calculator;
using System;
using Xunit;

public class StringCalculatorTests
{
    private readonly IStringCalculator _calculator;

    public StringCalculatorTests()
    {
        _calculator = new StringCalculator();
    }

    [Fact]
    public void Add_EmptyString_ReturnsZero()
    {
        var result = _calculator.Add("");
        Assert.True(result.Success);
        Assert.Equal(0, result.Result);
    }

    [Fact]
    public void Add_SingleNumber_ReturnsNumber()
    {
        var result = _calculator.Add("1"); 
        Assert.True(result.Success);
        Assert.Equal(1, result.Result);
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        var result = _calculator.Add("1,2");
        Assert.True(result.Success);
        Assert.Equal(3, result.Result);
    }

    [Fact]
    public void Add_UnknownAmountOfNumbers_ReturnsSum()
    {
        var result = _calculator.Add("1,2,3,4,5");
        Assert.True(result.Success);
        Assert.Equal(15, result.Result);
    }

    [Fact]
    public void Add_NewLineBetweenNumbers_ReturnsSum()
    {
        var result = _calculator.Add("1\n2,3");
        Assert.True(result.Success);
        Assert.Equal(6, result.Result);
    }

    [Fact]
    public void Add_DifferentDelimiter_ReturnsSum()
    {
        var result = _calculator.Add("//;\n1;2");
        Assert.True(result.Success);
        Assert.Equal(3, result.Result);
    }

    [Fact]
    public void Add_NegativeNumber_ReturnsErrorMessage()
    {
        var result = _calculator.Add("1,-2,3");
        Assert.False(result.Success);
        Assert.Equal("Negatives not allowed: -2", result.ErrorMessage);
    }

    [Fact]
    public void Add_MultipleNegativeNumbers_ReturnsErrorMessage()
    {
        var result = _calculator.Add("1,-2,-3");
        Assert.False(result.Success);
        Assert.Equal("Negatives not allowed: -2, -3", result.ErrorMessage);
    }
}