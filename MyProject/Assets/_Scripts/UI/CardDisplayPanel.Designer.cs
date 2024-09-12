using UnityEngine;

namespace _Scripts.UI
{
	// Generate Id:2be9ce16-9d08-4c22-a648-667005603db5
	public partial class CardDisplayPanel
	{
		public const string Name = "CardDisplayPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image FrongroundImage;
		[SerializeField]
		public UnityEngine.UI.Button ClostBtn;
		[SerializeField]
		public UnityEngine.RectTransform CardArea;
	
		private CardDisplayPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FrongroundImage = null;
			ClostBtn = null;
			CardArea = null;
			
			mData = null;
		}
		
		public CardDisplayPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		CardDisplayPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new CardDisplayPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
