using System.Collections.Generic;
using _Scripts.Game.Player;
using cfg;

namespace _Scripts.Game.Card
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