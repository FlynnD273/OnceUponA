using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;

public class InteractHintController : MonoBehaviour
{
  public float speed;
  private float movement = 0.25f;
  private GameObject player;
  private Vector2 startPos;
  private TextMesh text;
  private LineRenderer line;
  private float targetAlpha;
  private float alpha;
  private bool isActivated = false;
  private bool couldActivate = false;
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

    if (couldActivate && Input.GetButtonDown("Swap"))
    {
      isActivated = !isActivated;
    }

    transform.position = startPos + new Vector2(player.transform.position.x - startPos.x, player.transform.position.y - startPos.y) * movement;

    if (isActivated)
    {
      targetAlpha = Mathf.Min(1, Mathf.Max(0, 1 - (Mathf.Abs(player.transform.position.x - transform.position.x) - 2) / 20));
    }
    else
    {
      targetAlpha = 0;
    }

    alpha = Mathf.Lerp(alpha, targetAlpha, speed);

    text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    if (line != null)
    {
      line.startColor = new Color(line.startColor.r, line.startColor.g, line.startColor.b, alpha);
      line.endColor = line.startColor;
    }
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer == (int)Layers.Player)
    {
      couldActivate = true;
    }
  }
  void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.gameObject.layer == (int)Layers.Player)
    {
      couldActivate = false;
    }
  }
}
