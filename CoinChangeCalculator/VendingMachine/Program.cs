using System;
using System.Threading;
using Lib;

namespace VendingMachine
{
    class Program
    {
        private static readonly Coin OnePence = new Coin("1p", 1);
        private static readonly Coin FivePence = new Coin("5p", 5);
        private static readonly Coin TenPence = new Coin("10p", 10);

        static void Main(string[] args)
        {
            var vendingMachine = new VendingMachine();

            bool run = true;
            while (run)
            {
                Console.Title = " Han Wei Teo : CME Test";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("==========  Welcome To CME Vending Machine  =========");
                foreach (var kv in vendingMachine.DrinksQuantity)
                {
                    Console.WriteLine("|           {0} : {1} Cans Left                     |", kv.Key, kv.Value);
                }
                Console.WriteLine("|             User Credits:  {0}                     |", vendingMachine.UserChange);
                Console.WriteLine("|             Machine Credits:  {0}                     |", vendingMachine.AvailableChange);
                Console.WriteLine("========      [Please Select An Option]    =========");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("========      Press 1 - Buy Coke - {0}p       =========",  vendingMachine.DrinksPrices["Coke"]);
                Console.WriteLine("========      Press 2 - Buy Pepsi - {0}p       =========", vendingMachine.DrinksPrices["Pepsi"]);
                Console.WriteLine("========      Press 3 - Buy 7Up - {0}p       =========", vendingMachine.DrinksPrices["7Up"]);
                Console.WriteLine("========      Press 4 - Quit               =========");
                Console.WriteLine("========      Press 5 - Insert 1p          =========");
                Console.WriteLine("========      Press 6 - Insert 5p          =========");
                Console.WriteLine("========      Press 7 - Insert 10p         =========");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("====================================================");

                var option = Console.ReadKey();
                switch (option.KeyChar)
                {
                    case '1':
                        DigestInstruction(vendingMachine, "Coke");
                        break;
                    case '2':
                        DigestInstruction(vendingMachine, "Pepsi");
                        break;
                    case '3':
                        DigestInstruction(vendingMachine, "7Up");
                        break;
                    case '4':
                        run = false;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("=============== Bye !!  ==================");
                        Thread.Sleep(500);
                        break;
                    case '5':
                        vendingMachine.InsertCoin(OnePence);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("=============== {0} Inserted ==================", OnePence);
                        Thread.Sleep(500);
                        break;
                    case '6':
                        vendingMachine.InsertCoin(FivePence);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("=============== {0} Inserted ==================", FivePence);
                        Thread.Sleep(500);
                        break;
                    case '7':
                        vendingMachine.InsertCoin(TenPence);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("=============== {0} Inserted ==================", TenPence);
                        Thread.Sleep(500);
                        break;
                }
                Console.Clear();
            }
        }

        private static void DigestInstruction(VendingMachine vendingMachine, string drink)
        {
            switch (vendingMachine.BuyCan(drink))
            {
                case MachineEvent.Success:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=============== Vending......  ==================");
                    Thread.Sleep(1000);
                    break;
                case MachineEvent.NoChange:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("============= Cannot Give Change!!  ==============");
                    Thread.Sleep(1000);
                    break;
                case MachineEvent.InsufficientCoins:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("============= Insert more coins!!  ==============");
                    Thread.Sleep(1000);
                    break;
                case MachineEvent.OutOfStock:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=============== Out Of Stock!! ================");
                    Thread.Sleep(1000);
                    break;
                case MachineEvent.SystemErr:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=============== System Error!! ================");
                    Thread.Sleep(1000);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
