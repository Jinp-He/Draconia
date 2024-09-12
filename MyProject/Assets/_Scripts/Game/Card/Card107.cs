using System.Collections.Generic;
using _Scripts.Game.Player;

namespace _Scripts.Game.Card
{
    public class Card107 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            CardUser.Player.Charge((e) =>
            {
                BattleSystem.DrawCard(CardUser, e);
            });
        }
    }
}