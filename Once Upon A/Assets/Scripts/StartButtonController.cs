using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour
{
  private TextMesh text;
  // Start is called before the first frame update
  void Start()
  {
    text = GetComponent<TextMesh>();
  }

  void OnMouseOver()
  {
    text.color = new Color(1, 1, 1, 0.5f);
  }

  void OnMouseExit()
  {
    text.color = new Color(1, 1, 1);
  }

  void OnMouseDown()
  {
    GameManager.Manager.JustSwapped();
    GameManager.Manager.JustActivated();
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
}
