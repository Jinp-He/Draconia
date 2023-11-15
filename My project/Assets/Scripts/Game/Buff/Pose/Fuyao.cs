using System.Linq;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff.Pose
{
    public class Fuyao : Pose
    {
        public override void OnAddBuff()
        {
            base.OnAddBuff();
            Buff.Stack = 0;
            

            UnRegisters.Add(this.RegisterEvent<UseCardEvent>(e =>
            {
                if (e.UsedCard.IsBasicCard && e.Character == Player)
                {
                    Buff.Stack++;
                    if (Buff.Stack >= 3)
                    {
                        Buff.Stack = 0;
                        e.Character.SendNotification("触发飞鸟式");
                        BattleSystem.Players.ForEach(e => e.MoveInTime(-1));
                    }
                }
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