using UnityEngine;

namespace _Scripts.UI
{
	// Generate Id:3672eaab-001c-47a6-9c9b-e9eb244c1947
	public partial class UIStartPanel
	{
		public const string Name = "UIStartPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button SettingBtn;
		[SerializeField]
		public UnityEngine.UI.Button StartGameBtn;
		[SerializeField]
		public UnityEngine.UI.Button ContinueGameBtn;
		[SerializeField]
		public UnityEngine.UI.Button InformationBtn;
		[SerializeField]
		public UnityEngine.UI.Button ExitBtn;
		
		private UIStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			SettingBtn = null;
			StartGameBtn = null;
			ContinueGameBtn = null;
			InformationBtn = null;
			ExitBtn = null;
			
			mData = null;
		}
		
		public UIStartPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIStartPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIStartPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
