using TMPro;
using UnityEngine;

namespace _3DUI.scripts
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private float alpha;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private FaderStatus currentStatus = FaderStatus.IDLE;
        
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private CanvasGroup faderCanvasGroup, infoCanvasGroup;

        private static Fader instance;
        
        private void Start()
        {
            instance = this;
        }
        
        private void Update()
        {
            if (currentStatus == FaderStatus.FADE_TO_SCENE)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                faderCanvasGroup.alpha = alpha;
                infoCanvasGroup.alpha = alpha;

                if (alpha <= 0.0f)
                {
                    currentStatus = FaderStatus.IDLE;
                    alpha = 0.0f;
                }
            }

            if (currentStatus == FaderStatus.FADE_TO_BLACK)
            {
                alpha += Time.deltaTime * fadeSpeed;
                faderCanvasGroup.alpha = alpha;
                infoCanvasGroup.alpha = alpha;

                if (alpha >= 1.0f)
                {
                    currentStatus = FaderStatus.IDLE;
                    alpha = 1.0f;
                }
            }
        }

        private enum FaderStatus
        {
            IDLE,
            FADE_TO_SCENE, // FadeIn means: The Faders opacity diminishes and the actual world becomes visible.
            FADE_TO_BLACK // FadeOut means: The Faders opacity increases and the actual world becomes hidden behind the fader.
        }

        public static void FadeToBlack(float fadeSpeed = 1.0f, string infoText = "")
        {
            instance.infoText.text = infoText;
            instance.fadeSpeed = fadeSpeed;
            instance.currentStatus = FaderStatus.FADE_TO_BLACK;
        }
        
        public static void FadeToScene(float fadeSpeed = 1.0f)
        {
            instance.fadeSpeed = fadeSpeed;
            instance.currentStatus = FaderStatus.FADE_TO_SCENE;
        }
    }
}
