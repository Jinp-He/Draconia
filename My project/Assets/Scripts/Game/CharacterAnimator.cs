using System;
using DG.Tweening;
using Draconia.MyComponent;
using QFramework;
using UnityEngine;


namespace Draconia.ViewController
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Character _character;
        private Sprite _isHitSprite;
        private Sprite _idleSprite;
        private Sprite _chosenSprite;
        public void Init(Character character)
        {
            _character = character;
            _isHitSprite = _character.CharacterAtlas.GetSprite("OnHit");
            _idleSprite = _character.CharacterAtlas.GetSprite("Idle");
            //_chosenSprite = _character.CharacterAtlas.GetSprite("Chosen");
        }

        public void IsHit()
        {
            ActionKit.Sequence()
                .Callback(() => { _character.CharacterImage.sprite = _isHitSprite; })
                .Delay(0.3f)
                .Callback(() => { _character.CharacterImage.sprite = _idleSprite; })
                .Start(this);
        }

        public void IsChose()
        {
            
        }

        public void EndChose()
        {
            
        }
        
        
    }
}