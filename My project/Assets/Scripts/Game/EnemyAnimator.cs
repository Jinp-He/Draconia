using System;
using DG.Tweening;
using Draconia.MyComponent;
using QFramework;
using UnityEngine;


namespace Draconia.ViewController
{
    public class EnemyAnimator : MonoBehaviour
    {
        private string _enemyName;
        private Enemy _enemy;
        private Sprite _isHitSprite;
        private Sprite _idleSprite;
        private Sprite _chosenSprite;
        public void Init(Enemy enemy)
        {
            _enemy = enemy;
            _isHitSprite = _enemy.EnemyAtlas.GetSprite("dog01_OnHit");
            _idleSprite = _enemy.EnemyAtlas.GetSprite("dog01_Idle");
            //_chosenSprite = _character.CharacterAtlas.GetSprite("Chosen");
        }

        public void IsHit()
        {
            ActionKit.Sequence()
                .Callback(() => { _enemy.EnemyImage.sprite = _isHitSprite; })
                .Delay(0.3f)
                .Callback(() => { _enemy.EnemyImage.sprite = _idleSprite; })
                .Start(this);
        }

        public void Move(Enemy enemy)
        {
            _enemy.transform.DOMove(enemy.transform.position, 1f);
            enemy.transform.DOMove(transform.position, 1f);
        }

        public void IsChose()
        {
            
        }

        public void EndChose()
        {
            
        }
        
        
    }
}