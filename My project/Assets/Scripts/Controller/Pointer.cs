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
        private int _originalPos;
        public bool _isPlayer;
        private TimeBar _timeBar;
        public bool _isInit;
        public bool IsStop = false;
        
        public void Init(Player player, TimeBar timeBar)
        {
            Debug.Log("Init!");
            _timeBar = timeBar;
            _mPlayer = player;
            _speed = _mPlayer.PlayerInfo.Speed;
            PointerImage.sprite = player.CharacterAtlas.GetSprite("Pointer");
            _isPlayer = true;
            _isInit = true;
        }
        
        public void Init(Enemy enemy, TimeBar timeBar)
        {
            Debug.Log("Init!");
            _timeBar = timeBar;
            _mEnemy = enemy;
            _speed = _mEnemy.EnemyInfo.Speed;
            PointerImage.sprite = _mEnemy.GetComponent<EnemyAnimator>().PointerSprite;
            _isPlayer = false;
            _isInit = true;
        }


        public void Move(int count)
        {
            
        }

        /// <summary>
        /// 立即开始回合
        /// </summary>
        public void Refresh()
        {
            if (_isPlayer)
            {
                Vector3 pos = transform.position;
                transform.localPosition = new Vector3(_timeBar.PlayerStartPoint.position.x, pos.y, pos.z);
            }
            else
            {
                Vector3 pos = transform.position;
                transform.localPosition = new Vector3(_timeBar.EnemyStartPoint.position.x, pos.y, pos.z);
            }
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
                //Debug.Log("Stop");
                return;
            }

            if (!_isInit)
            {
                Debug.Log("NotInit");
                return;
            }
            if(_isPlayer)
                transform.localPosition += Vector3.right * _speed / 10;
            else
            {
                transform.localPosition -= Vector3.right * _speed / 10;
            }

            if (_isPlayer)
            {
                if (IsTouch(transform.position.x,_timeBar.PlayerActionPoint.position.x))
                {
                    Debug.LogFormat("#DEBUG# {0} {1}",transform.position.x,_timeBar.PlayerActionPoint.position.x);
                    BattleSystem.Stop();
                    BattleSystem.PlayerTurnStart(_mPlayer);
                }
            }
            else
            {
                if (IsTouch(transform.position.x,_timeBar.EnemyActionPoint.position.x))
                {
                    EnemyStrategy enemy = _mEnemy.EnemyStrategy;
                    _mEnemy.EnemyStrategy.Action();
                }
            }
        }

        private bool IsTouch(float a, float b)
        {
            return Math.Abs(a - b) < .01f;
        }
    }
}