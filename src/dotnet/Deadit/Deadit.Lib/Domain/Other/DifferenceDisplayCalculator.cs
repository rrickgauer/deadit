/********************************************************************************************

This class calculates the appropriate display of the difference between the 2 given dates.

The timing/ordering of the constructor arguments does not matter.

example results: 
    - 2 years ago
    - 7 months ago
    - 21 days ago
    - 8 hours ago
    - 50 minutes ago
*********************************************************************************************/

namespace Deadit.Lib.Domain.Other;

public class DifferenceDisplayCalculator(DateTime date1, DateTime date2)
{
    private const int DaysInYear = 365;
    private const int DaysInMonth = 30;
    private const int MinutesInHour = 60;

    private readonly DateTime _date1 = date1;
    private readonly DateTime _date2 = date2;

    private TimeSpan Difference => _date1 - _date2;

    public string GetDifferenceDisplay()
    {
        int days = GetPositiveInt(Difference.TotalDays);
        int minutes = GetPositiveInt(Difference.TotalMinutes);
        int hours = GetPositiveInt(Difference.TotalHours);
        
        if (days > DaysInYear)
        {
            return GetYearsDisplay(days);
        }

        if (days > DaysInMonth)
        {
            return GetMonthsDisplay(days);
        }

        if (days > 1)
        {
            return GetDaysDisplay(days);
        }

        if (minutes >= MinutesInHour)
        {
            return GetHoursDisplay(hours);
        }

        return GetMinutesDisplay(minutes);
    }



    private static string GetYearsDisplay(int days)
    {
        int years = GetNumberOfYears(days);

        string suffix = years > 1 ? "years" : "year";

        return $"{years} {suffix}";
    }

    private static int GetNumberOfYears(int days)
    {
        int years = 0;

        while (days > DaysInYear)
        {
            years++;
            days -= DaysInYear;
        }

        return years;
    }



    private static string GetMonthsDisplay(int days)
    {
        int months = GetNumberOfMonths(days);

        string suffix = months > 1 ? "months" : "month";

        return $"{months} {suffix}";
    }

    private static int GetNumberOfMonths(int days)
    {
        int months = 0;

        while(days > DaysInMonth)
        {
            months++;
            days -= DaysInMonth;
        }

        return months;
    }


    private static string GetDaysDisplay(int days)
    {
        string suffix = days > 1 ? "days" : "day";

        return $"{days} {suffix}";
    }

    private static string GetHoursDisplay(int hours)
    {
        string suffix = hours > 1 ? "hours" : "hour";

        return $"{hours} {suffix}";
    }

    private static string GetMinutesDisplay(int minutes)
    {
        string suffix = minutes > 1 ? "minutes" : "minute";

        return $"{minutes} {suffix}";
    }

    private static int GetPositiveInt(double value)
    {
        int result = (int)value;

        if (result < 0)
        {
            result *= -1;
        }

        return result;
    }


    public static string FromNow(DateTime date)
    {
        DifferenceDisplayCalculator diff = new(date, DateTime.UtcNow);

        return diff.GetDifferenceDisplay();
    }
}
