using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class MyCanvasScaler : CanvasScaler
    {
        private Canvas _canvas;
        protected override void OnEnable()
        {
            _canvas = GetComponent<Canvas>();
            base.OnEnable();
        }

        protected override void HandleScaleWithScreenSize()
        {
            base.HandleScaleWithScreenSize();
            Vector2 screenSize;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                screenSize = _canvas.pixelRect.size;
            }
            else
            {
                screenSize = new Vector2(Screen.width, Screen.height);
            }
        }
    }
}