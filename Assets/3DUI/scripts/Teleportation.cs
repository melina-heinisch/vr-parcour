using System.Collections;
using System.Collections.Generic;
using _3DUI.scripts;
using UnityEngine;
using UnityEngine.XR;

public class Teleportation : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;
    public string RayCollisionLayer = "Default";

    private GameObject handControllerGameObject;
    private RaycastHit lastRayCastHit;
    private bool bButtonWasPressed = false;

    [SerializeField] private GameObject preTravelObject;

    //https://vionixstudio.com/2021/10/26/how-to-make-a-character-jump-in-unity/
    void Start()
    {
        getXRHandController();
    }

    void Update()
    {
        if (VRHostSystem == null) Debug.LogError("VRHostSystem variable was not defined via inspector!");
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                getPointCollidingWithRayCasting();
                
                if (StateController.preTravelModeActivated)
                {
                    // Debug.Log("last ray cast hit point: " + lastRayCastHit.point);
                    preTravelObject.gameObject.transform.position = lastRayCastHit.point;
                    // Debug.Log("pre travel position: " + preTravelObject.gameObject.transform.position);
                    RotatePreTravelObject();
                }

                MoveTrackingSpaceRootWithTeleportation();
            }
        }
    }

    private void getXRHandController()
    {
        handControllerGameObject = this.gameObject;
    }
    private void getPointCollidingWithRayCasting()
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

    private void RotatePreTravelObject()
    {
        // adapted from ChatGPT
        float hmdY = VRHostSystem.GetCamera().transform.rotation.eulerAngles.y;
        Vector3 rotationPreTravel = preTravelObject.transform.rotation.eulerAngles;
        rotationPreTravel.y = hmdY;
        preTravelObject.transform.rotation = Quaternion.Euler(rotationPreTravel);
    }

    private void MoveTrackingSpaceRootWithTeleportation()
    {
        if (VRHostSystem.GetRightHandDevice().isValid)
        {
            if (VRHostSystem.GetRightHandDevice().TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTriggerButton))
            {
                if (!bButtonWasPressed && rightTriggerButton && lastRayCastHit.collider != null)
                {
                    bButtonWasPressed = true;
                    StateController.preTravelModeActivated = true;
                    preTravelObject.SetActive(true);
                    preTravelObject.transform.rotation = VRHostSystem.getXROriginGameObject().transform.rotation;
                }
                if (!rightTriggerButton && bButtonWasPressed)
                {
                    // Fader.FadeToBlack();
                    bButtonWasPressed = false;
                    StateController.preTravelModeActivated = false;
                    GenerateSound();
                    VRHostSystem.getXROriginGameObject().transform.position = lastRayCastHit.point;
                    Debug.Log("Teleportation! ");
                    preTravelObject.SetActive(false);
                    // Fader.FadeToScene();
                }
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
}
