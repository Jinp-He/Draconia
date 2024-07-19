using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	// Generate Id:0a5d4ab6-5e84-49d9-af0c-3c8fd8aaff23
	public partial class UITestSkeletonPanel
	{
		public const string Name = "UITestSkeletonPanel";
		
		[SerializeField]
		public RectTransform TestSkeleton;
		[SerializeField]
		public RectTransform ButtonList;
		
		private UITestSkeletonPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TestSkeleton = null;
			ButtonList = null;
			
			mData = null;
		}
		
		public UITestSkeletonPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UITestSkeletonPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UITestSkeletonPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
