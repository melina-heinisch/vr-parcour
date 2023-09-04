using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightGun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight(bool enable)
    {
        var outliner = this.gameObject.GetComponent<OutlineModified>();
        if (outliner == null) // if not, we will add a component to be able to outline it
        {
            //Debug.Log("Outliner added t" + lastObjectCollidingWithRay.gameObject.ToString());
            outliner = this.gameObject.AddComponent<OutlineModified>();
        }

        if (outliner != null)
        {
            outliner.enabled = enable;
            //Debug.Log("outline new object color"+ lastObjectCollidingWithRay);
        }
    }
}
