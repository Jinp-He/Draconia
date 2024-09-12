using _Scripts.Game.Event;
using _Scripts.Game.Player;
using cfg;
using QFramework;

namespace _Scripts.Game.Buff.Pose
{
    public class Ta : Pose
    {
        public override void OnAddBuff()
        {
            base.OnAddBuff();

            this.RegisterEvent<IsHitEvent>(e =>
            {
                if (e.Attacker == CharacterViewController && e.AttackType == AttackType.Physical && e.RealDamage > 0)
                {
                    BattleSystem.Attack((PlayerViewController)CharacterViewController, (Enemy)e.AttackReceiver.NextCharacter(), AttackType.Magic,
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