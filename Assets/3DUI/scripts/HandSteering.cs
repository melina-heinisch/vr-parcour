using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandSteering : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    public float speedInMeterPerSecond = 1;
    public float angleInDegreePerSecond = 25;
    public float anglePerClick = 45;

    private GameObject handController;
    private bool bModeSnapRotation;
    private bool isStickWasPressed;

    void Start()
    {
        GetHandControllerGameObject();
    }

    void Update()
    {
        if (VRHostSystem == null) Debug.LogError("VRHostSystem variable was not defined via inspector!");
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                MoveTrackingSpaceRootWithHandSteering();
            }
        }
    }

    private void GetHandControllerGameObject()
    {
        handController = this.gameObject; // i.e. with this script component and an XR controller component
    }

    private void MoveTrackingSpaceRootWithHandSteering()  // simple - with no strafing 
    {
        if (VRHostSystem.GetLeftHandDevice().isValid) // still connected?
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool isStickPressedNow))
            {    // see https://docs.unity3d.com/Manual/xr_input.html 
                if (isStickPressedNow)
                {
                    isStickWasPressed = true;
                }
                else if (isStickWasPressed) // release
                {
                    bModeSnapRotation = !bModeSnapRotation;
                    isStickWasPressed = false;
                    if (bModeSnapRotation) Debug.Log("Snap Turning Is ON");
                    else Debug.Log("Snap Turning Is OFF (Smooth Rotation");
                }
            }

            Vector2 thumbstickAxisValue; //  where left (-1.0,0.0), right (1.0,0.0), up (0.0,1.0), down (0.0,-1.0)

            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickAxisValue))
            {
                // Translate front/back Moving
                VRHostSystem.getXROrigin().transform.position +=
                    handController.transform.forward * 
                    (speedInMeterPerSecond * Time.deltaTime * thumbstickAxisValue.y);
                
                //Translate Left/right Moving
                    // do something here (Exercise tasks)

                if (bModeSnapRotation)
                {
                    // do something here (Exercise tasks)
                }
                else
                {
                    //// Smooth Rotate Left/right Moving
                    VRHostSystem.getXROrigin()
                        .transform
                        .Rotate(Vector3.up, angleInDegreePerSecond * Time.deltaTime * thumbstickAxisValue.x);
                }
            }
        }
    }
}
