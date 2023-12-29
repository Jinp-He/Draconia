using System.Collections.Generic;
using UnityEngine;

namespace Draconia.ViewController
{
    public class Card100 : Card
    {
        public override void Play(List<Enemy> _enemies, List<PlayerViewController> _allies)
        {
            Debug.Log("DEBUG 手术成功");
        }
    }
}