﻿using System;
using System.Collections;
using cfg;
using Draconia.Controller;
using Draconia.MyComponent;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Draconia.ViewController
{
    public class CharacterAnimator : MyViewController
    {
        protected Sprite _isHitSprite;
        protected Sprite _idleSprite;
        protected Sprite _chosenSprite;
        protected Character Character;
        
        public TextMeshProUGUI HitTextPrefab;
        public TextMeshProUGUI CriticalHitTextPrefab;

        public Image CharacterImage;
        public virtual void Init(Character character)
        {
            Character = character;
            CharacterImage = character.CharacterImage;
        }
        
        public void IsChosen()
        {
            CharacterImage.sprite = _chosenSprite;
            CharacterImage.SetNativeSize();
        }
        
        public void EndChosen()
        {
            CharacterImage.sprite = _idleSprite;
            CharacterImage.SetNativeSize();
        }
        
        public void Move(Player player)
        {
        }

        public void IsHit(int damage,  AttackType attackType, int armorDamage = 0, bool isCritical = false)
        {
            ActionKit.Sequence()
                .Callback(() => {StartCoroutine(HitText(damage, attackType, isCritical)); })
                .Callback(() => {if(armorDamage != 0) StartCoroutine(HitText(armorDamage, attackType, isCritical, true)); })
                .Callback(() => { CharacterImage.sprite = _isHitSprite; })
                .Delay(0.5f)
                .Callback(() => { CharacterImage.sprite = _idleSprite; })
                .Start(this);
        }

        public IEnumerator Miss()
        {
            TextMeshProUGUI hitText = Instantiate(CriticalHitTextPrefab, Character.DamageTextField);
            hitText.text = "Miss";
            hitText.color = Color.gray;
            Sequence seq = DOTween.Sequence();
            seq.Append(hitText.transform.DOLocalMoveY(100, .3f))
                .Join(hitText.DOFade(0f, 1f))
                .OnComplete(() => { hitText.DestroySelf(); })
                .Play();
            yield return new WaitForSeconds(1f);
        }
        
        public IEnumerator HitText(int damage, AttackType hitType, bool isCritical, bool isArmor = false)
        {
            Debug.Log("HitText 触发");
            TextMeshProUGUI hitText;
            if (isCritical)
                hitText = Instantiate(CriticalHitTextPrefab, Character.DamageTextField);
            else
            {
                hitText  = Instantiate(HitTextPrefab, Character.DamageTextField);
            }
            hitText.gameObject.SetActive(true);
            hitText.text = damage.ToString();
            
            switch (hitType)
            {
                case AttackType.Physical:
                    hitText.color = Color.red;
                    break;
                case AttackType.Magic:
                    hitText.color = Color.blue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hitType), hitType, null);
            }

            if (isArmor)
            {
                hitText.color = Color.gray;
            }
            if (isCritical)
            {
                hitText.color = Color.yellow;
            }
            
            Sequence seq = DOTween.Sequence();
            seq.Append(hitText.transform.DOLocalMoveY(100, .3f))
                .Join(hitText.DOFade(0f, 1f))
                .OnComplete(() => { hitText.DestroySelf(); })
                .Play();
            yield return new WaitForSeconds(1f);
        }
        


    }
}