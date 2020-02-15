using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public WorldClock ClockToObserve;
    public TextMeshProUGUI displayText;

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
            displayText.text = ClockToObserve.GetFormattedMinutesAndHours();
        else
            Debug.Log("Assign a World Clock to display time");
    }
}
