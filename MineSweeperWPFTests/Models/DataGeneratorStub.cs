namespace MineSweeperWPF.Models.Tests;

/// <summary>
/// 初期データジェネレーター テスト用stub
/// </summary>
public class DataGeneratorStub : IDataGenerator
{
    public IEnumerable<int> GetRandomIntArray(int _, int __)
    {
        return new[] { 6, 7, 9, 15, 17, 22, 29 };
    }
}
