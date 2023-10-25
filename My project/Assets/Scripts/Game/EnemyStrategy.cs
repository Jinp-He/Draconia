﻿using System;
using cfg;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Draconia.ViewController
{
    public class EnemyStrategy : ICanGetSystem, ICanRegisterEvent
    {
        protected Enemy _enemy;
        protected EnemyAction _currentAction;
        protected BattleSystem BattleSystem => this.GetSystem<BattleSystem>();

        protected Sprite Attack, Move, Other, Skill;
        
        protected EnemyStrategy(Enemy enemy)
        {
            _enemy = enemy;
            Attack = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Attack");
            Skill = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Skill");
            Other = this.GetSystem<ResLoadSystem>().LoadSprite("Intention_Other");
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
            if(_enemy.Intention == null)
                Debug.LogError("IntentionNullity");
            if (_enemy.Intention.GetComponent<Intention>() == null)
            {
                Debug.LogError("IntentionConponentNullity");
                Debug.Log(_enemy.name);
            }

            if(_enemy.Intention.GetComponent<Intention>().IntentionImage == null)
                Debug.LogError("IntentionImageNullity");
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
        }

        public void EndAction()
        {
            _enemy.EnemyTurnEnd();
        }

        protected virtual void ChooseTarget()
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}