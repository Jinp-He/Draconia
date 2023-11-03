using System;
using DG.Tweening;
using Draconia.Controller;
using Draconia.MyComponent;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;


namespace Draconia.ViewController
{
    public class PlayerAnimator : CharacterAnimator
    {
        private string _characterName;

        public SpriteAtlas CharacterAtlas;
        
        
        public override void Init(Character character)
        {
            base.Init(character);
            _isHitSprite = character.CharacterAtlas.GetSprite("OnHit");
            _idleSprite = character.CharacterAtlas.GetSprite("Idle");
            _chosenSprite = character.CharacterAtlas.GetSprite("Chosen");

        }
        
    }
}