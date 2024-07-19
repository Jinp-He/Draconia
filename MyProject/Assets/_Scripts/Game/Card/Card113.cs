using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card113 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            int k = CardUser.Player.CardUsageHistory.Where(e => e.Card.IsBasicCard).Count();
            BattleSystem.Attack( CardUser, _enemies[0], AttackType.Physical, 3 + 2 * k);
        }
    }
}