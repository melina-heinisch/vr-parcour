
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
    public float jumpforceFront = 2f;

    private RaycastHit lastRayCastHit;
    private bool rButtonWasPressed = false;
    private bool lButtonWasPressed = false;


    private int jumpCounter = 1;

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
                    if (jumpCounter < 3)
                    {
                        lButtonWasPressed = true;
                        rigidbodyObj.AddForce(Vector3.up * jumpforceUp/jumpCounter, ForceMode.Impulse);
                        rigidbodyObj.AddForce(forwardDirection.transform.forward * jumpforceFront, ForceMode.Impulse); //to do right forward

                        GenerateSound();
                        
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
                    if (jumpCounter < 3)
                    {
                        rButtonWasPressed = true;
                        rigidbodyObj.AddForce(Vector3.up * jumpforceUp/jumpCounter, ForceMode.Impulse);
                        rigidbodyObj.AddForce(forwardDirection.transform.forward * jumpforceFront, ForceMode.Impulse); //to do right forward

                        GenerateSound();
                        
                        jumpCounter++;
                        Debug.Log("Jumping! " + Time.deltaTime); 
                    }
                }
                if(!rightTrigger && rButtonWasPressed)
                    rButtonWasPressed = false;
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
            Debug.Log("No Audio Source Found!");
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        jumpCounter = 1;
    }
}
