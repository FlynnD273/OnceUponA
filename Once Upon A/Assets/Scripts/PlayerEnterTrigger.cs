using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;

public class PlayerEnterTrigger : TriggerLogic
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == (int)Layers.Player)
        {
          State = true;
        }
    }

    /* void OnTriggerExit2D(Collider2D coll) */
    /* { */
    /*     if (coll.gameObject.layer == (int)Layers.Player) */
    /*     { */
    /*       State = false; */
    /*     } */
    /* } */
}
