using _Scripts.UIToolkit;
using UnityEngine;

namespace _Scripts.UI
{
    // Generate Id:960edfd7-3048-4bbe-b290-dcfa129b2230
    public partial class UISettingPanel
    {
        public const string Name = "UISettingPanel";
		
        [SerializeField]
        public UnityEngine.UI.Button CloseBtn;
        [SerializeField]
        public SettingOptions FullscreenToggle;
        [SerializeField]
        public UnityEngine.UI.Toggle Toggle;
        [SerializeField]
        public SettingOptions ScreenSizeDropdown;
        [SerializeField]
        public SettingOptions ConfirmTipsToggle;
        [SerializeField]
        public SettingOptions MainVolumeSlider;
        [SerializeField]
        public SettingOptions EnvVolumeSlider;
        [SerializeField]
        public SettingOptions VFX;
        [SerializeField]
        public SettingOptions LanguageDropdown;
        [SerializeField]
        public UnityEngine.UI.Button RestartBtn;
        [SerializeField]
        public UnityEngine.UI.Button ExitBtn;
		
        private UISettingPanelData mPrivateData = null;
		
        protected override void ClearUIComponents()
        {
            CloseBtn = null;
            FullscreenToggle = null;
            Toggle = null;
            ScreenSizeDropdown = null;
            ConfirmTipsToggle = null;
            MainVolumeSlider = null;
            EnvVolumeSlider = null;
            VFX = null;
            LanguageDropdown = null;
            RestartBtn = null;
            ExitBtn = null;
			
            mData = null;
        }
		
        public UISettingPanelData Data
        {
            get
            {
                return mData;
            }
        }
		
        UISettingPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new UISettingPanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
    }
}