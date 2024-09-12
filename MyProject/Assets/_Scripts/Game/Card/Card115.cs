using System.Collections.Generic;
using _Scripts.Game.Player;

namespace _Scripts.Game.Card
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