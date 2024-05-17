using Assesment.Models;

namespace Assessment.Models
{
    public class OrderModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderType { get; set; }
        public Guid OrderBy { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime ShippedOn { get; set; }
        public bool IsActive { get; set; }
        public ProductModel Product { get; internal set; }
        public SupplierModel Supplier { get; internal set; }
    }
}
