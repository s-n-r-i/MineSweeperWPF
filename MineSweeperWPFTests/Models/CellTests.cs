using Xunit;

namespace MineSweeperWPF.Models.Tests;

/// <summary>
/// セル test
/// </summary>
public class CellTests
{
    [Fact(DisplayName = "1.オブジェクト構築")]
    public void CellTest1()
    {
        var cell = new Cell(1, true, 2);
        Assert.Equal(1, cell.Index);
        Assert.True(cell.IsBomb);
        Assert.Equal(2, cell.NeighborBombCount);
    }

    [Fact(DisplayName = "2.オブジェクト構築")]
    public void CellTest2()
    {
        var cell = new Cell(2, false, 5);
        Assert.Equal(2, cell.Index);
        Assert.False(cell.IsBomb);
        Assert.Equal(5, cell.NeighborBombCount);
    }

    [Fact(DisplayName = "3.値一致")]
    public void CellTest3()
    {
        var actual = new Cell(3, true, 7);
        var expected = new Cell(3, true, 7);
        Assert.Equal(expected, actual);　//インスタンスが異なっても一致
    }
}
