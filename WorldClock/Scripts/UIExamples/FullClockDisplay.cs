using TMPro;
using UnityEngine;

public class FullClockDisplay : MonoBehaviour
{
    public WorldClock ClockToObserve;
    public TextMeshProUGUI minutesText;
    public TextMeshProUGUI hoursText;
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI monthsText;
    public TextMeshProUGUI yearsText;

    public void Start()
    {
        DisplayTime();
    }

    /// <summary>
    /// Displays the time for the given WorldClock
    /// </summary>
    public void DisplayTime()
    {
        if (ClockToObserve != null)
        {
            minutesText.text = (ClockToObserve.GetMinutes() < 10) ? "0" + ClockToObserve.GetMinutes() : ClockToObserve.GetMinutes().ToString();
            hoursText.text = (ClockToObserve.GetHours() < 10) ? "0" + ClockToObserve.GetHours() : ClockToObserve.GetHours().ToString();
            daysText.text = ClockToObserve.GetDays().ToString();
            monthsText.text = ClockToObserve.GetMonths().ToString();
            yearsText.text = ClockToObserve.GetYears().ToString();
        }
        else
            Debug.Log("Assign a World Clock to display time");
    }
}
