using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Teleportation : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;
    public string RayCollisionLayer = "Default";

    private GameObject handControllerGameObject;
    private RaycastHit lastRayCastHit;
    private bool bButtonWasPressed = false;


    public float angleInDegreePerSecond = 100;

    [SerializeField] private GameObject preTravelObject;

    void Start()
    {
        getXRHandController();
    }

    void Update()
    {
        if (VRHostSystem == null) Debug.LogError("VRHostSystem variable was not defined via inspector!");
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                getPointCollidingWithRayCasting();
                
                if (StateController.preTravelModeActivated)
                {
                    preTravelObject.gameObject.transform.position = lastRayCastHit.point;
                    RotatePreTravelObject();
                }

                MoveTrackingSpaceRootWithJumping();
            }
        }
    }

    private void getXRHandController()
    {
        handControllerGameObject = this.gameObject;
    }
    private void getPointCollidingWithRayCasting()
    {
        // see raycast example from https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
        if (Physics.Raycast(transform.position,
            transform.TransformDirection(Vector3.forward),
            out RaycastHit hit,
            Mathf.Infinity,
            1 << LayerMask.NameToLayer(RayCollisionLayer))) // 1 << because must use bit shifting to get final mask!
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            // Debug.Log("Ray collided with:  " + hit.collider.gameObject + " collision point: " + hit.point);
            Debug.DrawLine(hit.point, (hit.point + hit.normal * 2));
            lastRayCastHit = hit;
        }
    }

    private void RotatePreTravelObject()
    {
        Vector2 thumbstickAxisValue; //  where left (-1.0,0.0), right (1.0,0.0), up (0.0,1.0), down (0.0,-1.0)

        if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickAxisValue))
        {
            preTravelObject
                .transform
                .Rotate(Vector3.up, angleInDegreePerSecond * Time.deltaTime * thumbstickAxisValue.x);
        }
    }

    private void MoveTrackingSpaceRootWithJumping()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripButton)
                && VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripButton))
            {
                if (!bButtonWasPressed && leftGripButton && rightGripButton && lastRayCastHit.collider != null)
                {
                    bButtonWasPressed = true;
                    StateController.preTravelModeActivated = true;
                    preTravelObject.SetActive(true);
                    preTravelObject.transform.rotation = VRHostSystem.getXROriginGameObject().transform.rotation;
                }
                if (!leftGripButton && !rightGripButton && bButtonWasPressed)
                {
                    bButtonWasPressed = false;
                    StateController.preTravelModeActivated = false;
                    VRHostSystem.getXROriginGameObject().transform.position = lastRayCastHit.point;
                    VRHostSystem.getXROriginGameObject().transform.rotation = preTravelObject.transform.rotation;
                    Debug.Log("Jumping! " + Time.deltaTime);
                    preTravelObject.SetActive(false);
                }
            }
        }
    }
}
