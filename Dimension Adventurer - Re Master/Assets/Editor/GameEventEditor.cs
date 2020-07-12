using UnityEditor;
using UnityEngine;
using DimensionAdventurer.Event;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEvent script = (GameEvent) target;
        if (GUILayout.Button("Raise"))
        {
            script.Raise();
        }
    }
}
