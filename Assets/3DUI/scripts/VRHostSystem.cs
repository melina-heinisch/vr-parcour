using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class VRHostSystem : MonoBehaviour
{
    [Tooltip("You need to manually add reference to the XROrigin GameObject")]
    public XROrigin xrOrigin = null;

    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    private Camera userXRCamera = null;
    private GameObject xrOriginGameObject = null;

    private bool foundUserCamera = false;
    private bool foundLeftHandDevice = false;
    private bool foundRightHandDevice = false;

    /************ VR Hardware Access ******************/
    public Camera GetCamera() { return userXRCamera; }
    public InputDevice GetLeftHandDevice() { return leftHandDevice; }
    public InputDevice GetRightHandDevice() { return rightHandDevice; }
    public XROrigin getXROrigin() { return xrOrigin; }
    public GameObject getXROriginGameObject() { return xrOriginGameObject; }

    /************ VR Hardware on Host Search ******************/

    private void Start()
    {
        if (xrOrigin == null)
        {
            Debug.LogError("XR Origin variable was not defined via inspector!");
        }
        else
        {
            StartCoroutine("WaitUntilFoundAllVRDevices");
        }
    }

    private IEnumerator WaitUntilFoundAllVRDevices()
    {
        GetVRDevices();
        if (AreAllDevicesFound())
        {
            Debug.Log("ALL VR Devices sucessfully found");
            StopCoroutine("WaitUntilFoundAllVRDevices"); // not mandatory just a safe guard
        }
        else
        {
            Debug.Log("ALL VR Devices not found yet.. wait a bit to try again");
            yield return new WaitForSeconds(1);
            StartCoroutine("WaitUntilFoundAllVRDevices");
        }
    }

    public bool AreAllDevicesFound()
    {
        return foundLeftHandDevice
           && foundLeftHandDevice
           && foundUserCamera;
    }

    void GetVRDevices()
    {
        ListAllDevicesFoundNowOnConsole();
        FindXROrigin();
        FindXROriginMainCamera();
        FindLeftHandDevice();
        FindRightHandDevice();
    }

    private void ListAllDevicesFoundNowOnConsole()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        foreach (var inputdevice in inputDevices)
            Debug.Log("Input Device Found : " + inputdevice.characteristics.ToString());
    }

    private void FindLeftHandDevice()
    {
        if (foundLeftHandDevice) return;
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand
            | InputDeviceCharacteristics.Left
            | InputDeviceCharacteristics.Controller;
        var desiredDevicesFoundList = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, desiredDevicesFoundList);
        foreach (var device in desiredDevicesFoundList)
        {
            Debug.Log(string.Format("Found Device name '{0}' has characteristics '{1}'",
                device.name, device.characteristics.ToString()));
            leftHandDevice = device;
            foundLeftHandDevice = true;
        }
    }

    private void FindRightHandDevice()
    {
        if (foundRightHandDevice) return;
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand
            | InputDeviceCharacteristics.Right
            | InputDeviceCharacteristics.Controller;
        var desiredDevicesFoundList = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, desiredDevicesFoundList);
        foreach (var device in desiredDevicesFoundList)
        {
            Debug.Log(string.Format("Found Device name '{0}' has characteristics '{1}'",
                device.name, device.characteristics.ToString()));
            rightHandDevice = device;
            foundRightHandDevice = true;
        }
    }

    private void FindXROriginMainCamera()
    {
        if (foundUserCamera) return;
        var XROrigin = GetComponentInParent<XROrigin>(); // i.e Roomscale tracking space 
        userXRCamera = XROrigin.GetComponentInChildren<Camera>();
        if (userXRCamera == null)
        {
            Debug.LogError("MainCamera in XR Rig not found! " +
                "(XR Rig should be parent of this game object:)"
                + gameObject + " =>> cannot open help menu");
        }
        else
        {
            Debug.Log("Found User Camera: " + userXRCamera);
            foundUserCamera = true;
        }
    }

    private void FindXROrigin()
    {
        xrOriginGameObject = xrOrigin.Origin; // Gameobject representing the center of tracking space in virtual enviroment
    }
}

