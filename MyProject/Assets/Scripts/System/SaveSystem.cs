using System.Collections.Generic;
using System.IO;
using cfg;
using Draconia.ViewController;
using QFramework;
using Newtonsoft.Json;
using LitJson;
using SimpleJSON;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;


namespace Draconia.System
{
    public class TestData : ICanGetSystem
    {

        public TestData()
        {
    
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
    //你好
    public class SaveSystem : AbstractSystem
    {
        protected override void OnInit()
        {
            
        }


        public void SaveByJson()
        {
            //ISavable playerData = this.GetSystem<BattleSystem>().Players;
            string datapath = Application.dataPath + "/SaveFiles" + "/PlayerData.bin";
            byte[] dateStr = SerializationUtility.SerializeValue(this.GetSystem<GameSystem>().Players, DataFormat.Binary);
            
            //Debug.Log(dateStr);//利用JsonMapper将date转换成字符串
            File.WriteAllBytes(datapath, dateStr);

        }
        public void LoadByJson()
        {
            string path = Application.dataPath + "/SaveFiles";
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string datePath = Application.dataPath + "/SaveFiles" + "/PlayerData.bin";
            
            if (File.Exists(datePath))  //判断这个路径里面是否为空
            {
                byte[] bytes = File.ReadAllBytes(datePath);
                
                
                List<Player> dataStorage = SerializationUtility.DeserializeValue<List<Player>>(bytes, DataFormat.Binary);
                //binary形式保存 Luban的文件会丢失
                foreach (var player in dataStorage)
                {
                    player.PlayerInfo = this.GetSystem<ResLoadSystem>().Table.TbPlayerInfo[player.Alias];
                }
                
            }
            else
            {
                Debug.Log("------未找到文件------");
            }
        }

    }
}