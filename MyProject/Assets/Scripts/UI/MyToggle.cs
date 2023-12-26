using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Draconia.UI
{
    public class MyToggle : MonoBehaviour
    {
        public Sprite ToggleSprite;
        public Sprite OriginalSprite;
        public bool IsOn;
        public void Start()
        {
            OriginalSprite = GetComponent<Image>().sprite;
            GetComponent<Toggle>().onValueChanged.AddListener((e) =>
            {
                if (!e)
                {
                    GetComponent<Image>().sprite = OriginalSprite;
                    transform.DOLocalMoveY(0f, .1f);
                }
                else
                {
                    GetComponent<Image>().sprite = ToggleSprite;
                    transform.DOLocalMoveY(-30f, .1f);
                }
            });
            if (IsOn)
            {
                GetComponent<Image>().sprite = ToggleSprite;
                transform.DOLocalMoveY(-30f, .1f);
                GetComponent<Toggle>().isOn = true;
            }
        }

        private void OnChange()
        {
            
        }
    }
}