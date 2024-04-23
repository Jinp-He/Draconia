using System.Collections.Generic;
using cfg;
using Draconia.System;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
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

			List<Tuple<CardInfo,int>> res = this.GetSystem<GameSystem>().GetStoreItem();
			GenerateCard(res);
		}

		private void GenerateCard(List<Tuple<CardInfo,int>> res)
		{
			var cardPrefab = this.GetSystem<ResLoadSystem>().LoadSync<GameObject>("CardPrefab").GetComponent<CardVC>();
			
			foreach (var tuple in res)
			{
				StoreItem storeItem = Instantiate(StoreItemPrefab, Store);
				storeItem.Init(tuple.Item1, tuple.Item2);
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
			return Draconia.Interface;
		}
	}
}