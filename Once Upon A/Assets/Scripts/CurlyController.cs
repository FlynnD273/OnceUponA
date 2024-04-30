using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using static Utils.Utils;

public class CurlyController : MonoBehaviour
{
  private float offset;
  public float Offset
  {
    get => offset; set
    {
      offset = value;
      trans.offsetMin = new Vector2(Offset, trans.offsetMin.y);
      trans.offsetMax = new Vector2(Offset, trans.offsetMax.y);
      twinTrans.offsetMin = new Vector2(-Offset, twinTrans.offsetMin.y);
      twinTrans.offsetMax = new Vector2(-Offset, twinTrans.offsetMax.y);
    }
  }

  public float targetOffset;
  private RectTransform trans;
  private RectTransform twinTrans;
  private GameObject twin;


  void Start()
  {
    twin = new GameObject("Curly 2");
    var img = twin.AddComponent<Image>();
    var thisImg = GetComponent<Image>();
    trans = GetComponent<RectTransform>();
    twinTrans = twin.GetComponent<RectTransform>();

    img.sprite = thisImg.sprite;
    twin.transform.SetParent(transform.parent);
    twin.transform.SetSiblingIndex(2);
    twinTrans.anchorMin = trans.anchorMin;
    twinTrans.anchorMax = trans.anchorMax;
    twinTrans.offsetMin = trans.offsetMin;
    twinTrans.offsetMax = trans.offsetMax;
    twin.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
  }

  void Update()
  {
    Offset = ExpDamp(Offset, targetOffset, 10);
  }
}
