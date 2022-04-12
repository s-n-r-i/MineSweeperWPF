using MineSweeperWPF.Models;
using Prism.Mvvm;
using Reactive.Bindings;

namespace MineSweeperWPF.ViewModels;

public class MainViewModel : BindableBase
{
    /// <summary>
    /// 残セル数
    /// </summary>
    public ReactivePropertySlim<int> RemainingCellCount { get; } = new();

    /// <summary>
    /// 状態
    /// </summary>
    public ReactivePropertySlim<string> Status { get; } = new(string.Empty);

    /// <summary>
    /// 開始リクエスト
    /// </summary>
    public ReactivePropertySlim<StartRequest?> StartRequest { get; } = new();

    /// <summary>
    /// 開くリクエスト
    /// </summary>
    public ReactivePropertySlim<OpenRequest?> OpenRequest { get; } = new();

    /// <summary>
    /// 開始コマンド
    /// </summary>
    public ReactiveCommand StartCommand { get; } = new();

    /// <summary>
    /// 開くコマンド
    /// </summary>
    public ReactiveCommand<object> OpenCommand { get; } = new();

    /// <summary>
    /// MineSweeperモデル
    /// </summary>
    private IMineSweeper MineSweeper { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="mineSweeper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MainViewModel(IMineSweeper mineSweeper)
    {
        MineSweeper = mineSweeper ?? throw new ArgumentNullException(nameof(mineSweeper));
        StartCommand.Subscribe(OnStart);
        OpenCommand.Subscribe(OnOpen);
    }

    /// <summary>
    /// 開始押下
    /// </summary>
    private void OnStart()
    {
        MineSweeper.Start(5, 6, 7); //引数は定数化、もしくは画面の設定値をあてる
        StartRequest.Value = new(MineSweeper.RowCount, MineSweeper.ColumnCount);
        Update();
    }

    /// <summary>
    /// 開く押下
    /// </summary>
    /// <param name="parameter"></param>
    private void OnOpen(object parameter)
    {
        if (!int.TryParse(parameter.ToString(), out var index))
        {
            return;
        }
        var cells = MineSweeper.Open(index);
        Update();
        OpenRequest.Value = new(cells);

        if (MineSweeper.Status is StatusType.Failure or StatusType.Success)
        {
            var remainingCells = MineSweeper.GetRemainingCells();
            OpenRequest.Value = new(remainingCells);
        }
    }

    /// <summary>
    /// 状態・残セル数更新
    /// </summary>
    private void Update()
    {
        RemainingCellCount.Value = MineSweeper.RemainingCellCount;
        Status.Value = MineSweeper.Status.ToString();
    }
}
