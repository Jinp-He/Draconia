using System.Linq;
using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff
{
    public class BasicCardBuff : BuffEffect
    {
        private IUnRegister _unRegister;
        public override void OnAddBuff()
        {
            base.OnAddBuff();
            BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                .ForEach(e => e.TempCostModifier -= 1);

            _unRegister = this.RegisterEvent<UseCardEvent>(e =>
            {
                if (e.UsedCard.IsBasicCard)
                {
                    BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                        .ForEach(e => e.TempCostModifier += 1);
                    Buff.Stack--;
                    if (Buff.Stack == 0)
                    {
                        _unRegister.UnRegister();
                    }
                }
            });
        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            //_unRegister.UnRegister();
        }
    }
}