using System;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:d3575b69-0955-47a9-a6c9-2beb7304c106
	public partial class UIBattlePanel
	{
		public const string Name = "UIBattlePanel";
		
		[SerializeField]
		public UnityEngine.UI.Image CardArea;
		[SerializeField]
		public Hands Hands;
		[SerializeField]
		public UnityEngine.UI.Image GlowEffect;
		[SerializeField]
		public TMPro.TextMeshProUGUI CardName;
		[SerializeField]
		public UnityEngine.UI.Image CardImage;
		[SerializeField]
		public TMPro.TextMeshProUGUI CardType;
		[SerializeField]
		public TMPro.TextMeshProUGUI CardCost;
		[SerializeField]
		public TMPro.TextMeshProUGUI CardDesc;
		[SerializeField]
		public UnityEngine.UI.Image PlayerArea;
		[SerializeField]
		public UnityEngine.UI.Image CharacterImage;
		[SerializeField]
		public UnityEngine.UI.Slider HPBar;
		[SerializeField]
		public RectTransform EnergyBar;
		[SerializeField]
		public TMPro.TextMeshProUGUI RestCardNum;
		[SerializeField]
		public UnityEngine.UI.Image EnemyArea;
		[SerializeField]
		public UnityEngine.UI.Image EnemyImage;
		[SerializeField]
		public RectTransform EnemyBar;
		[SerializeField]
		public RectTransform ChooseBracelet;
		[SerializeField]
		public TimeBar TimeBar;
		[SerializeField]
		public RectTransform PlayerSlot;
		[SerializeField]
		public RectTransform EnemySlot;
		
		private UIBattlePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CardArea = null;
			Hands = null;
			GlowEffect = null;
			CardName = null;
			CardImage = null;
			CardType = null;
			CardCost = null;
			CardDesc = null;
			PlayerArea = null;
			CharacterImage = null;
			HPBar = null;
			EnergyBar = null;
			RestCardNum = null;
			EnemyArea = null;
			EnemyImage = null;
			EnemyBar = null;
			ChooseBracelet = null;
			TimeBar = null;
			PlayerSlot = null;
			EnemySlot = null;
			
			mData = null;
		}
		
		public UIBattlePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBattlePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBattlePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
