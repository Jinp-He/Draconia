using System;
using _Scripts.Game.Player;
using _Scripts.System;
using Draconia.MyComponent;
using QFramework;
using UnityEngine;
using Utility;

namespace _Scripts.Game.PlayerStrategy
{
    public class MyPointer : MyViewController
    {
        public UnityEngine.UI.Image PointerImage;
        public CharacterViewController CharacterViewController;
        private PlayerViewController _mPlayerViewController;
        private Enemy _mEnemy;
        private int _speed;
        private int _originalPos;
        public bool _isPlayer;
        private TimeBar _timeBar;
        public bool IsInit = false;
        public bool IsStop = false;
        
        
        public BindableProperty<float> PosX;
        public BindableProperty<int> Pos;
        public BindableProperty<int> PosDiff;



        public void Init(PlayerViewController playerViewController, TimeBar timeBar)
        {
            IsInit = false;
            CharacterViewController = playerViewController;
            name = "Pointer_" + playerViewController.PlayerInfo.Name;
            _timeBar = timeBar;
            _mPlayerViewController = playerViewController;
            _speed = _mPlayerViewController.PlayerInfo.Speed;
            PointerImage.sprite = this.GetSystem<ResLoadSystem>().LoadSprite("Pointer_" + playerViewController.Alias);
            _isPlayer = true;
            
            
            PosX = new BindableProperty<float>(transform.localPosition.x);
            Pos = new BindableProperty<int>(GetPos(PosX.Value));
            PosDiff = new BindableProperty<int>(0);
            
            
            PosX.Register(e =>
            {
                transform.localPosition = new Vector3(e, transform.position.y, transform.position.z);
                int diff = Pos.Value - GetPos(PosX.Value);
                Pos.Value = GetPos(PosX.Value );
                PosDiff.Value = diff;
            });

            _timeBar.MoveAbsoluteTimePosition(this, _mPlayerViewController.Player.BackNum, true);

            MyToolKit<int>.StartTimer(.2f, () => { IsInit = true;});
        }
        
        public void Init(Enemy enemy, TimeBar timeBar)
        {
            IsInit = false;
            CharacterViewController = enemy;
            _timeBar = timeBar;
            _mEnemy = enemy;
            _speed = _mEnemy.EnemyInfo.Speed;
            PointerImage.sprite = _mEnemy.CharacterAtlas.GetSprite("Pointer");
            _isPlayer = false;
           
            PosX = new BindableProperty<float>(transform.localPosition.x);
            Pos = new BindableProperty<int>(GetPos(PosX.Value));
            PosDiff = new BindableProperty<int>(0);
            
            //TODO: 更改初始位置
          

            PosX.Register(e =>
            {
                transform.localPosition = new Vector3(e, transform.position.y, transform.position.z);
                Pos.Value = GetPos(PosX.Value);
                
            });
            Pos.Register(e =>
            {
                if (_mPlayerViewController != null)
                    Debug.LogFormat("DEBUG {0} : {1}", _mPlayerViewController.PlayerInfo.Alias, e);
            });
            _timeBar.MoveAbsoluteTimePosition(this, 4, true);
           
            MyToolKit<int>.StartTimer(.2f, () => { IsInit = true;});
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
                if (PosX.Value  > 0)
                {
                    PosX.Value = 0;
                }

                if (PosX.Value < -TimeBar.TimeBarEdge)
                {
                    PosX.Value = -TimeBar.TimeBarEdge;
                }
            }
            else
            {
                if (PosX.Value  < 0)
                {
                    PosX.Value = 0;
                }

                if (PosX.Value > TimeBar.TimeBarEdge)
                {
                    PosX.Value = TimeBar.TimeBarEdge;
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

            if (!IsInit)
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
                PosX.Value +=  _speed / 10;
            else
            {
                PosX.Value -= _speed / 10;
            }

            if (_isPlayer)
            {
                //Debug.LogFormat("#DEBUG# Position {0} {1}",transform.position.x,_timeBar.PlayerActionPoint.position.x);

                if (IsTouch(transform.position.x,_timeBar.PlayerActionPoint.position.x))
                {
                    //Debug.LogFormat("#DEBUG# {0} {1}",transform.position.x,_timeBar.PlayerActionPoint.position.x);
                    BattleSystem.PlayerTurnStart(_mPlayerViewController);
                }
            }
            else
            {
                if (IsTouch(transform.position.x,_timeBar.EnemyActionPoint.position.x))
                {
                    _mEnemy.OnTurnStart.Invoke();
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

        private int GetPos(float position)
        {
            float h = Math.Abs(position);
            
            return (int)h / (int)TimeBar.TimeBarScale;
        }
    }
}