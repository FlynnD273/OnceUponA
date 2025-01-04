using System;
using UnityEngine;
using static Utils.Constants;

public class DangerController : DynamicText
{
  private bool isDangerous;
  public bool IsDangerous
  {
    get => isDangerous;
    set
    {
      isDangerous = value;
      if (IsDangerous)
      {
        trigger.enabled = true;
        line.enabled = true;
        textMesh.color = WordToColor[WordType.Danger];
      }
      else
      {
        trigger.enabled = false;
        line.enabled = false;
        textMesh.color = WordToColor[WordType.White];
      }
    }
  }

  public Vector2 Direction;
  public float Power;
  public float LockTime = 0.5f;
  private BoxCollider2D trigger;
  private LineRenderer line;

  public TriggerLogic DeactivateTrigger;

  public new void Awake()
  {
    base.Awake();
    trigger = GetComponent<BoxCollider2D>();
    VisibilityChanged += DangerVisibilityChanged;
  }

  private void DangerVisibilityChanged()
  {
    if (IsVisible)
    {
      IsDangerous = IsDangerous;
    }
    else
    {
      line.enabled = false;
    }
  }

  public void Start()
  {
    isDangerous = true;
    if (DeactivateTrigger != null)
    {
      DeactivateTrigger.StateChanged += () => IsDangerous = !DeactivateTrigger.State;
    }
    trigger.offset = new Vector2(base.Width / 2, trigger.offset.y);
    trigger.size = new Vector2(base.Width, trigger.size.y);

    line = gameObject.AddComponent<LineRenderer>();
    line.useWorldSpace = false;
    line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
    line.startWidth = 0.15f;
    line.endWidth = line.startWidth;
    line.SetPositions(new Vector3[] { new(0, -25), new(0, -25) });
    line.numCapVertices = 5;
    line.numCornerVertices = 5;
    line.startColor = WordToColor[WordType.Danger];
    line.endColor = line.startColor;

    const float spacing = 10f;
    line.positionCount = (int)(Width / spacing) + 4;
    Vector3[] positions = new Vector3[line.positionCount];
    const float baseline = -25;
    const float height = 12;
    int i;

    for (i = 0; i < positions.Length - 3; i++)
    {
      positions[i] = new Vector2(i * spacing, baseline - ((i % 2) == 0 ? 0 : height));
    }

    float prevY = baseline - (((i - 1) % 2) == 0 ? 0 : height);
    float y = baseline - ((i % 2) == 0 ? 0 : height);
    float l = (Width - ((i - 1) * spacing)) / spacing;
    positions[i] = new Vector2(Width, Mathf.Lerp(prevY, y, l));

    i++;
    positions[i] = new Vector2(Width, baseline);

    i++;
    positions[i] = new Vector2(0, baseline);

    line.SetPositions(positions);
    line.loop = true;
  }

  public void OnTriggerEnter2D(Collider2D coll)
  {
    bool triggered = DeactivateTrigger != null && !DeactivateTrigger.State;
    if (IsVisible && (triggered || coll.gameObject.layer == (int)Layers.Player))
    {
      coll.GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.normalized.x * Power, Direction.normalized.y * Power);
      coll.GetComponent<PlayerController>().LockControls(LockTime);
    }
  }
}
