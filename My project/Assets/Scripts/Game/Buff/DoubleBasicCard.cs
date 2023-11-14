using System.Linq;
using cfg;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff
{
    public class DoubleBasicCard : BuffEffect
    {
        private IUnRegister _unRegister;
        public override void OnAddBuff()
        {
            base.OnAddBuff();
            BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                .ForEach(e => e.Properties.Add(EnumCardProperty.Double));

            _unRegister = this.RegisterEvent<UseCardEvent>(e =>
            {
                if (e.UsedCard.IsBasicCard)
                {
                    BattleSystem.OngoingPlayer.PlayerStrategy.Hands.Where(e => e.IsBasicCard)
                        .ForEach(e => e.Properties.Remove(EnumCardProperty.Double));
                }
            });
        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            _unRegister.UnRegister();
        }
    }
}