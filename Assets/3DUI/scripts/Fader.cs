using System;
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
            if (currentStatus == FaderStatus.FADE_IN)
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

            if (currentStatus == FaderStatus.FADE_OUT)
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
            FADE_IN, // FadeIn means: The Faders opacity diminishes and the actual world becomes visible.
            FADE_OUT // FadeOut means: The Faders opacity increases and the actual world becomes hidden behind the fader.
        }

        public static void FadOut(float fadeSpeed = 1.0f, string infoText = "")
        {
            instance.infoText.text = infoText;
            instance.fadeSpeed = fadeSpeed;
            instance.currentStatus = FaderStatus.FADE_OUT;
        }
        
        public static void FadeIn(float fadeSpeed = 1.0f)
        {
            instance.fadeSpeed = fadeSpeed;
            instance.currentStatus = FaderStatus.FADE_IN;
        }
    }
}
