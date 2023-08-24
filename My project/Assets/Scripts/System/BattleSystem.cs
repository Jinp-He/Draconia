using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using cfg;
using Draconia.UI;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;
using Utility;
using Debug = UnityEngine.Debug;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.System
{
    public enum BattleState
    {
        Stop, Player, Enemy, Ending
    }
    public class BattleSystem : AbstractSystem, ICanSendEvent
    {
        public Hands Hands;
        public List<Character> Characters;
        public List<Enemy> Enemies;
        public BattleState BattleState;

        public BindableProperty<int> Energy;
        public int InitEnergy = 0;
        public int MaxEnergy = 10;
        protected override void OnInit()
        {
            
        }

        public void TestInit()
        {
            StageInfo stageInfo = this.GetSystem<ResLoadSystem>().Table.TbStageInfo[0];
            //UIKit.OpenPanel<UISettingPanel>();
            UIKit.OpenPanel<UIBattlePanel>(new UIBattlePanelData() {StageInfo = stageInfo});
            Hands = UIKit.GetPanel<UIBattlePanel>().Hands;
            Characters = UIKit.GetPanel<UIBattlePanel>().Characters;
            Enemies =  UIKit.GetPanel<UIBattlePanel>().Enemies;
            Energy = new BindableProperty<int>(InitEnergy);
            Energy.Register(e =>
            {
                UIKit.GetPanel<UIBattlePanel>().EnergyCounter.Energy.text = e.ToString();
                //Debug.Log(Energy.Value);
            });
            Energy.Value = InitEnergy;
            BattleState = BattleState.Enemy;
            this.SendEvent<BattleStartEvent>();
        }

        private BattleState PrevBattleState;
        public void Stop()
        {
            PrevBattleState = BattleState;
            BattleState = BattleState.Stop;
            UIKit.OpenPanel<UIBattlePanel>().TimeBar.Stop();
            UIKit.OpenPanel<UIBattlePanel>().EnergyCounter.Stop();
        }

        public void Continue()
        {
            BattleState = PrevBattleState;
            UIKit.OpenPanel<UIBattlePanel>().TimeBar.Continue();
            UIKit.OpenPanel<UIBattlePanel>().EnergyCounter.Continue();
        }

        public void DrawCard(Character character, int num)
        {
            List<CardInfo> cards = character.Cards.PickRandom(num).ToList();
            foreach (var cardInfo in cards)
            {
                Hands.AddCard(cardInfo,character);
            }
        }
        

        public void DrawAttackCard(Character character)
        {
            foreach (var cardInfo in character.PlayerInfo.NormalAttackCard_Ref)
            {
                Hands.AddCard(cardInfo,character);
            }
        }
        public void PlayerTurnStart(Character character)
        {
            Energy.Value += 2;
            BattleState = BattleState.Player;
            DrawCard(character, 1);
            DrawAttackCard(character);
        }

        public void EnemyTurnStart(Enemy enemy)
        {
            Attack(enemy, Characters[0]);
        }

        /// <summary>
        /// Move ICharacter to a comparative position
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="position"></param>
        public void Move(Enemy enemy, int position)
        {
            int pos = enemy.Position;
            int finalPos = pos + position;
            
            //TODO best practice of this
            if (finalPos < 0)
            {
                finalPos = 0;
            }

            if (finalPos >= Enemies.Count)
            {
                finalPos = Enemies.Count;
            }

            (Enemies[pos], Enemies[finalPos]) = (Enemies[finalPos], Enemies[pos]);
        }

        public void Move(Character character, int position)
        {
            int pos = Characters.FindIndex(a => a = character);
            int finalPos = pos + position;
            
            //TODO best practice of this
            if (finalPos < 0)
            {
                finalPos = 0;
            }

            if (finalPos >= Characters.Count)
            {
                finalPos = Characters.Count;
            }

            (Characters[pos], Characters[finalPos]) = (Characters[finalPos], Characters[pos]);

        }

        public void Attack(Enemy enemy, Character character)
        {
            character.IsHit(enemy.EnemyInfo.AttackPower);
        }

        public void Attack(Character character, Enemy enemy)
        {
            
        }

        public void Restart()
        {
            UIKit.CloseAllPanel();
            TestInit();
        }
        
        

        
        
        
        public void GameOver()
        {
            
        }

        private void BattleEnd()
        {
            
        }
    }
}