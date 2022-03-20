namespace MineSweeperWPF.ViewModels;

/// <summary>
/// 開始リクエスト
/// </summary>
public class StartRequest
{
    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// 列数
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="columnCount"></param>
    public StartRequest(int rowCount, int columnCount)
    {
        RowCount = rowCount;
        ColumnCount = columnCount;
    }
}
