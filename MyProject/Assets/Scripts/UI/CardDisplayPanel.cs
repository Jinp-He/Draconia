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
		public List<Card> UsedCards;
		public List<Card> UnUsedCards;

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
				Card tempCard = Instantiate(card, CardArea);
				tempCard.Init(card._cardInfo, card.CardPlayerViewController);
				tempCard.ShowMode(); 
			}
			foreach (var card in mData.OnGoingPlayerViewController.Player.Hands)
			{
				Card tempCard = Instantiate(card, CardArea);
				tempCard.Init(card._cardInfo, card.CardPlayerViewController);
				tempCard.ShowMode();
			}
			foreach (var card in mData.OnGoingPlayerViewController.Player.Bin)
			{
				Card tempCard = Instantiate(card, CardArea);
				tempCard.Init(card._cardInfo, card.CardPlayerViewController);
				tempCard.ShowMode(true);
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
