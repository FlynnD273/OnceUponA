using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Utils;
using static Utils.Utils;

public class DynamicText : MonoBehaviour
{
    public event Action TextChanged;
    public event Action VisibilityChanged;
    internal TextMeshPro textMesh;
    private bool savedIsVisible;

    private List<VisibilityTrigger> setInvisibleTriggers = new();
    private object visibilityLock = new();
    private bool isVisible = true;
    public bool IsVisible
    {
        get => isVisible;
    }

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
                isVisible = false;
                UpdateVisibility(source);
                return;
            }
            else if (setInvisibleTriggers.Contains(source))
            {
                setInvisibleTriggers.Remove(source);
            }

            if (setInvisibleTriggers.Count == 0 && value)
            {
                isVisible = true;
                UpdateVisibility(source);
            }
        }
    }

    public string Text
    {
        get
        {
            if (textMesh != null)
            {
                return textMesh.text;
            }
            return "";
        }
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

    void Awake() // DON'T FORGET TO COPY TO DANGER CONTROLLER
    {
        textMesh = GetComponent<TextMeshPro>();
        Position = new(transform.position, transform.position, () => transform.position = Position.Value);
    }

    void FixedUpdate()
    {
        if (GameManager.Manager.IsPaused) { return; }

        transform.localPosition = Position.Next(Constants.StandardAnim, Time.fixedDeltaTime);
    }

    public float Width
    {
        get
        {
            var spacingCont = GetComponent<TextSpacingController>();
            if (spacingCont != null)
            {
                return spacingCont.Width;
            }
            var slot = GetComponent<WordSlotController>();
            if (!IsVisible || (slot != null && !slot.IsSwappable && slot.CurrentWord == null))
            {
                return 0;
            }

            if (Text == "")
            {
                return Constants.CharWidths['M'] * 3;
            }
            float sum = 0;
            foreach (var c in Text)
            {
                sum += Constants.CharWidths[c];
            }
            return sum;
        }
    }

    private bool init = false;
    void Update()
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
        GetComponent<Renderer>().enabled = isVisible;
        foreach (var coll in GetComponents<Collider2D>())
        {
            coll.enabled = IsVisible;
        }
        foreach (var child in GetComponentsInChildren<DynamicText>())
        {
            if (child.gameObject == this.gameObject)
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

    void OnDestroy()
    {
        GameManager.Manager.ResetOccurred -= Reset;
        GameManager.Manager.SaveStateOccurred -= SaveState;
    }
}
