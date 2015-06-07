using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Tests
{
    [TestClass]
    public class CoinTests
    {
        [TestMethod]
        public void Coin_implements_icomparable_less()
        {
            // Arrange 
            var coin1 = new Coin("1p", 1);
            var coin2 = new Coin("2p", 2);

            // Act
            var res = ((IComparable<Coin>) coin1).CompareTo(coin2);

            // Assert
            Assert.AreEqual(-1, res);
        }

        [TestMethod]
        public void Coin_implements_icomparable_greater()
        {
            // Arrange 
            var coin1 = new Coin("1p", 1);
            var coin2 = new Coin("2p", 2);

            // Act
            var res = ((IComparable<Coin>)coin1).CompareTo(coin1);

            // Assert
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void Coin_implements_icomparable_equals()
        {
            // Arrange 
            var coin1 = new Coin("1p", 1);
            Coin coin2 = null;

            // Act
            var res = ((IComparable<Coin>)coin1).CompareTo(coin2);

            // Assert
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void Coin_implements_iequatable_equals_value()
        {
            // Arrange 
            var coin1 = new Coin("1p", 1);
            var coin2 = new Coin("1p", 1);

            // Act
            var equals = ((IEquatable<Coin>)coin1).Equals(coin2);

            // Assert
            Assert.IsTrue(equals);
        }

        [TestMethod]
        public void Coin_implements_iequatable_negative()
        {
            // Arrange 
            var coin1 = new Coin("1p", 1);
            var coin2 = new Coin("2p", 2);

            // Act
            var equals = ((IEquatable<Coin>)coin1).Equals(coin2);

            // Assert
            Assert.IsFalse(equals);
        }

        [TestMethod]
        public void Coin_implements_iequatable_negative_denominatio()
        {
            // Arrange 
            var coin1 = new Coin("1p", 1);
            var coin2 = new Coin("something", 1);

            // Act
            var equals = ((IEquatable<Coin>)coin1).Equals(coin2);

            // Assert
            Assert.IsFalse(equals);
        }
    }
}
