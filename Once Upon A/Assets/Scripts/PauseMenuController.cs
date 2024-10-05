using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
  public Selectable DefaultFocus;
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
      DefaultFocus.Select();
      curly.Offset.Value = -20;
      scale.Scale.Value = 1.1f;
    }
  }

  public void UnPause()
  {
    GameManager.Manager.Input.Disable();
    GameManager.Manager.IsPaused = false;
    GameManager.Manager.Input.Enable();
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
