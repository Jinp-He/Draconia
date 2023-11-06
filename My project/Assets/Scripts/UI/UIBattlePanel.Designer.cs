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
		public RectTransform EnemyIntentionArea;
		[SerializeField]
		public Utility.BelzierArrows.BezierArrows Bezier;
		[SerializeField]
		public Button EndTurnButton;
		[SerializeField]
		public Toggle ItemToggle;
		[SerializeField]
		public Toggle HandsToggle;
		[SerializeField]
		public Button CardBinButton;
		private UIBattlePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			SettingBtn = null;
			TimeBar = null;
			PlayerSlot = null;
			EnemySlot = null;
			PlayerArea = null;
			EnemyArea = null;
			EnemyImage = null;
			EnemyBar = null;
			CardArea = null;
			Hands = null;
			EnergyCounter = null;

			Bezier = null;
			EndTurnButton = null;
			ItemToggle = null;
			HandsToggle = null;
			CardBinButton = null;
			
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
