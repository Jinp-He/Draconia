using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Draconia.UIToolkit
{
    public class SliderText : MonoBehaviour
    {
        public Slider Slider;
        public TextMeshProUGUI Text;
        private void Start()
        {
            if (Slider == null)
            {
                return;
                
            }
            Slider.onValueChanged.AddListener((e) =>
            {
                Text.text = e + "%";
            });

        }
    }
}