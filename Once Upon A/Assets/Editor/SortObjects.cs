using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class SortObjects : EditorWindow
{
  [MenuItem("Tools/Sort Objects")]
  static void Init()
  {
    SortObjects window = (SortObjects)EditorWindow.GetWindow(typeof(SortObjects));
    window.Show();
  }

  void OnGUI()
  {
    GUILayout.Label("Sort Objects", EditorStyles.boldLabel);

    if (GUILayout.Button("Sort"))
    {
      var objects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
      Undo.RecordObjects(objects, "Sort Objects");
      List<GameObject> sortedObjs = objects.ToList();
      sortedObjs.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

      for (int i = 0; i < sortedObjs.Count; i++)
      {
        GameObject obj = sortedObjs[i];
        obj.transform.SetSiblingIndex(i);
      }
    }
  }
}
