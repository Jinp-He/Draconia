using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.ViewController
{
    public class Timbuktu : PlayerStrategy
    {
        public int Mastery;
        public Timbuktu(Player player)
        {
            
        }


        private bool _isFirstTimeEnter = true;
        public override void Init(Player player)
        {
            base.Init(player);
            this.RegisterEvent<AttackEvent>(e =>
            {
                if (e.Attacker == player)
                {
                    if (e.AttackReceiver is Enemy)
                    {
                        e.AttackReceiver.As<Enemy>().RefreshBuff("苦难残留", 2);
                    }
                }
            });
            Mastery = 1;

        }

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            _isFirstTimeEnter = true;
        }
    }
}