﻿using System;
using System.Collections.Generic;
using _3DUI.scripts;
using UnityEngine;
using UnityEngine.XR;

public class HelpMenuController : MonoBehaviour
{
    [Tooltip("If should reference the prebab: Assets/3DUI/prefabs/HelpMenu.prefab")]
    public GameObject menuPrefab;

    private string defaultMenuPrefabPath = "Assets/3DUI/prefabs/HelpMenu.prefab";
    private GameObject menuInstanced;
    private bool bButtonWasPressed = false;
    private Canvas canvas;

    private VRHostSystem VRHostSystem = null;

    private List<GameObject> hiddenObjects = new();

    void Start()
    {
        findVRHostSystem();
    }

    void findVRHostSystem()
    {
        VRHostSystem = GameObject.FindGameObjectWithTag("VRHostSystemDevices").GetComponent<VRHostSystem>();
    }

    void Update()
    {
        if (VRHostSystem == null) findVRHostSystem();
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                OpenOrCloseHelpMenu();
            }
        }

        if (StateController.showInitialHelpMenu)
        {
            StateController.showInitialHelpMenu = false;
            Open();
        }
    }

    private void OpenOrCloseHelpMenu()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid) // still connected?
        {
            if (VRHostSystem.GetLeftHandDevice()
                .TryGetFeatureValue(CommonUsages.menuButton, out bool bButtonPressedNow))
            {
                if (!bButtonWasPressed && bButtonPressedNow)
                {
                    bButtonWasPressed = true;
                }

                if (!bButtonPressedNow && bButtonWasPressed) // Button was released?
                {
                    bButtonWasPressed = false;
                    if (menuInstanced == null && VRHostSystem.GetCamera() != null)
                    {
                        Open(); // actually doesnt matter where as Canvas is render in camera view, here just to let students modify the position later
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }
    }

    public void EditorOpenOrCloseHelpMenu()
    {
        if (menuInstanced == null)
        {
            Open();
        }
        else
        {
            Close();
        }
    }


    public void Open()
    {
        CreateMenuFromPrefab();
        AttachCameraToMenuCanvasAndDisplayMenu();
        VRHostSystem.getXROrigin().GetComponent<HandSwinging>().enabled = false;
        VRHostSystem.getXROrigin().GetComponent<Jumping>().enabled = false;

        //stop the timer
        FindObjectOfType<GameLogic>().isGameRunning = false;

        //disable any objects that could hide the help menu
        if (menuInstanced)
        {
            var rt = menuInstanced.GetComponentInChildren<RectTransform>();
            var centerPoint = rt.TransformPoint(rt.rect.center);
            var distance = VRHostSystem.GetCamera().transform.position - centerPoint;
            var halfExtents = new Vector3(4, 2, 1);
            halfExtents.z = distance.magnitude;

            Collider[] colliders = Physics.OverlapBox(centerPoint + 0.5f * distance, halfExtents);
            foreach (var collider in colliders)
            {
                var hitTarget = collider.gameObject;
                if (hitTarget.layer == LayerMask.NameToLayer("player")) continue;
                hitTarget.SetActive(false);
                hiddenObjects.Add(hitTarget);
            }
        }
    }

    private void AttachCameraToMenuCanvasAndDisplayMenu()
    {
        if (menuInstanced != null)
        {
            canvas = menuInstanced.GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("canvas Not Found! in " + gameObject + " =>> cannot open help menu");
            }
            else if (VRHostSystem.GetCamera() != null)
            {
                canvas.worldCamera = VRHostSystem.GetCamera();
            }

            menuInstanced.SetActive(true); // just to make sure
        }
    }

    private void CreateMenuFromPrefab()
    {
        if (menuPrefab != null)
        {
            var where = VRHostSystem.GetCamera().transform;
            var translation = new Vector3(-3.5f, -1f, 6f);
            var angle = where.eulerAngles.y + VRHostSystem.getXROriginGameObject().transform.eulerAngles.y;
            translation = Quaternion.AngleAxis(angle, Vector3.up) * translation;
            var rotation = Quaternion.identity;
            var position = where.position + translation;
            menuInstanced = Instantiate(menuPrefab, position, rotation, null);
            menuInstanced.transform.Rotate(0,
                where.eulerAngles.y + VRHostSystem.getXROriginGameObject().transform.eulerAngles.y, 0);
            var slideManager = menuInstanced.GetComponent<SlideManager>();
            if (slideManager)
                slideManager.closeAction = Close;
        }
        else
        {
            Debug.LogError("No Menu Prefab Specified - You should reference this one: " + defaultMenuPrefabPath);
        }
    }

    public void Close()
    {
        if (menuInstanced != null)
        {
            menuInstanced.SetActive(false); // just to make sure
            Destroy(menuInstanced);
            menuInstanced = null;
            VRHostSystem.getXROrigin().GetComponent<HandSwinging>().enabled = true;
            VRHostSystem.getXROrigin().GetComponent<Jumping>().enabled = true;
        }

        foreach (var hiddenObject in hiddenObjects)
        {
            hiddenObject.SetActive(true);
        }

        FindObjectOfType<GameLogic>().isGameRunning = true;

        hiddenObjects.Clear();
    }

    /* Needed for debugging what objects need to be disabled for the help menu
    public void OnDrawGizmos()
    {
        //To use this method, add a bool started to the script and set it to true in the start method
        if (!started) return;
        Gizmos.color = Color.red;
        if (menuInstanced)
        {
            var rt = menuInstanced.GetComponentInChildren<RectTransform>();
            var centerPoint = rt.TransformPoint(rt.rect.center);
            var distance = VRHostSystem.GetCamera().transform.position - centerPoint;
            var halfExtents = new Vector3(4,3,1);
            halfExtents.z = distance.magnitude;
            Gizmos.DrawWireCube(centerPoint + 0.5f * distance, halfExtents);
        }
    }*/
}