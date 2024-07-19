using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card116 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            int k = CardUser.Player.CardUsageHistory.Where(e => e._cardInfo.Id == 101).Count();
            BattleSystem.TimeBar.MoveRelativeTimePosition(CardUser,k);
            BattleSystem.DrawCard(CardUser, k);
        }
    }
}