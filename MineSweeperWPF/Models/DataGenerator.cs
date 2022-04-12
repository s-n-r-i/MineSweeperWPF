namespace MineSweeperWPF.Models;

/// <summary>
/// 初期データジェネレーター
/// </summary>
public class DataGenerator : IDataGenerator
{
    /// <summary>
    /// 重複無しの配列作成
    /// </summary>
    /// <param name="count"></param>
    /// <param name="rangeMax"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public IEnumerable<int> GetRandomIntArray(int count, int rangeMax)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (rangeMax < 1)
            throw new ArgumentOutOfRangeException(nameof(rangeMax));

        if (count > rangeMax)
            throw new ArgumentException("count > rangeMax");

        if (count == 0)
        {
            return Enumerable.Empty<int>();
        }

        if (count == rangeMax)
        {
            return Enumerable.Range(0, rangeMax).ToArray();
        }

        var results = new List<int>();
        var random = new Random();

        while (results.Count < count)
        {
            if (random.Next(0, rangeMax) is var next && !results.Any(x => x == next)) //乱数は0以上rangeMax未満
            {
                results.Add(next);
            }
        }

        return results.OrderBy(x => x).ToArray();
    }
}
