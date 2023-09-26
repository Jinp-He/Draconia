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
        
        
        public void Init(BuffInfo buffInfo, int stack, BuffManager buffManager)
        {
            _unRegister = new List<IUnRegister>();
            _buffManager = buffManager;
            Stack = stack;
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
        }

        public virtual void PlayerTurnStart()
        {
            
        }

        public void End()
        {
            OnEnd();
            _buffManager.Buffs.Remove(_buffInfo.BuffName);
            Destroy(gameObject);
        }

        public virtual void OnEnd()
        {
            foreach (var iunRegister in _unRegister)
            {
                iunRegister?.UnRegister();
            }
            _unRegister = new List<IUnRegister>();
        }
        
        
        
        
    }
}