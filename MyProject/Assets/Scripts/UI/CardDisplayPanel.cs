using System.Collections.Generic;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
{
	public class CardDisplayPanelData : UIPanelData
	{
		public PlayerViewController OnGoingPlayerViewController;
		public bool IsBattleMode;
	}
	public partial class CardDisplayPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as CardDisplayPanelData ?? new CardDisplayPanelData();
			// please add init code here
			if (mData.OnGoingPlayerViewController == null)
			{
				
			}
			//PassiveName.text = mData.OnGoingPlayer.PlayerInfo.Alias;
			ClostBtn.onClick.AddListener(CloseSelf);

			
			InitCards();
		}

		private void InitCards()
		{
			if (mData.IsBattleMode)
			{
				foreach (var card in mData.OnGoingPlayerViewController.Player.Deck)
				{
					if (card.IsBasicCard)
						return;
					CardVC tempCardVc = Instantiate(card, CardArea);
					tempCardVc.Init(card._cardInfo, card.CardUser);
					tempCardVc.ShowMode(); 
				}
				foreach (var card in mData.OnGoingPlayerViewController.Player.Hands)
				{
					if (card.IsBasicCard)
						return;
					CardVC tempCardVc = Instantiate(card, CardArea);
					tempCardVc.Init(card._cardInfo, card.CardUser);
					tempCardVc.ShowMode();
				}
				foreach (var card in mData.OnGoingPlayerViewController.Player.Bin)
				{
					if (card.IsBasicCard)
						return;
					CardVC tempCardVc = Instantiate(card, CardArea);
					tempCardVc.Init(card._cardInfo, card.CardUser);
					tempCardVc.ShowMode(true);
				}
			} 
			else
			{
				
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
	}
}
