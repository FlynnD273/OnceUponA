using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public GameObject Target;

    public void Update()
    {
        if (GameManager.Manager.IsPaused) { return; }
        transform.position = Target.transform.position;
        transform.rotation = Target.transform.rotation;
    }
}
