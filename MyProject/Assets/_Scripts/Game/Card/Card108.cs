using System.Collections.Generic;
using _Scripts.Game.Player;

namespace _Scripts.Game.Card
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