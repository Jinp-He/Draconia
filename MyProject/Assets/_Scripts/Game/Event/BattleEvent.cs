using System.Collections.Generic;
using _Scripts.Game.Card;
using _Scripts.Game.Player;
using cfg;

namespace _Scripts.Game.Event
{
    public struct BattleStartEvent
    {
        
    }
    
    public struct PlayerTurnStartEvent
    {
        public PlayerStrategy.Player Player;
    }
    
    public struct EnterDangerAreaEvent
    {
        public CharacterViewController CharacterViewController;
    }


    public struct UseCardEvent
    {
        public CharacterViewController CharacterViewController;
        public CardVC UsedCardVc;
    }

    public struct DrawCardEvent
    {
        public List<CardVC> Cards;
    }

    public struct AddBuffEvent
    {
        public CharacterViewController CharacterViewController;
        public Buff.Buff Buff;
    }

    public struct AttackEvent
    {
        public CharacterViewController Attacker;
        public CharacterViewController AttackReceiver;
        public AttackType AttackType;
    }
    
    public struct IsHitEvent
    {
        public CharacterViewController Attacker;
        public CharacterViewController AttackReceiver;
        public AttackType AttackType;
        public int RealDamage;
    }

  
}