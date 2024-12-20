using UnityEngine;


public class CameraController : MonoBehaviour
{
    public GameObject Target;
    public float HoriThreshold = 2;
    public float VertThreshold = 5;
    public float Speed = 20;

    public float ZoomSpeed = 0.5f;

    private Camera cam;
    private float normalSize;
    private ExpDamp zoom;
    private ExpDampVec3 position;

    public void Awake()
    {
        cam = GetComponent<Camera>();
        normalSize = cam.orthographicSize;
        zoom = new(normalSize, normalSize, () => { cam.orthographicSize = zoom?.Value ?? 1; transform.localScale = Vector3.one * cam.orthographicSize; });
        position = new(Vector3.zero, Vector3.zero, () => transform.position = position?.Value ?? Vector3.zero);
    }

    public void Start()
    {
        position.TargetValue = Target.transform.position;
        position.Value = Target.transform.position;
    }

    public void Update()
    {
        zoom.TargetValue = GameManager.Manager.IsPaused ? normalSize * 1.1f : normalSize;

        _ = zoom.Next(ZoomSpeed, Time.unscaledDeltaTime);
    }

    public void FixedUpdate()
    {
        Vector2 deltaPos = Target.transform.position - transform.position;
        float x = transform.position.x, y = transform.position.y;

        if (deltaPos.x > HoriThreshold)
        {
            x = Target.transform.position.x - HoriThreshold;
        }
        else if (deltaPos.x < -HoriThreshold)
        {
            x = Target.transform.position.x + HoriThreshold;
        }

        if (deltaPos.y > VertThreshold)
        {
            y = Target.transform.position.y - VertThreshold;
        }
        else if (deltaPos.y < -VertThreshold)
        {
            y = Target.transform.position.y + VertThreshold;
        }

        position.TargetValue = new Vector3(x, y, -100);
        _ = position.Next(Speed, Time.fixedDeltaTime);
    }
}
