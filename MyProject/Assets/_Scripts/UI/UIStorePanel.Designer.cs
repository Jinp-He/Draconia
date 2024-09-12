using UnityEngine;

namespace _Scripts.UI
{
	// Generate Id:88c2799f-75d4-4ae7-998d-effa5b908daf
	public partial class UIStorePanel
	{
		public const string Name = "UIStorePanel";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI MoneyTxt;
		[SerializeField]
		public RectTransform Store;
		[SerializeField]
		public UnityEngine.UI.Button CardBinButton;
		[SerializeField]
		public RectTransform ConfirmBuyPanel;
		[SerializeField]
		public UnityEngine.UI.Button ConfirmButton;
		[SerializeField]
		public UnityEngine.UI.Button CancelButton;
		[SerializeField]
		public UnityEngine.UI.Toggle ConfirmBuyToggle;
		
		private UIStorePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MoneyTxt = null;
			Store = null;
			CardBinButton = null;
			ConfirmBuyPanel = null;
			ConfirmButton = null;
			CancelButton = null;
			ConfirmBuyToggle = null;
			
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
