using System;
using System.Collections.Generic;
using cfg;
using Draconia.ViewController;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.System
{
    /// <summary>
    /// 管理游戏的进程，状态，保存和加载的系统
    /// </summary>
    public class GameSystem : AbstractSystem
    {
        public List<Player> Players;
        
        //TODO Save it in the savable
        //public int Money;
        public BindableProperty<int> Money;
        protected override void OnInit()
        {
            Money = new BindableProperty<int>();
            Players = new List<Player>();
            Money.Value = 500;
            StageInfo stageInfo = this.GetSystem<ResLoadSystem>().Table.TbStageInfo[0];
            foreach (var playerInfo in stageInfo.CharacterList_Ref)
            {
                Player player = Player.GetPlayer(playerInfo);
                player.Init(playerInfo);
                Players.Add(player);
            }
            //Save();
            //Load();
        }


        //Open Bag Panel
        public void OpenBag()
        {
            Debug.Log("Open Bag!");
        }
        
        //Open Bag Panel
        public void OpenCharacter()
        {
            Debug.Log("Open Character!");
        }
        
        
        //Store System
        public List<QFramework.Tuple<CardInfo, int>> GetStoreItem()
        {
            int StoreCardCount = 5;
            List<QFramework.Tuple<CardInfo, int>> res = new List<QFramework.Tuple<CardInfo, int>>();
            List<CardInfo> InStoreCards = this.GetSystem<ResLoadSystem>().Table.TbCardInfo.DataList.FindAll(x => x
            .Tags.Contains("InStore"));
            foreach (var cardInfo in InStoreCards.PickRandom(StoreCardCount))
            {
                res.Add(new QFramework.Tuple<CardInfo, int>(cardInfo, 100));
            }
            return res;
        }

        public void BuyCard(CardInfo cardInfo, int price)
        {
            if (Money.Value >= price)
            {
                Money.Value -= price;
                Debug.Log("Buy Card!");
            }
            else
            {
                Debug.Log("Not Enough Money!");
            }            
        }







        public void StartGame()
        {
            
        }
        
        public void Save()
        {


            this.GetSystem<SaveSystem>().SaveByJson();
        }

        public void Load()
        {
            this.GetSystem<SaveSystem>().LoadByJson();
        }
    }

    
}