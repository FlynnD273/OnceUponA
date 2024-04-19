using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
  public Font font;
  public Dictionary<char, float> fontAdvances;
  private string allChars = " \"#$%&'(*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

  public AudioClip[] PageTurnClips;

  private static GameManager manager;
  public static GameManager Manager
  {
    get => manager;
    set
    {
      if (Manager != null)
      {
        Destroy(value.gameObject);
        return;
      }
      manager = value;
      DontDestroyOnLoad(value);
    }
  }

  public event Action ResetOccurred;
  public event Action SaveStateOccurred;

  private AudioSource audioSource;

  private bool justSwapped;
  private bool justActivated;

  private bool save;

  // Start is called before the first frame update
  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    Manager = this;
    SceneManager.sceneLoaded += OnSceneLoaded;

    fontAdvances = new();
    foreach (char c in allChars)
    {
      font.GetCharacterInfo(c, out CharacterInfo info, 500);
      fontAdvances.Add(c, info.advance / 10f);
    }
  }

  private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
  {
    save = true;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetButtonDown("Reset"))
    {
      Reset();
    }
  }

  void LateUpdate()
  {
    if (save)
    {
      save = false;
      SaveState();
    }
    if (justSwapped && justActivated)
    {
      audioSource.clip = GameManager.Manager.PageTurnClips[Random.Range(0, PageTurnClips.Length)];
      audioSource.Play();
    }
    justSwapped = false;
    justActivated = false;
  }

  public void Reset()
  {
    ResetOccurred?.Invoke();
  }

  public void SaveState()
  {
    SaveStateOccurred?.Invoke();
  }

  public void JustSwapped()
  {
    justSwapped = true;
  }

  public void JustActivated()
  {
    justActivated = true;
  }
}
