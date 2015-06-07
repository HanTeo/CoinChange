using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Tests
{
    [TestClass]
    public class ChangeTests
    {
        private readonly Coin onePence = new Coin("1p", 1);
        private readonly Coin twoPence = new Coin("2p", 2);

        private Change change;

        [TestInitialize]
        public void Setup()
        {
            change = new Change();
        }

        [TestMethod]
        public void Change_can_add_coins()
        {            
            // Act
            change
                .Add(onePence, 2)
                .Add(twoPence);

            // Assert
            Assert.AreEqual(3, change.Coins.Count());
            Assert.IsTrue(change.Coins.Contains(onePence));
            Assert.IsTrue(change.Coins.Contains(twoPence));
        }

        [TestMethod]
        public void Change_has_aggregate_value_property()
        {
            // Arrange
            Change_can_add_coins();

            // Act
            var value = change.Value;

            // Assert
            Assert.AreEqual(4, value);
        }

        [TestMethod]
        public void Change_to_string_shows_constituents()
        {
            // Arrange
            Change_can_add_coins();

            // Act
            var contents = change.ToString();

            // Assert
            Assert.AreEqual("2x1p 1x2p ", contents);
        }

        [TestMethod]
        public void Change_subtract()
        {
            // Arrange
            change
                .Add(onePence, 2)
                .Add(twoPence);
            var original = change.ToString();

            // Act
            var canSubtract = change.Subtract(onePence);

            // Assert
            Assert.IsTrue(canSubtract);
            Assert.AreEqual("2x1p 1x2p ", original);
            Assert.AreEqual("1x1p 1x2p ", change.ToString());
        }

        [TestMethod]
        public void Change_subtract_changes_available_denominations()
        {
            // Arrange
            change
                .Add(onePence, 2)
                .Add(twoPence);
            var originalDenominations = change.Denominations.Count();
            var original = change.ToString();
            
            // Act
            var canSubtract = change.Subtract(twoPence);

            // Assert
            Assert.IsTrue(canSubtract);
            Assert.AreEqual(2, originalDenominations);
            Assert.AreEqual(1, change.Denominations.Count());
            Assert.AreEqual("2x1p 1x2p ", original);
            Assert.AreEqual("2x1p ", change.ToString());
        }

        [TestMethod]
        public void Change_subtract_constrained_by_quantity()
        {
            // Arrange
            change
                .Add(onePence, 2)
                .Add(twoPence);

            // Act
            var canSubtract = change.Subtract(twoPence);
            var before = change.ToString();
            var canSubtractSecond = change.Subtract(twoPence);
            var after = change.ToString();

            // Assert
            Assert.IsTrue(canSubtract);
            Assert.IsFalse(canSubtractSecond);
            Assert.AreEqual(before, after);
        }

        [TestMethod]
        public void Change_clear()
        {
            // Arrange
            change
                .Add(onePence, 2)
                .Add(twoPence);

            // Act
            change.Clear();

            // Assert
            Assert.AreEqual(0, change.Value);
            Assert.AreEqual("empty", change.ToString());
            Assert.AreEqual(0, change.Coins.Count());
            Assert.AreEqual(0, change.Denominations.Count());
        }

        [TestMethod]
        public void Change_copy_ctr()
        {
            // Arrange
            change
                .Add(onePence, 2)
                .Add(twoPence);

            // Act
            var copy = new Change(change);

            // Assert
            Assert.AreNotSame(change, copy);
            Assert.AreEqual(change.Value, copy.Value);
        }
    }
}
