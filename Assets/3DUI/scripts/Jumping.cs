
using UnityEngine;
// using UnityEngine.InputSystem; FOr testing at home
using UnityEngine.XR;

public class Jumping : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    public Rigidbody rigidbody;
    public float jumpforce = 4f;

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
                    bButtonWasPressed = true;
                    rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
                    jumpCounter++;
                    Debug.Log("Jumping! " + Time.deltaTime);
                }
                if (!leftGripButton && !rightGripButton && bButtonWasPressed)
                {
                    bButtonWasPressed = false;
                }
            }
        } 

     /* For testing Jump at Home
      if (Keyboard.current.spaceKey.wasPressedThisFrame)
      {
          if (jumpCounter < 2)
          {
              rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
              jumpCounter++;
              Debug.Log("Jumping! " + Time.deltaTime);
          }
      } */
    }
    
    void OnCollisionEnter(Collision collision)
    {
        jumpCounter = 0;
    }
}
