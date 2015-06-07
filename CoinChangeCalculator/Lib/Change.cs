using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib
{
    public class Change
    {
        private readonly Dictionary<Coin, int> coins = new Dictionary<Coin, int>();

        public Change()
        {
        }

        public Change(Change change)
        {
            foreach (var c in change.Coins)
            {
                Add(c);
            }
        }

        public int Value
        {
            get
            {
                return Coins.Sum(coin => coin.Value);
            }
        }

        public Change Add(Coin coin, int count = 1)
        {
            if (count < 1)
                return this;

            int value;
            if (coins.TryGetValue(coin, out value))
            {
                coins[coin] = value + count;
            }
            else
            {
                coins[coin] = count;
            }

            return this;
        }

        public IEnumerable<Coin> Denominations
        {
            get
            {
                return coins.Keys;
            }
        }

        public void Clear()
        {
            coins.Clear();
        }

        public bool Subtract(Coin coin, int count = 1)
        {
            if (count < 1)
                return false;

            int value;
            if (!coins.TryGetValue(coin, out value)) return false;

            if (value > count)
            {
                coins[coin] = value - count;
            }
            else if (value == count)
            {
                coins.Remove(coin);
            }
            else
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Coin> Coins
        {
            get
            {
                return coins.SelectMany(kv => Enumerable.Repeat(kv.Key, kv.Value));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var kv in coins.OrderBy(x => x.Key.Value))
            {
                sb.Append(string.Format("{1}x{0} ", kv.Key.Denomination, kv.Value));
            }
            return coins.Any() ? sb.ToString() : "empty";
        }
    }
}
