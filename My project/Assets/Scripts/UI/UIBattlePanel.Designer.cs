using System;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:a63b4da4-2004-4a7d-8837-0fc144068bea
	public partial class UIBattlePanel
	{
		public const string Name = "UIBattlePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button SettingBtn;
		[SerializeField]
		public TimeBar TimeBar;
		[SerializeField]
		public RectTransform PlayerSlot;
		[SerializeField]
		public RectTransform EnemySlot;
		[SerializeField]
		public UnityEngine.UI.Image PlayerArea;
		[SerializeField]
		public UnityEngine.UI.Image CharacterImage;
		[SerializeField]
		public UnityEngine.UI.Slider HPBar;
		[SerializeField]
		public UnityEngine.UI.Slider HPBarSlider;
		[SerializeField]
		public TMPro.TextMeshProUGUI HPText;
		[SerializeField]
		public RectTransform EnergyBar;
		[SerializeField]
		public TMPro.TextMeshProUGUI RestCardNum;
		[SerializeField]
		public RectTransform ChooseBracelet;
		[SerializeField]
		public UnityEngine.UI.Image EnemyArea;
		[SerializeField]
		public UnityEngine.UI.Image EnemyImage;
		[SerializeField]
		public RectTransform EnemyBar;
		[SerializeField]
		public UnityEngine.UI.Image CardArea;
		[SerializeField]
		public Hands Hands;
		[SerializeField]
		public EnergyCounter EnergyCounter;
		[SerializeField]
		public UnityEngine.UI.Image EnergyCounterImage;
		[SerializeField]
		public TMPro.TextMeshProUGUI Energy;
		[SerializeField]
		public Utility.BelzierArrows.BezierArrows Bezier;
		[SerializeField]
		public Button EndTurnButton;
		private UIBattlePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			SettingBtn = null;
			TimeBar = null;
			PlayerSlot = null;
			EnemySlot = null;
			PlayerArea = null;
			CharacterImage = null;
			HPBar = null;
			HPBarSlider = null;
			HPText = null;
			EnergyBar = null;
			RestCardNum = null;
			ChooseBracelet = null;
			EnemyArea = null;
			EnemyImage = null;
			EnemyBar = null;
			CardArea = null;
			Hands = null;
			EnergyCounter = null;
			EnergyCounterImage = null;
			Energy = null;
			Bezier = null;
			EndTurnButton = null;
			
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
