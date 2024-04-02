using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;

public class DangerController : MonoBehaviour
{
    public Vector2 Direction;
    public float Power;
    public float LockTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMesh>().color = WordToColor[WordType.Danger];
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == (int)Layers.Player)
        {
            coll.GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.normalized.x * Power, Direction.normalized.y * Power);
            coll.GetComponent<PlayerController>().LockControls(LockTime);
        }
    }
}
