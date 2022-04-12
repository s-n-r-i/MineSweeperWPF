namespace MineSweeperWPF.Models;

/// <summary>
/// 初期データジェネレーター インターフェース
/// </summary>
public interface IDataGenerator
{
    public IEnumerable<int> GetRandomIntArray(int count, int rangeMax);
}
