using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utility;
using static UnityEngine.Screen;

namespace Draconia.ViewController
{
    public class Hands : QFramework.ViewController, ICanGetSystem, ICanSendEvent
    {
        public CardVC CardVcPrefab, BasicCardVcPrefab;
        public Image FrontImage;

        public PlayerViewController OnGoingPlayerViewController;
        public List<CardVC> Cards;
        public RectTransform DisplayArea,ItemHands,PlayerHands;

        private Dictionary<string, RectTransform> _playerHandsList;
        public RectTransform OngoingPlayerHands;

        private List<CardVC> tempRemovedCard;

        public void Start()
        {
            tempRemovedCard = new List<CardVC>();
            //Refresh();
            Cards = new List<CardVC>();
            Width = GetComponent<RectTransform>().rect.width;

            _playerHandsList = new Dictionary<string, RectTransform>();
            foreach (var player in this.GetSystem<BattleSystem>().Players)
            {
                var playerHands = Instantiate(PlayerHands, transform);
                _playerHandsList.Add(player.PlayerInfo.Alias, playerHands);
            }
        }

        
        public void AddBasicCard(PlayerViewController playerViewController)
        {
            List<CardVC> res = new List<CardVC>();
            foreach (var cardInfo in playerViewController.Player.PlayerInfo.NormalAttackCard_Ref)
            {
                CardVC cardVc = Instantiate(BasicCardVcPrefab, _playerHandsList[playerViewController.Alias]);
                cardVc.Init(cardInfo, playerViewController,true);
                res.Add(cardVc);
                playerViewController.Player.Hands.Add(cardVc);
                Refresh();
            }
            this.SendEvent(new DrawCardEvent(){Cards = res});
        }

        public void AddRandomBasicCard(PlayerViewController playerViewController, int num)
        {
            List<CardVC> res = new List<CardVC>();
            for (int i = 0; i < num; i++)
            {
                CardInfo cardInfo = playerViewController.Player.PlayerInfo.NormalAttackCard_Ref.PickRandom(1).ToList()[0];
                CardVC cardVc = Instantiate(BasicCardVcPrefab, _playerHandsList[playerViewController.Alias]);
                cardVc.Init(cardInfo, playerViewController,true);
                res.Add(cardVc);
                playerViewController.Player.Hands.Add(cardVc);
                Refresh();
            }
            this.SendEvent(new DrawCardEvent(){Cards = res});
            
        }
        
        public CardVC AddCard(CardVC cardVc, PlayerViewController playerViewController)
        {
            //player.PlayerStrategy.Hands.Add(card);
            cardVc.transform.parent = _playerHandsList[playerViewController.Alias];
            cardVc.transform.localPosition = Vector3.zero;
            Refresh();
            this.SendEvent(new DrawCardEvent(){Cards = new List<CardVC>(){cardVc}});
            return cardVc;
        }
        

        public void AddItemCard()
        {
            
        }

        public void OnPlayerTurnStart(PlayerViewController playerViewController)
        {
            OnGoingPlayerViewController = playerViewController;
            Debug.LogFormat("DEBUG {0}", _playerHandsList.Count);
            OngoingPlayerHands = _playerHandsList[playerViewController.Alias];
            DisplayHands();
        }

        public void DisplayItem()
        {
            OngoingPlayerHands.gameObject.SetActive(false);
            ItemHands.gameObject.SetActive(true);
            FrontImage.sprite = this.GetSystem<ResLoadSystem>()
                .LoadSprite("FrontGround_Item");
        }

        public void DisplayHands()
        {
            if (OnGoingPlayerViewController == null)
            {
                return;
                
            }
            foreach (var rect in _playerHandsList.Values)
            {
                rect.gameObject.SetActive(false);
            }
            OngoingPlayerHands = _playerHandsList[OnGoingPlayerViewController.Alias];
            OngoingPlayerHands.gameObject.SetActive(true);
            ItemHands.gameObject.SetActive(false);
            if (OnGoingPlayerViewController == null)
            {
                return;
            }
            FrontImage.sprite = this.GetSystem<ResLoadSystem>()
                .LoadSprite("FrontGround_" + OnGoingPlayerViewController.Player.PlayerInfo.Alias);
        }

