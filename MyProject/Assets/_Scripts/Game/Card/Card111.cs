using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card111 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {   
            BattleSystem.Hands.AddBasicCard(CardUser, 2);
            CardUser.Player.AddBuff("轻盈", 1);
        }
    }
}