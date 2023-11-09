using Draconia.System;
using QFramework;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
    public class PlayerCardProcessor : ICanGetSystem, ICanRegisterEvent
    {

        public void PlayCard(Card card)
        {
            
        }

        public void EnterPose(Player player, string poseId)
        {
            
        }
        
        
        
        
        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}