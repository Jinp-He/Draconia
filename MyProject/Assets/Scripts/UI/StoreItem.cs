using cfg;
using Draconia.System;
using Draconia.ViewController;
using QFramework;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
{
    
    
    
    public class StoreItem : QFramework.ViewController, ICanGetSystem
    {
        public TMPro.TextMeshProUGUI Price;
        public CardVC CardVc;
        //public CardVC CardVcPrefab;

        public void Init(CardInfo cardInfo, int price)
        {
            CardVc.Init(cardInfo, cardPosition: CardPosition.Store);
            Price.text = price.ToString();
            
            GetComponent<Button>().onClick.AddListener(() =>
            {
                this.GetSystem<GameSystem>().BuyCard(cardInfo, price);
            });
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}