using System.Collections.Generic;
using Draconia.System;
using Draconia.ViewController.Event;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
    public class TileDropper : QFramework.ViewController, IDropHandler, ICanSendEvent, ICanGetSystem
    {
        public int row, col;
        public bool IsOccupied;
        public List<TileDirection> Directions = new List<TileDirection>();
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            Tile tile = eventData.pointerDrag.GetComponent<Tile>();
            if (IsOccupied == false && this.GetSystem<MapSystem>().IsValidSpot(tile,this))
            {
                eventData.pointerDrag.transform.position =
                    transform.position;
                IsOccupied = true;
                eventData.pointerDrag.transform.parent = transform;
                eventData.pointerDrag.GetComponent<Tile>().Row = row;
                eventData.pointerDrag.GetComponent<Tile>().Col = col;
                this.GetSystem<MapSystem>().AddTileOnMap(eventData.pointerDrag.GetComponent<Tile>());
                tile.FixPosition();
                this.SendEvent<DropTileEvent>();
            }
            else
            {
                tile.ReturnToStore();
                
            }
        }

        //加入特殊拼图
        public void DropTile(Tile t)
        {
            IsOccupied = true;
            t.transform.position =
                transform.position;
            t.transform.SetParent(transform,false);
            t.Row = row;
            t.Col = col;

        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}