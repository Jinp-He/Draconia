using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:638a4010-65fc-4d7c-9b21-2c6dad92efcc
	public partial class UISettingPanel
	{
		public const string Name = "UISettingPanel";
		
		
		private UISettingPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
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
