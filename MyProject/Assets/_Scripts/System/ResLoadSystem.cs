using System.Collections.Generic;
using System.IO;
using cfg;
using QFramework;
using SimpleJSON;
using UnityEngine;
using UnityEngine.U2D;
using NotImplementedException = System.NotImplementedException;
using Object = UnityEngine.Object;

namespace _Scripts.System
{
    public class ResLoadSystem : AbstractSystem
    {
        private ResLoader mResLoader;
        private Tables _table;
        private Dictionary<string, Sprite> _dictionaryPool;
         public Tables Table
         {
             get { return _table ??= new Tables(Loader); }
         }

        protected override void OnInit()
        {
            mResLoader = ResLoader.Allocate();
            _dictionaryPool = new Dictionary<string, Sprite>(100);
        }

        public T LoadSync<T>(string objectName) where T : Object
        {
            return mResLoader.LoadSync<T>(objectName);
        }

        public Sprite LoadSprite(string objectName, string defaultName = "")
        {

            if (_dictionaryPool.ContainsKey(objectName))
            {
                return _dictionaryPool[objectName];
            }
            else
            {


                //mResLoader.Dispose();
                if (mResLoader.CheckLoad<Sprite>(objectName))
                {
                    Sprite s = mResLoader.LoadSync<Sprite>(objectName);
                    _dictionaryPool.Add(objectName, s);
                    return s;
                }
                else
                {
                    if (_dictionaryPool.ContainsKey(defaultName))
                    {
                        return _dictionaryPool[defaultName];
                    }
                    else if (mResLoader.CheckLoad<Sprite>(defaultName))
                    {
                        Sprite s = mResLoader.LoadSync<Sprite>(defaultName);
                        _dictionaryPool.Add(defaultName, s);
                        return s;
                    }
                    else
                    {
                        Debug.LogError("No Sprite Name or default is given");
                        throw new NotImplementedException();
                    }
                }
                //mResLoader.

            }
        }

        public SpriteAtlas LoadSpriteAtlas(string name)
        {
            return mResLoader.LoadSync<SpriteAtlas>(name);
        }
         

        private JSONNode Loader(string fileName)
        {
            return JSON.Parse(File.ReadAllText(Application.dataPath + "/../GenerateDatas/json/" + fileName + ".json"));
        }
        
        
    }
    
    public static class ResLoadSystemExtension 
    {
        public static bool CheckLoad<T>(this ResLoader self, string name)
        {
            return true;
        }
    }
}