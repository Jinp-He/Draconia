using _Scripts.Game.Event;
using QFramework;

namespace _Scripts.Game.Buff
{
    public class Charge : BuffEffect
    {

        
        public override void OnAddBuff()
        {
            base.OnAddBuff();
            UnRegisters.Add(_buffManager.CharacterViewController.MyPointer.PosDiff.Register(e =>
            {
                if (e > 0)
                {
                    Buff.Stack += e;
                }
            }));
            UnRegisters.Add(this.RegisterEvent<PlayerTurnStartEvent>(e =>
            {
                if (e.Player.Alias == _buffManager.CharacterViewController.Player.Alias)
                {
                    Buff.End();
                }
            }));
        }

        public override void OnRemoveBuff()
        {
            base.OnRemoveBuff();
            UnRegisters.ForEach(e => e.UnRegister());
        }
    }
}