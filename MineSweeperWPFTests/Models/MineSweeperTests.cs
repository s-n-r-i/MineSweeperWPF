using Xunit;

namespace MineSweeperWPF.Models.Tests;

/// <summary>
/// MineSweeper本体 test
/// </summary>
public class MineSweeperTests
{
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    private MineSweeper MineSweeper { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

    private const int RowCount = 5;
    private const int ColumnCount = 6;
    private const int BombCount = 7;

    /// <summary>
    /// 開始時共通処理
    /// </summary>
    public MineSweeperTests()
    {
        // できあがる盤面
        // 222110
        // **3*31
        // 223*4*
        // 0012*3
        // 00012*
        MineSweeper = new(new DataGeneratorStub());
        MineSweeper.Start(RowCount, ColumnCount, BombCount);
    }

    [Fact(DisplayName = "01.オブジェクト構築")]
    public void MineSweeperTest()
    {
        // MineSweeperオブジェクト構築直後の状態を確認
        var mineSweeper = new MineSweeper(new DataGeneratorStub());
        Assert.Equal(StatusType.Init, mineSweeper.Status);
    }

    [Fact(DisplayName = "02.開始(1)")]
    public void StartTest1()
    {
        // 開始直後の状態を確認
        System.Diagnostics.Trace.WriteLine(MineSweeper.OpenedString());

        Assert.Equal(StatusType.Playing, MineSweeper.Status);
        Assert.Equal(RowCount, MineSweeper.RowCount);
        Assert.Equal(ColumnCount, MineSweeper.ColumnCount);
        Assert.Equal(BombCount, MineSweeper.BombCount);
        Assert.Equal(RowCount * ColumnCount - BombCount, MineSweeper.RemainingCellCount);
    }

    [Fact(DisplayName = "03.開始(2)")]
    public void StartTest2()
    {
        // 開始直後の状態を確認
        var mineSweeper = new MineSweeper(new DataGeneratorStub());
        mineSweeper.Start(1, 1, 0);

        Assert.Equal(StatusType.Playing, mineSweeper.Status);
        Assert.Equal(1, mineSweeper.RowCount);
        Assert.Equal(1, mineSweeper.ColumnCount);
        Assert.Equal(0, mineSweeper.BombCount);
        Assert.Equal(1, mineSweeper.RemainingCellCount);
    }

    [Fact(DisplayName = "04.開く(1)")]
    public void OpenTest1()
    {
        // 通常セルを開いた際の動作を確認
        var results = MineSweeper.Open(0);
        Assert.Single(results);
        Assert.Equal(0, results.First().Index);
        Assert.Equal(2, results.First().NeighborBombCount);
    }

    [Fact(DisplayName = "05.開く(2)")]
    public void OpenTest2()
    {
        // 通常セルを開いた際の動作を確認
        var results = MineSweeper.Open(16);
        Assert.Single(results);
        Assert.Equal(16, results.First().Index);
        Assert.Equal(4, results.First().NeighborBombCount);
    }

    [Fact(DisplayName = "06.開く(3)")]
    public void OpenTest3()
    {
        // 「開始」未実行時に開くを行っても状態変化が起きないことを確認
        var mineSweeper = new MineSweeper(new DataGeneratorStub());
        var result = mineSweeper.Open(0);
        Assert.Equal(StatusType.Init, mineSweeper.Status);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "07.再帰的に開く")]
    public void RecursiveOpenTest()
    {
        // 隣接爆弾数0のセルを開いた場合に再帰的にその周囲セルを全て開くので、その挙動の確認
        var results = MineSweeper.Open(24); //一番左下をオープン
        System.Diagnostics.Trace.WriteLine(MineSweeper.ToString());
        var expected = new[] { (12, 2), (13, 2), (14, 3), (18, 0), (19, 0), (20, 1), (21, 2), (24, 0), (25, 0), (26, 0), (27, 1) };
        var matchCount = results
                            .Join(
                                expected,
                                r => (r.Index, r.NeighborBombCount),
                                e => (e.Item1, e.Item2),
                                (r, e) => (r, e))
                            .Count();
        Assert.Equal(11, matchCount);
    }

    [Fact(DisplayName = "08.クリア(1)")]
    public void SuccessTest1()
    {
        // 爆弾セル以外全て開いた際の確認
        DoClear(MineSweeper);
        System.Diagnostics.Trace.WriteLine(MineSweeper.ToString());
        Assert.Equal(StatusType.Success, MineSweeper.Status);
        Assert.Equal(0, MineSweeper.RemainingCellCount);
    }

    [Fact(DisplayName = "09.クリア(2)")]
    public void SuccessTest2()
    {
        // 爆弾セル以外全て開いたあとに爆弾セルを開こうとしても状態変化が起きないことを確認
        DoClear(MineSweeper);
        MineSweeper.Open(6);
        System.Diagnostics.Trace.WriteLine(MineSweeper.ToString());
        Assert.Equal(StatusType.Success, MineSweeper.Status);
        Assert.Equal(0, MineSweeper.RemainingCellCount);
    }

