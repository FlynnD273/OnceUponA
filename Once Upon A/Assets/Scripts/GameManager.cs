using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Awake()
    {
        Manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            Reset();
        }
    }

    public void Reset()
    {
        ResetOccurred?.Invoke();
    }

    public void SaveState()
    {
        SaveStateOccurred?.Invoke();
    }
}
