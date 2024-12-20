using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class DynamicText : MonoBehaviour
{
    public event Action TextChanged;
    public event Action VisibilityChanged;
    internal TextMeshPro textMesh;

    private readonly List<VisibilityTrigger> setInvisibleTriggers = new();
    private readonly object visibilityLock = new();
    public bool IsVisible { get; private set; } = true;

    public void SetVisibility(bool value, VisibilityTrigger source)
    {
        lock (visibilityLock)
        {
            if (!value)
            {
                if (!setInvisibleTriggers.Contains(source))
                {
                    setInvisibleTriggers.Add(source);
                }
                IsVisible = false;
                UpdateVisibility(source);
                return;
            }
            else if (setInvisibleTriggers.Contains(source))
            {
                _ = setInvisibleTriggers.Remove(source);
            }

            if (setInvisibleTriggers.Count == 0 && value)
            {
                IsVisible = true;
                UpdateVisibility(source);
            }
        }
    }

    public string Text
    {
        get => textMesh != null ? textMesh.text : "";
        set
        {
            if (textMesh != null)
            {
                textMesh.text = value;
            }
            TextChanged?.Invoke();
        }
    }

    public Color Color
    {
        get => textMesh?.color ?? Color.white;
        set
        {
            if (textMesh != null)
            {
                textMesh.color = value;
            }
            TextChanged?.Invoke();
        }
    }

    public ExpDampVec3 Position;

    public void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        Position = new(transform.position, transform.position, () => transform.position = Position.Value);
    }

    public void FixedUpdate()
    {
        if (GameManager.Manager.IsPaused) { return; }

        transform.localPosition = Position.Next(Constants.StandardAnim, Time.fixedDeltaTime);
    }

    public float Width
    {
        get
        {
            TextSpacingController spacingCont = GetComponent<TextSpacingController>();
            if (spacingCont != null)
            {
                return spacingCont.Width;
            }
            WordSlotController slot = GetComponent<WordSlotController>();
            if (!IsVisible || (slot != null && !slot.IsSwappable && slot.CurrentWord == null))
            {
                return 0;
            }

            if (Text == "")
            {
                return Constants.CharWidths['M'] * 3;
            }
            float sum = 0;
            foreach (char c in Text)
            {
                sum += Constants.CharWidths[c];
            }
            return sum;
        }
    }

    private bool init = false;
    public void Update()
    {
        if (!init)
        {
            init = true;
            GameManager.Manager.ResetOccurred += Reset;
            GameManager.Manager.SaveStateOccurred += SaveState;
        }
    }

    private void UpdateVisibility(VisibilityTrigger source)
    {
        GetComponent<Renderer>().enabled = IsVisible;
        foreach (Collider2D coll in GetComponents<Collider2D>())
        {
            coll.enabled = IsVisible;
        }
        foreach (DynamicText child in GetComponentsInChildren<DynamicText>())
        {
            if (child.gameObject == gameObject)
            {
                continue;
            }
            child.SetVisibility(IsVisible, source);
        }
        VisibilityChanged?.Invoke();
    }

    private void Reset()
    {
        /* lock (visibilityLock) */
        /* { */
        /*   setInvisibleTriggers = new(); */
        /* } */
        /* IsVisible = savedIsVisible; */
    }

    private void SaveState()
    {
        /* savedIsVisible = IsVisible; */
    }

    public void OnDestroy()
    {
        GameManager.Manager.ResetOccurred -= Reset;
        GameManager.Manager.SaveStateOccurred -= SaveState;
    }
}
