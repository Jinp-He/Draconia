using System.Collections.Generic;
using System.Linq;
using _Scripts.Game;
using _Scripts.Game.Card;
using _Scripts.Game.Event;
using _Scripts.Game.Player;
using _Scripts.UI;
using cfg;
using QFramework;
using Utility;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
namespace _Scripts.System
{
    public enum BattleState
    {
        Stop, Player, Enemy, Ending
    }

    public class BattleSystem : AbstractSystem, ICanSendEvent
    {
        public Hands Hands;
        public List<PlayerViewController> Players;

        public List<Enemy> Enemies;

        public BattleState BattleState;
        private UIBattlePanel UIBattlePanel;

        public BindableProperty<int> Energy;
        public int InitEnergy = 0;
        public int MaxEnergy = 10;

        public PlayerViewController OngoingPlayerViewController;
        public TimeBar TimeBar;
        
        //暂停状态
        private bool _systemStopState;
        public bool SystemStopState
        {
            get => _systemStopState;
            set
            {
                _systemStopState = value;
                if (_systemStopState)
                {
                    Stop();
                }
                else if (!StopState)
                {
                    Continue();
                }
                Debug.LogFormat("all state {0} {1} {2}",_systemStopState,_playerTurnStopState,_animationStopState);
            }
        }
        private bool _playerTurnStopState;
        public bool PlayerTurnStopState
        {
            get => _playerTurnStopState;
            set
            {
                _playerTurnStopState = value;
                if (_playerTurnStopState)
                {
                    Stop();
                }
                else if (!StopState)
                {
                    Continue();
                }
            }
        }
        private bool _animationStopState;
        public bool AnimationStopState
        {
            get => _animationStopState;
            set
            {
                _animationStopState = value;
                if (_animationStopState)
                {
                    Stop();
                }
                else if (!StopState)
                {
                    Continue();
                }
            }
        }

        public bool StopState => SystemStopState || AnimationStopState || PlayerTurnStopState;

        protected override void OnInit()
        {

        }



        private bool _isBattleInit = false;
        public void InitBattle()
        {
            StageInfo stageInfo = this.GetSystem<ResLoadSystem>().Table.TbStageInfo[0];
            //UIKit.OpenPanel<UISettingPanel>();
            
            UIKit.OpenPanel<UIBattlePanel>(new UIBattlePanelData() { StageInfo = stageInfo });
            UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
            Hands = UIBattlePanel.Hands;
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
            //TimeBar.Stop();
            //默认使用角色成为第一个
            OngoingPlayerViewController = Players[0];
            //this.SendEvent<BattleStartEvent>();

            _isBattleInit = true;
        }

        private BattleState _prevBattleState;
        

        public void Stop()
        {
            _prevBattleState = BattleState;
            Debug.LogFormat("Battlestate save {0}", _prevBattleState);
            //TODO 因为逻辑锁的问题 暂时取消了stop
            //BattleState = BattleState.Stop;
            UIKit.OpenPanel<UIBattlePanel>().TimeBar.Stop();
            UIKit.OpenPanel<UIBattlePanel>().EnergyCounter.Stop();
        }

        public void Continue()
        {
            BattleState = _prevBattleState;
            Debug.LogFormat("Battlestate load {0}", _prevBattleState);
            UIKit.OpenPanel<UIBattlePanel>().TimeBar.Continue();
            UIKit.OpenPanel<UIBattlePanel>().EnergyCounter.Continue();
        }

        /// <summary>
        /// 从玩家抽牌堆抽卡
        /// </summary>
        /// <param name="playerViewController"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<CardVC> DrawCard(PlayerViewController playerViewController, int num)
        {
            //如果没有卡牌了
            if (num > playerViewController.Player.Deck.Count)
            {
                foreach (var card in playerViewController.Player.Bin)
                {
                    playerViewController.Player.Deck.Add(card);
                    playerViewController.Player.Deck.Shuffle();
                }

                playerViewController.Player.Bin.Clear();
            }

            List<CardVC> cards = playerViewController.Player.Deck.PickRandom(num).ToList();
            playerViewController.Player.Hands.AddRange(cards);
            foreach (var card in cards)
            {
                Hands.AddCard(card, playerViewController);
                playerViewController.Player.Deck.Remove(card);
            }

            return cards;
        }





