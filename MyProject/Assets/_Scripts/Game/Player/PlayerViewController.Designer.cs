// Generate Id:9964f028-b0f7-4392-bbf2-61645677e1ea

using UnityEngine;

namespace _Scripts.Game.Player
{
	public partial class PlayerViewController : CharacterViewController
	{

		//public UnityEngine.UI.Image CharacterImage;

		public RectTransform EnergyBar;

		public TMPro.TextMeshProUGUI RestCardNum;

		public RectTransform ChooseBracelet;

		public RectTransform CardDeck;
		
		public RectTransform CardBin;

		public CharacterDisplayBar CharacterDisplayBar;
		
		
		
		[HideInInspector]
		public Sprite CardImageSprite;
		private PlayerStrategy.Player _player;
		
		public PlayerStrategy.Player Player
		{
			get { return _player; }
			set { _player = value; }
		}
	}
}

