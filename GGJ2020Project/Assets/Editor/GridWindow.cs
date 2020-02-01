using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridWindow : EditorWindow
{
    Grid grid;
    public void Init()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Color");
        grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
        GUILayout.EndHorizontal();
    }
}
