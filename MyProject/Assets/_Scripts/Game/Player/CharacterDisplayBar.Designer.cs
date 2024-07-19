using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Player
{
    public partial class CharacterDisplayBar :  ViewController
    {
        public Image ExperienceBulbPrefab;
        public HPBar HpBar;
        public TextMeshProUGUI Level;
        public RectTransform ExperienceBar;
        public TextMeshProUGUI Name;
    }
}