namespace Data.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string Description { get; set; } = null!;

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();
}
