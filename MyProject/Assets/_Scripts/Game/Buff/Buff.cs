using System.Collections.Generic;
using _Scripts.System;
using cfg;
using Draconia.MyComponent;
using QFramework;
using TMPro;
using UnityEngine.Events;
using Utility;

namespace _Scripts.Game.Buff
{
    public class Buff : MyViewController, ICanRegisterEvent
    {


        
        public UnityEngine.UI.Image BuffImage;
        public TextMeshProUGUI BuffIndicator;

        public bool IsShowStack;
        
        private int _stack;
        public int Stack
        {
            set
            {
                _stack = value;
                BuffIndicator.text = _stack.ToString();
                if (_stack < 0) _stack = 0;
                //姿态的指示器没有通过的功效
                if (_buffInfo.IsPose)
                {
                    return;
                }
                switch (_stack)
                {
                    case 0:
                        break;
                    case 1:
                        BuffIndicator.gameObject.SetActive(false);
                        break;
                    default:
                        BuffIndicator.gameObject.SetActive(false);
                        if(IsShowStack)
                            BuffIndicator.gameObject.SetActive(true);
                        break;
                }
                
                
            }
            get => _stack;
        }
        
        private BuffInfo _buffInfo;
        private List<IUnRegister> _unRegister;
        private BuffManager _buffManager;
        private BuffEffect _buffEffect;

        public string BuffName => _buffInfo.BuffName;
        public UnityAction<int> OnEnd = (e) => { };



        public virtual void Init(BuffInfo buffInfo, int stack, BuffManager buffManager)
        {
            _buffManager = buffManager;
            _buffInfo = buffInfo;
            _stack = stack;
            _buffEffect = BuffEffect.GetEffect(buffInfo);
            _buffEffect.Init(this,buffInfo,buffManager);
            BuffImage.sprite = this.GetSystem<ResLoadSystem>().LoadSprite("buff_" + buffInfo.BuffName);
            GetComponent<MyTooltipManager>().InitTooltip(
                new Tooltip(){Name = buffInfo.BuffName, Desc = buffInfo.Description});
            
            
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
            
        }

        public void End()
        {
            _buffEffect.End();
            OnEnd?.Invoke(Stack);
            Destroy(gameObject);
        }



        
        
        
        
    }
}