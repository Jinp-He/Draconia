using System.Collections.Generic;
using _Scripts.Game.Player;
using cfg;

namespace _Scripts.Game.Card
{
    public class Card103 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            BattleSystem.Attack(CardUser, _enemies[0], AttackType.Physical, 4);
            //TODO cost modifieir 
            CardUser.Player.AddBuff("轻盈", 1);
        }
    }
}