using System.Collections.Generic;
using cfg;
using Draconia.Game.Buff.Pose;
using Draconia.System;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;
using Utility;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.Game.Buff
{
    public class BuffEffect: ICanRegisterEvent, ICanGetSystem
    {
  

        protected BattleSystem BattleSystem => this.GetSystem<BattleSystem>();
        protected Buff Buff;
        protected Player Player;
        protected BuffInfo _buffInfo;
        protected List<IUnRegister> UnRegisters;
        protected BuffManager _buffManager;


        public static BuffEffect GetEffect(BuffInfo buffId)
        {
            switch (buffId.BuffName)
            {
                case "轻盈":
                    return new BasicCardBuff();
                case "溃敌":
                    return new DoubleBasicCard();
                case "飞鸟式":
                    return new Feiniao();
                case "扶摇式":
                    return new Fuyao();
                default:
                    return new BasicCardBuff();
            }
        }
        
        public virtual void Init(Buff buff, BuffInfo buffInfo, int stack, BuffManager buffManager)
        {
            Buff = buff;
            UnRegisters = new List<IUnRegister>();
            _buffManager = buffManager;
            _buffInfo = buffInfo;
            Player = buffManager.Player;

            if (!_buffInfo.IsConsis)
            {
                UnRegisters.Add(this.RegisterEvent<PlayerTurnStartEvent>(e =>
                {
                    PlayerTurnStart();
                    Buff.Stack--;
                }));
            }
        }

        //添加buff时候施加的效果
        public virtual void OnAddBuff()
        {
            
        }

        //Buff消失时候世家的效果
        public virtual void OnRemoveBuff()
        {
            
        }

        public virtual void PlayerTurnStart()
        {
            
        }

        public void End()
        {
            OnRemoveBuff();
            OnEnd();
            _buffManager.Buffs.Remove(_buffInfo.BuffName);
        }

        public virtual void OnEnd()
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}