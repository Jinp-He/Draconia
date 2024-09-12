using System;
using System.Collections.Generic;
using _Scripts.Game.Player;
using cfg;

namespace _Scripts.Game.Card
{
    public class Card109 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            for (int i = 0; i < Math.Min(CardUser.Player.Bin.Count,2); i++)
            {
                CardVC cardVc = CardUser.Player.Bin[0];
                cardVc.Card.Properties.Add(EnumCardProperty.Virtual);
                CardUser.Player.Hands.Add(cardVc);
                //CardUser.Player.Bin.Remove(cardVc);
            }
        }
    }
}