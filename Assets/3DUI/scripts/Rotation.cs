using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Rotation : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    public float angleInDegreePerSecond = 25;
    
    private bool bModeSnapRotation;
    private bool isStickWasPressed;
    private bool snapRotationExecuted = false;
    void Update()
    {
        if (VRHostSystem == null) 
            Debug.LogError("VRHostSystem variable was not defined via inspector!");
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                CheckForRotation();
            }
        }
    }

    private void CheckForRotation() // simple - with no strafing 
    {
        if (StateController.preTravelModeActivated)
            return;
        
        if (VRHostSystem.GetRightHandDevice().isValid) // still connected?
        {
            if (VRHostSystem.GetRightHandDevice()
                .TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool isStickPressedNow))
            {
                if (isStickPressedNow)
                    isStickWasPressed = true;
                else if (isStickWasPressed) // release
                {
                    bModeSnapRotation = !bModeSnapRotation;
                    isStickWasPressed = false;
                    if (bModeSnapRotation) Debug.Log("Snap Turning Is ON");
                    else Debug.Log("Snap Turning Is OFF (Smooth Rotation");
                }
            }

            Vector2 thumbstickAxisValue; //  where left (-1.0,0.0), right (1.0,0.0), up (0.0,1.0), down (0.0,-1.0)

            if (VRHostSystem.GetRightHandDevice()
                .TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickAxisValue))
            {
                if (bModeSnapRotation)
                {
                    if (thumbstickAxisValue.x == 0)
                        snapRotationExecuted = false;

                    if (!snapRotationExecuted && Mathf.Abs(thumbstickAxisValue.x) >= 0.5)
                    {
                        int direction = thumbstickAxisValue.x > 0 ? 1 : -1;
                        VRHostSystem.getXROrigin().transform.Rotate(Vector3.up, direction * 45);
                        snapRotationExecuted = true;
                    }
                }
                else
                {
                    // Smooth Rotate Left/right Moving
                    VRHostSystem.getXROrigin()
                        .transform
                        .Rotate(Vector3.up, angleInDegreePerSecond * Time.deltaTime * thumbstickAxisValue.x);
                }
            }
        }
    }

    
}