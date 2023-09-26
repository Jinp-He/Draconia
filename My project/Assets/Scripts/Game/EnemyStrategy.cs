using System;
using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;
using UnityEngine;
using Utility;

namespace Draconia.ViewController
{
    public class EnemyStrategy : ICanGetSystem, ICanRegisterEvent
    {
        protected Enemy _enemy;
        protected EnemyAction _currentAction;

        protected Sprite Attack, Defense, Ult, Move;
        
        protected EnemyStrategy(Enemy enemy)
        {
            _enemy = enemy;
            Attack = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Attack");
            Defense = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Defense");
            Ult = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Ult");
            Move = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Move");

            this.RegisterEvent<BattleStartEvent>(e => { PreAction(); });
        }

        public static EnemyStrategy GetEnemyStrategy(Enemy enemy)
        {
            switch (enemy.EnemyInfo.Name)
            {
                case "猎狗":
                    return new Hounds(enemy);
                default:
                    return new Hounds(enemy);
            }
        }

        /// <summary>
        /// Decide what Action is and update the Intention
        /// </summary>
        protected virtual void PreAction()
        {
            _enemy.Intention.InitTooltip(new Tooltip(){Name = "撕咬", Desc = "造成5点伤害"});
        }

        /// <summary>
        /// if no strategy, attack/move/Ult
        /// </summary>
        public virtual void Action()
        {
            PreAction();
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

        protected virtual void ChooseIntention()
        {
            switch (_currentAction.ActionType)
            {
                case ActionType.Attack:
                    _enemy.IntentionImage.sprite = Attack;
                    break;
                case ActionType.Defense:
                    _enemy.IntentionImage.sprite = Defense;
                    break;
                case ActionType.Move:
                    _enemy.IntentionImage.sprite = Move;
                    break;
                case ActionType.Ult:
                    _enemy.IntentionImage.sprite = Ult;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}