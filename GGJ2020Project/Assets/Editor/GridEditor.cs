using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    Grid grid;
    bool initialized = false;
    bool pressed = false;

    private void OnEnable()
    {
        if (initialized) return;
        initialized = true;
        grid = (Grid)target;
        SceneView.duringSceneGui += GridUpdate;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= GridUpdate;
    }

    void GridUpdate(SceneView sceneView)
    {
        Event e = Event.current;

        Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = r.origin;

        if (e.type.Equals(EventType.KeyDown))
        {
            if (e.isKey && e.character == 'a' && !pressed)
            {
                pressed = true;
                GameObject obj;
                Object prefab = PrefabUtility.GetCorrespondingObjectFromSource(Selection.activeObject);

                if (prefab)
                {
                    obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                                                  Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f,
                                                  0.0f);
                    obj.transform.position = aligned;
                    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                }
            }

            else if (e.isKey && e.character == 'd' && !pressed)
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    Undo.DestroyObjectImmediate(obj);
                }
            }

            if (e.isKey && !pressed)
            {
                pressed = true;

                GameObject obj;
                Object prefab = null;

                switch (e.keyCode)
                {
           
                    case KeyCode.Keypad0:
                        prefab = grid.layout[0];
                        Debug.Log(e.keyCode);
                        break;
                    case KeyCode.Keypad1:
                        prefab = grid.layout[1];
                        break;
                    case KeyCode.Keypad2:
                        prefab = grid.layout[2];
                        break;
                    case KeyCode.Keypad3:
                        prefab = grid.layout[3];
                        break;
                    case KeyCode.Keypad4:
                        prefab = grid.layout[4];
                        break;
                    case KeyCode.Keypad5:
                        prefab = grid.layout[5];
                        break;
                    case KeyCode.Keypad6:
                        prefab = grid.layout[6];
                        break;
                    case KeyCode.Keypad7:
                        prefab = grid.layout[7];
                        break;
                    case KeyCode.Keypad8:
                        prefab = grid.layout[8];
                        break;
                    case KeyCode.Keypad9:
                        prefab = grid.layout[9];
                        break;
                }

                if (prefab)
                {
                    obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                                                  Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f,
                                                  0.0f);
                    obj.transform.position = aligned;
                    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                }
            }
        }

        else if (e.type == EventType.KeyUp)
        {
            pressed = false;

        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(" Grid Width ");
        grid.width = EditorGUILayout.FloatField(grid.width, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(" Grid Height ");
        grid.height = EditorGUILayout.FloatField(grid.height, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Custom editor:");
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty("layout");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Open Grid Window", GUILayout.Width(255)))
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
            window.Init();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        SceneView.RepaintAll();
    }
}
