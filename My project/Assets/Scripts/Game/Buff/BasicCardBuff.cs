using System.Linq;
using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff
{
    public class BasicCardBuff : BuffEffect
    {

        public override void OnAddBuff()
        {
            base.OnAddBuff();
            BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                .ForEach(e => e.TempCostModifier -= 1);
            
            

            UnRegisters.Add(this.RegisterEvent<UseCardEvent>(e =>
            {
                if (e.UsedCard.IsBasicCard && e.Character == Character)
                {
                    BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                        .ForEach(e => e.TempCostModifier += 1);
                    Buff.Stack--;
                   
                }
            }));
            
            UnRegisters.Add(this.RegisterEvent<DrawCardEvent>(e =>
            {
                e.Cards.Where(e => e.IsBasicCard && e.CardPlayer == Character).ForEach(e => e.TempCostModifier -= 1);
            }));
        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            foreach (var unRegister in UnRegisters)
            {
                unRegister.UnRegister();
            }
        }
    }
}