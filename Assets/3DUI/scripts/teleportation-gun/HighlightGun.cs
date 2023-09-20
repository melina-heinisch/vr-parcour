using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightGun : MonoBehaviour
{
    public void Highlight(bool enable)
    {
        var outliner = this.gameObject.GetComponent<OutlineModified>();
        if (outliner == null) // if null, we will add a component to be able to outline it
        {
            outliner = this.gameObject.AddComponent<OutlineModified>();
        }

        if (outliner != null)
        {
            outliner.enabled = enable;
        }
    }
}
