namespace MineSweeperWPF.Models;

/// <summary>
/// MineSweeper本体 インターフェース
/// </summary>
public interface IMineSweeper
{
    public StatusType Status { get; }
    public int RowCount { get; }
    public int ColumnCount { get; }
    public int BombCount { get; }
    public int RemainingCellCount { get; }
    public string OpenedString();
    public void Start(int rowCount, int columnCount, int bombCount);
    public IEnumerable<Cell> Open(int index);
    public IEnumerable<Cell> GetRemainingCells();
}
