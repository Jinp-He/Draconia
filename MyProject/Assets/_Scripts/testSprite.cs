using System.Collections;
using System.Collections.Generic;
using Draconia.System;
using QFramework;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Draconia
{


    public class testSprite : MonoBehaviour, ICanGetSystem
    {
        public SpriteAtlas testSP;

        // Start is called before the first frame update
        void Start()
        {
            this.GetComponent<UnityEngine.UI.Image>().sprite = testSP.GetSprite("背景");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}
