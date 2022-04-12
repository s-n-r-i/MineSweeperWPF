namespace MineSweeperWPF.Models;

/// <summary>
/// MineSweeper本体
/// </summary>
public class MineSweeper : IMineSweeper
{
    /// <summary>
    /// 盤面の状態
    /// </summary>
    public StatusType Status { get; private set; } = StatusType.Init;

    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount { get; private set; }

    /// <summary>
    /// 列数
    /// </summary>
    public int ColumnCount { get; private set; }

    /// <summary>
    /// 爆弾数
    /// </summary>
    public int BombCount { get; private set; }

    /// <summary>
    /// 未オープンセル数
    /// </summary>
    public int RemainingCellCount { get; private set; }

    /// <summary>
    /// 初期データ生成クラス
    /// </summary>
    private IDataGenerator DataGenerator { get; }

    /// <summary>
    /// 盤面(状態保持用)
    /// </summary>
    private IEnumerable<InnerCell> Cells { get; set; } = Enumerable.Empty<InnerCell>();

    /// <summary>
    /// Index最大値+1
    /// </summary>
    private int RangeMax => RowCount * ColumnCount;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="dataGenerator"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MineSweeper(IDataGenerator dataGenerator)
    {
        DataGenerator = dataGenerator ?? throw new ArgumentNullException(nameof(dataGenerator));
    }

    /// <summary>
    /// 盤面の文字列化(正解が見える)
    /// </summary>
    /// <returns></returns>
    public string OpenedString() => CreateString(cell => cell.OpenedString());

    /// <summary>
    /// 盤面の文字列化
    /// </summary>
    /// <returns></returns>
    public override string ToString() => CreateString(cell => cell.ToString());

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="columnCount"></param>
    /// <param name="bombCount"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Start(int rowCount, int columnCount, int bombCount)
    {
        Status = StatusType.Playing;

        if (rowCount < 1)
            throw new ArgumentOutOfRangeException(nameof(rowCount));

        if (columnCount < 1)
            throw new ArgumentOutOfRangeException(nameof(columnCount));

        if (bombCount < 0)
            throw new ArgumentOutOfRangeException(nameof(bombCount));

        if (bombCount > rowCount * columnCount)
            throw new ArgumentException("bombCount > rowCount * columnCount");

        RowCount = rowCount;
        ColumnCount = columnCount;
        BombCount = bombCount;
        RemainingCellCount = RangeMax - bombCount;

        var bombPositions = DataGenerator.GetRandomIntArray(bombCount, RangeMax);
        Cells = Enumerable.Range(0, RangeMax)
                            .Select(index =>
                                new InnerCell(index, bombPositions.Any(pos => pos == index), GetNeighborBombCount(bombPositions, index)))
                            .ToArray();
    }

    /// <summary>
    /// セルオープン
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerable<Cell> Open(int index)
    {
        if (Status is not StatusType.Playing)
        {
            return Enumerable.Empty<Cell>();
        }

        var currentCell = Cells.Single(x => x.Index == index);

        if (currentCell.IsOpen)
        {
            return Enumerable.Empty<Cell>();
        }

        var indexes = RecursiveOpenCells(index).OrderBy(x => x).ToArray();

        if (currentCell.IsBomb)
        {
            Status = StatusType.Failure;
        }
        else
        {
            RemainingCellCount = Cells.Count(x => !x.IsOpen && !x.IsBomb);
            if (RemainingCellCount == 0)
            {
                Status = StatusType.Success;
            }
        }

        return Cells.Where(x => indexes.Contains(x.Index)).Select(x => x.Cell).ToArray();
    }

    /// <summary>
    /// 残セル取得
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Cell> GetRemainingCells() => Cells.Where(x => !x.IsOpen).Select(x => x.Cell).ToArray();

