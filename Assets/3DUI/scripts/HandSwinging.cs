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
    
    //Speed
    public float speed = 50; 
    private float handSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //Set original Previous frame positions at start up
        positionPreviousFrameLeftHand = leftHand.transform.localPosition;
        positionPreviousFrameRightHand = rightHand.transform.localPosition;
        
    }
    
    // Update is called once per frame

    void Update()
    {
        float yRotation = VRHostSystem.getXROriginGameObject().transform.eulerAngles.y;
        forwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);
        

        //Get current positions of hands
        positionThisFrameLeftHand = leftHand.transform.localPosition;
        positionThisFrameRigthHand = rightHand.transform.localPosition;
        

        //Get distance the hands have moved since the last frame
        var leftHandDistanceMoved = Vector3.Distance(positionPreviousFrameLeftHand, positionThisFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(positionPreviousFrameRightHand, positionThisFrameRigthHand);

        //Add them up to get the hand speed from the user
        handSpeed = ((leftHandDistanceMoved) +
                     (rightHandDistanceMoved)) * 2f;

        if (Time.timeSinceLevelLoad > 1f)
            transform.position += forwardDirection.transform.forward * (handSpeed * speed * Time.deltaTime);

            //Set previous positions of hands for the next frame
        positionPreviousFrameLeftHand = positionThisFrameLeftHand; //Set player position previous frame
        positionPreviousFrameRightHand = positionThisFrameRigthHand;

    }
}