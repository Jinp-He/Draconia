using System.Linq;
using cfg;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;

namespace Draconia.Game.Buff.Pose
{
    public class Ta : Pose
    {
        public override void OnAddBuff()
        {
            base.OnAddBuff();

            this.RegisterEvent<IsHitEvent>(e =>
            {
                if (e.Attacker == Character && e.AttackType == AttackType.Physical && e.RealDamage > 0)
                {
                    BattleSystem.Attack((Player)Character, (Enemy)e.AttackReceiver.NextCharacter(), AttackType.Magic,
                        1);
                }
            });

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