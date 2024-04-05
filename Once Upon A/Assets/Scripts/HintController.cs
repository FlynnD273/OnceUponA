using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
  private float movement = 0.25f;
  private GameObject player;
  private Vector2 startPos;
  private TextMesh text;
  // Start is called before the first frame update
  void Start()
  {
    text = gameObject.GetComponent<TextMesh>();
    player = GameObject.Find("Camera");
    startPos = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    transform.position = startPos + new Vector2(player.transform.position.x - startPos.x, player.transform.position.y - startPos.y) * movement;
    var a = Mathf.Min(1, Mathf.Max(0, 1 - (Mathf.Abs(player.transform.position.x - transform.position.x) - 2) / 20));
    text.color = new Color(text.color.r, text.color.g, text.color.b, a);
  }
}
