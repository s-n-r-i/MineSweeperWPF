using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeperWPF.Models;
using MineSweeperWPF.Models.Tests;
using System.Linq;

namespace MineSweeperWPF.ViewModels.Tests;

[TestClass()]
public class MainViewModelTests
{
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    private MainViewModel ViewModel { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

    [TestInitialize]
    public void TestInitialize() => ViewModel = new(new MineSweeper(new DataGeneratorStub()));

    [TestMethod("1.オブジェクト構築")]
    public void MainViewModelTest()
    {
        // オブジェクト構築と構築時の初期値が正しいことを確認
        Assert.AreEqual(0, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(string.Empty, ViewModel.Status.Value);
    }

    [TestMethod("2.開始(1)")]
    public void StartTest1()
    {
        // 開始時の状態が正しいことを確認
        ViewModel.StartCommand.Execute();
        var startRequest = ViewModel.StartRequest.Value;
        Assert.IsNotNull(startRequest);
        Assert.AreEqual(5, startRequest.RowCount);
        Assert.AreEqual(6, startRequest.ColumnCount);
        Assert.AreEqual(23, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(StatusType.Playing.ToString(), ViewModel.Status.Value);
    }

    [TestMethod("3.開始(2)")]
    public void StartTest2()
    {
        // 開始→セルオープン後に開始を行い、その状態が開始直後の状態であることを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(7); //Status.Failure
        ViewModel.StartCommand.Execute();
        Assert.AreEqual(23, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(StatusType.Playing.ToString(), ViewModel.Status.Value);
    }

    [TestMethod("4.開く(1)")]
    public void OpenTest1()
    {
        // セルオープン実行後の状態が正しいことを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(0);
        var openRequest = ViewModel.OpenRequest.Value;
        Assert.IsNotNull(openRequest);
        Assert.AreEqual(1, openRequest.Cells.Count());
        Assert.AreEqual(22, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(StatusType.Playing.ToString(), ViewModel.Status.Value);
    }

    [TestMethod("5.開く2")]
    public void OpenTest2()
    {
        // セルオープン実行で複数のセルが開かれた際の状態が正しいことを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(24);
        var openRequest = ViewModel.OpenRequest.Value;
        Assert.IsNotNull(openRequest);
        Assert.AreEqual(11, openRequest.Cells.Count());
    }

    [TestMethod("6.クリア")]
    public void SuccessTest()
    {
        // 手順実行後にクリア状態になっていることを確認
        ViewModel.StartCommand.Execute();
        var indexes = new[] { 0, 1, 2, 3, 4, 5, 8, 12, 13, 14, 16, 18, 23, 28 };
        foreach (var index in indexes)
        {
            ViewModel.OpenCommand.Execute(index);
        }
        Assert.AreEqual(0, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(StatusType.Success.ToString(), ViewModel.Status.Value);
    }

    [TestMethod("7.ゲームオーバー(1)")]
    public void FailureTest1()
    {
        // 爆弾セルオープン後にゲームオーバー状態になっていることを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(6);
        Assert.AreEqual(23, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(StatusType.Failure.ToString(), ViewModel.Status.Value);
    }

    [TestMethod("8.ゲームオーバー(2)")]
    public void FailureTest2()
    {
        // 通常セルオープン→爆弾セルオープンでゲームオーバー状態になっていることを確認
        ViewModel.StartCommand.Execute();
        ViewModel.OpenCommand.Execute(0);
        ViewModel.OpenCommand.Execute(6);
        Assert.AreEqual(22, ViewModel.RemainingCellCount.Value);
        Assert.AreEqual(StatusType.Failure.ToString(), ViewModel.Status.Value);
    }
}
