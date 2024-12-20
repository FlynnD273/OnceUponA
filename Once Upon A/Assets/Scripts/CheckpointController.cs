using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
