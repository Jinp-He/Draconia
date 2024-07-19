using Draconia.System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
{
	public class UIStartPanelData : UIPanelData
	{
	}
	public partial class UIStartPanel : UIPanel, ICanGetSystem
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIStartPanelData ?? new UIStartPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			StartGameBtn.onClick.AddListener(() =>
			{
				this.GetSystem<GameSystem>().StartGame();
			});
			if (!this.GetSystem<SaveSystem>().CanBeLoadable())
			{
				ContinueGameBtn.interactable = false;
			}
			
			ContinueGameBtn.onClick.AddListener(() =>
			{
				this.GetSystem<GameSystem>().ContinueGame();
			});
			
			InformationBtn.onClick.AddListener(OpenInformationPanel);
			
			ExitBtn.onClick.AddListener(() =>
			{
				this.GetSystem<GameSystem>().ExitGame();
			});
			
			
			
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
		
		public void OpenInformationPanel()
		{
			//UIKit.OpenPanel<UIInformationPanel>();
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}