    /// <summary>
    /// 再帰的にセルオープン
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private IEnumerable<int> RecursiveOpenCells(int current)
    {
        var currentCell = Cells.Single(x => x.Index == current);
        if (currentCell.IsOpen)
        {
            yield break;
        }

        currentCell.IsOpen = true;
        yield return current;

        if (!currentCell.IsBomb && currentCell.NeighborBombCount == 0)
        {
            // 隣接爆弾数が0ならば周囲8方向のオープンを試行
            foreach (var pos in GetNeighborIndexes(current).ToArray())
            {
                foreach (var next in RecursiveOpenCells(pos).ToArray())
                {
                    yield return next;
                }
            }
        }
    }

    /// <summary>
    /// 隣接セルの爆弾数合計取得
    /// </summary>
    /// <param name="bombPositions">爆弾indexの配列</param>
    /// <param name="current">対象位置index</param>
    /// <returns></returns>
    private int GetNeighborBombCount(IEnumerable<int> bombPositions, int current)
    {
        var result = bombPositions
                        .Join(
                            GetNeighborIndexes(current).ToArray(),
                            b => b,
                            n => n,
                            (b, n) => (b, n))
                        .Count();

        return result;
    }

    /// <summary>
    /// 隣接8箇所のinxexを取得
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private IEnumerable<int> GetNeighborIndexes(int current)
    {
        var left = GetLeft(current);
        var right = GetRight(current);
        var neighbors = new[]
        {
            GetUp(left),
            GetUp(current),
            GetUp(right),
            left,
            right,
            GetDown(left),
            GetDown(current),
            GetDown(right),
        };
        return neighbors.OfType<int>().ToArray(); //nullを除外してintにキャスト
    }

    /// <summary>
    /// 左側index取得
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private int? GetLeft(int? current)
    {
        if (current.HasValue && current.Value - 1 is var tempLeft && tempLeft >= 0 && current.Value % ColumnCount != 0)
        {
            // 左端以外なら現在位置-1を返す
            return tempLeft;
        }

        return null;
    }

    /// <summary>
    /// 右側index取得
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private int? GetRight(int? current)
    {
        if (current.HasValue && current.Value + 1 is var tempRight && tempRight <= RangeMax && tempRight % ColumnCount != 0)
        {
            // 右端以外なら現在位置+1を返す
            return tempRight;
        }

        return null;
    }

    /// <summary>
    /// 上側index取得
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private int? GetUp(int? current)
    {
        if (current.HasValue && current.Value - ColumnCount is var tempUp && tempUp >= 0)
        {
            // 上端以外なら現在位置-ColumnCountを返す
            return tempUp;
        }

        return null;
    }

    /// <summary>
    /// 下側index取得
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private int? GetDown(int? current)
    {
        if (current.HasValue && current.Value + ColumnCount is var tempDown && tempDown < RangeMax)
        {
            // 下端以外なら現在位置+ColumnCountを返す
            return tempDown;
        }

        return null;
    }

    /// <summary>
    /// (共通)debug出力用文字列作成
    /// </summary>
    /// <param name="getString"></param>
    /// <returns></returns>
    private string CreateString(Func<InnerCell, string> getString)
    {
        var temp = $"状態:{Status} 残:{RemainingCellCount}";
        foreach (var cell in Cells)
        {
            if (cell.Index % ColumnCount == 0)
            {
                temp += Environment.NewLine;
            }
            temp += getString(cell);
        }

        temp += Environment.NewLine;

        return temp;
    }

    /// <summary>
    /// セル (状態保持用)
    /// </summary>
    private class InnerCell
    {
        /// <summary>
        /// セル
        /// </summary>
        public Cell Cell => new(Index, IsBomb, NeighborBombCount);

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
        /// 開かれたか
        /// </summary>
        public bool IsOpen { get; set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isBomb"></param>
        /// <param name="neighborBombCount"></param>
        public InnerCell(int index, bool isBomb, int neighborBombCount)
        {
            Index = index;
            IsBomb = isBomb;
            NeighborBombCount = neighborBombCount;
        }

        /// <summary>
        /// セルの文字列化(正解が見える)
        /// </summary>
        /// <returns></returns>
        public string OpenedString() => IsBomb ? "*" : NeighborBombCount.ToString();

        /// <summary>
        /// セルの文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString() => IsOpen ? OpenedString() : "_";
    }
}
