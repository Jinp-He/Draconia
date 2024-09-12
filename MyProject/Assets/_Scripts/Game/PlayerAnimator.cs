using _Scripts.Game.Player;
using UnityEngine.U2D;

namespace _Scripts.Game
{
    public class PlayerAnimator : CharacterAnimator
    {
        private string _characterName;

        public SpriteAtlas CharacterAtlas;
        
        
        public override void Init(CharacterViewController characterViewController)
        {
            base.Init(characterViewController);
            //_isHitSprite = characterViewController.CharacterAtlas.GetSprite("OnHit");
            //_idleSprite = characterViewController.CharacterAtlas.GetSprite("Idle");
            //_chosenSprite = characterViewController.CharacterAtlas.GetSprite("Chosen");

        }
        
    }
}