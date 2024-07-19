using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card110 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            CardUser.Player.Charge((e) =>
            {
                BattleSystem.Attack(CardUser, _enemies[0], AttackType.Physical, 2*e);
            });
        }
    }
}