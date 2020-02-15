using UnityEditor;

[CustomEditor(typeof(WorldClock))]
public class WorldClockInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldClock clock = (WorldClock)target;

        clock.StartAtSpecificTime = EditorGUILayout.Toggle("Start At Specific Time", clock.StartAtSpecificTime);
        if (clock.StartAtSpecificTime)
        {
            clock.startMinutes = EditorGUILayout.IntField("Minutes", clock.startMinutes);
            clock.startHours = EditorGUILayout.IntField("Hours", clock.startHours);
            clock.startDays = EditorGUILayout.IntField("Days", clock.startDays);
            clock.startMonths = EditorGUILayout.IntField("Months", clock.startMonths);
            clock.startYears = EditorGUILayout.IntField("Years", clock.startYears);
        }
    }
}