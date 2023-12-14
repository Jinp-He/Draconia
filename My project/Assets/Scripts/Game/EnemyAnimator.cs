using System;
using System.Collections;
using DG.Tweening;
using Draconia.MyComponent;
using QFramework;
using TMPro;
using UnityEngine;


namespace Draconia.ViewController
{
    public class EnemyAnimator : CharacterAnimator
    {

        private string _enemyName;
        private Enemy _enemy;

        private Sprite _meleeSprite;
        
        public void Init(Enemy enemy)
        {
            base.Init(enemy);
            _enemy = enemy;
            _isHitSprite = _enemy.CharacterAtlas.GetSprite("OnHit");
            _idleSprite = _enemy.CharacterAtlas.GetSprite("Idle");
            _meleeSprite = _enemy.CharacterAtlas.GetSprite("Melee");
        }

        public void Attack()
        {
            ActionKit.Sequence()
                .Callback(() =>
                {
                    //UIKit.Root.Camera.orthographicSize = 5;
                    BattleSystem.AnimationStopState = true;
                    _enemy.EnemyImage.sprite = _meleeSprite;
                })
                .Delay(0.6f)
                .Callback(() =>
                {
                    //UIKit.Root.Camera.orthographicSize = 100;
                    _enemy.EnemyImage.sprite = _idleSprite;
                    BattleSystem.AnimationStopState = false;
                })
                .Start(this);
        }

        public void IsHitAnimation()
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

        public void HitText(string s)
        {
            
            TextMeshProUGUI hitText = Instantiate(HitTextPrefab, transform);
            hitText.gameObject.SetActive(true);
            hitText.text = s;
            Sequence seq = DOTween.Sequence();
            seq.Append(hitText.transform.DOLocalMoveY(-10, 1f))
                .Join(hitText.DOFade(0f, 1f))
                .OnComplete(() => { hitText.DestroySelf(); })
                .Play();
        }


        
        
    }
}