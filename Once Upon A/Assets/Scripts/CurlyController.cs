using UnityEngine;
using UnityEngine.UI;

public class CurlyController : MonoBehaviour
{
    public ExpDamp Offset { get; private set; }

    private RectTransform trans;
    private RectTransform twinTrans;
    private GameObject twin;


    public void Awake()
    {
        twin = new GameObject("Curly 2");
        Image img = twin.AddComponent<Image>();
        Image thisImg = GetComponent<Image>();
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

        Offset = new(0, 0, () =>
        {
            trans.offsetMin = new Vector2(Offset.Value, trans.offsetMin.y);
            trans.offsetMax = new Vector2(Offset.Value, trans.offsetMax.y);
            twinTrans.offsetMin = new Vector2(-Offset.Value, twinTrans.offsetMin.y);
            twinTrans.offsetMax = new Vector2(-Offset.Value, twinTrans.offsetMax.y);
        });
    }

    public void Update()
    {
        _ = Offset.Next(15, Time.unscaledDeltaTime);
    }
}
