using System;
using UnityEngine;
using Utility;

namespace Draconia.ViewController
{
    public class EnemyStrategy
    {
        private Enemy _enemy;

        public EnemyStrategy(Enemy enemy)
        {
            _enemy = enemy;
        }

        /// <summary>
        /// if no strategy, attack/move/Ult
        /// </summary>
        public void Action()
        {
            if (_enemy.EnemyInfo.EnemyStrategy != "")
            {
                throw new NotImplementedException();
            }

            if (_enemy.EnergyCount.Value == _enemy.EnemyInfo.MaxEnergy)
            {
                if (_enemy.EnemyInfo.AttackRange < _enemy.Position)
                {
                    _enemy.BattleSystem.Move(_enemy, -1);
                }
                else
                {
                    UseUlt();
                }
            }
            else 
            {
                if (_enemy.EnemyInfo.AttackRange < _enemy.Position)
                {
                    _enemy.BattleSystem.Move(_enemy, -1);
                }
                else
                {
                    UseNormalAttack();
                }
				
            }

            _enemy.BattleSystem.EnemyTurnStart(_enemy);
        }

        /// <summary>
        /// Normal Attack
        /// </summary>
        private void UseNormalAttack()
        {
            Character character = _enemy.BattleSystem.Characters.GetRange(0, Mathf.Min(_enemy.EnemyInfo.AttackRange - _enemy.Position, _enemy.BattleSystem.Characters.Count)).PickRandom();
            _enemy.BattleSystem.Attack(_enemy, character);
            _enemy.EnergyCount.Value++;
        }

        /// <summary>
        /// Use Ultimate
        /// </summary>
        private void UseUlt()
        {
            _enemy.EnergyCount.Value = 0;
        }
    }
}