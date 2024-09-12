using System;
using _Scripts.Game.Buff;
using _Scripts.Game.Event;
using _Scripts.Game.PlayerStrategy;
using cfg;
using Draconia.MyComponent;
using QFramework;
using UnityEngine;
using UnityEngine.U2D;

namespace _Scripts.Game.Player
{
    public class CharacterViewController : MyViewController
    {
        public HPBar HpBar;
        public string Alias;
        private int _currHp;

        protected int CurrHP
        {
            set
            {
                _currHp = value;
                if(IsPlayer)
                    Player.Hp = value;
            }
            get => _currHp;
        }
        protected int _armor;
        public Action TriggerDanger;
        public Action OnTurnStart;
        public MyPointer MyPointer;

        public PlayerStrategy.Player Player
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
        public UnityEngine.UI.Image CharacterImage;
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
