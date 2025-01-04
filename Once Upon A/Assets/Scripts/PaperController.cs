using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{
  private float _width = 1;
  public float Width
  {
    get => _width;
    set
    {
      if (_width != value)
      {
        if (value == 0)
        {
          _width = 0;
          transform.localScale = new Vector2(0, transform.localScale.y);
        }
        else
        {
          float buffer = rend.material.GetFloat("_Threshold") * 60;
          _width = value + buffer;
          DebugWidth = Width;
          transform.localPosition = new Vector2(-10, 0);
          transform.localScale = new Vector2(Width / 10, transform.localScale.y);
          UpdateTexture();
        }
      }
    }
  }

  public Texture2D[] Textures;
  public float DebugWidth;

  private void UpdateTexture()
  {
    if (Textures.Length > 0)
    {
      Texture2D tex = Textures[Random.Range(0, Textures.Length)];
      rend.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0.5f));
    }
  }

  private SpriteRenderer rend;

  void Start()
  {
    rend = GetComponent<SpriteRenderer>();
    Width = 0;
  }
}
