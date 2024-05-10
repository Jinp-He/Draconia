using System.Collections.Generic;
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
			GameSetting gs = this.GetSystem<GameSystem>().GameSetting;
			mData = uiData as UISettingPanelData ?? new UISettingPanelData();
			// please add init code here
			CloseBtn.onClick.AddListener(UIKit.ClosePanel<UISettingPanel>);
			RestartBtn.onClick.AddListener(() => { this.GetSystem<BattleSystem>().Restart(); });
			ExitBtn.onClick.AddListener(() =>
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

			Resolution[] resolutions;


			resolutions = Screen.resolutions;

			ScreenSizeDropdown.Dropdown.ClearOptions();

			List<string> options = new List<string>();

			int currentResolutionIndex = 0;
			foreach (var t in resolutions)
			{
				string option = t.width + " x " + t.height;
				options.Add(option);
			}
			ScreenSizeDropdown.Dropdown.AddOptions(options);
			LanguageDropdown.Dropdown.ClearOptions();
			List<string> languageOptions = new List<string>();
			languageOptions.Add("简体中文");
			languageOptions.Add("English");
			LanguageDropdown.Dropdown.AddOptions(languageOptions);


			FullscreenToggle.Toggle.onValueChanged.AddListener((value) =>
			{
				gs.FullScreen = value;
				Screen.fullScreen = value;
			});	
			ConfirmTipsToggle.Toggle.onValueChanged.AddListener((value) =>
			{
				gs.ConfirmTips = value;
			});
			MainVolumeSlider.Slider.onValueChanged.AddListener((value) =>
			{
				gs.MainVolume = (int)value;
			});
			EnvVolumeSlider.Slider.onValueChanged.AddListener((value) =>
			{
				gs.EnvironmentVolume = (int)value;
			});
			VFX.Slider.onValueChanged.AddListener((value) =>
			{
				gs.SoundVolume = (int)value;
			});
			LanguageDropdown.Dropdown.onValueChanged.AddListener((value) =>
			{
				gs.Language = (GameLanguage)value;
			});
			ScreenSizeDropdown.Dropdown.onValueChanged.AddListener((value) =>
			{
				Screen.SetResolution(resolutions[value].width, resolutions[value].height, Screen.fullScreen);
			});
			

		}


		protected override void OnOpen(IUIData uiData = null)
		{
			//TODO Find Somewhere else to open it
			//this.GetSystem<BattleSystem>().SystemStopState = true;
			
			GameSetting gs = this.GetSystem<GameSystem>().GameSetting;
			
			FullscreenToggle.Toggle.isOn = gs.FullScreen;
			ConfirmTipsToggle.Toggle.isOn = gs.ConfirmTips;
			MainVolumeSlider.Slider.value = gs.MainVolume;
			MainVolumeSlider.SliderTxt.text = gs.MainVolume.ToString();
			EnvVolumeSlider.Slider.value = gs.EnvironmentVolume;
			EnvVolumeSlider.SliderTxt.text = gs.EnvironmentVolume.ToString();
			VFX.Slider.value = gs.SoundVolume;
			VFX.SliderTxt.text = gs.SoundVolume.ToString();
			LanguageDropdown.Dropdown.value = (int)gs.Language;
			ScreenSizeDropdown.Dropdown.value = 0;
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			//TODO Find Somewhere else to open it
			//this.GetSystem<BattleSystem>().SystemStopState = false;
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}
