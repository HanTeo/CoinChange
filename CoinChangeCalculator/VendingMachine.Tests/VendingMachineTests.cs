using System.Linq;
using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachine.Tests
{
    [TestClass]
    public class VendingMachineTests
    {
        // Coins
        private static readonly Coin OnePence = new Coin("1p", value: 1);
        private static readonly Coin FivePence = new Coin("5p", value: 5);
        private static readonly Coin TenPence = new Coin("10p", value: 10);

        // Drinks
        private static readonly Drink Coke = new Drink("Coke", price: 32);
        private static readonly Drink Pepsi = new Drink("Pepsi", price: 30);
        private static readonly Drink SevenUp = new Drink("Coke", price: 25);

        private VendingMachine vendingMachine;

        [TestInitialize]
        public void Init()
        {
            vendingMachine = new VendingMachine();

            // 100 x 1p, 50 x 5p, 50 x 10p
            vendingMachine.AvailableChange.Add(OnePence, count: 100)
                .Add(FivePence, count: 50)
                .Add(TenPence, count: 50);

            // 10 Cans of each Drink
            vendingMachine.Drinks.Add(Coke, 10);
            vendingMachine.Drinks.Add(Pepsi, 10);
            vendingMachine.Drinks.Add(SevenUp, 10);
        }

        [TestMethod]
        public void Cannot_buy_drink_with_insufficient_credits_empty()
        {
            // Arrange
            var remaining = vendingMachine.UserChange.Value;
            var drinkCount = vendingMachine.Drinks.Values.Sum();

            // Act
            var result = vendingMachine.BuyCan(Coke);

            // Assert
            Assert.AreEqual(0, remaining);
            Assert.AreEqual(result, MachineEvent.InsufficientCoins);
            Assert.AreEqual(drinkCount, vendingMachine.Drinks.Values.Sum());
        }

        [TestMethod]
        public void Cannot_buy_drink_with_insufficient_credits()
        {
            // Arrange
            var drinkCount = vendingMachine.Drinks.Values.Sum();
            vendingMachine.InsertCoin(OnePence);
            vendingMachine.InsertCoin(FivePence);
            vendingMachine.InsertCoin(TenPence);
            var coins = vendingMachine.UserChange.ToString();

            // Act
            var result = vendingMachine.BuyCan(Coke);

            // Assert
            Assert.AreEqual(coins, vendingMachine.UserChange.ToString());
            Assert.AreEqual(result, MachineEvent.InsufficientCoins);
            Assert.AreEqual(drinkCount, vendingMachine.Drinks.Values.Sum());
        }

        [TestMethod]
        public void Cannot_buy_non_existent_drink()
        {
            // Arrange
            var originalCount = vendingMachine.Drinks.Values.Sum();

            // Act
            var result = vendingMachine.BuyCan(new Drink("Does Not Exist", 100));
            var nullResult = vendingMachine.BuyCan(null);

            // Assert
            Assert.AreEqual(result, MachineEvent.DrinkNotFound);
            Assert.AreEqual(nullResult, MachineEvent.DrinkNotFound);
            Assert.AreEqual(originalCount, vendingMachine.Drinks.Values.Sum());
        }

        [TestMethod]
        public void Successful_buy_can_exact_change()
        {
            // Arrange
            var originalCount = vendingMachine.Drinks.Values.Sum();
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            var credits = vendingMachine.AvailableChange.Value;

            // Act
            var result = vendingMachine.BuyCan(Pepsi);

            // Assert
            Assert.AreEqual(result, MachineEvent.Success);
            // Drinks capacity reduced by 1
            Assert.AreEqual(originalCount - 1, vendingMachine.Drinks.Values.Sum());
            // No remaining credits as exact amount taken
            Assert.AreEqual("empty", vendingMachine.UserChange.ToString());
            // Machine credit increases by the price amount
            Assert.AreEqual(credits + Pepsi.Price, vendingMachine.AvailableChange.Value);
        }

        [TestMethod]
        public void Successful_buy_can_correct_change()
        {
            // Arrange : 40p Inserted
            var originalCount = vendingMachine.Drinks.Values.Sum();
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            var machineBalance = vendingMachine.AvailableChange.Value;

            // Act : buys Coke for 32p
            var result = vendingMachine.BuyCan(Coke);

            // Assert : expect 8p change, machine credits increase by price of Coke sold
            Assert.AreEqual(result, MachineEvent.Success);
            Assert.AreEqual(originalCount -1 , vendingMachine.Drinks.Values.Sum());
            Assert.AreEqual(8, vendingMachine.UserChange.Value);
            Assert.AreEqual(machineBalance + Coke.Price, vendingMachine.AvailableChange.Value);
        }

        [TestMethod]
        public void Cannot_make_change()
        {
            // Arrange : 40p Inserted but no change in machine
            vendingMachine.AvailableChange.Clear();
            var originalCount = vendingMachine.Drinks.Values.Sum();
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            var machineBalance = vendingMachine.AvailableChange.Value;
            var userChange = vendingMachine.UserChange.ToString();

            // Act : buys Coke for 32p
            var result = vendingMachine.BuyCan(Coke);

            // Assert : user credits unchanged, machine credits unchanged
            Assert.AreEqual(result, MachineEvent.NoChange);
            Assert.AreEqual(originalCount, vendingMachine.Drinks.Values.Sum());
            Assert.AreEqual(userChange, vendingMachine.UserChange.ToString());
            Assert.AreEqual(machineBalance, vendingMachine.AvailableChange.Value);
        }

        [TestMethod]
        public void Cannot_make_change_from_denomination()
        {
            // Arrange : 40p Inserted but no appropriate change in machine as 1p or 2p need to make 8p
            vendingMachine.AvailableChange.Clear();
            vendingMachine.AvailableChange.Add(TenPence, 10);
            vendingMachine.AvailableChange.Add(FivePence, 10);
            var originalCount = vendingMachine.Drinks.Values.Sum();
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            vendingMachine.InsertCoin(TenPence);
            var machineBalance = vendingMachine.AvailableChange.Value;
            var userChange = vendingMachine.UserChange.ToString();

            // Act : buys Coke for 32p
            var result = vendingMachine.BuyCan(Coke);

            // Assert : expects fail, user credits unchanged, machine credits unchanged
            Assert.AreEqual(result, MachineEvent.NoChange);
            Assert.AreEqual(originalCount, vendingMachine.Drinks.Values.Sum());
            Assert.AreEqual(userChange, vendingMachine.UserChange.ToString());
            Assert.AreEqual(machineBalance, vendingMachine.AvailableChange.Value);
        }
    }
}
