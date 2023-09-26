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
#if UNITY_EDITOR
            Debug.Log("You have quit the game");
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
#else
				Application.Quit();
				#endif
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
