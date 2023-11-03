using System;
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
        public Sprite IsHitSprite;
        public Sprite IdleSprite;
        public Sprite ChosenSprite;
        public Sprite PointerSprite;
        public void Init(Enemy enemy)
        {
            _enemy = enemy;
            _isHitSprite = _enemy.CharacterAtlas.GetSprite("OnHit");
            _idleSprite = _enemy.CharacterAtlas.GetSprite("Idle");
            CharacterImage = _enemy.CharacterImage;
        }

        public void IsHitAnimation()
        {
            ActionKit.Sequence()
                .Callback(() => { _enemy.EnemyImage.sprite = IsHitSprite; })
                .Delay(0.3f)
                .Callback(() => { _enemy.EnemyImage.sprite = IdleSprite; })
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