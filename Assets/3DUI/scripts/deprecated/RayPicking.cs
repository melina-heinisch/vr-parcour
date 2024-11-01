﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RayPicking : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    public float translationIncrement = 0.1f;
    public float rotationIncrement = 1.0f;
    public float thumbstickDeadZone = 0.5f;  // a bit of a dead zone (make it less sensitive to axis movement)
    public string RayCollisionLayer = "Default";
    public bool PickedUpObjectPositionNotControlledByPhysics = true; //otherwise object position will be still computed by physics engine, even when attached to ray

    private GameObject rightHandController;
    private RaycastHit lastRayCastHit;
    private bool bButtonWasPressed = false;
    private GameObject objectPickedUP = null;
    private GameObject previousObjectCollidingWithRay = null;
    private GameObject lastObjectCollidingWithRay = null;
    private bool IsThereAnewObjectCollidingWithRay = false;

    void Start()
    {
        rightHandController = this.gameObject;
    }

    void Update()
    {
        if (VRHostSystem == null) Debug.LogError("VRHostSystem variable was not defined via inspector!");
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                if (objectPickedUP == null)
                {
                    GetTargetedObjectCollidingWithRayCasting();
                    UpdateObjectCollidingWithRay();
                    UpdateFlagNewObjectCollidingWithRay();
                    OutlineObjectCollidingWithRay();
                }
                AttachOrDetachTargetedObject();
                // MoveTargetedObjectAlongRay();
                // RotateTargetedObjectOnLocalUpAxis();
            }
        }
    }

    private void GetTargetedObjectCollidingWithRayCasting()
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

    private void UpdateObjectCollidingWithRay()
    {
        if (lastRayCastHit.collider != null)
        {
            GameObject currentObjectCollidingWithRay = lastRayCastHit.collider.gameObject;
            if (lastObjectCollidingWithRay != currentObjectCollidingWithRay)
            {
                previousObjectCollidingWithRay = lastObjectCollidingWithRay;
                lastObjectCollidingWithRay = currentObjectCollidingWithRay;
            }
        }
    }
    private void UpdateFlagNewObjectCollidingWithRay()
    {
        if (lastObjectCollidingWithRay != previousObjectCollidingWithRay)
        {
            IsThereAnewObjectCollidingWithRay = true;
        }
        else
        {
            IsThereAnewObjectCollidingWithRay = false;
        }
    }
    private void OutlineObjectCollidingWithRay()
    {
        if (IsThereAnewObjectCollidingWithRay)
        {
            //add outline to new one
            if (lastObjectCollidingWithRay != null)
            {
                var outliner = lastObjectCollidingWithRay.GetComponent<OutlineModified>();
                if (outliner == null) // if not, we will add a component to be able to outline it
                {
                    //Debug.Log("Outliner added t" + lastObjectCollidingWithRay.gameObject.ToString());
                    outliner = lastObjectCollidingWithRay.AddComponent<OutlineModified>();
                }

                if (outliner != null)
                {
                    outliner.enabled = true;
                    //Debug.Log("outline new object color"+ lastObjectCollidingWithRay);
                }
                // remove outline from previous one
                //add outline new one
                if (previousObjectCollidingWithRay != null)
                {
                    outliner = previousObjectCollidingWithRay.GetComponent<OutlineModified>();
                    if (outliner != null)
                    {
                        outliner.enabled = false;
                        //Debug.Log("outline new object color"+ previousObjectCollidingWithRay);
                    }
                }
            }
        }
    }

    private void AttachOrDetachTargetedObject()
    {
        if (VRHostSystem.GetRightHandDevice().isValid) // still connected?
        {
            if (VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.primaryButton, out bool bButtonAPressedNow))
            {
                if (!bButtonWasPressed && bButtonAPressedNow && lastRayCastHit.collider != null)
                {
                    bButtonWasPressed = true;
                }
                if (!bButtonAPressedNow && bButtonWasPressed) // Button was released?
                {
                    if (objectPickedUP != null) // already pick up an object?
                    {
                        if (PickedUpObjectPositionNotControlledByPhysics)
                        {
                            Rigidbody rb = objectPickedUP.GetComponent<Rigidbody>();
                            if (rb != null)
                            {
                                rb.isKinematic = false;
                            }
                        }
                        objectPickedUP.transform.parent = null;
                        objectPickedUP = null;
                        Debug.Log("Object released: " + objectPickedUP);
                    }
                    else
                    {
                        GenerateSound();
                        GenerateVibrations();
                        objectPickedUP = lastRayCastHit.collider.gameObject;
                        objectPickedUP.transform.parent = gameObject.transform; // see Transform.parent https://docs.unity3d.com/ScriptReference/Transform-parent.html?_ga=2.21222203.1039085328.1595859162-225834982.1593000816
                        if (PickedUpObjectPositionNotControlledByPhysics)
                        {
                            Rigidbody rb = objectPickedUP.GetComponent<Rigidbody>();
                            if (rb != null)
                            {
                                rb.isKinematic = true;
                            }
                        }
                        Debug.Log("Object Picked up:" + objectPickedUP);
                    }
                    bButtonWasPressed = false;
                }
            }
        }
    }

    // private void MoveTargetedObjectAlongRay()
    // {
    //     if (VRHostSystem.GetRightHandDevice().isValid) // still connected?
    //     {
    //         if (VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickAxis))
    //         {
    //             if (objectPickedUP != null) // already picked up an object?
    //             {
    //                 if (thumbstickAxis.y > thumbstickDeadZone || thumbstickAxis.y < -thumbstickDeadZone)
    //                 {
    //                     objectPickedUP.transform.position += transform.TransformDirection(Vector3.forward) * translationIncrement * thumbstickAxis.y;
    //                     Debug.Log("Move object along ray: " + objectPickedUP + " axis: " + thumbstickAxis);
    //                 }
    //             }
    //         }
    //     }
    // }
    //
    // private void RotateTargetedObjectOnLocalUpAxis()
    // {
    //     if (VRHostSystem.GetRightHandDevice().isValid) // still connected?
    //     {
    //         if (VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickAxis))
    //         {
    //             if (objectPickedUP != null) // already pick up an object?
    //             {
    //                 if (thumbstickAxis.x > thumbstickDeadZone || thumbstickAxis.x < -thumbstickDeadZone)
    //                 {
    //                     objectPickedUP.transform.Rotate(Vector3.up, rotationIncrement * thumbstickAxis.x, Space.Self);
    //                 }
    //                 Debug.Log("Rotate Object: " + objectPickedUP + "axis " + thumbstickAxis);
    //             }
    //         }
    //     }
    // }

    private void GenerateVibrations()
    {
        HapticCapabilities capabilities;
        if (VRHostSystem.GetRightHandDevice().TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 1.0f;
                VRHostSystem.GetRightHandDevice().SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

    private void GenerateSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No Audio Source Found!");
        }
    }
}
