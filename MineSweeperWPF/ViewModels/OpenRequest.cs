using MineSweeperWPF.Models;

namespace MineSweeperWPF.ViewModels;

/// <summary>
/// セルオープンリクエスト
/// </summary>
public class OpenRequest
{
    /// <summary>
    /// オープン対象セル
    /// </summary>
    public IEnumerable<Cell> Cells { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cells"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public OpenRequest(IEnumerable<Cell> cells)
    {
        Cells = cells ?? throw new ArgumentNullException(nameof(cells));
    }
}
