using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
    public class TileDropper : QFramework.ViewController, IDropHandler
    {

        public bool IsOccupied;
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && IsOccupied == false)
            {
                eventData.pointerDrag.transform.position =
                    transform.position;
                IsOccupied = true;
            }
        }
    }
}