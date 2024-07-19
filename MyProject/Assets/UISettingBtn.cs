using System.Collections;
using System.Collections.Generic;
using Draconia.UI;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class UISettingBtn : Button
{
    // Start is called before the first frame update
    void Start()
    {
        onClick.AddListener(() =>
        {
            UIKit.OpenPanel<UISettingPanel>();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
