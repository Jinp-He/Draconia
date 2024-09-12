using System.Collections.Generic;
using _Scripts.Game.Card;
using _Scripts.System;
using cfg;
using QFramework;
using UnityEngine;

namespace _Scripts.UI
{
	public class UIStorePanelData : UIPanelData
	{
	}
	public partial class UIStorePanel : UIPanel, ICanGetSystem
	{

		public StoreItem StoreItemPrefab;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIStorePanelData ?? new UIStorePanelData();
			// please add init code here
			

			ConfirmBuyToggle.isOn = this.GetSystem<GameSystem>().GameSetting.ConfirmTips;
			List<Tuple<CardInfo,int>> res = this.GetSystem<GameSystem>().GetStoreItem();
			CancelButton.onClick.AddListener(() =>
			{
				ConfirmBuyPanel.gameObject.SetActive(false);
			});
			ConfirmBuyToggle.onValueChanged.AddListener((value) =>
			{
				this.GetSystem<GameSystem>().GameSetting.ConfirmTips = value;
			});
			GenerateCard(res);
		}

		private void GenerateCard(List<Tuple<CardInfo,int>> res)
		{
			var cardPrefab = this.GetSystem<ResLoadSystem>().LoadSync<GameObject>("CardPrefab").GetComponent<CardVC>();
			
			foreach (var tuple in res)
			{
				StoreItem storeItem = Instantiate(StoreItemPrefab, Store);
				storeItem.Init(tuple.Item1, tuple.Item2, this);
			}
		}
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Draconia.Interface;
		}
	}
}
