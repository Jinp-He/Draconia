using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.System;
using QFramework;
using UnityEngine;
using static UnityEngine.Screen;

namespace Draconia.ViewController
{
    public class Hands : QFramework.ViewController, ICanGetSystem
    {
        public Card CardPrefab, BasicCardPrefab;
        
        
        public List<Card> Cards;
        public RectTransform DisplayArea,BasicCardArea;

        private List<Card> tempRemovedCard;

        public void Start()
        {
            tempRemovedCard = new List<Card>();
            Refresh();
            Cards = new List<Card>();
            Width = GetComponent<RectTransform>().rect.width;
        }

        public Card AddCard(CardInfo cardInfo, Player player)
        {
            Card card = Instantiate(CardPrefab, transform);
            card.Init(cardInfo, player);
            Cards.Add(card);
            Refresh();
            return card;
        }

        public void AddBasicCard(Player player)
        {
            foreach (var cardInfo in player.PlayerInfo.NormalAttackCard_Ref)
            {
                Card card = Instantiate(BasicCardPrefab, BasicCardArea);
                card.Init(cardInfo, player, true);
                Cards.Add(card);
                Refresh();
            }
        }
        
        public Card AddCard(Card card, Player player)
        {
            Cards.Add(card);
            card.transform.parent = transform;
            card.transform.localPosition = Vector3.zero;
            Refresh();
            return card;
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
            tempRemovedCard.Add(card);
            card.gameObject.SetActive(false);
            Refresh();
        }

        private void CleanRemovedCard()
        {
            for (int i = 0; i < tempRemovedCard.Count; i++)
            {
                tempRemovedCard[i].Destroy();
            }
        }

        public bool HasCard(CardInfo cardInfo)
        {
            return Cards.Any(card => card._cardInfo == cardInfo);
        }

        private const float R = 2100.0f; //半径
        private const float Y = 2000.0f; //圆心Y值
        private const float CardRotateDegreeLimit = 10;

        /// <summary>
        /// 对所有的卡牌进行排序
        /// </summary>
        public void Refresh()
        {
            Cards = new List<Card>();
            foreach (var card in transform.GetComponentsInChildren<Card>())
            {
                Cards.Add(card);
            }
            ReorderCard();
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
            
            float p0 = transform.position.x;
            float dist = IdealDist + CardPrefab.GetComponent<RectTransform>().rect.width * CardPrefab.GetComponent<RectTransform>().Scale().y;
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
            //Debug.LogFormat("Dist: {0}, pos : {1}, p0 : {2}",dist,pos,p0);
            
            foreach (var card in Cards)
            {
                Transform tf = card.transform;
                tf.localPosition = new Vector3(pos, -50, 0);
                pos += dist;
                if (card.IsChosen)
                {
                    pos += 20f;
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
        private void ReorderCard()
        {
            List<Card> list = transform.GetComponentsInChildren<Card>().ToList();
            list.Sort((x, y) => x._cardInfo.Id - y._cardInfo.Id);
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