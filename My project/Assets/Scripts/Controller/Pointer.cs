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
        private Player _mPlayer;
        private Enemy _mEnemy;
        private int _speed;
        private bool _isPlayer;
        public bool IsStop = false;
        public void Init(Player player)
        {
            _mPlayer = player;
            _speed = _mPlayer.PlayerInfo.Speed;
            PointerImage.sprite = player.CharacterAtlas.GetSprite("Pointer");
            _isPlayer = true;
        }
        
        public void Init(Enemy enemy)
        {
            _mEnemy = enemy;
            _speed = _mEnemy.EnemyInfo.Speed;
            PointerImage.sprite = _mEnemy.GetComponent<EnemyAnimator>().PointerSprite;
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
                    BattleSystem.PlayerTurnStart(_mPlayer);
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