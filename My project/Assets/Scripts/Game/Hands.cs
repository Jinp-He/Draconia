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
        public Card CardPrefab;
        public List<Card> Cards;

        private List<Card> tempRemovedCard;

        public void Start()
        {
            tempRemovedCard = new List<Card>();
            Refresh();
            Cards = new List<Card>();
        }

        public Card AddCard(CardInfo cardInfo, Character character)
        {
            Card card = Instantiate(CardPrefab, transform);
            card.Init(cardInfo, character);
            Cards.Add(card);
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
            ReorderCard();
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

            foreach (var card in transform.GetComponentsInChildren<Card>())
            {
                if (card._cardInfo.Cost <= this.GetSystem<BattleSystem>().Energy &&
                    this.GetSystem<BattleSystem>().BattleState == BattleState.Player)
                {
                    card.UseEffect.gameObject.SetActive(true);
                }
                else
                {
                    card.UseEffect.gameObject.SetActive(false);
                }
            }
        }

        public void OnEndTurn(Character character)
        {
            for (int i = 0; i < Cards.Count(); i++)
            {
                Cards[i].OnEndTurn();
            }
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