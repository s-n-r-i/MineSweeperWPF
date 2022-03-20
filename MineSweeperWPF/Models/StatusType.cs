namespace MineSweeperWPF.Models;

public enum StatusType
{
    /// <summary>
    /// 初期状態
    /// </summary>
    Init,

    /// <summary>
    /// プレイ中
    /// </summary>
    Playing,

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    Failure,

    /// <summary>
    /// クリア
    /// </summary>
    Success,
}
