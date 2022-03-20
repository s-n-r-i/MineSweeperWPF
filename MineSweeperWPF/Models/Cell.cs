namespace MineSweeperWPF.Models;

/// <summary>
/// セル
/// </summary>
public record class Cell
{
    /// <summary>
    /// Index
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// 爆弾セルか
    /// </summary>
    public bool IsBomb { get; }

    /// <summary>
    /// 隣接爆弾セル数
    /// </summary>
    public int NeighborBombCount { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isBomb"></param>
    /// <param name="neighborBombCount"></param>
    public Cell(int index, bool isBomb, int neighborBombCount)
    {
        Index = index;
        IsBomb = isBomb;
        NeighborBombCount = neighborBombCount;
    }
}
