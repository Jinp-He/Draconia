using System.Linq;
using cfg;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff
{
    public class Charge : BuffEffect
    {

        
        public override void OnAddBuff()
        {
            base.OnAddBuff();
            BattleSystem.OngoingPlayerViewController.Player.Hands.Where(e => e.IsBasicCard)
                .ForEach(e => e.Properties.Add(EnumCardProperty.Double));

            UnRegisters.Add(this.RegisterEvent<UseCardEvent>(e =>
            {
                if (e.UsedCard.IsBasicCard && e.CharacterViewController == CharacterViewController)
                {
                    BattleSystem.OngoingPlayerViewController.Player.Hands.Where(e => e.IsBasicCard)
                        .ForEach(e => e.Properties.Remove(EnumCardProperty.Double));
                }
            }));
            
            UnRegisters.Add(this.RegisterEvent<DrawCardEvent>(e =>
            {
                e.Cards.Where(e => e.IsBasicCard && e.CardPlayerViewController == CharacterViewController).ForEach(e => e.TempCostModifier -= 1);
            }));
        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            UnRegisters.ForEach(e => e.UnRegister());
        }
    }
}