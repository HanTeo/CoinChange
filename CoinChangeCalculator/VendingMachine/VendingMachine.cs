using System.Collections.Generic;
using Lib;
using CoinChangeCalculator;

namespace VendingMachine
{
    public enum MachineEvent
    {
        Success,
        NoChange,
        InsufficientCoins,
        OutOfStock,
        DrinkNotFound
    }

    public class VendingMachine
    {
        public readonly Change AvailableChange = new Change();
        public Change UserChange = new Change();
        public Dictionary<string, int> DrinksQuantity { get; private set; }
        public Dictionary<string, int> DrinksPrices { get; private set; }

        public VendingMachine()
        {
            // 100 x 1p, 50 x 5p, 50 x 10p
            AvailableChange.Add(new Coin("1p", 1), count: 100)
                .Add(new Coin("5p", 5), count: 50)
                .Add(new Coin("10p", 10), count: 50);

            DrinksQuantity = new Dictionary<string, int>
            {
                { "Coke", 10 },
                { "Pepsi", 10 },
                { "7Up", 10 },
            };

            DrinksPrices = new Dictionary<string, int>
            {
                { "Coke", 32 },
                { "Pepsi", 30 },
                { "7Up", 25 },
            };
        }

        public void InsertCoin(Coin coin)
        {
            UserChange.Add(coin);
        }

        public MachineEvent BuyCan(string choice)
        {
            // Find the price of the drink
            int price;
            if (!DrinksPrices.TryGetValue(choice, out price))
            {
                return MachineEvent.DrinkNotFound;
            }

            // Check if the user has inserted sufficient credits
            if (UserChange.Value < price)
            {
                return MachineEvent.InsufficientCoins;
            }

            // Check if the machine has stock of the drink
            int amount;
            if (!DrinksQuantity.TryGetValue(choice, out amount))
            {
                return MachineEvent.DrinkNotFound;
            }
        
            if (amount == 0)
            {
                return MachineEvent.OutOfStock;
            }

            // Figure out the change
            Change output;
            if (!price.MakeChange(UserChange, out output))
            {
                var combined = new Change(AvailableChange).Add(UserChange);
                var expectedChange = UserChange.Value - price;
                if (!expectedChange.MakeChange(combined, out output))
                {
                    return MachineEvent.NoChange;
                }
                
                AvailableChange.Subtract(output);
                UserChange = output;
            }
            else
            {
                UserChange.Subtract(output);
            }

            // Update the drinks quantity
            if (amount > 1) 
            {
                DrinksQuantity[choice] = amount - 1;
            }
            else if (amount == 1)
            {
                DrinksQuantity.Remove(choice); 
            }

            return MachineEvent.Success;
        }
    }
}
