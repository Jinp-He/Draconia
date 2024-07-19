using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card105 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            _allies[0].Player.MoveInTime(2);
            BattleSystem.DrawCard(CardUser, 1);

        }
    }
}