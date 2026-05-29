 using OnlineOrdering; // or whatever namespace your classes are in

 class Program
{
    static void Main(string[] args)
    {
        // --- Order 1: Domestic customer in the USA ---
        Address address1 = new Address("742 Evergreen Terrace", "Springfield", "IL", "USA");
        Customer customer1 = new Customer("Emily Carter", address1);

        Order order1 = new Order(customer1);
        order1.AddProduct(new Product("Wireless Keyboard", "WK-1042", 49.99, 1));
        order1.AddProduct(new Product("USB-C Hub", "UH-2210", 34.95, 2));
        order1.AddProduct(new Product("Screen Cleaning Kit", "SK-0088", 12.50, 3));

        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine("ORDER 1");
        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine();
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine();
        Console.WriteLine($"Order Total: ${order1.GetTotalCost():F2}  (domestic shipping: $5.00)");
        Console.WriteLine();

        // --- Order 2: International customer outside the USA ---
        Address address2 = new Address("18 Maple Crescent", "Toronto", "Ontario", "Canada");
        Customer customer2 = new Customer("Liam Nakamura", address2);

        Order order2 = new Order(customer2);
        order2.AddProduct(new Product("Mechanical Pencil Set", "MP-5503", 22.00, 2));
        order2.AddProduct(new Product("Hardcover Notebook", "NB-0771", 18.75, 1));

        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine("ORDER 2");
        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine();
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine();
        Console.WriteLine($"Order Total: ${order2.GetTotalCost():F2}  (international shipping: $35.00)");
        Console.WriteLine();
    }
}
