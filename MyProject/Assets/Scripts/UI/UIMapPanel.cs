using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	public class UIMapPanelData : UIPanelData
	{
	}
	public partial class UIMapPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIMapPanelData ?? new UIMapPanelData();
			// please add init code here
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
