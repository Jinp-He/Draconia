using System;
using DG.Tweening;
using Draconia.MyComponent;
using QFramework;
using TMPro;
using UnityEngine;


namespace Draconia.ViewController
{
    public class PlayerAnimator : MonoBehaviour
    {
        public TextMeshProUGUI HitTextPrefab;
        
        private string _characterName;
        private Player _player;
        private Sprite _isHitSprite;
        private Sprite _idleSprite;
        private Sprite _chosenSprite;
        
        public void Init(Player player)
        {
            _player = player;
            _isHitSprite = _player.CharacterAtlas.GetSprite("OnHit");
            _idleSprite = _player.CharacterAtlas.GetSprite("Idle");
            //_chosenSprite = _character.CharacterAtlas.GetSprite("Chosen");
        }

        public void IsHit()
        {
            ActionKit.Sequence()
                .Callback(() => { _player.CharacterImage.sprite = _isHitSprite; })
                .Delay(0.3f)
                .Callback(() => { _player.CharacterImage.sprite = _idleSprite; })
                .Start(this);
        }
        
        public void Move(Player player)
        {
            _player.transform.DOMove(player.transform.position, 1f);
            player.transform.DOMove(transform.position, 1f);
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
        
        


        public void IsChose()
        {
            
        }

        public void EndChose()
        {
            
        }
        
        
    }
}