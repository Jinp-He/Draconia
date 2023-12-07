using System.Collections.Generic;
using cfg;
using Draconia.Controller;
using Draconia.Game.Buff;

namespace Draconia.ViewController.Event
{
    public struct BattleStartEvent
    {
        
    }
    
    public struct PlayerTurnStartEvent
    {
    
    }
    
    public struct EnterDangerAreaEvent
    {
        public Character Character;
    }


    public struct UseCardEvent
    {
        public Character Character;
        public Card UsedCard;
    }

    public struct DrawCardEvent
    {
        public List<Card> Cards;
    }

    public struct AddBuffEvent
    {
        public Character Character;
        public Buff Buff;
    }

    public struct AttackEvent
    {
        public Character Attacker;
        public Character AttackReceiver;
        public AttackType AttackType;
    }
    
    public struct IsHitEvent
    {
        public Character Attacker;
        public Character AttackReceiver;
        public AttackType AttackType;
        public int RealDamage;
    }

  
}