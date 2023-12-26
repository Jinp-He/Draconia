using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.ViewController
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
            this.RegisterEvent<EnterDangerAreaEvent>(e =>
            {
                if (_isFirstTimeEnter)
                {
                    this.GetSystem<BattleSystem>().Hands.AddRandomBasicCard(PlayerViewController, Mastery);
                    PlayerViewController.StartCoroutine(PlayerViewController.PlayerAnimator.SendNotificationText("触发避柳"));
                }

                _isFirstTimeEnter = false;
            });
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