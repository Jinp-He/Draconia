using QFramework;
using Spine.Unity;
using UnityEngine;

namespace _Scripts.UI
{
	public class UITestSkeletonPanelData : UIPanelData
	{
	}
	public partial class UITestSkeletonPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UITestSkeletonPanelData ?? new UITestSkeletonPanelData();
			// please add init code here
			SkeletonGraphic sg = TestSkeleton.GetComponent<SkeletonGraphic>();
			foreach (var VARIABLE in sg.skeletonDataAsset.fromAnimation)
			{
				Debug.Log("DEBUG " + VARIABLE);
			}
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
