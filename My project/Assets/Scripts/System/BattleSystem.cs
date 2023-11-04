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

        public Player OngoingPlayer;
        public TimeBar TimeBar;
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
            TimeBar = UIKit.GetPanel<UIBattlePanel>().TimeBar;
            //默认使用角色成为第一个
            OngoingPlayer = Characters[0];
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

        /// <summary>
        /// 从玩家抽牌堆抽卡
        /// </summary>
        /// <param name="player"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<Card> DrawCard(Player player, int num)
        {
            //如果没有卡牌了
            if (num > player.Deck.Count)
            {
                foreach (var card in player.Bin)
                {
                    player.Deck.Add(card);
                    player.Deck.Shuffle();
                }
                player.Bin.Clear();
            }
            
            List<Card> cards = player.Deck.PickRandom(num).ToList();
            player.Hands.AddRange(cards);
            foreach (var card in cards)
            {
                Hands.AddCard(card,player);
                player.Deck.Remove(card);
            }
            return cards;
        }
        
        

        

        /// <summary>
        /// 玩家手上默认的卡牌，调整位置，攻击等
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public void DrawNormalCard(Player player)
        {
            Hands.AddBasicCard(player);
        }
        
        public void PlayerTurnStart(Player player)
        {
            //Energy.Value += 2;
            BattleState = BattleState.Player;
            player.OnTurnStart();
            OngoingPlayer = player;
            Hands.OnPlayerTurnStart(OngoingPlayer);
            DrawCard(player, player.DrawCard);
            DrawNormalCard(player);
        }

        public void PlayerTurnEnd()
        {
            if (BattleState != BattleState.Player)
            {
                return;
            }
            BattleState = BattleState.Enemy;
            Continue();
            Hands.OnEndTurn(OngoingPlayer);
            OngoingPlayer.OnTurnEnd();
            //OngoingPlayer = null;
        }

        public void EnemyTurnStart(Enemy enemy)
        {
            //Attack(enemy, Characters[0], 1f, );
        }


        public void Attack(Enemy enemy, Player player,  AttackType attackType ,int attackPower)
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
                dmg = (int)(attackPower - player.PlayerInfo.MagicResist);
            else
            {
                dmg = (int)(attackPower - player.PlayerInfo.Armor);

            }
            //是否暴击
            if (Random.Range(0, 1f) <= enemy.EnemyInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * enemy.EnemyInfo.CriticalDamage);
            }
            player.IsHit(dmg,attackType);
        }

        public void Attack(Player player, Enemy enemy, AttackType attackType, int attackPower)
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
                dmg = (int)(attackPower  - enemy.EnemyInfo.MagicResist);
            else
            {
                dmg = (int)(attackPower - enemy.EnemyInfo.Armor);
            }
            //计算伤害
            
            //是否暴击
            if (Random.Range(0, 1f) <= player.PlayerInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * player.PlayerInfo.CriticalDamage);
            }
            enemy.IsHit(dmg, attackType);
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