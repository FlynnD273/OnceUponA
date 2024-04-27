using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Utils;

public class TextSpacingController : MonoBehaviour
{
  private float width;
  public float Width { get => width; private set => width = value; }

  public GameObject StaticTextPrefab;
  public DynamicText[] Children;
  private string placeholderString = "$$";
  private List<DynamicText> allChildren;
  private List<float> extraWidth;

  private BoxCollider2D coll;

  private DynamicText CreateStaticText(string text)
  {
    string subtext = text;

    var go = Instantiate(StaticTextPrefab);
    go.name = $"{gameObject.name}:{subtext}";
    var gText = go.GetComponent<TextMesh>();
    gText.text = subtext;
    return go.GetComponent<DynamicText>();
  }

  // Start is called before the first frame update
  void Start()
  {
    GetComponent<DynamicText>().TargetPosition = transform.position;
    coll = GetComponent<BoxCollider2D>();
    TextMesh textMesh = GetComponent<TextMesh>();
    allChildren = new();
    extraWidth = new();
    string text = textMesh.text;

    int startIndex = 0;
    int dynamicIndex = 0;
    for (int i = 0; i < text.Length - placeholderString.Length + 1; i++)
    {
      if (placeholderString == text.Substring(i, placeholderString.Length))
      {
        if (startIndex != i)
        {
          allChildren.Add(CreateStaticText(text.Substring(startIndex, i - startIndex)));
          extraWidth.Add(0);
        }
        if (dynamicIndex < Children.Length)
        {
          allChildren.Add(Children[dynamicIndex]);
        }
        else
        {
          allChildren.Add(CreateStaticText("null"));
        }
        if ((i > 0 && text[i - 1] != ' ') || (i >= placeholderString.Length && text.Substring(i - placeholderString.Length - 1, placeholderString.Length) != placeholderString))
        {
          extraWidth.Add(0);
        }
        else
        {
          extraWidth.Add(-Constants.CharWidths[' ']);
        }
        dynamicIndex++;
        startIndex = i + placeholderString.Length;
        i += placeholderString.Length - 1;
      }
    }

    if (startIndex < text.Length)
    {
      string subtext = text.Substring(startIndex, text.Length - startIndex);
      allChildren.Add(CreateStaticText(subtext));

      extraWidth.Add(0);
    }

    textMesh.text = "";

    foreach (var child in allChildren)
    {
      child.TextChanged += UpdateAll;
      child.VisibilityChanged += UpdateVis;
      child.transform.SetParent(transform);
    }
  }

  private bool hasInit = false;


  void Update()
  {
    if (GameManager.Manager.IsPaused) { return; }

    if (!hasInit)
    {
      hasInit = true;
      UpdateAll();
      SnapPosition();
    }
  }

  private void SnapPosition()
  {
    foreach (var child in allChildren)
    {
      child.transform.localPosition = child.TargetPosition;
    }
  }

  private void UpdateVis()
  {
    UpdateAll();
    SnapPosition();
  }

  private void UpdateAll()
  {
    float spacing = 0;
    for (int i = 0; i < allChildren.Count; i++)
    {
      DynamicText child = allChildren[i];
      float extraSpace = extraWidth[i];
      child.TargetPosition = new Vector3(spacing, 0, 1);
      child.transform.localScale = new Vector3(1, 1, 1);
      child.transform.rotation = transform.rotation;
      if (child.Width == 0)
      {
        spacing += extraSpace;
      }
      else
      {
        spacing += child.Width;
      }
    }

    if (coll != null)
    {
      coll.size = new Vector2(spacing, coll.size.y);
      coll.offset = new Vector2(spacing / 2, coll.offset.y);
    }
    Width = spacing;
  }
}
