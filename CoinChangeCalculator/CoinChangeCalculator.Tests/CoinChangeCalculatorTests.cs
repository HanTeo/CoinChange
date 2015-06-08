using System.Linq;
using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoinChangeCalculator.Tests
{
    [TestClass]
    public class CoinChangeCalculatorTests
    {
        private readonly Coin onePence = new Coin("1p", 1);
        private readonly Coin twoPence = new Coin("2p", 2);
        private readonly Coin fivePence = new Coin("5p", 5);
        private readonly Coin tenPence = new Coin("10p", 10);
        private readonly Coin twentyPence = new Coin("20p", 20);
        private readonly Coin fiftyPence = new Coin("50p", 50);
        private Change availableChange;

        [TestInitialize]
        public void Setup()
        {
            availableChange = new Change();
        }

        [TestMethod]
        public void Positive_optimal()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(onePence)
                .Add(twoPence)
                .Add(fivePence)
                .Add(tenPence)
                .Add(twentyPence)
                .Add(fiftyPence);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(target, change.Value);
            Assert.AreEqual("1x2p 1x10p 1x50p ", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Positive_singular_low()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(onePence, 62);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(target, change.Value);
            Assert.AreEqual("62x1p ", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Positive_singular_high()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(twoPence, 31);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(target, change.Value);
            Assert.AreEqual("31x2p ", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Negative_singular_low()
        {
            // Arrange
            const int target = 61;
            availableChange.Add(twoPence, 50);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, change.Coins.Count());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Positive_pick_optimal_coins()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(onePence, 5)
                .Add(twoPence, 3)
                .Add(fivePence, 5)
                .Add(tenPence, 5)
                .Add(twentyPence, 5)
                .Add(fiftyPence);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(target, change.Value);
            Assert.AreEqual("1x2p 1x10p 1x50p ", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Positive_pick_next_best_solution()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(onePence, 62)
                .Add(twoPence, 3)
                .Add(fivePence, 20)
                .Add(tenPence, 10)
                .Add(twentyPence, 2);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(target, change.Value);
            Assert.AreEqual("1x2p 2x10p 2x20p ", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Negative_insufficient_amount()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(onePence)
                .Add(twoPence)
                .Add(fivePence);
            Change change;
            var original = availableChange.ToString();

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, change.Value);
            Assert.AreEqual("empty", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Negative_missing_denominations_low()
        {
            // Arrange
            const int target = 62;
            availableChange.Add(onePence)
                .Add(fivePence, 4)
                .Add(tenPence, 4)
                .Add(fiftyPence);
            Change change;
            var original = availableChange.ToString();

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsFalse(success);
            Assert.IsTrue(availableChange.Value > target);
            Assert.AreEqual(0, change.Value);
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Negative_missing_denominations_high()
        {
            // Arrange
            const int target = 62;
            availableChange
                .Add(fivePence, 4)
                .Add(tenPence, 4)
                .Add(fiftyPence);
            var original = availableChange.ToString();
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, change.Value);
            Assert.AreEqual("empty", change.ToString());
            Assert.AreEqual(original, availableChange.ToString());
        }

        [TestMethod]
        public void Negative_null_input()
        {
            // Arrange
            const int target = 62;
            availableChange = null;
            Change change;

            // Act
            var success = target.MakeChange(availableChange, out change);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(0, change.Value);
            Assert.AreEqual("empty", change.ToString());
        }

        [TestMethod]
        public void Negative_target_zero_or_negative()
        {
            // Arrange
            availableChange = null;
            Change change;
            Change change2;

            // Act
            var successZero = 0.MakeChange(availableChange, out change);
            var successNegative = (-1).MakeChange(availableChange, out change2);

            // Assert
            Assert.IsFalse(successZero);
            Assert.IsFalse(successNegative);
            Assert.AreEqual(0, change.Value);
            Assert.AreEqual(0, change2.Value);
        }
    }
}
