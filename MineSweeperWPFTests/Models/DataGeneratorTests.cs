using Xunit;

namespace MineSweeperWPF.Models.Tests;

/// <summary>
/// 初期データジェネレーター test
/// </summary>
public class DataGeneratorTests
{
    [Fact(DisplayName = "1.配列生成(1)")]
    public void GetRandomIntArrayTest1()
    {
        const int count = 7;
        const int rangeMax = 30;

        var bombData = new DataGenerator();
        var results = bombData.GetRandomIntArray(count, rangeMax);
        Assert.Equal(count, results.Count());
        var distinctCount = results.Distinct().Count();
        Assert.Equal(count, distinctCount);
        Assert.True(results.Min() >= 0, "配列最小値不正");
        Assert.True(results.Max() < rangeMax, "配列最大値不正");
    }

    [Fact(DisplayName = "2.配列生成(2)")]
    public void GetRandomIntArrayTest2()
    {
        var bombData = new DataGenerator();
        var results = bombData.GetRandomIntArray(0, 1);
        Assert.Empty(results);
    }

    [Fact(DisplayName = "3.例外(1)")]
    public void GetRandomIntArrayExceptionTest1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var bombData = new DataGenerator();
            _ = bombData.GetRandomIntArray(-1, 1);
        });
    }

    [Fact(DisplayName = "4.例外(2)")]
    public void GetRandomIntArrayExceptionTest2()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var bombData = new DataGenerator();
            _ = bombData.GetRandomIntArray(1, 0);
        });
    }

    [Fact(DisplayName = "5.例外(3)")]
    public void GetRandomIntArrayExceptionTest3()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var bombData = new DataGenerator();
            _ = bombData.GetRandomIntArray(2, 1);
        });
    }
}
