using System.Collections;
using System.Collections.Generic;
using Draconia.System;

using QFramework;
using UnityEngine;

namespace Draconia
{
    public class Draconia : Architecture<Draconia>
    {
        protected override void Init()
        {
            
            RegisterSystem(new ResLoadSystem());
            RegisterSystem(new BattleSystem());
            RegisterSystem(new SaveSystem());
            RegisterSystem(new GameSystem());
            RegisterSystem(new MapSystem());
        }
    }
}                       