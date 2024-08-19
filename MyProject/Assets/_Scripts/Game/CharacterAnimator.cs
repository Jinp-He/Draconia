using System;
using System.Collections;
using cfg;
using Draconia.MyComponent;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Draconia.System;
using Spine.Unity;

namespace Draconia.ViewController
{
    public class CharacterAnimator : MyViewController
    {
        protected Sprite _isHitSprite;
        protected Sprite _idleSprite;
        protected Sprite _chosenSprite;
        protected CharacterViewController CharacterViewController;
        protected readonly float _attackAnimationTime = 1f;
        
        public TextMeshProUGUI HitTextPrefab;
        public TextMeshProUGUI CriticalHitTextPrefab;
        public CanvasGroup NotificationTextPrefab;

        public SkeletonGraphic SkeletonGraphic;

        public UnityEngine.UI.Image CharacterImage;
        public virtual void Init(CharacterViewController characterViewController)
        {
            CharacterViewController = characterViewController;
            //CharacterImage = characterViewController.CharacterImage;

            if (SkeletonGraphic != null)
            {
                SkeletonGraphic.Clear();
                SkeletonGraphic.skeletonDataAsset = this.GetSystem<ResLoadSystem>().LoadSync<SkeletonDataAsset>
                ("Skeleton_" + CharacterViewController.Alias);
                SkeletonGraphic.Initialize(true);

            }
        }
        
        public void IsChosen()
        {

            if (SkeletonGraphic != null)
            {
                SkeletonGraphic.skeletonDataAsset = this.GetSystem<ResLoadSystem>().LoadSync<SkeletonDataAsset>
                ("Skeleton_" + CharacterViewController.Alias);
                Debug.Log("#DEBUG# 被选中了");
                SkeletonGraphic.AnimationState.SetAnimation(0,"idle2",true);
            }
            //CharacterImage.sprite = _chosenSprite;
            //CharacterImage.SetNativeSize();
        }
        
        public void EndChosen()
        {
            Debug.Log("#DEBUG# 取消选中");
            if (SkeletonGraphic != null)
                SkeletonGraphic.AnimationState.SetAnimation(0,"idle",true);
            //CharacterImage.sprite = _idleSprite;
            //CharacterImage.SetNativeSize();
        }
        
        public void Move(PlayerViewController playerViewController)
        {
        }

        public void IsHit(int damage,  AttackType attackType, int armorDamage = 0, bool isCritical = false)
        {
            if(SkeletonGraphic != null)
            {

                ActionKit.Sequence()
                    .Callback(() => {StartCoroutine(SendHitText(damage, attackType, isCritical)); })
                    .Callback(() => {if(armorDamage != 0) StartCoroutine(SendHitText(armorDamage, attackType, isCritical, true)); })
                    .Callback(() => {                 
                        SkeletonGraphic.AnimationState.SetAnimation(0,"shouji",false);
                    })
                    .Delay(_attackAnimationTime)
                    .Callback(() => {                         
                        SkeletonGraphic.AnimationState.SetAnimation(0,"idle",true);
                    })
                    .Start(this);
            }
        }

        public IEnumerator Miss()
        {
            TextMeshProUGUI hitText = Instantiate(CriticalHitTextPrefab, CharacterViewController.DamageTextField);
            hitText.text = "Miss";
            hitText.color = Color.gray;
            Sequence seq = DOTween.Sequence();
            seq.Append(hitText.transform.DOLocalMoveY(100, .3f))
                .Join(hitText.GetComponent<CanvasGroup>().DOFade(0f, 1f))
                .OnComplete(() => { hitText.DestroySelf(); })
                .Play();
            yield return new WaitForSeconds(1f);
        }
        
        /// <summary>
        /// 伤害数字跳出
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="hitType"></param>
        /// <param name="isCritical"></param>
        /// <param name="isArmor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IEnumerator SendHitText(int damage, AttackType hitType, bool isCritical, bool isArmor = false)
        {
            TextMeshProUGUI hitText;
            if (isCritical)
                hitText = Instantiate(CriticalHitTextPrefab, CharacterViewController.DamageTextField);
            else
            {
                hitText  = Instantiate(HitTextPrefab, CharacterViewController.DamageTextField);
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
                case AttackType.TrueDamage:
                    hitText.color = Color.white;
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


        private bool _isSendingNotification;
        /// <summary>
        /// 信息跳出（Buff或者是姿态的提示信息）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerator SendNotificationText(string text)
        {
            if (_isSendingNotification)
            {
                yield return 0.1f;
            }
            _isSendingNotification = true;
            CanvasGroup s = Instantiate(NotificationTextPrefab, CharacterViewController.NotificationTextField);
            s.gameObject.SetActive(true);
            s.GetComponentInChildren<TextMeshProUGUI>().text = text;
            s.alpha = 0;
            Sequence seq = DOTween.Sequence();
            seq.Append(s.transform.DOLocalMoveY(-50, .3f))
                .Join(s.DOFade(1f, .3f))
                .AppendInterval(.5f)
                .Append(s.transform.DOLocalMoveY(-80, .1f))
                .Join(s.DOFade(0f, .3f))
                .OnComplete(()=> s.gameObject.DestroySelf())
                .Play();
            yield return new WaitForSeconds(2f);
            _isSendingNotification = false;
        }

    }
}