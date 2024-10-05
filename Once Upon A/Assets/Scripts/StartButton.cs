using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : ButtonController
{
  public GameManager Manager;

  public override void OnClick()
  {
    GameManager.Manager.LoadLevel();
  }

  void Update()
  {
    if (Manager.Input.Actions.Jump.WasPressedThisFrame())
    {
      OnClick();
    }
  }
}
