using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:28bd30e9-9148-4e41-a64f-b35454a16bad
	public partial class UISettingPanel
	{
		public const string Name = "UISettingPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button RestartBtn;
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn;
		[SerializeField]
		public UnityEngine.UI.Button CloseBtn;
		
		private UISettingPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			RestartBtn = null;
			ExitBtn = null;
			CloseBtn = null;
			
			mData = null;
		}
		
		public UISettingPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UISettingPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UISettingPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
