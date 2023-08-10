using System;
using UnityEngine;
using UnityEngine.XR;

public class Jumping : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid && VRHostSystem.GetRightHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripButton)
                && VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripButton))
            {
                if (leftGripButton && rightGripButton)
                {
                    anim.Play("jump");
                }
               
            }
        }
    }
  
}
