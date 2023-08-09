using Draconia.System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
{
	public class UISettingPanelData : UIPanelData
	{
	}
	public partial class UISettingPanel : UIPanel, ICanGetSystem
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UISettingPanelData ?? new UISettingPanelData();
			// please add init code here
			CloseBtn.onClick.AddListener(UIKit.ClosePanel<UISettingPanel>);
			RestartBtn.onClick.AddListener(() => { this.GetSystem<BattleSystem>().Restart(); });
			ExitBtn.onClick.AddListener(()=>
			{
				UnityEditor.EditorApplication.isPlaying = false;
				Application.Quit();
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			this.GetSystem<BattleSystem>().Stop();
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			this.GetSystem<BattleSystem>().Continue();
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}
