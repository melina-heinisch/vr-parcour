using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Jumping : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;
    public string RayCollisionLayer = "Default";

    private GameObject handControllerGameObject;
    private RaycastHit lastRayCastHit;
    private bool bButtonWasPressed = false;

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

    private void MoveTrackingSpaceRootWithJumping()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool triggerButton))
            {
                if (!bButtonWasPressed && triggerButton && lastRayCastHit.collider != null)
                {
                    bButtonWasPressed = true;
                }
                if (!triggerButton && bButtonWasPressed)
                {
                    bButtonWasPressed = false;
                    VRHostSystem.getXROriginGameObject().transform.position = lastRayCastHit.point;
                    Debug.Log("Jumping! " + Time.deltaTime);
                }
            }
        }
    }
}
