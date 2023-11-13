using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.ViewController
{
    public class Zhouzhou : PlayerStrategy
    {
        public int Mastery;
        public Zhouzhou(Player player)
        {
            
        }


        private bool _isFirstTimeEnter = true;
        public override void Init(Player player)
        {
            base.Init(player);
            Mastery = 1;
            this.RegisterEvent<EnterDangerAreaEvent>(e =>
            {
                if (_isFirstTimeEnter)
                {
                    this.GetSystem<BattleSystem>().Hands.AddRandomBasicCard(_player, Mastery);
                    _player.StartCoroutine(_player.PlayerAnimator.SendNotificationText("触发避柳"));
                }

                _isFirstTimeEnter = false;
            });
        }

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            _isFirstTimeEnter = true;
        }
    }
}