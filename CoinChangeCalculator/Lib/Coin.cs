using System;

namespace Lib
{
    public class Coin : IComparable<Coin>, IEquatable<Coin>
    {
        public int Value { get; private set; }

        public string Denomination { get; private set; }

        public Coin(string denomination, int value)
        {
            Value = value;
            Denomination = denomination;
        }

        public Coin(Coin coin)
        {
            Denomination = coin.Denomination;
            Value = coin.Value;
        }

        int IComparable<Coin>.CompareTo(Coin other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        bool IEquatable<Coin>.Equals(Coin other)
        {
            return Value.Equals(other.Value) && (Denomination == other.Denomination);
        }

        public override string ToString()
        {
            return Denomination;
        }
    }
}
