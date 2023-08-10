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
    private Vector3 playerPositionPreviousFrame; 
    private Vector3 playerPositionThisFrame;
    private Vector3 positionThisFrameLeftHand;
    private Vector3 positionThisFrameRigthHand;
    
    //Speed
    public float speed = 50; 
    private float handSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //Set original Previous frame positions at start up
        playerPositionPreviousFrame = transform.position;
        positionPreviousFrameLeftHand = leftHand.transform.position;
        positionPreviousFrameRightHand = rightHand.transform.position;
        
    }
    
    // Update is called once per frame

    void Update()
    {
        if (VRHostSystem.GetLeftHandDevice()
            .TryGetFeatureValue(CommonUsages.primary2DAxis,
                out var thumbstickAxisValue)) //  where left (-1.0,0.0), right (1.0,0.0), up (0.0,1.0), down (0.0,-1.0)
        {
            //forwardDirection.transform.forward += new Vector3(thumbstickAxisValue.x, 0, thumbstickAxisValue.y);
        }

        float yRotation = VRHostSystem.getXROriginGameObject().transform.eulerAngles.y;
        forwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);
        

        //Get current positions of hands
        positionThisFrameLeftHand = leftHand.transform.position;
        positionThisFrameRigthHand = rightHand.transform.position;

        //position of Player
        playerPositionThisFrame = transform.position;

        //Get distance the hands and player has moved since the last frame
        var playerDistanceMoved = Vector3.Distance(playerPositionThisFrame, playerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(positionPreviousFrameLeftHand, positionThisFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(positionPreviousFrameRightHand, positionThisFrameRigthHand);

        //Add them up to get the hand speed from the user minus the movement of the player to neglegt the movement of the player
        handSpeed = ((leftHandDistanceMoved - playerDistanceMoved) +
                     (rightHandDistanceMoved - playerDistanceMoved)) * 2f;

        if (Time.timeSinceLevelLoad > 1f)
            VRHostSystem.getXROriginGameObject().transform.position += forwardDirection.transform.forward * (handSpeed * speed * Time.deltaTime);

            //Set previous positions of hands for the next frame
        positionPreviousFrameLeftHand = positionThisFrameLeftHand; //Set player position previous frame
        positionPreviousFrameRightHand = positionThisFrameRigthHand;
        playerPositionPreviousFrame = playerPositionThisFrame;

    }
}