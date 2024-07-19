using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card102 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            CardUser.Defense(2);        
        }
    }
}