using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectFactoryMenuController : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
    public VRHostSystem VRHostSystem;

    [Tooltip("It should reference the prefab: Assets/3DUI/prefabs/ObjectFactoryMenu.prefab")]
    public GameObject menuPrefab;

    private string defaultMenuPrefabPath = "Assets/3DUI/prefabs/ObjectFactoryMenu.prefab";
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
                OpenOrCloseHelpMenu();
            }
        }
    }
  
    private void OpenOrCloseHelpMenu()
    {
        if (VRHostSystem.GetLeftHandDevice().isValid)
        {
            if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.primaryButton, out bool bButtonPressedNow))
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
                        Open(gameObject.transform); // actually doent matter as we are going to place it in relative position to left hand controller
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
        AttachCameraToMenuCanvaAndPositioninFrontofLeftController();
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


    private void AttachCameraToMenuCanvaAndPositioninFrontofLeftController()
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
                canvas.worldCamera = VRHostSystem.GetCamera(); // required for corrects event system registion
                Vector3 position = gameObject.transform.position + (gameObject.transform.forward.normalized * 5f);// position 5 metters in front of user hand
                menuInstanced.transform.position = position;
                // orientation facing user head
                menuInstanced.transform.LookAt(VRHostSystem.GetCamera().transform.position);
                menuInstanced.SetActive(true); // just to make sure
            }
            menuInstanced.SetActive(true); // just to make sure
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