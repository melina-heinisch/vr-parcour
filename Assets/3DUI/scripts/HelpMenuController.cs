﻿using _3DUI.scripts;
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
    }
    
    private void OpenOrCloseHelpMenu()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid) // still connected?
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.menuButton, out bool bButtonPressedNow))
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
        StateController.isHelpMenuOpened = true;
        VRHostSystem.getXROrigin().GetComponent<HandSwinging>().enabled = false;
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
            var rotation = Quaternion.identity;
            var position = where.position + translation;
            menuInstanced = Instantiate(menuPrefab, position, rotation, null);
            menuInstanced.transform.Rotate(0, where.rotation.y + VRHostSystem.getXROriginGameObject().transform.rotation.y, 0);
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
            StateController.isHelpMenuOpened = false;
            VRHostSystem.getXROrigin().GetComponent<HandSwinging>().enabled = true;
        }
    }
    
}