        /// <summary>
        /// 玩家手上默认的卡牌，调整位置，攻击等
        /// </summary>
        /// <param name="playerViewController"></param>
        /// <returns></returns>
        public void DrawNormalCard(PlayerViewController playerViewController)
        {
            Hands.AddAllBasicCard(playerViewController);
        }
        
        

        public void PlayerTurnStart(PlayerViewController playerViewController)
        {
            //Energy.Value += 2;
            PlayerTurnStopState = true;
            BattleState = BattleState.Player;
            playerViewController.OnTurnStart.Invoke();
            OngoingPlayerViewController = playerViewController;
            Debug.Log("#DEBUG# _isBattleInit: " + _isBattleInit);
            Debug.Log("#DEBUG# OngoingPlayerViewController: " + OngoingPlayerViewController);

            Hands.OnPlayerTurnStart(OngoingPlayerViewController);
            DrawCard(playerViewController, playerViewController.Player.CardDrawNum);
            DrawNormalCard(playerViewController);
        }

        public void PlayerTurnEnd()
        {
            if (BattleState != BattleState.Player)
            {
                Debug.Log("非法PlayerTurn");
                Debug.Log(BattleState);
                return;
            }

            BattleState = BattleState.Enemy;
            PlayerTurnStopState = false;
            Hands.OnEndTurn(OngoingPlayerViewController);
            OngoingPlayerViewController.Player.OnTurnEnd();
            //OngoingPlayer = null;
        }

        public void EnemyTurnStart(Enemy enemy)
        {
            //Attack(enemy, Characters[0], 1f, );
        }


        public void Attack(Enemy enemy, PlayerViewController playerViewController, AttackType attackType, int attackPower)
        {
            int dmg;
            //是否闪避
            if (Random.Range(0, 1f) > enemy.EnemyInfo.HitRate
                || Random.Range(0, 1f) <= playerViewController.Player.PlayerInfo.DodgeRate)
            {
                playerViewController.Miss();
                return;
            }

            //计算伤害
            if (attackType == AttackType.Magic)
                dmg = (int)(attackPower - playerViewController.Player.PlayerInfo.MagicResist);
            else
            {
                dmg = (int)(attackPower - playerViewController.Player.PlayerInfo.Armor);

            }
            
            //是否在危险区，如果在危险区加伤
            dmg += playerViewController.IsOnDangerArea() ? playerViewController.DamageDangerModifier : 0;

            //是否暴击
            if (Random.Range(0, 1f) <= enemy.EnemyInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * enemy.EnemyInfo.CriticalDamage);
            }

            playerViewController.IsHit(dmg, attackType);
            this.SendEvent(new AttackEvent(){Attacker = enemy, AttackReceiver = playerViewController, AttackType = attackType});
        }

        public void RandomAttack(PlayerViewController playerViewController, AttackType attackType, int attackPower)
        {
            Enemy enemy = Enemies[Random.Range(0, Enemies.Count)];
            Attack(playerViewController, enemy, attackType, attackPower);
        }
        


        public void Attack(PlayerViewController playerViewController, Enemy enemy, AttackType attackType, int attackPower)
        {
            int dmg;
            //是否闪避
            if (Random.Range(0, 1f) > playerViewController.Player.PlayerInfo.HitRate
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
            if (Random.Range(0, 1f) <= playerViewController.Player.PlayerInfo.CriticalHitRate)
            {
                dmg = (int)(dmg * playerViewController.Player.PlayerInfo.CriticalDamage);
            }

            enemy.IsHit(dmg, attackType);
            this.SendEvent(new AttackEvent(){Attacker = playerViewController, AttackReceiver = enemy, AttackType = attackType});
        }
        public void RangeAttack(Enemy enemy, List<int> range, AttackType attackType, int attackPower)
        {
            foreach (var i in range)
            {
                Debug.Log(i + " ");
            }
            List<PlayerViewController> list = UIBattlePanel.PlayerArea.GetComponentsInChildren<PlayerViewController>().ToList();
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
            //TestInit();
        }

        public void PlayerDie(PlayerViewController playerViewController)
        {
            Players.Remove(playerViewController);
            TimeBar.RemoveCharacter(playerViewController);
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

        public List<PlayerViewController> GetPlayersAtRange(int min, int max)
        {
            return UIBattlePanel.PlayerArea.GetComponentsInChildren<PlayerViewController>().ToList().GetRange(min, max);
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