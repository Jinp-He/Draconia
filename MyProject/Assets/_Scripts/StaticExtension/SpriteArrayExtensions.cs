using System.Linq;
using UnityEngine;

namespace Draconia.StaticExtension
{

    public static class SpriteArrayExtensions
    {
        /// <summary>
        /// 获取 Sprite 数组中的一个随机元素
        /// </summary>
        /// <param name="sprites">Sprite 数组</param>
        /// <returns>随机的 Sprite</returns>
        public static Sprite GetSprite(this Sprite[] sprites, string name)
        {
            if (sprites.Any(e => e.name == name))
            {
                return sprites.First(e => e.name == name);
            }
            else
            {
                Debug.LogError($"SpriteArrayExtensions.GetRandomSprite: No Sprite named {name} found in the array.");
                return null;
            }
        }
    }
}
    
