using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtilities {

    /// <summary>
    /// Converts a fractional hour value like 1.25 to 1:15  hours:minutes format
    /// </summary>
    /// <param name="hours">Decimal hour value</param>
    /// <param name="format">An optional format string where {0} is hours and {1} is minutes.</param>
    /// <returns></returns>
    public static string FractionalHoursToString(decimal hours, string format)
    {
        if (string.IsNullOrEmpty(format))
            format = "{0}:{1}";
 
        TimeSpan tspan = TimeSpan.FromHours( (double) hours);            
        return string.Format(format, tspan.Hours, tspan.Minutes);
    }
    /// <summary>
    /// Converts a fractional hour value like 1.25 to 1:15  hours:minutes format
    /// </summary>
    /// <param name="hours">Decimal hour value</param>
    public static string FractionalHoursToString(decimal hours)
    {
        return FractionalHoursToString(hours, null);
    }
     /// <summary>
    /// Displays a long date in friendly notation
    /// </summary>
    /// <param name="Date"></param>
    /// <param name="ShowTime"></param>
    /// <returns></returns>
    public static string FriendlyDateString(DateTime Date, bool ShowTime)
    {

        string FormattedDate = "";
        if (Date.Date == DateTime.Today)
            FormattedDate = "Today"; //Resources.Resources.Today; 
        else if (Date.Date == DateTime.Today.AddDays(-1))
            FormattedDate = "Yesterday"; //Resources.Resources.Yesterday;
        else if (Date.Date > DateTime.Today.AddDays(-6))
            // *** Show the Day of the week
            FormattedDate = Date.ToString("dddd").ToString();
        else
            FormattedDate = Date.ToString("MMMM dd, yyyy");

        if (ShowTime)
            FormattedDate += " @ " + Date.ToString("t").ToLower();

        return FormattedDate;
    }
 
    /// <summary>
    /// Rounds an hours value to a minute interval
    /// 0 means no rounding
    /// </summary>
    /// <param name="minuteInterval">Minutes to round up or down to</param>
    /// <returns></returns>
    public static decimal RoundDateToMinuteInterval(decimal hours, int minuteInterval,
                                                    RoundingDirection direction)
    {
        if (minuteInterval == 0)
            return hours;
 
        decimal fraction = 60 / minuteInterval;
 
        switch (direction)
        {
            case RoundingDirection.Round:
                return Math.Round(hours * fraction, 0) / fraction;        
            case RoundingDirection.RoundDown:
                return Math.Truncate(hours * fraction) / fraction;        
 
        }
        return Math.Ceiling(hours * fraction) / fraction;        
    }
 
    /// <summary>
    /// Rounds a date value to a given minute interval
    /// </summary>
    /// <param name="time">Original time value</param>
    /// <param name="minuteInterval">Number of minutes to round up or down to</param>
    /// <returns></returns>
    public static DateTime RoundDateToMinuteInterval(DateTime time, int minuteInterval,
                                                     RoundingDirection direction)
    {
        if (minuteInterval == 0)
            return time;
 
        decimal interval = (decimal) minuteInterval;
        decimal actMinute = (decimal) time.Minute;
 
        if (actMinute == 0.00M)
            return time;
 
        int newMinutes = 0;
 
        switch(direction)
        {
            case RoundingDirection.Round:
                newMinutes = (int) ( Math.Round(actMinute / interval,0) * interval);             
                break;
            case RoundingDirection.RoundDown:
                newMinutes = (int)(Math.Truncate(actMinute / interval) * interval);      
                break;
            case RoundingDirection.RoundUp:
                newMinutes = (int)(Math.Ceiling(actMinute / interval) * interval);
                break;
        }
 
        // *** strip time 
        time = time.AddMinutes(time.Minute * -1);
        time = time.AddSeconds(time.Second * -1);
        time = time.AddMilliseconds(time.Millisecond * -1);
 
        // *** add new minutes back on            
        return time.AddMinutes(newMinutes);
    }
 
}
 
public enum RoundingDirection
{
    RoundUp,
    RoundDown,
    Round
}
