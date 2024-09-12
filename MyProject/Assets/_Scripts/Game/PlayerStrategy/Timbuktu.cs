using _Scripts.Game.Event;
using _Scripts.Game.Player;
using cfg;
using QFramework;

namespace _Scripts.Game.PlayerStrategy
{
    public class Timbuktu : Player
    {
        public Timbuktu()
        {
            
        }
        public Timbuktu(PlayerViewController playerViewController)
        {
            
        }


        private bool _isFirstTimeEnter = true;
        public override void Init(PlayerInfo playerInfo)
        {
            base.Init(playerInfo);
            this.RegisterEvent<AttackEvent>(e =>
            {
                if (e.Attacker == PlayerViewController)
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