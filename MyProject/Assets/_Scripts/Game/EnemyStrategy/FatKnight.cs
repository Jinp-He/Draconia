using System;
using cfg;
using Draconia.System;
using UnityEngine;
using Utility;

namespace Draconia.ViewController
{
    public class FatKnight : EnemyStrategy
    {
        public FatKnight(Enemy enemy) : base(enemy)
        {
            ChooseNextTurnAction();
        }

        
        public override void ChooseNextTurnAction()
        {
            
            if (_enemy.EnemyInfo.AttackRange < _enemy.Position)
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[1];
            }
            else if (_enemy.Energy < _enemy.MaxEnergy)
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[1];
            }
            else
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[2];
            }
            //GetPossibleTarget(_currentAction.range);
            //Debug.LogFormat("#dEBUG# Change to {0} {1}",_currentAction.Name, _currentAction.ActionType);
            //TODO Add More Eco way to do this
            TryGetTarget(_currentAction);
            _enemy.Intention.InitTooltip(new Tooltip(){Name = _currentAction.Name, Desc = _currentAction.Desc});
            UpdateIntention();
            //base.Action();
        }
        
        public override void Action()
        {
       
            switch (_currentAction.Id)
            {
                case 1:
                    UseNormalAttack();
                    break;
                case 0:
                    _enemy.Move( 1);
                    break;
                case 2:
                    UseUlt();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ChooseNextTurnAction();
            EndAction();
        }
        protected override void UseNormalAttack()
        {
            
            _enemy.Energy+=1;
            BattleSystem.RangeAttack(_enemy, Range, AttackType.Physical, 3 );
            //Debug.Log("#DEBUG# USe Normal Attack");
        }
        protected override void UseUlt()
        {
            base.UseUlt();
            BattleSystem.RangeAttack(_enemy, Range,AttackType.Physical, 4 );
            //BattleSystem.Attack(_enemy, BattleSystem.Characters[1],AttackType.Physical, 4 );
            
            _enemy.Energy = 0;
            //Debug.Log("#DEBUG# USe Ult");
        }

      
    }
}