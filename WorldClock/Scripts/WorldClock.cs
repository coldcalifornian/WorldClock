using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WorldClock : MonoBehaviour
{
    #region Base Clock Fields
    /// <summary>
    /// The values for how time is measured with this clock
    /// </summary>
    [Tooltip("The values for how time is measured with this clock")]
    public GlobalTime GlobalTimeValues;
    /// <summary>
    /// A lookup value for finding a world clock without a direct reference to the monobehaviour
    /// </summary>
    [Tooltip("A lookup value for finding a world clock without a direct reference to the monobehaviour")]
    public string ClockID = Guid.NewGuid().ToString();
    /// <summary>
    /// This clock will be set as the activeClock in the <see cref="ClockLibrary"/> during the Awake method
    /// </summary>
    /// <remarks>
    /// If more that one clock has this value set to true, the last one to be instantiated will be the active clock
    /// </remarks>
    [Tooltip("This clock will be set as the activeClock in the ClockLibrary during the Awake method")]
    public bool StartAsActiveClock;
    [HideInInspector]
    public bool StartAtSpecificTime;
    [HideInInspector]
    public int startMinutes;
    [HideInInspector]
    public int startHours;
    [HideInInspector]
    public int startDays;
    [HideInInspector]
    public int startMonths;
    [HideInInspector]
    public int startYears;
    //The current time of this clock
    TimeData currentTime;
    /// <summary>
    /// The TimeData object representing the current time of this clock.
    /// </summary>
    public TimeData CurrentTime
    {
        get { return currentTime; }
        set
        {
            if (currentTime != null)
            {
                currentTime.Tick -= InvokeTick;
                currentTime.NewHour -= InvokeNewHour;
                currentTime.NewDay -= InvokeNewDay;
                currentTime.NewMonth -= InvokeNewMonth;
                currentTime.NewYear -= InvokeNewYear;
            }
            currentTime = value;
            currentTime.Tick += InvokeTick;
            currentTime.NewHour += InvokeNewHour;
            currentTime.NewDay += InvokeNewDay;
            currentTime.NewMonth += InvokeNewMonth;
            currentTime.NewYear += InvokeNewYear;
        }
    }
    public UnityEvent Tick;
    void InvokeTick() { Tick.Invoke(); }
    public UnityEvent NewHour;
    void InvokeNewHour() { NewHour.Invoke(); }
    public UnityEvent NewDay;
    void InvokeNewDay() { NewDay.Invoke(); }
    public UnityEvent NewMonth;
    void InvokeNewMonth() { NewMonth.Invoke(); }
    public UnityEvent NewYear;
    void InvokeNewYear() { NewYear.Invoke(); }
    #endregion

    #region Alarm Fields
    //Must derive and declare the generic event for unity to show in the inspector
    [System.Serializable]
    public class AlarmEvent : UnityEvent<TimeData> { }
    /// <summary>
    /// This event fires when one of the alarms set by the <see cref="SetAlarm"/> method has passed
    /// </summary>
    [Tooltip("This event fires when one of the alarms set by the SetAlarm method has passed")]
    public AlarmEvent AlarmTriggered;
    //Private reference to the alarms
    List<TimeData> alarms = new List<TimeData>();
    #endregion

    #region Public Methods
    public void Awake()
    {
        if (StartAsActiveClock)
            ClockLibrary.Instance.SetActiveClock(this);
        if (StartAtSpecificTime)
            CurrentTime = new TimeData(startMinutes, startHours, startDays, startMonths, startYears, GlobalTimeValues);
        else if (currentTime == null)
            CurrentTime = new TimeData(0, 0, 0, 0, 0, GlobalTimeValues);
    }
    /// <summary>
    /// Sets this as the active clock in the <see cref="ClockLibrary"/>
    /// </summary>
    public void SetAsActiveClock()
    {
        ClockLibrary.Instance.SetActiveClock(this);
    }
    /// <summary>
    /// Moves the CurrentTime forward by <see cref="WorldClockMinutesPerTick"/>
    /// </summary>
    public virtual void TickTime()
    {
        if (CurrentTime != null)
            CurrentTime.TickMinutes(GlobalTimeValues.WorldClockMinutesPerTick);
        else
            Debug.LogWarning("No Current Time");
        CheckAlarms();
    }
    /// <summary>
    /// Sets an alarm that will fire AlarmTriggered Event
    /// </summary>
    /// <param name="alarm">The time for the alarm</param>
    public virtual void SetAlarm(TimeData alarm)
    {
        if (alarm == null)
        {
            Debug.LogWarning("[WORLDCLOCK] Cannot add a null alarm.");
            return;
        }
        if (alarms.Contains(alarm))
        {
            Debug.LogWarning("[WORLDCLOCK] Alarm already set. Not setting a second one.");
            return;
        }
        if (!GlobalTimeValues.IsMatch(alarm.MyGlobalTime))
        {
            Debug.LogWarning("[WORLDCLOCK] Alarm cannot be set. Global times do not match.");
            return;
        }

        alarms.Add(alarm);
    }
    /// <summary>
    /// Advances the <see cref="CurrentTime"/> to the next day
    /// </summary>
    /// <remarks>
    /// This will fire the NewDay event
    /// </remarks>
    /// <param name="newDayStartMinute">The minute to start the next day at</param>
    /// <param name="newDayStartHour">The hour to start the next day at</param>
    public virtual void JumpToNextDayStartTime(int newDayStartMinute, int newDayStartHour)
    {
        CurrentTime.AdvanceToNextDayAtTime(newDayStartHour, newDayStartMinute);
        CheckAlarms();
    }
    /// <summary>
    /// Advances the CurrentTime to the passed Time. No events will be fired when this jump occurs
    /// </summary>
    /// <param name="toJumpTo">The time to jump to</param>
    public virtual void JumpToTime(int minutes, int hours, int day, int month, int year)
    {
        TimeData newTime = new TimeData(minutes, hours, day, month, year, GlobalTimeValues);
        CurrentTime = newTime;
        CheckAlarms();
    }
    /// <summary>
    /// Get a formatted hours and mintues string for a clock
    /// </summary>
    /// <returns>HH:MM</returns>
    public virtual string GetFormattedMinutesAndHours()
    {
        string hours = (CurrentTime.Hours < 10) ? "0" + CurrentTime.Hours : CurrentTime.Hours.ToString();
        string minutes = (CurrentTime.Minutes < 10) ? "0" + CurrentTime.Minutes : CurrentTime.Minutes.ToString();
        return string.Concat(hours, ':', minutes);
    }
    public int GetMinutes() { return currentTime.Minutes; }
    public int GetHours() { return currentTime.Hours; }
    public int GetDays() { return currentTime.Days; }
    public int GetMonths() { return currentTime.Months; }
    public int GetYears() { return currentTime.Years; }
    public void AddAnHour() { currentTime.AddAnHour(); CheckAlarms(); }
    public void AddADay() { currentTime.AddADay(); CheckAlarms(); }
    public void AddAMonth() { currentTime.AddAMonth(); CheckAlarms(); }
    public void AddAYear() { currentTime.AddAYear(); CheckAlarms(); }
    #endregion

    #region Other
    protected void CheckAlarms()
    {
        List<TimeData> toRemove = new List<TimeData>();
        foreach (TimeData td in alarms)
        {
            if (td.IsAtOrPastTime(CurrentTime))
            {
                if (AlarmTriggered != null)
                {
                    AlarmTriggered.Invoke(td);
                }
                toRemove.Add(td);
            }
        }
        if (toRemove.Count > 0)
        {
            foreach (TimeData td in toRemove)
            {
                alarms.Remove(td);
            }
            alarms = alarms.Where(x => x != null).ToList();
        }
    }
    #endregion
}