using DG.Tweening;
using Draconia.MyComponent;
using Draconia.System;
using QFramework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Draconia.UI
{
    public class MoneyIndicator : MyViewController
    {
        public TextMeshProUGUI MoneyText;
        public IUnRegister UnRegister;
        public void Start()
        {
            MoneyText.text = this.GetSystem<GameSystem>().Money.Value.ToString();
            UnRegister = this.GetSystem<GameSystem>().Money.Register(e =>
            {
                DOTween.To(value => MoneyText.text = ((int) value).ToString(), int.Parse(MoneyText.text), e, 0.5f);
                //MoneyText.text = e.ToString();
            });
        }

        public void OnDestroy()
        {
            UnRegister?.UnRegister();
        }
    }
}