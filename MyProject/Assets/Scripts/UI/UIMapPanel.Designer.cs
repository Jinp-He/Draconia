using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:e0e8c686-5085-4785-b8dc-dd7d4cabc0ea
	public partial class UIMapPanel
	{
		public const string Name = "UIMapPanel";
		
		
		private UIMapPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
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
