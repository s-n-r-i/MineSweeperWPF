using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeperWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperWPF.Models.Tests;

/// <summary>
/// 初期データジェネレーター test
/// </summary>
[TestClass()]
public class DataGeneratorTests
{
    [TestMethod("1.配列生成(1)")]
    public void GetRandomIntArrayTest1()
    {
        const int count = 7;
        const int rangeMax = 30;

        var bombData = new DataGenerator();
        var results = bombData.GetRandomIntArray(count, rangeMax);
        Assert.AreEqual(count, results.Count(), "配列生成結果不正");
        var distinctCount = results.Distinct().Count();
        Assert.AreEqual(count, distinctCount, "配列に重複値がある");
        Assert.IsTrue(results.Min() >= 0, "配列最小値不正");
        Assert.IsTrue(results.Max() < rangeMax, "配列最大値不正");
    }

    [TestMethod("2.配列生成(2)")]
    public void GetRandomIntArrayTest2()
    {
        var bombData = new DataGenerator();
        var results = bombData.GetRandomIntArray(0, 1);
        Assert.AreEqual(0, results.Count(), "配列生成結果不正");
    }

    [TestMethod("3.例外(1)")]
    public void GetRandomIntArrayExceptionTest1()
    {
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            var bombData = new DataGenerator();
            _ = bombData.GetRandomIntArray(-1, 1);
        });
    }

    [TestMethod("4.例外(2)")]
    public void GetRandomIntArrayExceptionTest2()
    {
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            var bombData = new DataGenerator();
            _ = bombData.GetRandomIntArray(1, 0);
        });
    }

    [TestMethod("5.例外(3)")]
    public void GetRandomIntArrayExceptionTest3()
    {
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            var bombData = new DataGenerator();
            _ = bombData.GetRandomIntArray(2, 1);
        });
    }
}
