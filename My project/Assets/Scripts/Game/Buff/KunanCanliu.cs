using System.Linq;
using cfg;
using Draconia.ViewController.Event;
using QFramework;

namespace Draconia.Game.Buff
{
    public class KunanCanliu : BuffEffect
    {

        public override void OnAddBuff()
        {
            base.OnAddBuff();
            Character.OnTurnStart += GetDamaged;
        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            Character.OnTurnStart -= GetDamaged;
        }

        private void GetDamaged()
        {
            Character.IsHit(1, AttackType.TrueDamage);
        }
    }
}