using System;
using cfg;
using Draconia.System;
using UnityEngine;
using Utility;

namespace Draconia.ViewController
{
    public class Hounds : EnemyStrategy
    {
        public Hounds(Enemy enemy) : base(enemy)
        {
            //PreAction();
        }


        protected override void PreAction()
        {
            
            if (_enemy.EnemyInfo.AttackRange < _enemy.Position)
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[0];
            }
            else if (_enemy.Energy < 2)
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[1];
            }
            else
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[2];
            }
            Debug.LogFormat("#dEBUG# Change to {0} {1}",_currentAction.Name, _currentAction.ActionType);
            //TODO Add More Eco way to do this
            _enemy.Intention.InitTooltip(new Tooltip(){Name = _currentAction.Name, Desc = _currentAction.Desc});
            ChooseIntention();
            //base.Action();
        }
        
        public override void Action()
        {
            PreAction();
            switch (_currentAction.ActionType)
            {
                case ActionType.Attack:
                    UseNormalAttack();
                    break;
                case ActionType.Defense:
                    break;
                case ActionType.Move:
                    _enemy.BattleSystem.Move(_enemy, -1);
                    break;
                case ActionType.Ult:
                    UseUlt();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EndAction();
        }

        protected override void UseUlt()
        {
            base.UseUlt();
            _enemy.Energy = 0;
            //Debug.Log("#DEBUG# USe Ult");
        }

        protected override void UseNormalAttack()
        {
            //BattleSystem.Attack(_enemy, );
            base.UseNormalAttack();
            _enemy.Energy++;
            //Debug.Log("#DEBUG# USe Normal Attack");
        }
    }
}