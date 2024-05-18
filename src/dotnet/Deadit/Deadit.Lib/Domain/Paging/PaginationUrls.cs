using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Domain.Paging;

public class PaginationUrls
{
    public required string Previous { get; set; }
    public required string Next { get; set; }
}
