using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card112 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            int k = CardUser.Player.Hp;
            CardUser.Player.Charge(e =>
            {
                if(CardUser.Player.Hp == k)
                    BattleSystem.Attack( CardUser, _enemies[0], AttackType.Physical, 3*e);
            });
        }
    }
}