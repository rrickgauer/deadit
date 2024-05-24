using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Utility;

public static class DateTimeUtility
{

    public static string ToDisplayDateOnly(this DateTime date)
    {
        return date.ToString("MM/dd/yyyy");
    }

}
