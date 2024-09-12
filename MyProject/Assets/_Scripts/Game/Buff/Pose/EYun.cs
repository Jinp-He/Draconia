namespace _Scripts.Game.Buff.Pose
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