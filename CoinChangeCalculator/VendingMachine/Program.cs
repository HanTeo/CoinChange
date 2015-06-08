using System;
using System.Threading;
using Lib;

namespace VendingMachine
{
    class Program
    {
        // Coins
        private static readonly Coin OnePence = new Coin("1p", value: 1);
        private static readonly Coin FivePence = new Coin("5p", value: 5);
        private static readonly Coin TenPence = new Coin("10p", value: 10);

        // Drinks
        private static readonly Drink Coke = new Drink("Coke", price: 32);
        private static readonly Drink Pepsi = new Drink("Pepsi", price: 30);
        private static readonly Drink SevenUp = new Drink("Coke", price: 25);

        private const int Delay = 500;

        static void Main(string[] args)
        {
            var vendingMachine = new VendingMachine();
            
            // 100 x 1p, 50 x 5p, 50 x 10p
            vendingMachine.AvailableChange.Add(OnePence, count: 100)
                .Add(FivePence, count: 50)
                .Add(TenPence, count: 50);

            // 10 Cans of each Drink
            vendingMachine.Drinks.Add(Coke, 10);
            vendingMachine.Drinks.Add(Pepsi, 10);
            vendingMachine.Drinks.Add(SevenUp, 10);

            var run = true;
            while (run)
            {
                PrintDisplay(vendingMachine);

                ReadInput(vendingMachine, ref run);

                Console.Clear();
            }
        }

        private static void ReadInput(VendingMachine vendingMachine, ref bool run)
        {
            var option = Console.ReadKey();
            switch (option.KeyChar)
            {
                case '1':
                    DigestInstruction(vendingMachine, Coke);
                    break;
                case '2':
                    DigestInstruction(vendingMachine, Pepsi);
                    break;
                case '3':
                    DigestInstruction(vendingMachine, SevenUp);
                    break;
                case '4':
                    run = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=============== Bye !!  ==================");
                    Thread.Sleep(Delay);
                    break;
                case '5':
                    vendingMachine.InsertCoin(OnePence);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=============== {0} Inserted ==================", OnePence);
                    Thread.Sleep(Delay);
                    break;
                case '6':
                    vendingMachine.InsertCoin(FivePence);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=============== {0} Inserted ==================", FivePence);
                    Thread.Sleep(Delay);
                    break;
                case '7':
                    vendingMachine.InsertCoin(TenPence);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=============== {0} Inserted ==================", TenPence);
                    Thread.Sleep(Delay);
                    break;
                case '8':
                    vendingMachine.UserChange.Clear();
                    Console.WriteLine("============== Returning Coins  ================");
                    Thread.Sleep(Delay);
                    break;
            }
        }

        private static void PrintDisplay(VendingMachine vendingMachine)
        {
            Console.Title = " Han Wei Teo";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("=====\t\tWelcome To VendingMachine\t====");
            Console.WriteLine();
            foreach (var kv in vendingMachine.Drinks)
            {
                Console.WriteLine("\t{0}: {1} Left", kv.Key.Name, kv.Value);
            }
            Console.WriteLine();
            Console.WriteLine("\tCredits:  {0}p", vendingMachine.UserChange.Value);
            Console.WriteLine("\tCoins:  {0}", vendingMachine.UserChange);
            Console.WriteLine();
            Console.WriteLine("========\tSelect An Option\t\t====");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("========      Press 1 - Buy Coke - {0}p         =====", Coke.Price);
            Console.WriteLine("========      Press 2 - Buy Pepsi - {0}p        =====", Pepsi.Price);
            Console.WriteLine("========      Press 3 - Buy 7Up - {0}p          =====", SevenUp.Price);
            Console.WriteLine("========                                       =====");
            Console.WriteLine("========      Press 4 - Quit                   =====");
            Console.WriteLine("========                                       =====");
            Console.WriteLine("========      Press 5 - Insert 1p              =====");
            Console.WriteLine("========      Press 6 - Insert 5p              =====");
            Console.WriteLine("========      Press 7 - Insert 10p             =====");
            Console.WriteLine("========                                       =====");
            Console.WriteLine("========      Press 8 - Return Coins           =====");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("====================================================");
        }

        private static void DigestInstruction(VendingMachine vendingMachine, Drink drink)
        {
            switch (vendingMachine.BuyCan(drink))
            {
                case MachineEvent.Success:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=============== Vending...{0} ==================", drink.Name);
                    Thread.Sleep(Delay);
                    break;
                case MachineEvent.NoChange:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("============= Cannot Give Change!!  ==============");
                    Thread.Sleep(Delay);
                    break;
                case MachineEvent.InsufficientCoins:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("============= Insert more credits!!  ==============");
                    Thread.Sleep(Delay);
                    break;
                case MachineEvent.OutOfStock:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=============== Out Of Stock!! ================");
                    Thread.Sleep(Delay);
                    break;
                case MachineEvent.DrinkNotFound:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=============== System Error!! ================");
                    Thread.Sleep(Delay);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
