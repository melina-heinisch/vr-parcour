using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Color;

namespace _3DUI.scripts
{
    public class VirtualHand : MonoBehaviour
    {
        [Tooltip("You need to manually add reference to the VRHostSystem GameObject")]
        public VRHostSystem VRHostSystem;

        private GameObject handController;
        private GameObject rightHandController;

        private GameObject grippedObject;
        private GameObject collidedObject;
        private bool gripButtonIsPressed;
        [SerializeField] private Material previousMaterial;
        [SerializeField] private GameObject previousParent;

        void Start()
        {
            GetHandControllerGameObject();
            rightHandController = GameObject.Find("RightHand Controller");
            gripButtonIsPressed = false;
        }

        void Update()
        {
            if (VRHostSystem == null) Debug.LogError("VRHostSystem variable was not defined via inspector!");
            else
            {
                if (VRHostSystem.AreAllDevicesFound())
                {
                    if (collidedObject != null && VRHostSystem.GetRightHandDevice()
                            .TryGetFeatureValue(CommonUsages.gripButton, out bool isPressedNow))
                    {
                        if (isPressedNow && !gripButtonIsPressed)
                        {
                            gripButtonIsPressed = true;
                            StartVirtualHand();
                        }
                        else if (!isPressedNow)
                        {
                            gripButtonIsPressed = false;
                            StopVirtualHand();
                        }
                    }
                }
            }
        }

        private void GetHandControllerGameObject()
        {
            handController = this.gameObject; // i.e. with this script component and an XR controller component
        }

        private void StartVirtualHand()
        {
            Renderer renderer = collidedObject.GetComponent<Renderer>();
            renderer.material.color = red;
            previousParent = collidedObject.transform.parent.gameObject;
            collidedObject.transform.parent = rightHandController.transform;
        }

        private void StopVirtualHand()
        {
            Renderer renderer = collidedObject.GetComponent<Renderer>();
            renderer.material.color = cyan;
            collidedObject.transform.parent = previousParent.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (gripButtonIsPressed)
                return;

            collidedObject = other.gameObject;
            Renderer renderer = collidedObject.GetComponent<Renderer>();
            previousMaterial = renderer.material;
            renderer.material.color = cyan;
            uint channel = 0;
            float amplitude = 0.5f;
            float duration = 0.5f;
            VRHostSystem.GetRightHandDevice().SendHapticImpulse(channel, amplitude, duration);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == collidedObject)
            {
                Renderer renderer = collidedObject.GetComponent<Renderer>();
                renderer.material = previousMaterial;
                collidedObject = null;
            }
        }
    }
}