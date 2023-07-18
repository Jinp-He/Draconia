using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	public class UISettingPanelData : UIPanelData
	{
	}
	public partial class UISettingPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UISettingPanelData ?? new UISettingPanelData();
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
