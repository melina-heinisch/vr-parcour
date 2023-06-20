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
    public AnimationCurve highSpeedModeAccelerationCurve;

    private GameObject handController;
    private bool bModeSnapRotation;
    private bool isStickWasPressed;
    private bool triggerWasPressed;
    private float timeSinceStartedHighSpeedMode = 0.0f;
    private bool snapRotationExecuted = false;
    private bool highSpeedModeActivated = false;

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
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressedNow))
            {
                if (triggerPressedNow)
                {
                    triggerWasPressed = true;
                }
                else if (triggerWasPressed) // release
                {
                    highSpeedModeActivated = !highSpeedModeActivated;
                    triggerWasPressed = false;
                    Debug.Log("Highspeedmode " + highSpeedModeActivated);
                }
            }
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
                float speed = speedInMeterPerSecond;
                if (highSpeedModeActivated)
                {
                    // TODO: Doesn't work with animation curve
                    timeSinceStartedHighSpeedMode = Time.deltaTime;
                    float factor = highSpeedModeAccelerationCurve.Evaluate(timeSinceStartedHighSpeedMode);
                    Debug.Log(factor); 
                    speed = speedInMeterPerSecond * factor;
                    //Debug.Log(speed);
                }
                else
                {
                    timeSinceStartedHighSpeedMode = 0f;
                }
                // Translate front/back Moving
                VRHostSystem.getXROrigin().transform.position +=
                    handController.transform.forward * 
                    (speed * Time.deltaTime * thumbstickAxisValue.y);
                
                //Translate Left/right Moving
                // VRHostSystem.getXROrigin().transform.position +=
                //     handController.transform.right * 
                //     (speedInMeterPerSecond * Time.deltaTime * thumbstickAxisValue.x);

                if (bModeSnapRotation)
                {
                   // Vector3 currentRotation = VRHostSystem.getXROrigin().transform.rotation.eulerAngles;
                   // VRHostSystem.getXROrigin().transform.rotation = Quaternion.Euler(currentRotation.x, 45 + currentRotation.y, currentRotation.z);
                   if (thumbstickAxisValue.x == 0)
                   {
                       snapRotationExecuted = false;
                   }
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
