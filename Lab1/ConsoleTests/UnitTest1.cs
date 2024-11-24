using System;
using System.Collections.Generic;
using Xunit;

public class ProgramTests
{
    [Fact]
    public void IsLucky_SingleLuckyDigit_ReturnsTrue()
    {
        Assert.True(Program.IsLucky(4));
        Assert.True(Program.IsLucky(7));
    }

    [Fact]
    public void IsLucky_ContainsNonLuckyDigit_ReturnsFalse()
    {
        Assert.False(Program.IsLucky(5));
        Assert.False(Program.IsLucky(47_3));
    }

    [Fact]
    public void IsLucky_AllLuckyDigits_ReturnsTrue()
    {
        Assert.True(Program.IsLucky(47));
        Assert.True(Program.IsLucky(74));
        Assert.True(Program.IsLucky(447));
    }

    [Fact]
    public void CountLuckyNumbers_NoLuckyNumbers_ReturnsZero()
    {
        Assert.Equal(0, Program.CountLuckyNumbers(3));
    }

    [Fact]
    public void CountLuckyNumbers_SmallN_ReturnsCorrectCount()
    {
        Assert.Equal(2, Program.CountLuckyNumbers(7)); // Числа: 4, 7
    }

    [Fact]
    public void CountLuckyNumbers_MediumN_ReturnsCorrectCount()
    {
        Assert.Equal(6, Program.CountLuckyNumbers(77)); // Числа: 4, 7, 44, 47, 74, 77
    }

    [Fact]
    public void CountLuckyNumbers_LargeN_ReturnsCorrectCount()
    {
        Assert.Equal(14, Program.CountLuckyNumbers(1000)); 
        // Щасливі числа: 4, 7, 44, 47, 74, 77, 444, 447, 474, 477, 744, 747, 774, 777
    }
}