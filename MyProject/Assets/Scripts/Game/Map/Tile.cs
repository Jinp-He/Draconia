using System;
using DG.Tweening;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Draconia.ViewController
{
    public class Tile : QFramework.ViewController, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        //是否固定在地图上了
        public bool IsFixed;
        //是否在抓握
        public bool IsDrag;
        private RectTransform _rectTransform;

        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDrag = true;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
        
        public void OnDrag(PointerEventData eventData)
        {
            if (IsFixed)
            {
                return;
            }
            Vector3 pos = UIKit.Root.Camera.ScreenToWorldPoint(Input.mousePosition);;
            pos.z = 0;
            transform.position = pos;
            //Debug.Log("#Position#" + pos);
            if (IsOnBoard())
            {
                GetComponent<Image>().color = Color.green;
            }
            else
            {
                GetComponent<Image>().color = Color.black;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDrag = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (IsOnBoard())
            {
                //IsFixed = true;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("DEBUG Enter " + other.name);
        }
        
        void Update()
        {
            if (Input.GetKeyDown("t") && IsDrag)
            {
                this.transform.DORotate(transform.rotation.eulerAngles + new Vector3(0,0,-90), .3f);
            }
        }


        private bool IsOnBoard()
        {
            return false;
        }

        private bool IsOnDeck()
        {
            return true;
        }
    }
}