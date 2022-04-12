using MineSweeperWPF.Models;
using MineSweeperWPF.Models.Tests;
using Xunit;

namespace MineSweeperWPF.ViewModels.Tests;

public class MainViewModelTests
{
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    private MainViewModel ViewModel { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

    public MainViewModelTests() => ViewModel = new(new MineSweeper(new DataGeneratorStub()));

    [Fact(DisplayName = "1.オブジェクト構築")]
    public void MainViewModelTest()
    {
        // オブジェクト構築と構築時の初期値が正しいことを確認
        Assert.Equal(0, ViewModel.RemainingCellCount.Value);
        Assert.Equal(string.Empty, ViewModel.Status.Value);
    }

    [Fact(DisplayName = "2.開始(1)")]
    public void StartTest1()
    {
        // 開始時の状態が正しいことを確認
        ViewModel.StartCommand.Execute();
        var startRequest = ViewModel.StartRequest.Value;
        Assert.NotNull(startRequest);
        Assert.Equal(5, startRequest!.RowCount);
        Assert.Equal(6, startRequest!.ColumnCount);
        Assert.Equal(23, ViewModel.RemainingCellCount.Value);
        Assert.Equal(StatusType.Playing.ToString(), ViewModel.Status.Value);
    }

    [Fact(DisplayName = "3.開始(2)")]
    public void StartTest2()
    {
        // 開始→セルオープン後に開始を行い、その状態が開始直後の状態であることを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(7); //Status.Failure
        ViewModel.StartCommand.Execute();
        Assert.Equal(23, ViewModel.RemainingCellCount.Value);
        Assert.Equal(StatusType.Playing.ToString(), ViewModel.Status.Value);
    }

    [Fact(DisplayName = "4.開く(1)")]
    public void OpenTest1()
    {
        // セルオープン実行後の状態が正しいことを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(0);
        var openRequest = ViewModel.OpenRequest.Value;
        Assert.NotNull(openRequest);
        Assert.Single(openRequest!.Cells);
        Assert.Equal(22, ViewModel.RemainingCellCount.Value);
        Assert.Equal(StatusType.Playing.ToString(), ViewModel.Status.Value);
    }

    [Fact(DisplayName = "5.開く2")]
    public void OpenTest2()
    {
        // セルオープン実行で複数のセルが開かれた際の状態が正しいことを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(24);
        var openRequest = ViewModel.OpenRequest.Value;
        Assert.NotNull(openRequest);
        Assert.Equal(11, openRequest!.Cells.Count());
    }

    [Fact(DisplayName = "6.クリア")]
    public void SuccessTest()
    {
        // 手順実行後にクリア状態になっていることを確認
        ViewModel.StartCommand.Execute();
        var indexes = new[] { 0, 1, 2, 3, 4, 5, 8, 12, 13, 14, 16, 18, 23, 28 };
        foreach (var index in indexes)
        {
            ViewModel.OpenCommand.Execute(index);
        }
        Assert.Equal(0, ViewModel.RemainingCellCount.Value);
        Assert.Equal(StatusType.Success.ToString(), ViewModel.Status.Value);
    }

    [Fact(DisplayName = "7.ゲームオーバー(1)")]
    public void FailureTest1()
    {
        // 爆弾セルオープン後にゲームオーバー状態になっていることを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(6);
        Assert.Equal(23, ViewModel.RemainingCellCount.Value);
        Assert.Equal(StatusType.Failure.ToString(), ViewModel.Status.Value);
    }

    [Fact(DisplayName = "8.ゲームオーバー(2)")]
    public void FailureTest2()
    {
        // 通常セルオープン→爆弾セルオープンでゲームオーバー状態になっていることを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(0);
        ViewModel.OpenCommand.Execute(6);
        Assert.Equal(22, ViewModel.RemainingCellCount.Value);
        Assert.Equal(StatusType.Failure.ToString(), ViewModel.Status.Value);
    }
}
