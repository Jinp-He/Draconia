using System.Collections.Generic;
using System.Linq;
using _Scripts.Game.Player;
using cfg;

namespace _Scripts.Game.Card
{
    public class Card117 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            int BasicAttackCount = CardUser.Player.CardUsageHistory.Where(e => e._cardInfo.Id == 100).Count();
            int BasicArmorCount = CardUser.Player.CardUsageHistory.Where(e => e._cardInfo.Id == 102).Count();

            for (int i = 0; i < BasicAttackCount; i++)
            {
                BattleSystem.RandomAttack(CardUser, AttackType.Physical, 2);
            }
            
            for (int i = 0; i < BasicArmorCount; i++)
            {
                CardUser.Defense(2);
            }

        }
    }
}