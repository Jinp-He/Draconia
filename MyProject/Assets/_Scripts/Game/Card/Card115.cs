using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card115 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            if (_enemies.Count != 0)
            {
                BattleSystem.TimeBar.MoveRelativeTimePosition(_enemies[0],2);
            }
            else
            {
                BattleSystem.TimeBar.MoveRelativeTimePosition(_allies[0], 2);
                BattleSystem.DrawCard(CardUser, 2);
            }
        }
    }
}