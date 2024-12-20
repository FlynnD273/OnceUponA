using TMPro;
using UnityEngine;
using static Utils.Constants;

public class HeldWordController : MonoBehaviour
{
    public GameObject Target;
    public float Speed = 0.1f;
    private Vector3 offset = new(0, 1.8f, 0);
    private ExpDampVec3 position;

    private TextMeshPro heldWordMesh;
    private Word heldWord;
    public Word HeldWord
    {
        get => heldWord;
        set
        {
            heldWord = value;
            if (value == null)
            {
                heldWordMesh.text = "";
            }
            else
            {
                heldWordMesh.text = value.Text;
                heldWordMesh.color = WordToColor[value.Type];
                float xOffset = 1f * Mathf.Sign(Target.transform.localScale.x);
                position.Value = Target.transform.position + new Vector3(xOffset, -1);
            }
        }
    }

    private Word savedWord;

    public void Awake()
    {
        heldWordMesh = GetComponent<TextMeshPro>();
        position = new(Vector3.zero, Vector3.zero, () => transform.position = position.Value);
    }

    public void Start()
    {
        position.TargetValue = Target.transform.position + offset;
        position.Value = Target.transform.position + offset;
        GameManager.Manager.ResetOccurred += Reset;
        GameManager.Manager.SaveStateOccurred += SaveState;
    }

    private void Reset()
    {
        HeldWord = savedWord;
    }

    private void SaveState()
    {
        savedWord = HeldWord;
    }

    public void OnDestroy()
    {
        GameManager.Manager.ResetOccurred -= Reset;
        GameManager.Manager.SaveStateOccurred -= SaveState;
    }

    public void FixedUpdate()
    {
        position.TargetValue = Target.transform.position + offset;
        _ = position.Next(Speed, Time.fixedDeltaTime);
    }
}
