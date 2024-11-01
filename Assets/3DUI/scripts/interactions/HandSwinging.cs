using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class HandSwinging : MonoBehaviour
{
    public VRHostSystem VRHostSystem;
    
    //GameObjects
    public GameObject leftHand;
    public GameObject rightHand; 
    public GameObject forwardDirection;
    
    //Vector3 positions
    private Vector3 positionPreviousFrameLeftHand;
    private Vector3 positionPreviousFrameRightHand;
    private Vector3 positionThisFrameLeftHand;
    private Vector3 positionThisFrameRigthHand;

    private Rigidbody rb;
    
    //Speed
    [FormerlySerializedAs("speed")] public float accelerationMultiplier = 200; 
    public float maxVelocity = 5f; 
    private float handSpeed;
    // Start is called before the first frame update

    void Start()
    {
        //Set original Previous frame positions at start up
        positionPreviousFrameLeftHand = new Vector3(leftHand.transform.localPosition.x, leftHand.transform.localPosition.y, 0);
        positionPreviousFrameRightHand = new Vector3(rightHand.transform.localPosition.x, rightHand.transform.localPosition.y, 0);

        rb = VRHostSystem.getXROrigin().GetComponent<Rigidbody>();

    }
    
    // Update is called once per frame

    void Update()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid && VRHostSystem.GetRightHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip) && 
                VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip))
            {
                float yRotationRight = rightHand.transform.eulerAngles.y;
                float yRotationLeft = leftHand.transform.eulerAngles.y;
                float yRotation = (float)(Math.Round(((yRotationRight + yRotationLeft) / 2) / 10.0)) * 10; // rounds rotation in steps of 10 e.g. 124.323 => 120
        
                // prevent walking backwards when controller show in the forward direction 
                if ((yRotationLeft > 320 || yRotationRight > 320) && (yRotationLeft < 40 || yRotationRight < 40))
                    yRotation = 0;
        
                forwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);
        
                //Get current positions of hands
                positionThisFrameLeftHand = new Vector3(leftHand.transform.localPosition.x, leftHand.transform.localPosition.y, 0);
                positionThisFrameRigthHand = new Vector3(rightHand.transform.localPosition.x, rightHand.transform.localPosition.y, 0);
        
        
                //Get distance the hands have moved since the last frame
                var leftHandDistanceMoved = Vector3.Distance(positionPreviousFrameLeftHand, positionThisFrameLeftHand);
                var rightHandDistanceMoved = Vector3.Distance(positionPreviousFrameRightHand, positionThisFrameRigthHand);
        
                //Add them up to get the hand speed from the user
                handSpeed = ((leftHandDistanceMoved) +
                             (rightHandDistanceMoved)) * 2f;

                if (leftGrip || rightGrip)
                {
                    if (Time.timeSinceLevelLoad > 1f)
                    {
                        Vector3 movement = forwardDirection.transform.forward * (handSpeed * accelerationMultiplier * Time.deltaTime);
                        if(rb.velocity.magnitude <= maxVelocity)
                            rb.AddForce(movement, ForceMode.VelocityChange);
                    }
                }
                //Set previous positions of hands for the next frame
                positionPreviousFrameLeftHand = positionThisFrameLeftHand; //Set player position previous frame
                positionPreviousFrameRightHand = positionThisFrameRigthHand;
            }
        }
    }
}