    [Fact(DisplayName = "10.クリア(3)")]
    public void SuccessTest3()
    {
        // 爆弾セル以外全て開いたあとに通常セルを再度開こうとしても状態変化が起きないことを確認
        DoClear(MineSweeper);
        MineSweeper.Open(0);
        System.Diagnostics.Trace.WriteLine(MineSweeper.ToString());
        Assert.Equal(StatusType.Success, MineSweeper.Status);
        Assert.Equal(0, MineSweeper.RemainingCellCount);
    }

    [Fact(DisplayName = "11.ゲームオーバー(1)")]
    public void FailureTest1()
    {
        // 爆弾セルを開いた際の挙動を確認
        var results = MineSweeper.Open(6);
        System.Diagnostics.Trace.WriteLine(MineSweeper.ToString());
        Assert.Equal(StatusType.Failure, MineSweeper.Status);
        Assert.Equal(23, MineSweeper.RemainingCellCount);
        Assert.Single(results);
        Assert.True(results.First().IsBomb);
    }

    [Fact(DisplayName = "12.ゲームオーバー(2)")]
    public void FailureTest2()
    {
        // 爆弾セルを開いたあとに通常セルを開こうとしても状態変化が起きないことを確認
        _ = MineSweeper.Open(6);
        var results = MineSweeper.Open(0);
        System.Diagnostics.Trace.WriteLine(MineSweeper.ToString());
        Assert.Equal(StatusType.Failure, MineSweeper.Status);
        Assert.Equal(23, MineSweeper.RemainingCellCount);
        Assert.Empty(results);
    }

    [Fact(DisplayName = "13.残セル取得(1)")]
    public void GetRemainingCellsTest1()
    {
        // 開始直後の残セル(未オープンセル)が」正しく取得できることを確認
        var results = MineSweeper.GetRemainingCells();
        Assert.Equal(30, results.Count());
    }

    [Fact(DisplayName = "14.残セル取得(2)")]
    public void GetRemainingCellsTest2()
    {
        // クリア後の残セルを正しく取得(全て爆弾セル)できることを確認
        DoClear(MineSweeper);
        var results = MineSweeper.GetRemainingCells();
        var expected = new[] { 6, 7, 9, 15, 17, 22, 29 };
        var matchCount = results
                            .Join(
                                expected,
                                r => r.Index,
                                e => e,
                                (r, e) => (r, e))
                            .Count();
        Assert.Equal(expected.Length, matchCount);
    }

    [Fact(DisplayName = "15.残セル取得(3)")]
    public void GetRemainingCellsTest3()
    {
        // ゲームオーバー後の残セルを正しく取得できることを確認
        var targets = Enumerable.Range(0, 7);
        foreach (var target in targets)
        {
            MineSweeper.Open(target);
        }
        var results = MineSweeper.GetRemainingCells();
        var expected = new[] { 7, 8, 9, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
        var matchCount = results
                    .Join(
                        expected,
                        r => r.Index,
                        e => e,
                        (r, e) => (r, e))
                    .Count();
        Assert.Equal(expected.Length, matchCount);
    }

    [Fact(DisplayName = "16.例外(1)")]
    public void ExceptionTest1()
    {
        // 開始時の行数指定が0の場合に例外が発生することを確認
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var mineSweeper = new MineSweeper(new DataGeneratorStub());
            mineSweeper.Start(0, 1, 1);
        });

    }

    [Fact(DisplayName = "17.例外(2)")]
    public void ExceptionTest2()
    {
        // 開始時の列数指定が0の場合に例外が発生することを確認
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var mineSweeper = new MineSweeper(new DataGeneratorStub());
            mineSweeper.Start(1, 0, 1);
        });

    }

    [Fact(DisplayName = "18.例外(3)")]
    public void ExceptionTest3()
    {
        // 開始時の爆弾数指定値がマイナスの場合に例外が発生することを確認
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var mineSweeper = new MineSweeper(new DataGeneratorStub());
            mineSweeper.Start(1, 1, -1);
        });

    }

    [Fact(DisplayName = "19.例外(4)")]
    public void ExceptionTest4()
    {
        // 座標外(範囲より小)を開くと例外が発生することを確認
        Assert.Throws<InvalidOperationException>(() => MineSweeper.Open(-1));
    }

    [Fact(DisplayName = "20.例外(5)")]
    public void ExceptionTest5()
    {
        // 座標外(範囲より大)を開くと例外が発生することを確認
        Assert.Throws<InvalidOperationException>(() => MineSweeper.Open(30));
    }

    /// <summary>
    /// クリア手順実施
    /// </summary>
    /// <param name="mineSweeper"></param>
    private static void DoClear(MineSweeper mineSweeper)
    {
        var indexes = new[] { 0, 1, 2, 3, 4, 5, 8, 12, 13, 14, 16, 18, 23, 28 };
        foreach (var index in indexes)
        {
            mineSweeper.Open(index);
        }
    }
}
