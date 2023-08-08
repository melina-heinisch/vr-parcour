
using UnityEngine;
using UnityEngine.XR;

public class Jumping : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    public Rigidbody rigidbody;
    public float jumpforce = 3f;
    public float velocity = 0.1f;

    private RaycastHit lastRayCastHit;
    private bool bButtonWasPressed = false;

    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripButton)
                && VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripButton))
            {
                if (!bButtonWasPressed && leftGripButton && rightGripButton)
                {
                    bButtonWasPressed = true;
                    if (VRHostSystem.GetLeftHandDevice()
                            .TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocityL) &&
                        VRHostSystem.GetRightHandDevice()
                            .TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocityR))
                    {
                        if ((velocityR.x > velocity || velocityR.y > velocity || velocityR.z > velocity) &&
                            (velocityL.x > velocity || velocityL.y > velocity || velocityL.z > velocity))
                        {
                            jumpforce = 5f;
                        }
                    }
                 
                }
                if (!leftGripButton && !rightGripButton && bButtonWasPressed)
                {
                    bButtonWasPressed = false;
                    rigidbody.AddForce(Vector3.up * jumpforce,ForceMode.Impulse);
                    Debug.Log("Jumping! " + Time.deltaTime);
                    jumpforce = 3f;
                }
            }
        }
    }
}
