using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PrefabOverrideReverter : MonoBehaviour
{
    [MenuItem("Tools/Revert Override on Selected GameObject")]
    public static void RevertOverride()
    {
        string[] prefabs = { "Corrupt text", "Danger text", "HeldWord", "Hint", "Slot", "Static Text", "Text" };
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Revert Prefab Overrides");

        foreach (GameObject selectedObject in Selection.gameObjects)
        {
            Undo.RecordObject(selectedObject, "Processing prefab");

            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(selectedObject);
            if (prefab == null || !prefabs.Contains(prefab.name)) { continue; }

            AddedComponent newComponent = PrefabUtility.GetAddedComponents(selectedObject)
              .Find(static x => x.instanceComponent is TextMeshPro);
            RemovedComponent prefabComponent = PrefabUtility.GetRemovedComponents(selectedObject)
              .Find(static x => x.assetComponent is TextMeshPro);

            if (newComponent == null || prefabComponent == null) { continue; }

            newComponent.Revert();
            prefabComponent.Revert();
            TextMeshPro newTmp = (TextMeshPro)newComponent.instanceComponent;
            TextMeshPro tmp = selectedObject.GetComponent<TextMeshPro>();
            tmp.text = newTmp.text;
            tmp.color = newTmp.color;
        }
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }
}
