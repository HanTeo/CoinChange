namespace VendingMachine
{
    public class Drink
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Drink(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}