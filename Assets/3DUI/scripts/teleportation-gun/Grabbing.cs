using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Grabbing : MonoBehaviour
{
    public VRHostSystem VRHostSystem;
    
    public void GenerateVibrations()
    {
        HapticCapabilities capabilities;
        if (VRHostSystem.GetRightHandDevice().TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 0.5f;
                VRHostSystem.GetRightHandDevice().SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
