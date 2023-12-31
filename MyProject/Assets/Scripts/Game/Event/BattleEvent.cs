﻿using System.Collections.Generic;
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
        public Player Player;
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
        public Buff Buff;
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