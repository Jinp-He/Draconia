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

        public UIStorePanel UIStorePanel;
        //public CardVC CardVcPrefab;

        public void Init(CardInfo cardInfo, int price, UIStorePanel uIStorePanel)
        {
            UIStorePanel = uIStorePanel;
            CardVc.Init(cardInfo, cardPosition: CardPosition.Store);
            Price.text = price.ToString();
            
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if(this.GetSystem<GameSystem>().GameSetting.ConfirmTips)
                    this.GetSystem<GameSystem>().BuyCard(CardVc, price);
                else
                {
                    UIStorePanel.ConfirmBuyPanel.gameObject.SetActive(true);
                    UIStorePanel.ConfirmButton.onClick.RemoveAllListeners();
                    UIStorePanel.ConfirmButton.onClick.AddListener(() =>
                    {
                        this.GetSystem<GameSystem>().BuyCard(CardVc, price);
                        UIStorePanel.ConfirmBuyPanel.gameObject.SetActive(false);
                    });
                  
                }
            });
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}