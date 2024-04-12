using System;
using System.Collections.Generic;

namespace Tuan3.Models;

public partial class CartDetail
{
    public int IdCd { get; set; }

    public int? SoldNum { get; set; }

    public int? Hide { get; set; }

    public int? IdCart { get; set; }

    public int? IdPro { get; set; }

    public virtual Cart? IdCartNavigation { get; set; }

    public virtual Product? IdProNavigation { get; set; }
}