        private void CleanRemovedCard()
        {
            for (int i = 0; i < tempRemovedCard.Count; i++)
            {
                tempRemovedCard[i].Destroy();
            }
        }
        

        private const float R = 2100.0f; //半径
        private const float Y = 2000.0f; //圆心Y值
        private const float CardRotateDegreeLimit = 10;

        /// <summary>
        /// 对所有的卡牌进行排序
        /// </summary>
        public void Refresh()
        {
            Cards = OnGoingPlayerViewController.Player.Hands;
            ReorderCard(Cards);
            RecView();
            
        }

        private void ArcView()
        {
            const float position2 = -3.5f;
            const float allLength = 4.9f;
            float p0 = transform.position.x;
            int count = Cards.Count;
            float degSpace;
            if (count < 2)
            {
                degSpace = 0;
            }
            else
            {
                degSpace = 2 * CardRotateDegreeLimit / (count - 1);
                if (degSpace > 10) degSpace = 10;
            }

            int i = 0;
            foreach (var card in transform.GetComponentsInChildren<CardVC>())
            {
                Transform tf = card.transform;
                //计算角度
                float deg = (degSpace * i) - (degSpace * (count - 1) / 2);
                //通过角度，计算坐标位置
                float x = Mathf.Sin(deg * Mathf.Deg2Rad) * R + p0;
                float y = Mathf.Cos(deg * Mathf.Deg2Rad) * R - Y;
                //设置坐标位置
                tf.localPosition = new Vector3(x, y, 0);
                //设置角度
                tf.rotation = Quaternion.Euler(new Vector3(0, 0, -deg));
                i++;
            }
        }

        private float Width;
        private const float IdealDist = 0f;
        public void RecView()
        {
            
            float p0 = OngoingPlayerHands.transform.position.x;
            float dist = IdealDist + CardVcPrefab.GetComponent<RectTransform>().rect.width * CardVcPrefab.GetComponent<RectTransform>().Scale().y;
            float basicDist = IdealDist + BasicCardVcPrefab.GetComponent<RectTransform>().rect.width * BasicCardVcPrefab.GetComponent<RectTransform>().Scale().y;

            int count = Cards.Count;
            float halfCount = (count - 1) / 2f;
            int i = 0;
            //计算初始位置
            float pos = p0 - halfCount * dist;
            if (Cards.Any(card => card.IsChosen))
                pos -= 40f;
            //如果超出了手牌transform范围
            if (pos <= p0 - Width / 2)
            {
                pos = p0 - Width / 2;
                dist = Width / 2 / halfCount;
            }

            bool nextChosen = false;
            //TODO: 更改卡牌显示逻辑，选中之后会更显眼。
            foreach (var card in Cards)
            {
                if (card.IsBasicCard)
                    pos += basicDist;
                else 
                    pos += dist;
                if (nextChosen)
                {
                    //如果之前的手牌被选中了
                    nextChosen = false;
                    pos += 95f;
                }
                Transform tf = card.transform;
                tf.localPosition = new Vector3(pos, -50, 0);
                if (card.IsChosen)
                {
                    nextChosen = true;
                    tf.localPosition = new Vector3(pos, 0, 0);
                }

                
                
                i++;
            }
        }

        public void OnEndTurn(PlayerViewController playerViewController)
        {
            for (int i = 0; i < Cards.Count();)
            {
                Cards[i].Discard();
            }

            Cards = new List<CardVC>();
        }

        /// <summary>
        /// Reorder the card order to certain type
        /// By User and then Cost?
        /// </summary>
        private void ReorderCard(List<CardVC> card)
        {
            card.Sort((x, y) =>  - x._cardInfo.Id + y._cardInfo.Id);
            for (int i = 0; i < card.Count; i++)
            {
                card[i].transform.SetSiblingIndex(i);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// 
        public void Choose(CardVC currCardVc)
        {
            List<CardVC> cards = transform.GetComponentsInChildren<CardVC>().ToList();
            foreach (var card in cards.Where(card => card == currCardVc))
            {
                card.Hover();
            }
        }



        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}