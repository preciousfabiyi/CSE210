namespace OnlineOrdering
{
    public class Order
    {
        private List<Product> _products;
        private Customer _customer;

        private const double _domesticShipping = 5.00;
        private const double _internationalShipping = 35.00;

        public Order(Customer customer)
        {
            _customer = customer;
            _products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public double GetTotalCost()
        {
            double total = 0;
            foreach (Product product in _products)
            {
                total += product.GetTotalCost();
            }
            total += _customer.LivesInUSA() ? _domesticShipping : _internationalShipping;
            return total;
        }

        public string GetPackingLabel()
        {
            string label = "--- Packing Label ---\n";
            foreach (Product product in _products)
            {
                label += $"  {product.GetName()} (ID: {product.GetProductId()})\n";
            }
            return label.TrimEnd();
        }

        public string GetShippingLabel()
        {
            return $"--- Shipping Label ---\n{_customer.GetShippingLabel()}";
        }
    }
}
