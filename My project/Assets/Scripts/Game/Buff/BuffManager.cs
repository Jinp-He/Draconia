using System;
using System.Collections.Generic;

using UnityEngine;
using cfg;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;


namespace Draconia.Game.Buff
{
    public class BuffManager : MonoBehaviour, ICanGetSystem,ICanSendEvent
    {
        public Dictionary<string, Buff> Buffs;
        public Buff BuffPrefab;
        public RectTransform BuffBar;
        public Player Player;
        private Buff Pose;
        public void Start()
        {
            Buffs = new Dictionary<string, Buff>();
            Player = GetComponent<Player>();
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
                BuffInfo info = this.GetSystem<ResLoadSystem>().Table.TbBuffInfo[buffName];
                buff.Init(info, stack, this);
                Buffs.Add(buffName, buff);
                if (info.IsPose)
                {
                    if (Pose != null)
                    {
                        Pose.End();
                    }
                    Pose = buff;
                }
                this.SendEvent(new AddBuffEvent(){Character = Player, Buff = buff});
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