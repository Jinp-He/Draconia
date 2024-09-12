using _Scripts.System;
using DG.Tweening;
using Draconia.MyComponent;
using QFramework;
using TMPro;

namespace _Scripts.UI
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