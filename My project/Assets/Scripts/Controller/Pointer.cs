using System;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.ViewController;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Draconia.Controller
{
    public class Pointer : MyViewController
    {
        public Image PointerImage;
        private Character _mCharacter;
        private Enemy _mEnemy;
        private int _speed;
        private bool _isPlayer;
        public bool IsStop = false;
        public void Init(Character character)
        {
            _mCharacter = character;
            _speed = _mCharacter.PlayerInfo.Speed;
            PointerImage.sprite = character.CharacterAtlas.GetSprite("Pointer");
            _isPlayer = true;
        }
        
        public void Init(Enemy enemy)
        {
            _mEnemy = enemy;
            _speed = _mEnemy.EnemyInfo.Speed;
            PointerImage.sprite = _mEnemy.EnemyAtlas.GetSprite("dog01_Pointer");
        }


        public void Move(int count)
        {
            
        }

        /// <summary>
        /// 立即开始回合
        /// </summary>
        public void Refresh()
        {
            transform.localPosition = new Vector3(500, transform.localPosition.y, transform.localPosition.z);
        }

        public void Move()
        {
            
        }
        
        public void Execute()
        {
            
        }

        public void FixedUpdate()
        {
            if (IsStop)
            {
                return;
                
            }
            transform.localPosition += Vector3.right * _speed / 10;
            
            if (transform.localPosition.x >= 500)
            {
                if (_isPlayer)
                {
                    BattleSystem.Stop();
                    BattleSystem.PlayerTurnStart(_mCharacter);
                    //_mCharacter.
                }
                else
                {
                    _mEnemy.EnemyStrategy.Action();
                }
                var localPosition = transform.localPosition;
                transform.localPosition = new Vector3(0, localPosition.y, localPosition.z);
            }
        }
    }
}