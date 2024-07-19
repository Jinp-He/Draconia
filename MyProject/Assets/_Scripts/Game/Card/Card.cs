using System;
using System.Collections.Generic;
using cfg;
using Draconia.System;
using Draconia.UI;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
    public class Card : ICanGetSystem
    {
        private CardInfo _cardInfo;
        private CardVC _cardVc;
        public int CardCost;
        public PlayerViewController CardUser;
        public List<EnumCardProperty> Properties;
        protected int index;
        private Vector3 _localScale, _localPos;
        public bool IsChosen;
        public bool IsBasicCard;
        public bool IsViewMode = true;
        public BattleSystem BattleSystem => this.GetSystem<BattleSystem>();

        protected int _tempCostModifier;


        public int TempCostModifier
        {
            get => _tempCostModifier;
            set
            {
                _tempCostModifier = value;
                SetCost();
            }
        }

        private int _battleCostModifier;

        public int BattleCostModifier
        {
            get => _battleCostModifier;
            set
            {
                _battleCostModifier = value;
                SetCost();
            }
        }

        public int Cost { set; get; }

        public static Card GetCard(CardVC cardVc, CardInfo cardInfo)
        {
            Type type = Type.GetType("Draconia.ViewController.Card" + cardInfo.Id);
            if (type == null)
            {
                Debug.Log("#DEBUG# Cannot Find Type " + "Card" + cardInfo.Id);
                Card oriCard = new Card();
                oriCard.Init(cardVc, cardInfo);
                return oriCard;
            }

            object obj = Activator.CreateInstance(type);
            Debug.Log("#DEBUG# Find Type " + "Card" + cardInfo.Id);
            Card card = (Card)obj;
            card.Init(cardVc, cardInfo);
            return card;
        }

        public void ChooseTarget(List<RaycastResult> res, List<CharacterViewController> Target,
            List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
        }


        public virtual void Init(CardVC cardVc, CardInfo cardInfo)
        {
            _cardInfo = cardInfo;
            _cardVc = cardVc;
            CardUser = cardVc.CardUser;
        }

        public virtual void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            int id = _cardInfo.Id;
            switch (id)
            {
                case 100:
                    BattleSystem.Attack(CardUser, _enemies[0], AttackType.Physical, 2);
                    CardUser.Player.Charge((e) => { Debug.Log("DEBUG 蓄力层数： " + e); });
                    break;
                case 101:
                    CardUser.Player.Move(_allies[0]);
                    break;
                case 102:
                    CardUser.Defense(2);
                    break;
                case 103:
                    BattleSystem.Attack(CardUser, _enemies[0], AttackType.Physical, 4);
                    //TODO cost modifieir 
                    CardUser.Player.AddBuff("轻盈", 1);
                    break;
                case 104:
                    CardUser.Player.InvokePassive();
                    CardUser.Player.InvokePassive();
                    break;
                case 105:
                    _allies[0].Player.MoveInTime(2);
                    BattleSystem.DrawCard(CardUser, 1);
                    break;
                case 106:
                    BattleSystem.Hands.AddRandomBasicCard(CardUser, 3);
                    break;
                case 107:
                    CardUser.Player.AddBuff("飞鸟式", 1);
                    break;
                case 108:
                    foreach (var player in BattleSystem.Players)
                    {
                        player.AddArmor(player.IsInDangerArea() ? 5 : 3);
                    }

                    break;
                case 109:
                    BattleSystem.Attack(CardUser, _enemies[0], AttackType.Physical, 5);
                    break;
            }
        }

        public void SetCost()
        {
            Cost = _cardInfo.Cost + TempCostModifier + BattleCostModifier;
            _cardVc.CardCost.text = Cost.ToString();
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}