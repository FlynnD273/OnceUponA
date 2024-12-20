using UnityEngine;

public class PauseScaler : MonoBehaviour
{
    public float StartSize = 1;
    public ExpDamp Scale;

    public void Awake()
    {
        Scale = new(StartSize, 1, () => transform.localScale = Vector3.one * Scale.Value);
    }

    public void Update()
    {
        _ = Scale.Next(10, Time.unscaledDeltaTime);
    }
}
