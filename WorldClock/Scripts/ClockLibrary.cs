using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton ClockLibrary for quick clock lookup
/// </summary>
public class ClockLibrary
{
    static ClockLibrary instance;
    /// <summary>
    /// Singleton Reference to the ClockLibrary
    /// </summary>
    /// <value></value>
    public static ClockLibrary Instance
    {
        get
        {
            if (instance == null)
                instance = new ClockLibrary();
            return instance;
        }
    }
    WorldClock activeClock;
    public WorldClock ActiveClock
    {
        get { return activeClock; }
        set
        {
            activeClock = value;
        }
    }
    /// <summary>
    /// Event called when the active clock ticks
    /// </summary>
    public event Action ActiveClockTick;
    /// <summary>
    /// Dictionary with ID for quick lookup
    /// </summary>
    Dictionary<string, WorldClock> allClocks;

    public ClockLibrary()
    {
        allClocks = new Dictionary<string, WorldClock>();
    }

    List<ITick> ObjectsToTick = new List<ITick>();

    #region Public Methods
    /// <summary>
    /// Registers a WorldClock for quick lookup 
    /// </summary>
    /// <param name="clockToRegister"> The <see cref="WorldClock"/> to track in the library</param>
    public void RegisterClock(WorldClock clockToRegister)
    {
        if (!allClocks.ContainsKey(clockToRegister.ClockID))
            allClocks.Add(clockToRegister.ClockID, clockToRegister);
        else
            Debug.LogWarningFormat("WorldClock {0} has already been registered. Do you have multiple instances of the same clock, or instances with the same id?", clockToRegister.ClockID);
    }
    /// <summary>
    /// UnRegisters a WorldClock 
    /// </summary>
    public void UnRegisterClock(WorldClock clockToRegister)
    {
        if (allClocks.ContainsKey(clockToRegister.ClockID))
            allClocks.Remove(clockToRegister.ClockID);
        else
            Debug.LogWarningFormat("WorldClock {0} was not registered.", clockToRegister.ClockID);
    }
    /// <summary>
    /// Returns the WorldClock represented by the passed ID
    /// </summary>
    /// <param name="ID">The ClockID of the <see cref="WorldClock"/> you would like to get a reference to.</param>
    /// <returns>A reference to the <see cref="WorldClock"/>. Null if the clock does not exist.</returns>
    public WorldClock GetClock(string ID)
    {
        if (allClocks.ContainsKey(ID))
            return allClocks[ID];
        else
            return null;
    }
    /// <summary>
    /// Set the active clock. Registers the clock if it is not already registered
    /// </summary>
    /// <param name="clock">A reference to the <see cref="WorldClock"/> to set active</param>
    public void SetActiveClock(WorldClock clock)
    {
        if (!allClocks.ContainsKey(clock.ClockID))
        {
            allClocks.Add(clock.ClockID, clock);
        }
        if (ActiveClock != null)
            ActiveClock.Tick.RemoveListener(ActiveClockTicked);
        ActiveClock = clock;
        ActiveClock.Tick.AddListener(ActiveClockTicked);

    }
    void ActiveClockTicked()
    {
        if (ActiveClockTick != null)
            ActiveClockTick();
    }
    /// <summary>
    /// Sets an already registered clock active by its ID
    /// </summary>
    /// <param name="clockID">the id of the <see cref="WorldClock"/> to set active</param>
    public void SetActiveClock(string clockID)
    {
        if (!allClocks.ContainsKey(clockID))
            Debug.LogWarningFormat("Please register the clock {0} before attempting to set it active.", clockID);
        else
            ActiveClock = allClocks[clockID];
    }
    #endregion
}