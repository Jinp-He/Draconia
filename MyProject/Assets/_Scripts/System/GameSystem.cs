using System;
using System.Collections.Generic;
using cfg;
using Draconia.UI;
using Draconia.ViewController;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.UI;
using Utility;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.System
{
    public enum GameLanguage
    {
        CHI,ENG
    }
    
    public class GameSetting
    {
        public void Save()
        {
            
        }
        
        //     确定提示
        // 打开后不再额外提示确定页面
        public bool FullScreen;
        
        //     确定提示
        // 打开后不再额外提示确定页面
        public bool ConfirmTips;
        
        //     屏幕震动       
        // 角色受伤时的屏幕抖动
        public bool IsHarmShake;
        
        // ///
        // /// 窗口尺寸        1600x900
        // 游戏窗口大小
        public int Width;
        public int Height;
        
        //
        //     主音量
        // 整体音量
        public int MainVolume;
        
        //
        //     环境音量
        // 仅改变环境音量
        public int EnvironmentVolume;
        
        //
        //     音效音量
        // 进改变音效音量
        public int SoundVolume;
        
        //
        //     语言    简体中文
        //     游戏文字
        public GameLanguage Language;
        //
        // /// 
    }

    /// <summary>
    /// 管理游戏的进程，状态，保存和加载的系统
    /// </summary>
    public class GameSystem : AbstractSystem
    {
        public List<Player> Players;
        public GameSetting GameSetting;
        
        //TODO Save it in the savable
        //public int Money;
        public BindableProperty<int> Money;
        protected override void OnInit()
        {
            //Game Setting On GameTest
            GameSetting = new GameSetting();
            GameSetting.ConfirmTips = false;
            GameSetting.FullScreen = false;
            GameSetting.IsHarmShake = true;
            GameSetting.Width = 1600;
            GameSetting.Height = 900;
            GameSetting.MainVolume = 50;
            GameSetting.EnvironmentVolume = 50;
            GameSetting.SoundVolume = 50;
            GameSetting.Language = GameLanguage.CHI;
            
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
            Debug.Log("#DEBUG# Player count:" + Players.Count);
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

        public void BuyCard(CardVC cardVc, int price)
        {
            if (Money.Value >= price)
            {
                Money.Value -= price;
                cardVc.BeingBought();
                Debug.Log("Buy Card!");
            }
            else
            {
                cardVc.DOShakePosition();
                Debug.Log("Not Enough Money!");
            }            
        }







        public void StartGame()
        {
            this.GetSystem<MapSystem>().TestInit();
            this.GetSystem<SaveSystem>().SaveByJson();
        }

        public void ContinueGame()
        {
            this.GetSystem<SaveSystem>().LoadByJson();
            this.GetSystem<MapSystem>().TestInit();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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