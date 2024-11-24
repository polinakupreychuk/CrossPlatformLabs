using System;
using Xunit;

public class ProgramTests
{
    [Fact]
    public void Test_NoPathExists()
    {
        var input = new[]
        {
            "4",
            "@###",
            "####",
            "###X",
            "####"
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.False(result);
        Assert.DoesNotContain("+", string.Join("", outputGrid));
    }

    [Fact]
    public void Test_StartSurroundedByObstacles()
    {
        var input = new[]
        {
            "3",
            "###",
            "#@#",
            "###"
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.False(result);
    }

    [Fact]
    public void Test_EndSurroundedByObstacles()
    {
        var input = new[]
        {
            "3",
            "@..",
            "...",
            "###X"
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.False(result);
    }

    [Fact]
    public void Test_SimplePath()
    {
        var input = new[]
        {
            "3",
            "@..",
            "...",
            "..X"
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.True(result);
        Assert.Contains("+", string.Join("", outputGrid));
    }
    
    [Fact]
    public void Test_SameRowDifferentColumns()
    {
        var input = new[]
        {
            "3",
            "@.X",
            "###",
            "..."
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.True(result);
        Assert.Contains("+", string.Join("", outputGrid));
    }

    [Fact]
    public void Test_EmptyGrid()
    {
        var input = new[]
        {
            "2",
            "@.",
            ".X"
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.True(result);
        Assert.Contains("+", string.Join("", outputGrid));
    }

    [Fact]
    public void Test_SingleCellGrid()
    {
        var input = new[]
        {
            "1",
            "@"
        };

        bool result = Program.Execute(input, out var outputGrid);

        Assert.False(result);
        Assert.DoesNotContain("+", string.Join("", outputGrid));
    }
}
