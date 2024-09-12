using System.Collections.Generic;
using _Scripts.Game.Player;

namespace _Scripts.Game.Card
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