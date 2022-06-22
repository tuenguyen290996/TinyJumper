using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLimit : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(TagConsts.PLATFORM))
        {
            Destroy(col.gameObject);
        }
    }
}
