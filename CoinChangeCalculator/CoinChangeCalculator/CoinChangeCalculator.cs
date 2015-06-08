using System.Linq;
using Lib;

namespace CoinChangeCalculator
{
    public static class CoinChangeCalculator
    {
        public static bool MakeChange(this int target, Change input, out Change output)
        {
            output = new Change();

            // Guard Clauses
            if (target <= 0)
                return false;
            if (input == null || input.Value < target)
                return false;

            // Bottom-Up Approach to Dynamic Programming mapping solution space
            var solutions = MapSolutionSpace(target, input);

            // Selecting the feasible solution
            var availableCoins = new Change(input);
            while (target > 0)
            {
                var coin = solutions[target];

                // Feasibility test
                if (coin == null)
                {
                    output.Clear();
                    return false;
                }

                // Find Next Best SubOptimal solution
                if (!availableCoins.Subtract(coin))
                {
                    Change nextBest;
                    if (target.MakeChange(availableCoins, out nextBest))
                    {
                        foreach (var c in nextBest.Coins)
                        {
                            output.Add(c);
                        }
                        return true;
                    }

                    output.Clear();
                    return false;
                }

                output.Add(coin);
                target = target - coin.Value;
            }

            return true;
        }

        private static Coin[] MapSolutionSpace(int target, Change input)
        {
            var minCoins = Enumerable.Repeat(0, target + 1).ToArray();
            var coinsUsed = Enumerable.Repeat<Coin>(null, target + 1).ToArray();
            foreach (var t in Enumerable.Range(0, target + 1))
            {
                var bestCoin = input.Denominations.FirstOrDefault(x => x == input.Denominations.Min());
                var minCountCount = t;
                foreach (var c in input.Denominations.Where(x => x.Value <= t))
                {
                    var coinCount = minCoins[t - c.Value] + 1;

                    if (coinCount < minCountCount)
                    {
                        minCountCount = coinCount;
                        bestCoin = c;
                    }

                    minCoins[t] = minCountCount;
                    coinsUsed[t] = bestCoin;
                }
            }
            return coinsUsed;
        }
    }
}
