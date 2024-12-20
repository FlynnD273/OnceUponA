using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SortObjects : EditorWindow
{
    [MenuItem("Tools/Sort Objects")]
    public static void Init()
    {
        SortObjects window = (SortObjects)GetWindow(typeof(SortObjects));
        window.Show();
    }

    public void OnGUI()
    {
        GUILayout.Label("Sort Objects", EditorStyles.boldLabel);

        if (GUILayout.Button("Sort"))
        {
            GameObject[] objects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            Undo.RecordObjects(objects, "Sort Objects");
            List<GameObject> sortedObjs = objects.ToList();
            sortedObjs.Sort(static (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

            for (int i = 0; i < sortedObjs.Count; i++)
            {
                GameObject obj = sortedObjs[i];
                obj.transform.SetSiblingIndex(i);
            }
        }
    }
}
