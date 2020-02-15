
using System.Collections;
using UnityEngine;
/// <summary>
/// A version of the world clock which runs with real world seconds
/// </summary>
public class WorldClockDynamic : WorldClock
{
    /// <summary>
    /// The number of real world seconds that represent a tick
    /// </summary>
    [Tooltip("The number of real world seconds that represent a tick")]
    public int RealSecondsToTick = 5;
    /// <summary>
    /// The Clock will start running on Start
    /// </summary>
    [Tooltip("The Clock will start running on Start")]
    public bool StartRunning;
    //used to control whether the clock is running or not
    bool running;

    #region Public Methods
    public void Start()
    {
        if (StartRunning)
            StartWorldClock();
    }
    /// <summary>
    /// Starts the clock ticking
    /// </summary>
    public void StartWorldClock()
    {
        running = true;
        StartCoroutine("RunTimer");
    }
    /// <summary>
    /// Stops the clock from ticking
    /// </summary>
    /// <remarks>
    /// Useful for pausing the game
    /// </remarks>
    public void StopWorldClock()
    {
        running = false;
    }
    /// <summary>
    /// Moves the CurrentTime forward by <see cref="WorldClockMinutesPerTick"/>. 
    /// Do not recommend calling this method directly. Use the <see cref="StartWorldClock"/>
    /// and <see cref="StopWorldClock"/> methods to control the timer
    /// </summary>
    public override void TickTime()
    {
        if (running)
        {
            if (CurrentTime != null)
                CurrentTime.TickMinutes(GlobalTimeValues.WorldClockMinutesPerTick);
            CheckAlarms();
            StartCoroutine("RunTimer");
        }
    }
    #endregion

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(RealSecondsToTick);
        TickTime();
    }
}