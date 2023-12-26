// Generate Id:9964f028-b0f7-4392-bbf2-61645677e1ea

using System;
using System.Collections.Generic;
using cfg;
using Draconia.ViewController;
using DG.Tweening;
using Draconia.Controller;
using Draconia.Game.Buff;
using Draconia.UI;
using Draconia.ViewController.Event;
using QFramework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Draconia.ViewController
{
	public partial class PlayerViewController : CharacterViewController
	{

		//public UnityEngine.UI.Image CharacterImage;

		public RectTransform EnergyBar;

		public TMPro.TextMeshProUGUI RestCardNum;

		public RectTransform ChooseBracelet;

		public RectTransform CardDeck;
		
		public RectTransform CardBin;
		
		[HideInInspector]
		public Sprite CardImageSprite;
		private Player _player;
		
		public Player Player
		{
			get { return _player; }
			set { _player = value; }
		}
	}
}

