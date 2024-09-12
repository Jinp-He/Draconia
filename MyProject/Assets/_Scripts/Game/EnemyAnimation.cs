using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Game
{
    public class EnemyAnimation : MonoBehaviour
    {
        private Enemy _enemy;

        public void Init(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Chosen();
            // UIKit.GetPanel<UIBattlePanel>().ChosenEnemy = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Unchosen();
            // UIKit.GetPanel<UIBattlePanel>().ChosenEnemy = null;
        }

        public void Chosen()
        {
            _enemy.ChooseBracelet.gameObject.SetActive(true);
        }

        public void Unchosen()
        {
            _enemy.ChooseBracelet.gameObject.SetActive(false);
        }
    }
}