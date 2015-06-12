using System.Collections.Generic;
using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CoinChangeCalculator.Specs
{
    [Binding]
    public class CoinChangeCalculatorSteps
    {
        private int target;
        private Change availableCoins;
        private Dictionary<string, Coin> coins;
        private Change results;

        [Given(@"I have a target amount of (.*) which to make change for")]
        public void GivenIHaveATargetAmountOfWhichToMakeChangeFor(int p0)
        {
            target = p0;
        }

        [Given(@"I have the available coins each have values")]
        public void GivenIHaveTheAvailableCoinsEachHaveValues(Table table)
        {
            coins = new Dictionary<string, Coin>();
            foreach (var row in table.Rows)
            {
                var name = row["Denomination"];
                var value = int.Parse(row["Value"]);
                coins.Add(name, new Coin(name, value));
            }
        }
       

        [Given(@"I have the denominations each are of quantities")]
        public void GivenIHaveTheDenominationsEachAreOfQuantities(Table table)
        {
            availableCoins = new Change();
            foreach (var row in table.Rows)
            {
                var denom = row["Denomination"];
                var value = int.Parse(row["Quantity"]);
                availableCoins.Add(coins[denom], value);
            }
        }

        [When(@"I calculate the change")]
        public void WhenICalculateTheChange()
        {
            target.MakeChange(availableCoins, out results);
        }

        [Then(@"the result should be")]
        public void ThenTheResultShouldBe(Table table)
        {
            var expected = new Change();
            foreach (var row in table.Rows)
            {
                var denom = row["Denomination"];
                var value = int.Parse(row["Quantity"]);
                expected.Add(coins[denom], value);
            }

            Assert.AreEqual(expected.ToString(), results.ToString());
        }
    }
}