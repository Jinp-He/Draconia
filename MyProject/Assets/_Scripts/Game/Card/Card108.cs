using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card108 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            foreach (var player in BattleSystem.Players)
            {
                player.AddArmor(player.IsInDangerArea() ? 5 : 3);
            }
        }
    }
}