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
        public void Start()
        {
            Refresh();
        }

        public void AddCard(CardInfo cardInfo, Character character)
        {
            Card card = Instantiate(CardPrefab, transform);
            card.Init(cardInfo,character);
            Refresh();
        }

        public bool HasCard(CardInfo cardInfo)
        {
            List<Card> cards = transform.GetComponentsInChildren<Card>().ToList();
            return cards.Any(card => card._cardInfo == cardInfo);
        }
        private const float R = 2100.0f;//半径
        private const float Y = 2000.0f;//圆心Y值
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
            int count = transform.childCount;
            float deg_space;
            if (count < 2)
            {
                deg_space = 0;
            }
            else
            {
                deg_space = 2*CardRotateDegreeLimit/(count-1);
                if (deg_space > 5) deg_space = 5;
            }
            int i = 0;
            foreach (Transform tf in transform)
            {
                //计算角度
                float deg = (deg_space*i)-(deg_space*(count-1)/2);
                //通过角度，计算坐标位置
                float x = Mathf.Sin(deg*Mathf.Deg2Rad)*R+p0;
                float y = Mathf.Cos(deg*Mathf.Deg2Rad)*R-Y;
                //设置坐标位置
                tf.localPosition = new Vector3(x, y, 0);
                //设置角度
                tf.rotation = Quaternion.Euler(new Vector3(0,0,-deg));
                i++;
            }

            foreach (var card in transform.GetComponentsInChildren<Card>())
            {
                if (card._cardInfo.Cost <= this.GetSystem<BattleSystem>().Energy && this.GetSystem<BattleSystem>().BattleState == BattleState.Player)
                {
                    card.UseEffect.gameObject.SetActive(true);
                }
                else
                {
                    card.UseEffect.gameObject.SetActive(false); 
                }
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