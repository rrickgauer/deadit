using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Utility;

public static class NumberUtilities
{
    public static string FormatNumberWithCommas(object number)
    {
        return string.Format("{0:#,0.##}", number);
    }

}
