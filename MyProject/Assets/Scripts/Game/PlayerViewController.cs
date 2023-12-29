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
    public class CharacterViewController : MyViewController
    {
        public HPBar HpBar;
        public int CurrHP;
        protected int _armor;
        public Action TriggerDanger;
        public Action OnTurnStart;
        public Pointer MyPointer;

        public Player Player
        {
            get
            {
                if (IsPlayer)
                {
                    PlayerViewController pvc = (PlayerViewController)this;
                    return pvc.Player;
                }
                else
                {
                    return null;
                }
            }
        }

        public int DamageDangerModifier = 1;
        public bool IsPlayer;


        public SpriteAtlas CharacterAtlas;
        public Image CharacterImage;
        public CharacterAnimator CharacterAnimator;
        public RectTransform DamageTextField;
        public RectTransform NotificationTextField;
        public int Position => transform.GetSiblingIndex();
        public BuffManager BuffManager;

        public readonly int DangerAreaDamageNum = 2;


        public int Armor
        {
            get => _armor;
            set => _armor = value;
        }

        public virtual void Init()
        {
            OnTurnStart = new Action(() => { });
            OnTurnStart += TurnStart;
            BuffManager = GetComponent<BuffManager>();
        }

        public virtual bool IsOnDangerArea()
        {
            return BattleSystem.TimeBar.IsInDangerArea(MyPointer);
        }


        public virtual void MoveInTime(int i)
        {
        }
        
        public void SendNotification(string text)
        {
            StartCoroutine(CharacterAnimator.SendNotificationText(text));
        }

        public virtual void IsHit(int damage, AttackType attackType, CharacterViewController Attacker = null)
        {
            if (attackType != AttackType.TrueDamage)
            {
                if (Armor >= damage)
                {
                    Armor -= damage;
                    HpBar.SetArmor(Armor);
                    damage = 0;
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
            }
            else
            {
                CurrHP -= damage;
                HpBar.SetHp(CurrHP);
                CharacterAnimator.IsHit(damage, attackType);
            }

            this.SendEvent(new IsHitEvent() {AttackReceiver = this, RealDamage = damage, AttackType = attackType, Attacker = Attacker});

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

        public virtual void TurnStart()
        {
            Armor = 0;
            HpBar.SetArmor(Armor);
        }

        public virtual void Die()
        {
        }

        public virtual void AddArmor(int armor)
        {
            Armor += armor;
        }

        public void Miss()
        {
            StartCoroutine(GetComponent<CharacterAnimator>().Miss());
        }

        public bool IsInDangerArea()
        {
            return BattleSystem.TimeBar.IsInDangerArea(MyPointer);
        }

        public CharacterViewController NextCharacter()
        {
            if (transform.parent.childCount > Position + 1)
            {
                return transform.parent.GetComponentsInChildren<CharacterViewController>()[Position + 1];
            }

            return null;
        }

        public CharacterViewController PrevCharacter()
        {
            if (Position > 0)
            {
                return transform.parent.GetComponentsInChildren<CharacterViewController>()[Position - 1];
            }

            return null;
        }
    }
}

namespace Draconia.ViewController
{
    public partial class PlayerViewController : CharacterViewController
    {
        //public SpriteAtlas CharacterAtlas;
        public PlayerAnimator PlayerAnimator;
        public PlayerInfo PlayerInfo;

        public string Alias;

        public void Init(Player player)
        {
            PlayerInfo playerInfo = player.PlayerInfo;
            IsPlayer = true;
            base.Init();
            Player = player;
            Player.PlayerViewController = this;
            PlayerInfo = playerInfo;
            Alias = playerInfo.Alias;
            
            CharacterAtlas = ResLoadSystem.LoadSync<SpriteAtlas>(playerInfo.Alias);
            CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
            CharacterImage.SetNativeSize();
            CardImageSprite = ResLoadSystem.LoadSync<Sprite>("CardImage_" + playerInfo.Alias);
            
            CurrHP = playerInfo.InitialHP;
            HpBar.Init(playerInfo.InitialHP, playerInfo.InitialHP);

            PlayerAnimator.Init(this);
            
            MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddCharacter(this);

            TriggerDanger = () => { };
            TriggerDanger += Player.EnterDangerArea;

            BattleStart();
        }


        public void BattleStart()
        {
            foreach (var cardInfo in Player.PlayerInfo.InitialCards_Ref)
            {
                var cardPrefab = ResLoadSystem.LoadSync<GameObject>("CardPrefab").GetComponent<CardVC>();
                CardVC cardVc = Instantiate(cardPrefab, CardDeck);
                cardVc.Init(cardInfo, this);
                Player.Deck.Add(cardVc);
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
            _player.Die();
        }

       


        //判断是否可以释放该卡


        public override void TurnStart()
        {
            _player.OnTurnStart();
            this.SendEvent(new PlayerTurnStartEvent() { Player = this.Player });
            StartCoroutine(PlayerAnimator.SendNotificationText("回合开始了"));
            UIKit.GetPanel<UIBattlePanel>().TimeBar.MoveAbsoluteTimePosition(MyPointer, _player.BackNum);
            PlayerAnimator.IsChosen();
        }
    }
}