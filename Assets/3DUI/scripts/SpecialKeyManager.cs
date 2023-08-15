using UnityEngine;
using UnityEngine.UI;

namespace _3DUI.scripts
{
    public class SpecialKeyManager : MonoBehaviour
    {
        [SerializeField] private KeyboardManager keyboardManager;
        [SerializeField] private string functionCode;

        public void Start()
        {
            GetComponent<Button>().onClick.AddListener(Pressed);
        }
        
        public void Pressed()
        {
            keyboardManager.EnterCharacter(functionCode);
        }
    }
}