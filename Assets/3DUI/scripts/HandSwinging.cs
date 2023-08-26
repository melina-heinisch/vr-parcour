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
    public AnimationCurve highSpeedModeAccelerationCurve;
    
    //Vector3 positions
    private Vector3 positionPreviousFrameLeftHand;
    private Vector3 positionPreviousFrameRightHand;
    private Vector3 positionThisFrameLeftHand;
    private Vector3 positionThisFrameRigthHand;
    
    //Speed
    public float speed = 50; 
    private float handSpeed;
    // Start is called before the first frame update

    public float timeSinceGameStart = 0.0f;
    void Start()
    {
        //Set original Previous frame positions at start up
        positionPreviousFrameLeftHand = new Vector3(0, leftHand.transform.localPosition.y, 0);
        positionPreviousFrameRightHand = new Vector3(0, rightHand.transform.localPosition.y, 0);
        
    }
    
    // Update is called once per frame

    void Update()
    {
        // Old version: float yRotation = VRHostSystem.getXROriginGameObject().transform.eulerAngles.y;
        float yRotationRight = rightHand.transform.eulerAngles.y;
        float yRotationLeft = leftHand.transform.eulerAngles.y;
        float yRotation = (float)(Math.Round(((yRotationRight + yRotationLeft) / 2) / 10.0)) * 10;

        if ((yRotationLeft > 320 || yRotationRight > 320) && (yRotationLeft < 40 || yRotationRight < 40))
            yRotation = 0;

        forwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);

        //Get current positions of hands
        positionThisFrameLeftHand = new Vector3(0, leftHand.transform.localPosition.y, 0);
        positionThisFrameRigthHand =  new Vector3(0, rightHand.transform.localPosition.y, 0);
        

        //Get distance the hands have moved since the last frame
        var leftHandDistanceMoved = Vector3.Distance(positionPreviousFrameLeftHand, positionThisFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(positionPreviousFrameRightHand, positionThisFrameRigthHand);

        //Add them up to get the hand speed from the user
        handSpeed = ((leftHandDistanceMoved) +
                     (rightHandDistanceMoved)) * 2f;
        timeSinceGameStart += Time.deltaTime;
        speed = highSpeedModeAccelerationCurve.Evaluate(timeSinceGameStart);

        if (Time.timeSinceLevelLoad > 1f)
            transform.position += forwardDirection.transform.forward * (handSpeed * speed * Time.deltaTime);

            //Set previous positions of hands for the next frame
        positionPreviousFrameLeftHand = positionThisFrameLeftHand; //Set player position previous frame
        positionPreviousFrameRightHand = positionThisFrameRigthHand;

    }
}