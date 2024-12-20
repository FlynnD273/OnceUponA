using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Selectable[] Buttons;

    private Selectable lastSelected;
    private Canvas canvas;
    private CurlyController curly;
    private PauseScaler scale;

    public void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Start()
    {
        curly = GetComponentInChildren<CurlyController>();
        scale = GetComponentInChildren<PauseScaler>();
        canvas.gameObject.SetActive(false);
        GameManager.Manager.PauseChanged += OnPauseChanged;
        foreach (Selectable sel in Buttons)
        {
            AddSelectionListeners(sel);
        }
    }

    private void OnPauseChanged()
    {
        canvas.gameObject.SetActive(GameManager.Manager.IsPaused);
        if (GameManager.Manager.IsPaused)
        {
            GameManager.Manager.Input.Actions.Move.performed += OnNavigate;
            GameManager.Manager.Input.Actions.Jump.performed += OnPress;
            Buttons.First().Select();
            lastSelected = Buttons.First();
            curly.Offset.Value = -20;
            scale.Scale.Value = 1.1f;
        }
        else
        {
            GameManager.Manager.Input.Actions.Move.performed -= OnNavigate;
            GameManager.Manager.Input.Actions.Jump.performed -= OnPress;
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

    public void OnDestroy()
    {
        GameManager.Manager.PauseChanged -= OnPauseChanged;
    }

    public void AddSelectionListeners(Selectable sel)
    {
        EventTrigger ev = sel.GetComponent<EventTrigger>() ?? sel.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry selectEntry = new()
        {
            eventID = EventTriggerType.Select,
        };
        selectEntry.callback.AddListener(OnSelect);
        ev.triggers.Add(selectEntry);

        EventTrigger.Entry deselectEntry = new()
        {
            eventID = EventTriggerType.Deselect,
        };
        deselectEntry.callback.AddListener(OnDeselect);
        ev.triggers.Add(deselectEntry);

        EventTrigger.Entry enterEntry = new()
        {
            eventID = EventTriggerType.PointerEnter,
        };
        enterEntry.callback.AddListener(OnPointerEnter);
        ev.triggers.Add(enterEntry);

        EventTrigger.Entry exitEntry = new()
        {
            eventID = EventTriggerType.PointerExit,
        };
        exitEntry.callback.AddListener(OnPointerExit);
        ev.triggers.Add(exitEntry);
    }

    public void OnSelect(BaseEventData data) { }

    public void OnDeselect(BaseEventData data) { }

    public void OnPointerEnter(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;
        Selectable obj = pointer.pointerEnter.GetComponentInParent<Selectable>() ?? pointer.pointerEnter.GetComponentInChildren<Selectable>();
        pointer.selectedObject = obj.gameObject;
        lastSelected = obj;
    }

    public void OnPointerExit(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;
        pointer.selectedObject = null;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
        }
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
        }
    }
}
