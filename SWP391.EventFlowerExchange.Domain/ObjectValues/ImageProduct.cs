using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class ImageProduct
{
    public int ProductId { get; set; }

    public string? LinkImage { get; set; }

    public int Id { get; set; }

    public virtual Product Product { get; set; } = null!;
}
