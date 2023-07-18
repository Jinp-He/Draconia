using System.Collections;
using System.Collections.Generic;
using Draconia.System;
using Draconia.UI;
using QFramework;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Draconia
{


    public class Root : MonoBehaviour, ICanGetSystem
    {
        // Start is called before the first frame update
        void Start()
        {
            ResKit.Init();
            this.GetSystem<BattleSystem>().TestInit();
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
