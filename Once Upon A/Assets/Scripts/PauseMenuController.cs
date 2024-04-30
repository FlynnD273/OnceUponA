using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
  private Canvas canvas;
  private CurlyController curly;
  private PauseScaler scale;

  void Awake()
  {
    canvas = GetComponentInChildren<Canvas>();
  }

  void Start()
  {
    curly = GetComponentInChildren<CurlyController>();
    scale = GetComponentInChildren<PauseScaler>();
    canvas.gameObject.SetActive(false);
    GameManager.Manager.PauseChanged += OnPauseChanged;
  }

  private void OnPauseChanged()
  {
    canvas.gameObject.SetActive(GameManager.Manager.IsPaused);
    if (GameManager.Manager.IsPaused)
    {
      curly.targetOffset = 0;
      curly.Offset = -20;
      scale.targetScale = 1;
      scale.Scale = 1.1f;
    }
  }

  public void UnPause()
  {
    GameManager.Manager.IsPaused = false;
  }

  public void LoadTitle()
  {
    GameManager.Manager.LoadTitle();
  }

  public void Restart()
  {
    UnPause();
    GameManager.Manager.Reset();
  }

  void OnDestroy()
  {
    GameManager.Manager.PauseChanged -= OnPauseChanged;
  }
}
