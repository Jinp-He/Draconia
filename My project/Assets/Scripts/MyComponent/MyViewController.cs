using Draconia.System;
using QFramework;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.MyComponent
{
    public class MyViewController : QFramework.ViewController, ICanGetSystem, ICanGetModel, ICanRegisterEvent
    {
        public BattleSystem BattleSystem => this.GetSystem<BattleSystem>();
        public ResLoadSystem ResLoadSystem => this.GetSystem<ResLoadSystem>();
        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}