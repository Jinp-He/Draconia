using System.Collections.Generic;

using TMPro;
using UnityEngine.UI;
using cfg;
using Draconia.MyComponent;
using Draconia.ViewController.Event;
using QFramework;
using Utility;

namespace Draconia.Game.Buff
{
    public class Buff : MyViewController, ICanRegisterEvent
    {

        public Image BuffImage;
        public TextMeshProUGUI BuffIndicator;

        private int _stack;
        public int Stack
        {
            set
            {
                
                _stack = value;
                if (_stack < 0) _stack = 0;
                switch (_stack)
                {
                    case 0:
                        End();
                        break;
                    case 1:
                        BuffIndicator.gameObject.SetActive(false);
                        break;
                    default:
                        BuffIndicator.gameObject.SetActive(true);
                        break;
                }
                BuffIndicator.text = _stack.ToString();
                
            }
            get => _stack;
        }
        
        private BuffInfo _buffInfo;
        private List<IUnRegister> _unRegister;
        private BuffManager _buffManager;
        private BuffEffect _buffEffect;
        
        public virtual void Init(BuffInfo buffInfo, int stack, BuffManager buffManager)
        {
            _buffEffect = BuffEffect.GetEffect(buffInfo);
            _buffEffect.Init(this,buffInfo,stack,buffManager);
            _buffManager = buffManager;
            _buffInfo = buffInfo;
            GetComponent<MyTooltipManager>().InitTooltip(
                new Tooltip(){Name = buffInfo.BuffName, Desc = buffInfo.Description});
            if (!_buffInfo.IsConsis)
            {
                _unRegister.Add(this.RegisterEvent<PlayerTurnStartEvent>(e =>
                {
                    PlayerTurnStart();
                    Stack--;
                    if (Stack == 0)
                    {
                        foreach (var iunRegister in _unRegister)
                        {
                            iunRegister?.UnRegister();
                        }

                        _unRegister = new List<IUnRegister>();
                        End();
                    }
                }));
            }
            OnAddBuff();
        }

        //添加buff时候施加的效果
        public virtual void OnAddBuff()
        {
            _buffEffect.OnAddBuff();
        }

        //Buff消失时候世家的效果
        public virtual void OnRemoveBuff()
        {
            _buffEffect.OnRemoveBuff();
        }

        public virtual void PlayerTurnStart()
        {
            _buffEffect.PlayerTurnStart();
        }

        public void End()
        {
            _buffEffect.End();
            Destroy(gameObject);
        }

        
        
        
        
    }
}