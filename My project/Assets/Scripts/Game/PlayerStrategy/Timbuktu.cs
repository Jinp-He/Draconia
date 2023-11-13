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
            Mastery = 1;

        }

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            _isFirstTimeEnter = true;
        }
    }
}