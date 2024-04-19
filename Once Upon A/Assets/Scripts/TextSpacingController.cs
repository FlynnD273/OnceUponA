using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextSpacingController : MonoBehaviour
{
  public GameObject StaticTextPrefab;
  public DynamicText[] Children;
  private string placeholderString = "$$";
  private List<DynamicText> allChildren;
  
  private BoxCollider2D coll;

  // Start is called before the first frame update
  void Start()
  {
    coll = GetComponent<BoxCollider2D>();
    TextMesh textMesh = GetComponent<TextMesh>();
    allChildren = new();
    string text = textMesh.text;

    int startIndex = 0;
    int dynamicIndex = 0;
    for (int i = 0; i < text.Length - placeholderString.Length; i++)
    {
      if (placeholderString == text.Substring(i, placeholderString.Length))
      {
        if (startIndex != i)
        {
          string subtext = text.Substring(startIndex, i - startIndex);

          var go = Instantiate(StaticTextPrefab);
          go.name = $"{gameObject.name}:{subtext}";
          var gText = go.GetComponent<TextMesh>();
          allChildren.Add(go.GetComponent<DynamicText>());
          gText.text = subtext;
        }
        allChildren.Add(Children[dynamicIndex]);
        dynamicIndex++;
        startIndex = i + placeholderString.Length;
        i += placeholderString.Length;
      }
    }

    if (startIndex < text.Length)
    {
      string subtext = text.Substring(startIndex, text.Length - startIndex);

      var go = Instantiate(StaticTextPrefab);
      go.name = $"{gameObject.name}:{subtext}";
      var gText = go.GetComponent<TextMesh>();
      allChildren.Add(go.GetComponent<DynamicText>());
      gText.text = subtext;
    }

    textMesh.text = "";
    foreach (var child in Children)
    {
      child.TextChanged += UpdateAll;
    }
  }

  private void UpdateAll()
  {
    float spacing = 0;
    foreach (var child in allChildren)
    {
      child.transform.position = new Vector3(transform.position.x + spacing * transform.localScale.x, transform.position.y, 1);
      child.transform.localScale = this.transform.localScale;
      spacing += child.Width;
    }

    if (coll != null) {
      coll.size = new Vector2(spacing, coll.size.y);
      coll.offset = new Vector2(spacing / 2, coll.offset.y);
    }
  }
}
