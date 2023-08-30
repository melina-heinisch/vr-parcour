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

    public void EditorOpenOrCloseHelpMenu()
    {
        if (menuInstanced == null)
        {
            Open(transform);
        }
        else
        {
            Close();
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
        }
    }
    
}