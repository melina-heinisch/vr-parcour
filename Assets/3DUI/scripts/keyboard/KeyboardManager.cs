using System;
using UnityEngine;

namespace _3DUI.scripts.keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        public Action<string> UpdateDisplay;
        public Action<string> EnterText;
        
        private KeyManager[] keys;
        private string typedContent;

        private void Awake()
        {
            keys = GetComponentsInChildren<KeyManager>();
        }

        public void Reset()
        {
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
    }
}