using System;
using System.Collections.Generic;

namespace BE_Productos.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly OrderDate { get; set; }

    public int Total { get; set; }

    public int EmployeeId { get; set; }

    public bool Active { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();
}
