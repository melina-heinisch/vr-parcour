
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
    private bool bButtonWasPressed = false;

    private int jumpCounter = 0;

    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid && VRHostSystem.GetRightHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripButton)
                && VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripButton))
            {
                if (!bButtonWasPressed && leftGripButton && rightGripButton)
                {
                    if (jumpCounter < 2)
                    {
                       bButtonWasPressed = true;
                       rigidbodyObj.AddForce(Vector3.up * jumpforceUp, ForceMode.Impulse);
                       rigidbodyObj.AddForce(forwardDirection.transform.forward * jumpforceFront, ForceMode.Impulse); //to do right forward

                       jumpCounter++;
                       Debug.Log("Jumping! " + Time.deltaTime); 
                    }
                    
                }
                if (!leftGripButton && !rightGripButton && bButtonWasPressed)
                {
                    bButtonWasPressed = false;
                }
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        rigidbodyObj.velocity *= 0.5f;
        jumpCounter = 0;
    }
}
