using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using static UnityEngine.Screen;

namespace Draconia.ViewController
{
    public class Hands : QFramework.ViewController
    {
        public Card CardPrefab;
        public void Start()
        {
            Refresh();
        }

        public void AddCard(CardInfo cardInfo)
        {
            Card card = Instantiate(CardPrefab, transform);
            card.Init(cardInfo);
            Refresh();
        }
        private const float R = 2100.0f;//半径
        private const float Y = 2000.0f;//圆心Y值
        private const float CardRotateDegreeLimit = 10;
        /// <summary>
        /// 对所有的卡牌进行排序
        /// </summary>
        public void Refresh()
        {
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
    }
}