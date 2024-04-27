using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
  private float movement = 0.25f;
  private GameObject player;
  private Vector2 startPos;
  private TextMesh text;
  private LineRenderer line;
  // Start is called before the first frame update
  void Start()
  {
    text = gameObject.GetComponent<TextMesh>();
    line = gameObject.GetComponent<LineRenderer>();
    player = GameObject.Find("Camera");
    startPos = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.Manager.IsPaused) { return; }

    transform.position = startPos + new Vector2(player.transform.position.x - startPos.x, player.transform.position.y - startPos.y) * movement;
    var a = Mathf.Min(1, Mathf.Max(0, 1 - (Mathf.Abs(player.transform.position.x - transform.position.x) - 2) / 20));
    text.color = new Color(text.color.r, text.color.g, text.color.b, a);
    if (line != null)
    {
      line.startColor = new Color(line.startColor.r, line.startColor.g, line.startColor.b, a);
      line.endColor = line.startColor;
    }
  }
}
