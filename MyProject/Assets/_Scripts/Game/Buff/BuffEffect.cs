using System.Collections.Generic;
using _Scripts.Game.Event;
using _Scripts.Game.Player;
using _Scripts.System;
using cfg;
using QFramework;

namespace _Scripts.Game.Buff
{
    public class BuffEffect: ICanRegisterEvent, ICanGetSystem
    {
  

        protected BattleSystem BattleSystem => this.GetSystem<BattleSystem>();
        protected Buff Buff;
        protected CharacterViewController CharacterViewController;
        protected BuffInfo _buffInfo;
        protected List<IUnRegister> UnRegisters;
        protected BuffManager _buffManager;


        public static BuffEffect GetEffect(BuffInfo buffInfo)
        {
            switch (buffInfo.BuffName)
            {
                default:
                    return new Charge();
            }
        }
        
        public virtual void Init(Buff buff, BuffInfo buffInfo,  BuffManager buffManager)
        {
            Buff = buff;
            UnRegisters = new List<IUnRegister>();
            _buffManager = buffManager;
            _buffInfo = buffInfo;
            CharacterViewController = buffManager.CharacterViewController;

                UnRegisters.Add(this.RegisterEvent<PlayerTurnStartEvent>(e =>
                {
                    PlayerTurnStart();
                }));

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
            return Draconia.Draconia.Interface;
        }
    }
}