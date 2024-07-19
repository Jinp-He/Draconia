using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:4e99fd7f-6e10-46c8-803c-379ea374dac5
	public partial class UIMapPanel
	{
		public const string Name = "UIMapPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button CharacterBtn;
		[SerializeField]
		public UnityEngine.UI.Button SettingBtn;
		[SerializeField]
		public UnityEngine.UI.Button BagBtn;
		[SerializeField]
		public RectTransform TeamBar;
		[SerializeField]
		public RectTransform Grid;
		[SerializeField]
		public UnityEngine.UI.Image CharacterPointer;
		[SerializeField]
		public TMPro.TextMeshProUGUI RestTileTxt;
		[SerializeField]
		public UnityEngine.UI.Image TileStore;
		
		private UIMapPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CharacterBtn = null;
			SettingBtn = null;
			BagBtn = null;
			TeamBar = null;
			Grid = null;
			CharacterPointer = null;
			RestTileTxt = null;
			TileStore = null;
			
			mData = null;
		}
		
		public UIMapPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIMapPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIMapPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
