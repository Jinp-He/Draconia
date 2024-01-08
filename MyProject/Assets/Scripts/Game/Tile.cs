using System;
using UnityEngine.EventSystems;

namespace Draconia.ViewController
{
    public class Tile : QFramework.ViewController, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        //是否固定在地图上了
        public bool IsOnBoard;
        public void OnBeginDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        
        
        public void OnDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        
    }
}