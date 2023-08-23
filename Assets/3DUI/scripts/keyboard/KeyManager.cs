using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3DUI.scripts.keyboard
{
    public class KeyManager : MonoBehaviour
    {
        public bool isShifted;
        [SerializeField] private KeyboardManager keyboardManager;
        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private string lowerCaseChar;
        [SerializeField] private string upperCaseChar;

        public void Start()
        {
            GetComponent<Button>().onClick.AddListener(EnterCharacter);
        }

        public void EnterCharacter()
        {
            keyboardManager.EnterCharacter(isShifted ? upperCaseChar : lowerCaseChar);
        }
        
        public void ToggleShift()
        {
            if (isShifted)
            {
                isShifted = false;
                text.text = lowerCaseChar;
            }
            else
            {
                isShifted = true;
                text.text = upperCaseChar;
            }
        }
    }
}