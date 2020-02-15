using UnityEngine;
/// <summary>
/// Controls the number of hours in a day, days in a month and months in a year.
/// Can be customized for a specific world time.
/// </summary>
[System.Serializable]
public class GlobalTime
{
    public int minutesInAnHour = 60;
    public int hoursInADay = 24;
    public int daysInAMonth = 15;
    public int monthsInAYear = 6;
    /// <summary>
    /// The number of world clock minutes that elapse during a tick
    /// </summary>
    [Tooltip("The number of world clock minutes that elapse during a tick")]
    public int WorldClockMinutesPerTick = 5;
    /// <summary>
    /// Returns true if this GlobalTime matches the passed GlobalTime
    /// </summary>
    /// <param name="toMatch">The GlobalTime to check</param>
    /// <returns>Returns true if this GlobalTime matches the passed GlobalTime, false otherwise</returns>
    public bool IsMatch(GlobalTime toMatch)
    {
        bool matches = true;
        if (toMatch.minutesInAnHour != minutesInAnHour) { matches = false; }
        if (toMatch.hoursInADay != hoursInADay) { matches = false; }
        if (toMatch.daysInAMonth != daysInAMonth) { matches = false; }
        if (toMatch.monthsInAYear != monthsInAYear) { matches = false; }
        if (toMatch.WorldClockMinutesPerTick != WorldClockMinutesPerTick) { matches = false; }
        return matches;
    }
}