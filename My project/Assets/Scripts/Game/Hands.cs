using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.System;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utility;
using static UnityEngine.Screen;

namespace Draconia.ViewController
{
    public class Hands : QFramework.ViewController, ICanGetSystem
    {
        public Card CardPrefab, BasicCardPrefab;
        public Image FrontImage;

        public Player OnGoingPlayer;
        public List<Card> Cards;
        public RectTransform DisplayArea,ItemHands,PlayerHands;

        private Dictionary<string, RectTransform> _playerHandsList;
        public RectTransform OngoingPlayerHands;

        private List<Card> tempRemovedCard;

        public void Start()
        {
            tempRemovedCard = new List<Card>();
            //Refresh();
            Cards = new List<Card>();
            Width = GetComponent<RectTransform>().rect.width;

            _playerHandsList = new Dictionary<string, RectTransform>();
            foreach (var player in this.GetSystem<BattleSystem>().Players)
            {
                var playerHands = Instantiate(PlayerHands, transform);
                _playerHandsList.Add(player.PlayerInfo.Alias, playerHands);
            }
        }

        
        public void AddBasicCard(Player player)
        {
            foreach (var cardInfo in player.PlayerStrategy.PlayerInfo.NormalAttackCard_Ref)
            {
                Card card = Instantiate(BasicCardPrefab, _playerHandsList[player.Alias]);
                card.Init(cardInfo, player,true);
                player.PlayerStrategy.Hands.Add(card);
                Refresh();
            }
        }

        public void AddRandomBasicCard(Player player, int num)
        {
            for (int i = 0; i < num; i++)
            {
                CardInfo cardInfo = player.PlayerStrategy.PlayerInfo.NormalAttackCard_Ref.PickRandom(1).ToList()[0];
                Card card = Instantiate(BasicCardPrefab, _playerHandsList[player.Alias]);
                card.Init(cardInfo, player,true);
                player.PlayerStrategy.Hands.Add(card);
                Refresh();
            }
            
        }
        
        public Card AddCard(Card card, Player player)
        {
            //player.PlayerStrategy.Hands.Add(card);
            card.transform.parent = _playerHandsList[player.Alias];
            card.transform.localPosition = Vector3.zero;
            Refresh();
            return card;
        }
        

        public void AddItemCard()
        {
            
        }

        public void OnPlayerTurnStart(Player player)
        {
            OnGoingPlayer = player;
            OngoingPlayerHands = _playerHandsList[player.Alias];
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
            if (OnGoingPlayer == null)
            {
                return;
                
            }
            foreach (var rect in _playerHandsList.Values)
            {
                rect.gameObject.SetActive(false);
            }
            OngoingPlayerHands = _playerHandsList[OnGoingPlayer.Alias];
            OngoingPlayerHands.gameObject.SetActive(true);
            ItemHands.gameObject.SetActive(false);
            if (OnGoingPlayer == null)
            {
                return;
            }
            FrontImage.sprite = this.GetSystem<ResLoadSystem>()
                .LoadSprite("FrontGround_" + OnGoingPlayer.PlayerStrategy.PlayerInfo.Alias);
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
            Cards = OnGoingPlayer.PlayerStrategy.Hands;
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
            foreach (var card in transform.GetComponentsInChildren<Card>())
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
            float dist = IdealDist + CardPrefab.GetComponent<RectTransform>().rect.width * CardPrefab.GetComponent<RectTransform>().Scale().y;
            float basicDist = IdealDist + BasicCardPrefab.GetComponent<RectTransform>().rect.width * BasicCardPrefab.GetComponent<RectTransform>().Scale().y;

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
            bool isPrevBasic = false;
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
                    if (isPrevBasic)
                    {
                        pos += 10f;
                    }
                    else
                    {
                        pos += 95f;
                    }
                }
                Transform tf = card.transform;
                tf.localPosition = new Vector3(pos, -50, 0);
                if (card.IsChosen)
                {
                    nextChosen = true;
                    isPrevBasic = card.IsBasicCard;
                    tf.localPosition = new Vector3(pos, 0, 0);
                }

                
                
                i++;
            }
        }

        public void OnEndTurn(Player player)
        {
            for (int i = 0; i < Cards.Count();)
            {
                Cards[i].Discard();
            }

            Cards = new List<Card>();
        }

        /// <summary>
        /// Reorder the card order to certain type
        /// By User and then Cost?
        /// </summary>
        private void ReorderCard(List<Card> card)
        {
            card.Sort((x, y) => - x._cardInfo.Id + y._cardInfo.Id);
            for (int i = 0; i < card.Count; i++)
            {
                card[i].transform.SetSiblingIndex(i);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// 
        public void Choose(Card currCard)
        {
            List<Card> cards = transform.GetComponentsInChildren<Card>().ToList();
            foreach (var card in cards.Where(card => card == currCard))
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