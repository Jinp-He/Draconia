using System;
using System.Collections.Generic;

using UnityEngine;
using cfg;
using Draconia.MyComponent;
using Draconia.System;
using QFramework;

namespace Draconia.Game.Buff
{
    public class BuffManager : MonoBehaviour, ICanGetSystem
    {
        public Dictionary<string, Buff> Buffs;
        public Buff BuffPrefab;
        public RectTransform BuffBar;
        public void Start()
        {
            Buffs = new Dictionary<string, Buff>();
        }

        public void AddBuff(string buffName, int stack)
        {
            if(Buffs.TryGetValue(buffName, out Buff value))
            {
                value.Stack += stack;
            }
            else
            {
                Buff buff = Instantiate(BuffPrefab, BuffBar);
                buff.Init(this.GetSystem<ResLoadSystem>().Table.TbBuffInfo[buffName], stack, this);
                Buffs.Add(buffName, buff);
            }
        }

        public void RemoveBuff(string buffName)
        {
            Buffs[buffName].End();
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}