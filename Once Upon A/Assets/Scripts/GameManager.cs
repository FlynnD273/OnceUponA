using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public interface IGameManager
{
    bool IsPaused { get; set; }
    InputSystem Input { get; }

    event Action PauseChanged;
    event Action ResetOccurred;
    event Action SaveStateOccurred;

    void Awake();
    void JustActivated();
    void JustSwapped();
    void LateUpdate();
    void LoadLevel();
    void LoadTitle();
    void PlayTurnSound();
    void Reset();
    void SaveState();
    void Update();
}
public class GameManager : MonoBehaviour, IGameManager
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
            Time.timeScale = isPaused ? 0 : 1;
            PauseChanged?.Invoke();
        }
    }

    public InputSystem Input { get; private set; }

    public event Action PauseChanged;
    public event Action ResetOccurred;
    public event Action SaveStateOccurred;

    private AudioSource audioSource;

    private bool justSwapped;
    private bool justActivated;

    private bool save;
    private bool reset;

    public void Awake()
    {
        Manager = this;
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        Input = new();
        Input.Enable();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SaveState();
    }

    public void Update()
    {
        if (!IsPaused && Input.Actions.Cancel.WasPressedThisFrame())
        {
            IsPaused = true;
        }

        // PLACEHOLDER the pause menu UI will handle unpausing when completed
        else if (IsPaused && Input.Actions.Cancel.WasPressedThisFrame())
        {
            IsPaused = false;
        }

        if (IsPaused) { return; }
    }

    public void LateUpdate()
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
        audioSource.clip = Manager.PageTurnClips[Random.Range(0, PageTurnClips.Length)];
        audioSource.Play();
    }

    public void LoadLevel()
    {
        IsPaused = false;
        PlayTurnSound();
        SceneManager.LoadScene(1);
    }

    public void LoadTitle()
    {
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
