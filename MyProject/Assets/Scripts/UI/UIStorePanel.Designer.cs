using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:d1b9334b-9751-42bc-a122-f7ae3f6c36a9
	public partial class UIStorePanel
	{
		public const string Name = "UIStorePanel";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI MoneyTxt;
		[SerializeField]
		public RectTransform Store;
		
		private UIStorePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MoneyTxt = null;
			Store = null;
			
			mData = null;
		}
		
		public UIStorePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIStorePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIStorePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
