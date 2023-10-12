using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using cfg;
using Draconia.UI;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;
using Unity.Mathematics;
using Utility;
using Debug = UnityEngine.Debug;
using NotImplementedException = System.NotImplementedException;
using Random = UnityEngine.Random;
namespace Draconia.System
{
    public enum BattleState
    {
        Stop, Player, Enemy, Ending
    }
    public class BattleSystem : AbstractSystem, ICanSendEvent
    {
        public Hands Hands;
        public List<Player> Characters;
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

        public List<Card> DrawCard(Player player, int num)
        {
            List<CardInfo> cards = player.Cards.PickRandom(num).ToList();
            List<Card> res = new List<Card>();
            foreach (var cardInfo in cards)
            {
                res.Add(Hands.AddCard(cardInfo,player));
            }

            return res;
        }
        
        
        public List<Card> DrawRandom(Player player, int num)
        {
            List<CardInfo> cards = player.Cards.PickRandom(num).ToList();
            List<Card> res = new List<Card>();
            foreach (var cardInfo in cards)
            {
                res.Add(Hands.AddCard(cardInfo,player));
            }

            return res;
        }
        

        public List<Card> DrawAttackCard(Player player)
        {
            List<Card> res = new List<Card>();
            foreach (var cardInfo in player.PlayerInfo.NormalAttackCard_Ref)
            {
                res.Add(Hands.AddCard(cardInfo,player));
            }

            return res;
        }
        
        public void PlayerTurnStart(Player player)
        {
            Energy.Value += 2;
            BattleState = BattleState.Player;
            DrawCard(player, 1);
            DrawAttackCard(player);
        }

        public void EnemyTurnStart(Enemy enemy)
        {
            //Attack(enemy, Characters[0], 1f, );
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

        public void Move(Player player, int position)
        {
            int pos = Characters.FindIndex(a => a = player);
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

        public void Attack(Enemy enemy, Player player, float attackModifier, AttackType attackType)
        {
            int dmg;
            //是否闪避
            if (Random.Range(0, 1f) > enemy.EnemyInfo.HitRate 
                || Random.Range(0, 1f) <= player.PlayerInfo.DodgeRate)
            {
                player.Miss();
                return;
            }

            //计算伤害
            if(attackType == AttackType.Magic)
                dmg = (int)(enemy.EnemyInfo.AttackPower * attackModifier - player.PlayerInfo.MagicResist);
            else
            {
                dmg = (int)(enemy.EnemyInfo.AttackPower * attackModifier - player.PlayerInfo.Armor);

            }
            //是否暴击
            if (Random.Range(0, 1f) <= enemy.EnemyInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * enemy.EnemyInfo.CriticalDamage);
            }
            player.IsHit(dmg);
        }

        public void Attack(Player player, Enemy enemy, float attackModifier, AttackType attackType)
        {
            int dmg;
            //是否闪避
            if (Random.Range(0, 1f) > player.PlayerInfo.HitRate 
                || Random.Range(0, 1f) <= enemy.EnemyInfo.DodgeRate)
            {
                enemy.Miss();
                return;
            }

            if(attackType == AttackType.Magic)
                dmg = (int)(player.PlayerInfo.AttackPower * attackModifier - enemy.EnemyInfo.MagicResist);
            else
            {
                dmg = (int)(player.PlayerInfo.AttackPower * attackModifier - enemy.EnemyInfo.Armor);
            }
            //计算伤害
            
            //是否暴击
            if (Random.Range(0, 1f) <= player.PlayerInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * player.PlayerInfo.CriticalDamage);
            }
            enemy.IsHit(dmg);
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