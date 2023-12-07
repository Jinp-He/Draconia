using System.Linq;
using cfg;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;

namespace Draconia.Game.Buff.Pose
{
    public class EYun : Pose
    {
        public override void OnAddBuff()
        {
            base.OnAddBuff();
            

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