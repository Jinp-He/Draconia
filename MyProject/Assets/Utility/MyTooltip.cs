using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class MyTooltip : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Passive;

        public string TitleText;
        public string DescriptionText;
        public string PassiveText;
        public void Start()
        {
            //Title.text = TitleText;
            //Description.text = DescriptionText;
            //Passive.text = PassiveText;
        }
        public void Initialize(Tooltip lists)
        {
            
            Title.text = lists.Name;
            Description.text = lists.Desc;
            Passive.text = lists.Type;
        }
        
    }
    
    
}