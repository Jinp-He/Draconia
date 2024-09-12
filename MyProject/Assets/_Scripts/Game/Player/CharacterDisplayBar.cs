using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Game.Player
{
    public partial class CharacterDisplayBar : ViewController, IPointerEnterHandler, IPointerExitHandler
    {
        public void Init(PlayerStrategy.Player player)
        {
            HpBar.Init(player.MaxHp, player.Hp);
            UpdateBar(player);
        }

        public void UpdateBar(PlayerStrategy.Player player)
        {
            Name.text = player.Alias;
            Name.gameObject.SetActive(false);
            Level.text = "LV." + player.Level;
            ExperienceBar.DestroyChildren();
            for (int i = 0; i < player.Exp; i++)
            {
                Image bulb = Instantiate(ExperienceBulbPrefab, ExperienceBar);
                bulb.color = Color.green;
            }
            for (int i = 0; i < player.RestExp; i++)
            {
                Image bulb = Instantiate(ExperienceBulbPrefab, ExperienceBar);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Name.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Name.gameObject.SetActive(false);
        }
    }
}