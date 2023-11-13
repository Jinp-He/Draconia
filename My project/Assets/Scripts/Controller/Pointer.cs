using System;
using System.Collections.Generic;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.ViewController;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Draconia.Controller
{
    public class Pointer : MyViewController
    {
        public Image PointerImage;
        public Character Character;
        private Player _mPlayer;
        private Enemy _mEnemy;
        private int _speed;
        private int _originalPos;
        public bool _isPlayer;
        private TimeBar _timeBar;
        public bool _isInit;
        public bool IsStop = false;

        public float PositionX
        {
            set => transform.localPosition = new Vector3(value,transform.position.y,transform.position.z);
            get => transform.localPosition.x;
        }
        
        
        public void Init(Player player, TimeBar timeBar)
        {
            Debug.Log("Init!");
            Character = player;
            name = "Pointer_" + player.PlayerInfo.Name;
            _timeBar = timeBar;
            _mPlayer = player;
            _speed = _mPlayer.PlayerInfo.Speed;
            PointerImage.sprite = player.CharacterAtlas.GetSprite("Pointer");
            _isPlayer = true;
            _isInit = true;
            
            _timeBar.MoveAbsoluteTimePosition(this, _mPlayer.PlayerStrategy.BackNum, true);
        }
        
        public void Init(Enemy enemy, TimeBar timeBar)
        {
            Debug.Log("Init!");
            Character = enemy;
            _timeBar = timeBar;
            _mEnemy = enemy;
            _speed = _mEnemy.EnemyInfo.Speed;
            PointerImage.sprite = _mEnemy.CharacterAtlas.GetSprite("Pointer");
            _isPlayer = false;
            _isInit = true;
            
            //TODO: 更改初始位置
            _timeBar.MoveAbsoluteTimePosition(this, 4, true);
        }



        /// <summary>
        /// 立即开始回合
        /// </summary>
        public void Refresh()
        {

        }

        public void Move(int i)
        {
            _timeBar.MoveRelativeTimePosition(this, i);
        }
        
        public void Execute()
        {
            
        }
        
        /// <summary>
        /// 移动到正确的位置 不会超出,同时判断是否进入危险区？
        /// </summary>
        public void Regulate()
        {
            if (_isPlayer)
            {
                if (PositionX > 0)
                {
                    PositionX = 0;
                }

                if (PositionX < -TimeBar.TimeBarEdge)
                {
                    PositionX = -TimeBar.TimeBarEdge;
                }
            }
            else
            {
                if (PositionX < 0)
                {
                    PositionX = 0;
                }

                if (PositionX > TimeBar.TimeBarEdge)
                {
                    PositionX = TimeBar.TimeBarEdge;
                }
            }
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

            if (transform.localPosition.y != 0)
            {
                
            }

            // PointerEventData eventData = new PointerEventData(EventSystem.current);
            // eventData.position = transform.position;
            // if (_isPlayer)
            // {
            //     _timeBar.Players
            // }
            
            
            
            
            
            
            if(_isPlayer)
                transform.localPosition += Vector3.right * _speed / 10;
            else
            {
                transform.localPosition -= Vector3.right * _speed / 10;
            }

            if (_isPlayer)
            {
                //Debug.LogFormat("#DEBUG# Position {0} {1}",transform.position.x,_timeBar.PlayerActionPoint.position.x);

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
                    _mEnemy.  OnTurnStart();
                }
            }
        }
        
        void OnTriggerEnter2D(Collider2D other) 
        {
            //如果两个指针距离过近，则将其中一个往上提
            if (Math.Abs(other.transform.localPosition.y - transform.localPosition.y) < .1f &&
                Math.Abs(other.transform.localPosition.x - transform.localPosition.x) < .1f)
            {
                Vector3 pos = transform.localPosition;
                pos.x -= 10f;
                transform.localPosition = pos;
            }
                
            //Debug.Log ("Triggered");
           
        }

        private bool IsTouch(float a, float b)
        {
            return Math.Abs(a - b) < .1f;
        }
    }
}