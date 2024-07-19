using System.Linq;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;

namespace Draconia.Game.Buff.Pose
{
    public class Feiniao : Pose
    {
        public override void OnAddBuff()
        {
            base.OnAddBuff();

            _buffManager.Buffs.Values.Where(e => e.BuffName is "轻盈" or "溃敌").ForEach(e => e.Stack = 99);
            
            UnRegisters.Add(this.RegisterEvent<AddBuffEvent>(e =>
            {
                if (e.Buff.BuffName is "轻盈" or "溃敌")
                {
                    e.Buff.Stack = 99;
                }
            }));

        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            _buffManager.Buffs.Values.Where(e => e.BuffName is "轻盈" or "溃敌").ForEach(e => e.Stack = 0);
            foreach (var unRegister in UnRegisters)
            {
                unRegister.UnRegister();
            }
        }
    }
}