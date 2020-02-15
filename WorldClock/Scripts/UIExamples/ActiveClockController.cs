using TMPro;
using UnityEngine;

public class ActiveClockController : MonoBehaviour
{
    /// <summary>
    /// The Text field used to show the time
    /// </summary>
    [Tooltip("The Text field used to show the time")]
    public TextMeshProUGUI displayText;

    void OnEnable()
    {
        ClockLibrary.Instance.ActiveClockTick += DisplayTime;
        DisplayTime();
    }
    void OnDisable()
    {
        ClockLibrary.Instance.ActiveClockTick -= DisplayTime;
    }
    /// <summary>
    /// Displays the time for the given WorldClock
    /// </summary>
    public void DisplayTime()
    {
        WorldClock clock = ClockLibrary.Instance.ActiveClock;
        if (clock != null)
            displayText.text = clock.GetFormattedMinutesAndHours();
        else
            Debug.Log("No Active Clock");
    }
}
