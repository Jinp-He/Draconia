using System;
using System.Collections.Generic;
using cfg;
using Draconia.ViewController;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.System
{
    /// <summary>
    /// 管理游戏的进程，状态，保存和加载的系统
    /// </summary>
    public class GameSystem : AbstractSystem
    {
        public List<Player> Players;
        protected override void OnInit()
        {
            Players = new List<Player>();
            StageInfo stageInfo = this.GetSystem<ResLoadSystem>().Table.TbStageInfo[0];
            foreach (var playerInfo in stageInfo.CharacterList_Ref)
            {
                Player player = Player.GetPlayer(playerInfo);
                player.Init(playerInfo);
                Players.Add(player);
                
            }
            Save();
            Load();
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