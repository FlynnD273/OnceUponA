using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
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

  private bool isPaused;
  public bool IsPaused
  {
    get => isPaused; set
    {
      isPaused = value;
      if (isPaused)
      {
        Time.timeScale = 0;
      }
      else
      {
        Time.timeScale = 1;
      }
      PauseChanged?.Invoke();
    }
  }

  public event Action PauseChanged;
  public event Action ResetOccurred;
  public event Action SaveStateOccurred;

  private AudioSource audioSource;

  private bool justSwapped;
  private bool justActivated;

  private bool save;
  private bool reset;

  void Awake()
  {
    Manager = this;
    audioSource = GetComponent<AudioSource>();
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
  {
    SaveState();
  }

  void Update()
  {
    if (!IsPaused && Input.GetButtonDown("Cancel"))
    {
      IsPaused = true;
    }

    // PLACEHOLDER the pause menu UI will handle unpausing when completed
    else if (IsPaused && Input.GetButtonDown("Cancel"))
    {
      IsPaused = false;
    }

    if (IsPaused) { return; }
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
      SaveStateOccurred?.Invoke();
    }
    if (reset)
    {
      reset = false;
      ResetOccurred?.Invoke();
    }
    if (justSwapped && justActivated)
    {
      PlayTurnSound();
      SaveState();
    }
    justSwapped = false;
    justActivated = false;
  }

  public void PlayTurnSound()
  {
    audioSource.clip = GameManager.Manager.PageTurnClips[Random.Range(0, PageTurnClips.Length)];
    audioSource.Play();
  }

  public void LoadLevel() {
    IsPaused = false;
    PlayTurnSound();
    SceneManager.LoadScene(1);
  }

  public void LoadTitle() {
    IsPaused = false;
    PlayTurnSound();
    SceneManager.LoadScene(0);
  }

  public void Reset()
  {
    reset = true;
  }

  public void SaveState()
  {
    save = true;
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
