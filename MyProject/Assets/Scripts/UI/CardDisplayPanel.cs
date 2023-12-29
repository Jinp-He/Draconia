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
		public List<CardVC> UsedCards;
		public List<CardVC> UnUsedCards;

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
			foreach (var card in mData.OnGoingPlayerViewController.Player.Deck)
			{
				CardVC tempCardVc = Instantiate(card, CardArea);
				tempCardVc.Init(card._cardInfo, card.CardUser);
				tempCardVc.ShowMode(); 
			}
			foreach (var card in mData.OnGoingPlayerViewController.Player.Hands)
			{
				CardVC tempCardVc = Instantiate(card, CardArea);
				tempCardVc.Init(card._cardInfo, card.CardUser);
				tempCardVc.ShowMode();
			}
			foreach (var card in mData.OnGoingPlayerViewController.Player.Bin)
			{
				CardVC tempCardVc = Instantiate(card, CardArea);
				tempCardVc.Init(card._cardInfo, card.CardUser);
				tempCardVc.ShowMode(true);
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
