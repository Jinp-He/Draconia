using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card114 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            int k = CardUser.Player.CardUsageHistory.Where(e => e.Card.IsBasicCard).Count();
            for (int i = 0; i < k; i++)
            {
                BattleSystem.Attack( CardUser, _enemies[0], AttackType.Physical, 3);
            }
        }
    }
}