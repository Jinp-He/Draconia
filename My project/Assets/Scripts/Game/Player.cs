using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using cfg;
using Draconia.Controller;
using Draconia.UI;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using cfg;
using DG.Tweening;
using Draconia.Game.Buff;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.Controller
{
    public class Character : MyViewController
    {
        public HPBar HpBar;
        public int CurrHP;
        private int _armor;
        public Action TriggerDanger;
        public Pointer MyPointer;

        public int DamageDangerModifier = 1;


        public SpriteAtlas CharacterAtlas;
        public Image CharacterImage;
        public CharacterAnimator CharacterAnimator;
        public RectTransform DamageTextField;
        public RectTransform NotificationTextField;

        public readonly int DangerAreaDamageNum = 2;


        public int Armor
        {
            get => _armor;
            set => _armor = value;
        }

        public virtual void Init()
        {
        }

        public virtual bool IsOnDangerArea()
        {
            return BattleSystem.TimeBar.IsInDangerArea(MyPointer);
        }


        public virtual void MoveInTime(int i)
        {
        }

        public virtual void IsHit(int damage, AttackType attackType)
        {
            if (Armor >= damage)
            {
                Armor -= damage;
                HpBar.SetArmor(Armor);
                CharacterAnimator.IsHit(0, attackType, damage);
            }
            else
            {
                int tempArmor = Armor;
                damage -= Armor;
                HpBar.SetArmor(Armor);
                CurrHP -= damage;
                HpBar.SetHp(CurrHP);
                CharacterAnimator.IsHit(damage, attackType, tempArmor);
            }

            if (CurrHP <= 0)
            {
                Die();
            }
        }

        public void Defense(int value)
        {
            Armor += value;
            HpBar.SetArmor(value);
        }

        public virtual void OnTurnStart()
        {
            Armor = 0;
            HpBar.SetArmor(Armor);
        }

        public virtual void Die()
        {
        }

        public void Miss()
        {
            StartCoroutine(GetComponent<CharacterAnimator>().Miss());
        }
    }
}

namespace Draconia.ViewController
{
    public partial class Player : Character
    {
        //public SpriteAtlas CharacterAtlas;
        public PlayerAnimator PlayerAnimator;
        public PlayerInfo PlayerInfo;

        public string Alias;

        public void Init(PlayerInfo playerInfo)
        {
            PlayerInfo = playerInfo;
            Alias = playerInfo.Alias;
            _playerStrategy = PlayerStrategy.GetStrategy(playerInfo.Alias, this);
            
            CharacterAtlas = ResLoadSystem.LoadSync<SpriteAtlas>(playerInfo.Alias);
            CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
            CharacterImage.SetNativeSize();
            CardImageSprite = ResLoadSystem.LoadSync<Sprite>("CardImage_" + playerInfo.Alias);
            
            CurrHP = playerInfo.InitialHP;
            HpBar.Init(playerInfo.InitialHP, playerInfo.InitialHP);
            
         
            //Debug.Log(playerInfo.Name);
          
            PlayerAnimator.Init(this);
            PlayerStrategy.Init(this);
            MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddCharacter(this);

            TriggerDanger = () => { };
            TriggerDanger += PlayerStrategy.EnterDangerArea;

            BattleStart();
        }


        public void BattleStart()
        {
            foreach (var cardInfo in PlayerStrategy.PlayerInfo.InitialCards_Ref)
            {
                var cardPrefab = ResLoadSystem.LoadSync<GameObject>("CardPrefab").GetComponent<Card>();
                Card card = Instantiate(cardPrefab, CardDeck);
                card.Init(cardInfo, this);
                PlayerStrategy.Deck.Add(card);
            }
        }

        public void Chosen()
        {
            ChooseBracelet.gameObject.SetActive(true);
        }

        public void Unchosen()
        {
            ChooseBracelet.gameObject.SetActive(false);
        }


        public override void Die()
        {
            _playerStrategy.Die();
        }


        //判断是否可以释放该卡


        public override void OnTurnStart()
        {
            _playerStrategy.OnTurnStart();
            StartCoroutine(PlayerAnimator.SendNotificationText("回合开始了"));
            UIKit.GetPanel<UIBattlePanel>().TimeBar.MoveAbsoluteTimePosition(MyPointer, _playerStrategy.BackNum);
            PlayerAnimator.IsChosen();
        }
    }
}