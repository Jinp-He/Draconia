using System;
using cfg;
using Utility;

namespace _Scripts.Game.EnemyStrategy
{
    public class Hounds : EnemyStrategy
    {
        public Hounds(Enemy enemy) : base(enemy)
        {
            ChooseNextTurnAction();
        }


        public override void ChooseNextTurnAction()
        {

            if (_enemy.Energy < 2)
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[1];
            }
            else
            {
                _currentAction = _enemy.EnemyInfo.EnemyActions[2];
            }
            TryGetTarget(_currentAction);
            _enemy.Intention.InitTooltip(new Tooltip(){Name = _currentAction.Name, Desc = _currentAction.Desc});
            UpdateIntention();
        }
        
        public override void Action()
        {
       
            switch (_currentAction.Id)
            {
                case 0:
                    _enemy.Move( _currentAction.AttackRange);
                    break; 
                case 1:
                    UseNormalAttack();
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
            BattleSystem.RangeAttack(_enemy, Range, AttackType.Physical, 3);
            _enemy.Energy+=1;

            //Debug.Log("#DEBUG# USe Normal Attack");
        }
        protected override void UseUlt()
        {
            base.UseUlt();
            BattleSystem.RangeAttack(_enemy, Range, AttackType.Physical, 5);
            _enemy.Energy = 0;
            //Debug.Log("#DEBUG# USe Ult");
        }

       
    }
}