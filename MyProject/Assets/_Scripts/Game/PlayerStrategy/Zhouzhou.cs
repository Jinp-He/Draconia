using _Scripts.Game.Event;
using _Scripts.System;
using cfg;
using QFramework;
using UnityEngine;

namespace _Scripts.Game.PlayerStrategy
{
    public class Zhouzhou : Player
    {
        public Zhouzhou()
        {
            
        }



        private bool _isFirstTimeEnter = true;
        public override void Init(PlayerInfo playerInfo)
        {
            base.Init(playerInfo);
            Mastery = 1;
            
        }

       
        public override void OnBattleStart()
        {
            BattleStartEvent = this.RegisterEvent<EnterDangerAreaEvent>(e =>
            {
                if (_isFirstTimeEnter)
                {
                    Debug.Log("#DEBUG#" + this.GetSystem<BattleSystem>().Hands.Cards.Count);
                    this.GetSystem<BattleSystem>().Hands.AddRandomBasicCard(PlayerViewController, Mastery);
                    PlayerViewController.StartCoroutine(PlayerViewController.PlayerAnimator.SendNotificationText("触发避柳"));
                }

                _isFirstTimeEnter = false;
            });
        }

        public override void OnBattleEnd()
        {
            base.OnBattleEnd();
            BattleStartEvent.UnRegister();
        }

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            _isFirstTimeEnter = true;
        }

        public override void InvokePassive()
        {
            this.GetSystem<BattleSystem>().Hands.AddRandomBasicCard(PlayerViewController, Mastery);
            PlayerViewController.StartCoroutine(PlayerViewController.PlayerAnimator.SendNotificationText("触发避柳"));
        }

        private int Counter = 3;


    }
}