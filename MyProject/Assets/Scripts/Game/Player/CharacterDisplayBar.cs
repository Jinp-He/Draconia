using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Player
{
    public partial class CharacterDisplayBar : ViewController
    {
        public void Init(Draconia.ViewController.Player player)
        {
            HpBar.Init(player.MaxHp, player.Hp);
            UpdateBar(player);
        }

        public void UpdateBar(Draconia.ViewController.Player player)
        {
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
    }
}