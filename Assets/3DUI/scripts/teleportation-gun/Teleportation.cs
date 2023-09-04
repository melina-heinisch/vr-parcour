﻿using System.Collections;
using System.Collections.Generic;
using _3DUI.scripts;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleportation : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    [SerializeField] VRHostSystem VRHostSystem;
    public string RayCollisionLayer = "Default";
    public XRRayInteractor rayInteractor;
    private GameObject handControllerGameObject;
    private RaycastHit lastRayCastHit;
    private bool bButtonWasPressed = false;
    private bool firstTeleport = false;

    [SerializeField] private GameObject preTravelObject;

    private GameObject currentTeleportationGun;

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
            if (firstTeleport)
            {
                DropTeleportationGun();
                firstTeleport = false;
            }
            if (VRHostSystem.AreAllDevicesFound())
            {
                getPointCollidingWithRayCasting();
                
                if (StateController.preTravelModeActivated)
                {
                    preTravelObject.gameObject.transform.position = lastRayCastHit.point;
                    RotatePreTravelObject();
                }

                MoveTrackingSpaceRootWithTeleportation();
            }
            
        }
    }

    private void DropTeleportationGun()
    {
        SelectExitEventArgs drop = new();
        currentTeleportationGun.GetComponent<XRGrabInteractable>().selectExited.Invoke(drop);

    }

    public void SetTeleportationGun(GameObject teleportationGun)
    {
        currentTeleportationGun = teleportationGun;
    }

    private void getXRHandController()
    {
        handControllerGameObject = this.gameObject;
    }
    private void getPointCollidingWithRayCasting()
    {
        rayInteractor.TryGetCurrent3DRaycastHit(out lastRayCastHit);
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
                    StartCoroutine(ActivateFader());
                }
            }
        }
    }

    IEnumerator ActivateFader()
    {
        Fader.FadeToBlack(10f);
        GenerateSound();
        bButtonWasPressed = false;
        StateController.preTravelModeActivated = false;
        VRHostSystem.getXROriginGameObject().transform.position = lastRayCastHit.point;
        Debug.Log("Teleportation! ");
        preTravelObject.SetActive(false);
        firstTeleport = true;
        yield return new WaitForSeconds(0.5f);
        Fader.FadeToScene(10f);
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