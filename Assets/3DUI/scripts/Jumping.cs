
using UnityEngine;
using UnityEngine.Serialization;
// using UnityEngine.InputSystem; FOr testing at home
using UnityEngine.XR;

public class Jumping : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    public GameObject forwardDirection;

    public Rigidbody rigidbodyObj;
    public float jumpforceUp = 4f;
    public float jumpforceFront = 3f;

    private RaycastHit lastRayCastHit;
    private bool rButtonWasPressed = false;
    private bool lButtonWasPressed = false;


    private int jumpCounter = 0;

    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid && VRHostSystem.GetRightHandDevice().isValid)
        {
            bool leftTrigger = false;
            bool rightTrigger = false;
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger))
            {
                if (!lButtonWasPressed && leftTrigger)
                {
                    if (jumpCounter < 2)
                    {
                        lButtonWasPressed = true;
                        rigidbodyObj.AddForce(Vector3.up * jumpforceUp, ForceMode.Impulse);
                        rigidbodyObj.AddForce(forwardDirection.transform.forward * jumpforceFront, ForceMode.Impulse); //to do right forward

                        jumpCounter++;
                        Debug.Log("Jumping! " + Time.deltaTime); 
                    }
                }
                if (!leftTrigger && lButtonWasPressed)
                    lButtonWasPressed = false;
            }

            if (VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger))
            {
                if(!rButtonWasPressed && rightTrigger)
                {
                    if (jumpCounter < 2)
                    {
                        rButtonWasPressed = true;
                        rigidbodyObj.AddForce(Vector3.up * jumpforceUp, ForceMode.Impulse);
                        rigidbodyObj.AddForce(forwardDirection.transform.forward * jumpforceFront, ForceMode.Impulse); //to do right forward

                        jumpCounter++;
                        Debug.Log("Jumping! " + Time.deltaTime); 
                    }
                }
                if(!rightTrigger && rButtonWasPressed)
                    rButtonWasPressed = false;
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        rigidbodyObj.velocity *= 0.5f;
        jumpCounter = 0;
    }
}
