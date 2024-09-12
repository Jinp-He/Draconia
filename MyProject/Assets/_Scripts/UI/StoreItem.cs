using _Scripts.Game.Card;
using _Scripts.System;
using cfg;
using QFramework;
using UnityEngine.UI;

namespace _Scripts.UI
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
            return Draconia.Draconia.Interface;
        }
    }
}