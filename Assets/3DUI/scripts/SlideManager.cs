using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

namespace _3DUI.scripts
{
    public class SlideManager : MonoBehaviour
    {
        public Action closeAction;
        
        [SerializeField] private GameObject previousSlide, nextSlide;
        [SerializeField] private TextMeshProUGUI title;
    
        private int currentSlide = 0;
        [SerializeField] private List<Slide> slides = new ();

        private VRHostSystem VRHostSystem = null;
        private bool thumbStickIsLeft, thumbStickIsRight;
        
        [Serializable] public struct Slide
        {
            public string Title;
            public GameObject Content;
        }

        public void Start()
        {
            VRHostSystem = GameObject.FindGameObjectWithTag("VRHostSystemDevices").GetComponent<VRHostSystem>();
            
            currentSlide = 0;
            ApplySlide();
        }

        public void Update()
        {
            if (VRHostSystem == null)
            {
                VRHostSystem = GameObject.FindGameObjectWithTag("VRHostSystemDevices").GetComponent<VRHostSystem>();
            }
            else
            {
                if (VRHostSystem.AreAllDevicesFound())
                {
                    if (VRHostSystem.GetLeftHandDevice().isValid)
                    {
                        if (VRHostSystem.GetLeftHandDevice()
                            .TryGetFeatureValue(CommonUsages.primary2DAxis, out var thumbstickAxisValue))
                        {
                            var x = thumbstickAxisValue.x;
                            if (x <= -.5f)
                            {
                                if (thumbStickIsLeft) return;
                                thumbStickIsLeft = true;
                                PreviousSlide();
                            }
                            else if (x >= .5f)
                            {
                                if (thumbStickIsRight) return;
                                thumbStickIsRight = true;
                                NextSlide();
                            }
                            else
                            {
                                thumbStickIsLeft = false;
                                thumbStickIsRight = false;
                            }
                        }
                    }
                }
            }
        }

        public void Close()
        {
            closeAction.Invoke();
        }
        
        public void PreviousSlide()
        {
            if (currentSlide > 0)
            {
                currentSlide--;
                ApplySlide();
            }
        }

        public void NextSlide()
        {
            if (currentSlide < slides.Count - 1)
            {
                currentSlide++;
                ApplySlide();
            }
        }

        private void ApplySlide()
        {
            previousSlide.SetActive(currentSlide != 0);
            nextSlide.SetActive(currentSlide != slides.Count-1);
            for (int i = 0; i < slides.Count; i++)
            {
                if (currentSlide == i)
                {
                    slides[i].Content.SetActive(true);
                    title.text = slides[i].Title;
                }
                else
                {
                    slides[i].Content.SetActive(false);
                }
            }
        
        }
    }
}