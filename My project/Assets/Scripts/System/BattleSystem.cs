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
        public List<Player> Players;
        public List<Enemy> Enemies;
        public BattleState BattleState;
        private UIBattlePanel UIBattlePanel;

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
            
            UIKit.OpenPanel<UIBattlePanel>(new UIBattlePanelData() { StageInfo = stageInfo });
            UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
            Hands = UIBattlePanel.Hands;
            Players = UIBattlePanel.Characters;
            Enemies = UIBattlePanel.Enemies;
            Energy = new BindableProperty<int>(InitEnergy);
            Energy.Register(e =>
            {
                UIBattlePanel.EnergyCounter.Energy.text = e.ToString();
                //Debug.Log(Energy.Value);
            });
            Energy.Value = InitEnergy;
            BattleState = BattleState.Enemy;
            TimeBar = UIBattlePanel.TimeBar;
            TimeBar.Init();
            //默认使用角色成为第一个
            OngoingPlayer = Players[0];
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
            if (num > player.PlayerStrategy.Deck.Count)
            {
                foreach (var card in player.PlayerStrategy.Bin)
                {
                    player.PlayerStrategy.Deck.Add(card);
                    player.PlayerStrategy.Deck.Shuffle();
                }

                player.PlayerStrategy.Bin.Clear();
            }

            List<Card> cards = player.PlayerStrategy.Deck.PickRandom(num).ToList();
            player.PlayerStrategy.Hands.AddRange(cards);
            foreach (var card in cards)
            {
                Hands.AddCard(card, player);
                player.PlayerStrategy.Deck.Remove(card);
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
            DrawCard(player, player.PlayerStrategy.CardDrawNum);
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
            OngoingPlayer.PlayerStrategy.OnTurnEnd();
            //OngoingPlayer = null;
        }

        public void EnemyTurnStart(Enemy enemy)
        {
            //Attack(enemy, Characters[0], 1f, );
        }


        public void Attack(Enemy enemy, Player player, AttackType attackType, int attackPower)
        {
            int dmg;
            //是否闪避
            if (Random.Range(0, 1f) > enemy.EnemyInfo.HitRate
                || Random.Range(0, 1f) <= player.PlayerStrategy.PlayerInfo.DodgeRate)
            {
                player.Miss();
                return;
            }

            //计算伤害
            if (attackType == AttackType.Magic)
                dmg = (int)(attackPower - player.PlayerStrategy.PlayerInfo.MagicResist);
            else
            {
                dmg = (int)(attackPower - player.PlayerStrategy.PlayerInfo.Armor);

            }
            
            //是否在危险区，如果在危险区加伤
            dmg += player.IsOnDangerArea() ? player.DamageDangerModifier : 0;

            //是否暴击
            if (Random.Range(0, 1f) <= enemy.EnemyInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * enemy.EnemyInfo.CriticalDamage);
            }

            player.IsHit(dmg, attackType);
        }
        

        public void Attack(Player player, Enemy enemy, AttackType attackType, int attackPower)
        {
            int dmg;
            //是否闪避
            if (Random.Range(0, 1f) > player.PlayerStrategy.PlayerInfo.HitRate
                || Random.Range(0, 1f) <= enemy.EnemyInfo.DodgeRate)
            {
                enemy.Miss();
                return;
            }

            if (attackType == AttackType.Magic)
                dmg = (int)(attackPower - enemy.EnemyInfo.MagicResist);
            else
            {
                dmg = (int)(attackPower - enemy.EnemyInfo.Armor);
            }
            //计算伤害
            
            //是否在危险区，如果在危险区加伤
            dmg += enemy.IsOnDangerArea() ? enemy.DamageDangerModifier : 0;

            //是否暴击
            if (Random.Range(0, 1f) <= player.PlayerStrategy.PlayerInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * player.PlayerStrategy.PlayerInfo.CriticalDamage);
            }

            enemy.IsHit(dmg, attackType);
        }
        public void RangeAttack(Enemy enemy, List<int> range, AttackType attackType, int attackPower)
        {
            foreach (var i in range)
            {
                Debug.Log(i + " ");
            }
            List<Player> list = UIBattlePanel.PlayerArea.GetComponentsInChildren<Player>().ToList();
            for (int i = 0; i < range.Count; i++)
            {
                Debug.Log(range.ToString());
                if(range[i] == 1)
                    Attack(enemy, list[i], attackType, attackPower);
            }
        }

        public void Restart()
        {
            UIKit.CloseAllPanel();
            TestInit();
        }

        public void PlayerDie(Player player)
        {
            Players.Remove(player);
            TimeBar.RemoveCharacter(player);
            if (Players.Count == 0)
            {
                GameOver();
            }
        }

        public void EnemyDie(Enemy enemy)
        {
            Enemies.Remove(enemy);
            TimeBar.RemoveEnemy(enemy);
            if (Enemies.Count == 0)
            {
                BattleEnd();
            }
        }

        public List<Player> GetPlayersAtRange(int min, int max)
        {
            return UIBattlePanel.PlayerArea.GetComponentsInChildren<Player>().ToList().GetRange(min, max);
        }

        public List<Enemy> GetEnemyAtRange(int min, int max)
        {
            return UIBattlePanel.EnemyArea.GetComponentsInChildren<Enemy>().ToList().GetRange(min, max);

        }




    public void GameOver()
        {
            
        }

        private void BattleEnd()
        {
            
        }
    }
}