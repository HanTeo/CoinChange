﻿using System.Collections.Generic;
using CoinChangeCalculator;
using Lib;

namespace VendingMachine
{
    public class VendingMachine
    {
        public Change AvailableChange { get; private set; }
        public Change UserChange { get; private set; }
        public Dictionary<Drink, int> Drinks { get; private set; } 

        public VendingMachine()
        {
            AvailableChange = new Change();
            UserChange = new Change();
            Drinks = new Dictionary<Drink, int>();
        }

        public void InsertCoin(Coin coin)
        {
            UserChange.Add(coin);
        }

        public MachineEvent BuyCan(Drink choice)
        {
            if(choice == null) return MachineEvent.DrinkNotFound;

            // Find the quantity of the drink
            int quantity;
            if (!Drinks.TryGetValue(choice, out quantity))
            {
                return MachineEvent.DrinkNotFound;
            }

            // Check if the user has inserted sufficient credits
            if (UserChange.Value < choice.Price)
            {
                return MachineEvent.InsufficientCoins;
            }
        
            if (quantity == 0)
            {
                return MachineEvent.OutOfStock;
            }

            Change output;
            // Give exact change
            if (choice.Price.MakeChange(UserChange, out output))
            {
                UserChange.Subtract(output);
                AvailableChange.Add(output);
            }
            else // Make Change from machine available change
            {
                var combined = new Change(AvailableChange).Add(UserChange);

                var expectedChange = UserChange.Value - choice.Price;
                if (!expectedChange.MakeChange(combined, out output))
                {
                    return MachineEvent.NoChange;
                }

                AvailableChange.Add(UserChange);
                AvailableChange.Subtract(output);
                UserChange = output;
            }

            // Update the drinks quantity
            if (quantity > 1) 
            {
                Drinks[choice] = quantity - 1;
            }
            else if (quantity == 1)
            {
                Drinks.Remove(choice); 
            }

            return MachineEvent.Success;
        }
    }
}
