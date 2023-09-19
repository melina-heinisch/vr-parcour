using System;
using UnityEngine;
using UnityEngine.XR;

namespace _3DUI.scripts.keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        public Action<string> UpdateDisplay; // initialized in GameLogic 
        public Action<string> EnterText; // initialized in GameLogic 

        private VRHostSystem VRHostSystem;
        private Vector3 startPosition;
        
        private KeyManager[] keys;
        private string typedContent;

        private void Awake()
        {
            keys = GetComponentsInChildren<KeyManager>();
            startPosition = gameObject.transform.position;
        }

        public void Update()
        {
            if (VRHostSystem == null) VRHostSystem = FindObjectOfType<VRHostSystem>();
            else if (VRHostSystem.AreAllDevicesFound()) 
                RepositionKeyboardButtonCheck();
        }

        public void Reset()
        {
            gameObject.transform.position = startPosition;
            typedContent = "";
            if(keys[0].isShifted)
                ToggleShift();
        }

        public void EnterCharacter(string character)
        {
            gameObject.GetComponent<AudioSource>().Play();
            switch (character)
            {
                case "shift":
                    ToggleShift();
                    break;
                case "backspace":
                    RemoveCharacter();
                    break;
                case "enter":
                    Enter();
                    break;
                case "space":
                    EnterCharacter(" ");
                    break;
                default:
                    typedContent += character;
                    UpdateDisplay.Invoke(typedContent);
                    break;
            }
        }

        private void ToggleShift()
        {
            foreach (var key in keys)
            {
                key.ToggleShift();
            }
        }

        public void RemoveCharacter()
        {
            typedContent = typedContent.Substring(0, typedContent.Length - 1);
            UpdateDisplay.Invoke(typedContent);
        }

        public void Enter()
        {
            EnterText.Invoke(typedContent);
        }

        private void RepositionKeyboardButtonCheck()
        {
            if (VRHostSystem.GetLeftHandDevice().isValid)
            {
                if (VRHostSystem.GetLeftHandDevice().TryGetFeatureValue(CommonUsages.primary2DAxis, out var primaryAxis))
                {
                    if (primaryAxis.x > 0.3f || primaryAxis.x < -0.3f)
                    {
                        gameObject.transform.Translate(Vector3.forward * (primaryAxis.x / 2 * Time.deltaTime), Space.World);
                    }

                    if (primaryAxis.y > 0.3f || primaryAxis.y < -0.3f)
                    {
                        gameObject.transform.Translate(Vector3.left * (primaryAxis.y / 2 * Time.deltaTime), Space.World);
                    }
                }
            }
        }
    }
}