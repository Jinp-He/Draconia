using System.Linq;
using cfg;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff
{
    public class DoubleBasicCard : BuffEffect
    {

        public override void OnAddBuff()
        {
            base.OnAddBuff();
            BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                .ForEach(e => e.Properties.Add(EnumCardProperty.Double));

            UnRegisters.Add(this.RegisterEvent<UseCardEvent>(e =>
            {
                if (e.UsedCard.IsBasicCard && e.Character == Character)
                {
                    BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                        .ForEach(e => e.Properties.Remove(EnumCardProperty.Double));
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
            UnRegisters.ForEach(e => e.UnRegister());
        }
    }
}