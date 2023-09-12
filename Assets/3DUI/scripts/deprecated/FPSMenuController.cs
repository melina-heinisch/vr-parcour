using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FPSMenuController : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    [Tooltip("It should reference the Assets/3DUI/prefabs/FPSMenuLiteFPSCounter.prefab")]
    public GameObject menuPrefab;

    private string defaultMenuPrefabPath = "Assets/3DUI/prefabs/FPSMenuLiteFPSCounter.prefab";

    private GameObject menuInstanced;
    private bool bButtonWasPressed = false;
    private Canvas canvas;

    void Update()
    {
        if (VRHostSystem == null) Debug.LogError("VRHostSystem variable was not defined via inspector!");
        else
        {
            if (VRHostSystem.AreAllDevicesFound())
            {
                OpenOrCloseMenu();
            }
        }
    }



    private void OpenOrCloseMenu()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid) // still connected?
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.secondaryButton, out bool bButtonPressedNow))
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
                        Open(gameObject.transform); // actually doent matter where as Canvas is render in camera view, here just to let students modif the position later
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }
    }

    public void Open(Transform Where)
    {
        CreateMenuFromPrebab(Where);
        AttachCameraToMenuCanvasAndDisplayMenu();
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
    private void CreateMenuFromPrebab(Transform Where)
    {
        if (menuPrefab != null)
        {
            menuInstanced = Instantiate(menuPrefab, Where.position, Quaternion.identity);
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
        }
    }
}