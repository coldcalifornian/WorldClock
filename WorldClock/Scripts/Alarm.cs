using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Example of how to set and indicate that an alarm has occurred
/// </summary>
public class Alarm : MonoBehaviour
{
    /// <summary>
    /// The Clock to listen to
    /// </summary>
    [Tooltip("The Clock to listen to")]
    public WorldClock ClockToSetAlarmOn;
    /// <summary>
    /// An image used to indicate that the alarm has occurred
    /// </summary>
    [Tooltip("The Clock to listen to")]
    public Image Indicator;

    public int alarmMinutes;
    public int alarmHours;
    public int alarmDays;
    public int alarmMonths;
    public int alarmYears;

    public UnityEvent MyAlarmWentOff;

    TimeData data;

    void Start()
    {
        data = new TimeData(alarmMinutes, alarmHours, alarmDays, alarmMonths, alarmYears, ClockToSetAlarmOn.GlobalTimeValues);
        ClockToSetAlarmOn.SetAlarm(data);
        ClockToSetAlarmOn.AlarmTriggered.AddListener(CheckAlarm);
    }

    void CheckAlarm(TimeData alarmFired)
    {
        if (alarmFired.IsEqual(data))
            Indicator.color = Color.red;
    }
}
