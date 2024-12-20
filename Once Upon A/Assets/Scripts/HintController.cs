using TMPro;
using UnityEngine;

public class HintController : MonoBehaviour
{
    private readonly float movement = 0.25f;
    private GameObject player;
    private Vector3 startPos;
    private TextMeshPro text;
    private LineRenderer line;

    public void Start()
    {
        text = gameObject.GetComponent<TextMeshPro>();
        line = gameObject.GetComponent<LineRenderer>();
        player = GameObject.Find("Camera");
        startPos = transform.position;
    }

    public void Update()
    {
        if (GameManager.Manager.IsPaused) { return; }

        transform.position = startPos + ((player.transform.position - startPos) * movement);
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        float a = Mathf.Min(1, Mathf.Max(0, 1 - ((Mathf.Abs(player.transform.position.x - transform.position.x) - 2) / 20)));
        text.color = new Color(text.color.r, text.color.g, text.color.b, a);
        if (line != null)
        {
            line.startColor = new Color(line.startColor.r, line.startColor.g, line.startColor.b, a);
            line.endColor = line.startColor;
        }
    }
}
