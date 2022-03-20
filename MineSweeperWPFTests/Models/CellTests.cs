using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MineSweeperWPF.Models.Tests;

/// <summary>
/// セル test
/// </summary>
[TestClass()]
public class CellTests
{
    [TestMethod("1.オブジェクト構築")]
    public void CellTest1()
    {
        var cell = new Cell(1, true, 2);
        Assert.AreEqual(1, cell.Index);
        Assert.IsTrue(cell.IsBomb);
        Assert.AreEqual(2, cell.NeighborBombCount);
    }

    [TestMethod("2.オブジェクト構築")]
    public void CellTest2()
    {
        var cell = new Cell(2, false, 5);
        Assert.AreEqual(2, cell.Index);
        Assert.IsFalse(cell.IsBomb);
        Assert.AreEqual(5, cell.NeighborBombCount);
    }

    [TestMethod("3.値一致")]
    public void CellTest3()
    {
        var actual = new Cell(3, true, 7);
        var expected = new Cell(3, true, 7);
        Assert.AreEqual(expected, actual);　//インスタンスが異なっても一致
    }
}
