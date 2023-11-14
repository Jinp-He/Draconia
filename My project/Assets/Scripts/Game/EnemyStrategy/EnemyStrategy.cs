using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Draconia.ViewController
{
    public class EnemyStrategy : ICanGetSystem, ICanRegisterEvent
    {
        protected Enemy _enemy;
        protected EnemyAction _currentAction;
        protected BattleSystem BattleSystem => this.GetSystem<BattleSystem>();

        protected List<int> Range;
        protected Sprite Attack, Move, Other, Skill;
        protected EnemyStrategy(Enemy enemy)
        {
            _enemy = enemy;
            Attack = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Attack");
            Skill = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Skill");
            Other = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Other");
            Move = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Move");

            this.RegisterEvent<BattleStartEvent>(e => { ChooseNextTurnAction(); });
        }

        public static EnemyStrategy GetEnemyStrategy(Enemy enemy)
        {
            switch (enemy.EnemyInfo.Alias)
            {
                case "dog01":
                    return new Hounds(enemy);
                case "fatknight":
                    return new FatKnight(enemy);
                default:
                    return new Hounds(enemy);
            }
        }
        
        

        /// <summary>
        /// 尝试使用指定的技能，如果无法达到攻击范围，那么将动作替换成向前移动
        /// 根据角色的攻击范围选定将要攻击的角色
        /// </summary>
        public void TryGetTarget(EnemyAction enemyAction)
        {
            Range = new List<int>() { 0,0,0,0 };
            
            int len = enemyAction.AttackRange - _enemy.Position;
            Debug.LogFormat("TrygetTarget {0}",len);
            //如果攻击距离不满足
            switch (enemyAction.ActionType)
            {
                case ActionType.Attack:
                    if (len <= 0)
                        _currentAction = new EnemyAction(0, "移动", "移动一格",
                            ActionType.Move, 1, 0, 0, SkillTarget.Self);
                    break;
                        //TODO Add Other
            }
            
            //计算攻击的角色位置
            var listNum = new List<int>();
            Debug.Log(_enemy.EnemyInfo.Name);
            len = Math.Min(len, BattleSystem.Players.Count);
            for (int i = 0; i < len; i++)
            {
                listNum.Add(i);
            }
            if (enemyAction.ActionType == ActionType.Attack)
            {
                switch (enemyAction.Target)
                {
                    case SkillTarget.AllEnemy:
                        break;
                    case SkillTarget.SingleEnemy:
                        listNum = listNum.PickRandom(1).ToList();
                        break;
                    case SkillTarget.MultipleEnemy:
                        listNum = listNum.PickRandom(_currentAction.AttackTarget).ToList();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            for (int i = 0; i < 4; i++)
            {
                if (listNum.Contains(i))
                {
                    Range[i] = 1;
                }
            }
            Debug.Log(Range);
            
            UpdateIntentionSpot(Range);
            
        }

        /// <summary>
        /// Decide what Action is and update the Intention
        /// </summary>
        public virtual void ChooseNextTurnAction()
        {
            _enemy.Intention.InitTooltip(new Tooltip(){Name = "撕咬", Desc = "造成5点伤害"});
        }

        /// <summary>
        /// if no strategy, attack/move/Ult
        /// </summary>
        public virtual void Action()
        {
            ChooseNextTurnAction();
            //_enemy.BattleSystem.EnemyTurnStart(_enemy);
        }

        /// <summary>
        /// Normal Attack
        /// </summary>
        protected virtual void UseNormalAttack()
        {
        }

        /// <summary>
        /// Use Ultimate
        /// </summary>
        protected virtual void UseUlt()
        {
        }

        protected virtual void UpdateIntention()
        {
            switch (_currentAction.ActionType)
            {
                case ActionType.Attack:
                    _enemy.Intention.GetComponent<Intention>().IntentionImage.sprite = Attack;
                    break;
                case ActionType.Defense:
                    _enemy.Intention.GetComponent<Intention>().IntentionImage.sprite = Skill;
                    break;
                case ActionType.Move:
                    _enemy.Intention.GetComponent<Intention>().IntentionImage.sprite = Move;
                    break;
                case ActionType.Ult:
                    _enemy.Intention.GetComponent<Intention>().IntentionImage.sprite = Other;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_currentAction.ActionType == ActionType.Attack)
            {
                //计算攻击距离，将打击范围点亮
                  
            }
        }

        protected void UpdateIntentionSpot(List<int> intentionSpot)
        {
            Debug.Log("UpdateIntentionSpot");
            List<Image> positionMarker = _enemy.Intention.GetComponent<Intention>().PositionMarker
                .GetComponentsInChildren<Image>().ToList();
            for (int i = 0; i < 4; i++)
            {
                if (intentionSpot[i] == 1)
                {
                    positionMarker[i].color = Color.red;
                }
                else
                    positionMarker[i].color = Color.white;
            }
            
        }

        public void EndAction()
        {
            _enemy.EnemyTurnEnd();
        }


        protected List<Player> PossibleTarget(int minRange, int maxRange)
        { 
            List<Player> res = new List<Player>();
            int min = Math.Clamp(minRange - _enemy.Position, 0, BattleSystem.Players.Count);
            int max = Math.Clamp(maxRange - _enemy.Position, 0, BattleSystem.Players.Count);
            BattleSystem.GetPlayersAtRange(min, max);
            
            return res;
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}