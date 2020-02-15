using System;
using UnityEngine;

/// <summary>
/// Creates a serializeable representation of the time. 
/// Is used to represent a specific game time
/// </summary>
[System.Serializable]
public class TimeData
{
    [SerializeField]
    private GlobalTime myGlobalTime;

    [SerializeField]
    private int minutes;
    [SerializeField]
    private int hours;
    [SerializeField]
    private int days;
    [SerializeField]
    private int months;
    [SerializeField]
    private int years;

    public GlobalTime MyGlobalTime { get => myGlobalTime; }
    public int Minutes { get => minutes; }
    public int Hours { get => hours; }
    public int Days { get => days; }
    public int Months { get => months; }
    public int Years { get => years; }

    public event Action Tick;
    public event Action NewHour;
    public event Action NewDay;
    public event Action NewMonth;
    public event Action NewYear;
    /// <summary>
    /// Create a new TimeData object based on a specific Time
    /// </summary>
    /// <param name="minutes">number of minutes</param>
    /// <param name="hours">number of hours</param>
    /// <param name="day">number of days</param>
    /// <param name="month">number of months</param>
    /// <param name="year">number of years</param>
    public TimeData(int minutes, int hours, int day, int month, int year, GlobalTime globalTimeOfData)
    {
        this.myGlobalTime = globalTimeOfData;

        int sanitizedMinutes = SanitizeMinutes(minutes);
        int sanitizedHours = SanitizeValue(hours, MyGlobalTime.hoursInADay);
        int sanitizedDay = SanitizeValue(day, MyGlobalTime.daysInAMonth);
        int sanitizedMonth = SanitizeValue(month, MyGlobalTime.monthsInAYear);
        int sanitizedYear = year < 0 ? 0 : year;

        this.minutes = sanitizedMinutes;
        this.hours = sanitizedHours;
        this.days = sanitizedDay;
        this.months = sanitizedMonth;
        this.years = sanitizedYear;
    }
    /// <summary>
    /// Prototype pattern. Returns a clone of thios TimeData object
    /// </summary>
    /// <returns></returns>
    public virtual TimeData Clone()
    {
        TimeData toReturn = new TimeData(minutes, hours, days, months, years, MyGlobalTime);
        return toReturn;
    }
    public virtual bool IsEqual(TimeData toCompare)
    {
        bool returnValue = false;
        if (!myGlobalTime.IsMatch(toCompare.myGlobalTime))
            return returnValue;
        if (minutes != toCompare.minutes)
            return returnValue;
        if (hours != toCompare.hours)
            return returnValue;
        if (days != toCompare.days)
            return returnValue;
        if (months != toCompare.months)
            return returnValue;
        if (years != toCompare.years)
            return returnValue;
        return true;
    }
    /// <summary>
    /// Moves this TimeData forward by the specified number of minutes
    /// </summary>
    /// <param name="minutesPerTick">The number of minutes to move forward</param>
    public virtual void TickMinutes(int minutesPerTick)
    {
        //Only moves forward
        if (minutesPerTick < 0)
            minutesPerTick = 0;

        //Ensure a portion of the hour has been added
        if (minutesPerTick > MyGlobalTime.minutesInAnHour)
        {
            Debug.LogWarning("Cannot tick more than an hour at a time. Will only tick 1 hour.");
            minutesPerTick = MyGlobalTime.minutesInAnHour;
        }

        int newMinutes = minutes + minutesPerTick;
        if (newMinutes >= MyGlobalTime.minutesInAnHour)
        {
            minutes = newMinutes - MyGlobalTime.minutesInAnHour;
            AddAnHour();
        }
        else
            minutes += minutesPerTick;

        if (Tick != null)
            Tick();
    }
    /// <summary>
    /// Moves the TimeData object forward an hour and calls any events that many occur during that rollover
    /// </summary>
    public virtual void AddAnHour()
    {
        if (hours + 1 == MyGlobalTime.hoursInADay)
        {
            hours = 0;
            AddADay();
        }
        else
            hours += 1;

        if (NewHour != null)
            NewHour();
    }
    /// <summary>
    /// Moves the TimeData object forward a day and calls any events that many occur during that rollover
    /// </summary>
    public virtual void AddADay()
    {
        if (days + 1 == MyGlobalTime.daysInAMonth)
        {
            days = 0;
            AddAMonth();
        }
        else
            days += 1;

        if (NewDay != null)
            NewDay();
    }
    /// <summary>
    /// Moves the TimeData object forward a month and calls any events that many occur during that rollover
    /// </summary>
    public virtual void AddAMonth()
    {
        if (months + 1 == MyGlobalTime.monthsInAYear)
        {
            months = 0;
            AddAYear();
        }
        else
            months += 1;

        if (NewMonth != null)
            NewMonth();
    }
    /// <summary>
    /// Moves the TimeData object forward a year and calls any events that many occur during that rollover
    /// </summary>
    public virtual void AddAYear()
    {
        years += 1;

        if (NewYear != null)
            NewYear();
    }
    /// <summary>
    /// Advances this TimeData forward to the next day at the given time. Calls the NewDay event. 
    /// </summary>
    /// <param name="startingHourOfNewDay">The hour to start the new day at</param>
    /// <param name="startingMinuteOfNewDay">The minute to start the new day at</param>
    public virtual void AdvanceToNextDayAtTime(int startingHourOfNewDay, int startingMinuteOfNewDay)
    {
        int sanitizedMinutes = SanitizeMinutes(startingMinuteOfNewDay);
        int sanitizedHours = SanitizeValue(startingHourOfNewDay, MyGlobalTime.hoursInADay);

        minutes = sanitizedMinutes;
        hours = sanitizedHours;

        if (days + 1 == MyGlobalTime.daysInAMonth)
        {
            days = 0;
            if (months + 1 == MyGlobalTime.monthsInAYear)
            {
                if (NewYear != null)
                    NewYear();
                months = 0;
                years += 1;
            }
            else
            {
                if (NewMonth != null)
                    NewMonth();
                months += 1;
            }
        }
        days += 1;
        if (NewDay != null)
            NewDay();
    }
    /// <summary>
    /// Check to see if the past TimeData is past this TimeData
    /// </summary>
    /// <param name="time">The timr to check</param>
    /// <returns>true if the passed TimeData is past this TimeData. false otherwise</returns>
    public virtual bool IsAtOrPastTime(TimeData time)
    {
        if (!myGlobalTime.IsMatch(time.myGlobalTime))
        {
            Debug.LogWarning("Cannot compare. These two TimeData objects do not have the same GlobalTime.");
            return false;
        }
        if (time.years < years)
            return false;
        else if (time.years == years)
        {
            if (time.months < months)
                return false;
            else if (time.months == months)
            {
                if (time.days < days)
                    return false;
                else if (time.days == days)
                {
                    if (time.hours < hours)
                        return false;
                    else if (time.hours == hours)
                    {
                        if (time.Minutes < Minutes)
                            return false;
                        else if (time.Minutes == Minutes)
                        {
                            //MatchTime
                            return true;
                        }
                        else return true;
                    }
                    else return true;
                }
                else return true;
            }
            else return true;
        }
        else return true;
    }
    /// <summary>
    /// Check to see if the passed TimeData is before this TimeData
    /// </summary>
    /// <param name="time">the TimeData to check</param>
    /// <returns>true if the passed TimeData is before this TimeData</returns>
    public virtual bool IsBehindCurrentTime(TimeData time)
    {
        if (!myGlobalTime.IsMatch(time.myGlobalTime))
        {
            Debug.LogWarning("Cannot compare. These two TimeData objects do not have the same GlobalTime.");
            return false;
        }
        return !IsAtOrPastTime(time);
    }
    /// <summary>
    /// Duplicates this TimeData and adds the passed time
    /// </summary>
    /// <returns>A clone of this TimeData with the added time</returns>
    public virtual TimeData CopyAndAddTime(int plus_minutes, int plus_hours, int plus_days, int plus_months, int plus_years)
    {
        TimeData toReturn = new TimeData(
            minutes + plus_minutes,
            hours + plus_hours,
            days + plus_days,
            months + plus_months,
            years + plus_years,
            MyGlobalTime
        );
        return toReturn;
    }

    protected virtual int SanitizeMinutes(int mintues)
    {
        int sanitizedMinutes = mintues;

        if (mintues % MyGlobalTime.WorldClockMinutesPerTick != 0)
            sanitizedMinutes = (mintues / MyGlobalTime.WorldClockMinutesPerTick) * MyGlobalTime.WorldClockMinutesPerTick;
        if (mintues > MyGlobalTime.minutesInAnHour)
            sanitizedMinutes = MyGlobalTime.minutesInAnHour - MyGlobalTime.WorldClockMinutesPerTick;
        else if (mintues < 0)
            sanitizedMinutes = 0;

        return sanitizedMinutes;
    }

    protected virtual int SanitizeValue(int value, int maxValue)
    {
        int sanitizedValue = value;

        if (value > maxValue)
            sanitizedValue = maxValue;

        else if (value < 0)
            sanitizedValue = 0;

        return sanitizedValue;
    }
}