using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.UI;
using Draconia.ViewController;
using QFramework;
using Utility;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.System
{
    public class BattleSystem : AbstractSystem
    {
        public Hands Hands;
        public List<Character> Characters;
        public List<Enemy> Enemies;
        protected override void OnInit()
        {
            
        }

        public void TestInit()
        {
            StageInfo stageInfo = this.GetSystem<ResLoadSystem>().Table.TbStageInfo[0];
            UIKit.OpenPanel<UISettingPanel>();
            UIKit.OpenPanel<UIBattlePanel>(new UIBattlePanelData() {StageInfo = stageInfo});
            Hands = UIKit.GetPanel<UIBattlePanel>().Hands;
            Characters = UIKit.GetPanel<UIBattlePanel>().Characters;
            Enemies =  UIKit.GetPanel<UIBattlePanel>().Enemies;

        }

        public void Stop()
        {
            UIKit.OpenPanel<UIBattlePanel>().TimeBar.Stop();
        }

        public void Continue()
        {
            UIKit.OpenPanel<UIBattlePanel>().TimeBar.Continue();
        }

        public void DrawCard(Character character, int num)
        {
            List<CardInfo> cards = character.Cards.PickRandom(num).ToList();
            foreach (var cardInfo in cards)
            {
                Hands.AddCard(cardInfo);
            }
        }

        public void DrawAttackCard(Character character)
        {
            
        }
        public void PlayerTurnStart(Character character)
        {
            DrawCard(character, 1);
            DrawAttackCard(character);
        }

        public void EnemyTurnStart(Enemy enemy)
        {
            Attack(enemy, Characters[0]);
        }

        public void Attack(Enemy enemy, Character character)
        {
            character.IsHit(enemy.EnemyInfo.AttackPower);
        }

        public void Attack(Character character, Enemy enemy)
        {
            
        }
        
        

        
        
        
        private void GameOver()
        {
            
        }

        private void BattleEnd()
        {
            
        }
    }
}