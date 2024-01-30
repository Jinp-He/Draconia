using System;
using System.Collections.Generic;
using DG.Tweening;
using Draconia.System;
using Draconia.UI;
using QFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Draconia.ViewController
{
    public enum TileDirection
    {
        N = 0, E = 1, S = 2, W = 3
    }

    public enum MapEventEnum
    {
        None,
        Start,
        Boss,
        Elite,
        Store,
        Rest

    }

    public enum TileEventEnum
    {
        None = 0, NormalEnemy = 1, EliteEnemy = 2, RandomEvent = 3
    }


    public partial class Tile : QFramework.ViewController, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanRegisterEvent, ICanGetSystem
    {
        
        
        

        //是否固定在地图上了
        public bool IsFixed;
        //是否在抓握
        public bool IsDrag;
        public int Row, Col;
        //事件序号
        public TileEventEnum EventIndex;
        public MapEventEnum MapEventIndex;
        private RectTransform _rectTransform;
        public SpriteAtlas TileType;
        public SpriteAtlas MapTileType;
        public SpriteAtlas IconAtlas;
        public List<TileDirection> TileDirections;
        public TextMeshProUGUI EventText;

        public HashSet<Tile> ConnectedTiles;
        
        //A*
        public int G, F, H;
        public Tile Parent;

        
        private int _tileIndex;

        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            IsDrag = false;
        }

        public void Init(int exits, TileEventEnum tileEventType = TileEventEnum.None, 
            MapEventEnum mapEvent = MapEventEnum.None, string tileName = "")
        {
            ConnectedTiles = new HashSet<Tile>();
            
            TileDirections = new List<TileDirection>();
            _tileIndex = exits;
            MapEventIndex = mapEvent;
            EventIndex = tileEventType;
            
            TileType = this.GetSystem<ResLoadSystem>().LoadSpriteAtlas("StoreTile");
            MapTileType = this.GetSystem<ResLoadSystem>().LoadSpriteAtlas("MapTile");
            IconAtlas = this.GetSystem<ResLoadSystem>().LoadSpriteAtlas("IconAtlas");
            switch (exits)
            {
                case 0:
                    
                    DirectionImage.sprite = TileType.GetSprite("00");
                    break;
                case 1:
                    TileDirections.Add(TileDirection.W);
                    DirectionImage.sprite = TileType.GetSprite("01");
                    break;
                case 2:
                    TileDirections.Add(TileDirection.W);
                    TileDirections.Add(TileDirection.E);
                    DirectionImage.sprite = TileType.GetSprite("02");
                    break;
                case 3:
                    TileDirections.Add(TileDirection.W);
                    TileDirections.Add(TileDirection.N);
                    DirectionImage.sprite = TileType.GetSprite("03");
                    break;
                case 4:
                    TileDirections.Add(TileDirection.E);
                    TileDirections.Add(TileDirection.N);
                    TileDirections.Add(TileDirection.S);
                    DirectionImage.sprite = TileType.GetSprite("04");
                    break;
                case 5:
                    TileDirections.Add(TileDirection.W);
                    TileDirections.Add(TileDirection.E);
                    TileDirections.Add(TileDirection.N);
                    TileDirections.Add(TileDirection.S);
                    DirectionImage.sprite = TileType.GetSprite("05");
                    break;
            }

            EventIndex = tileEventType;
            switch (tileEventType)
            {
                case TileEventEnum.None:
                    RandomEvent.gameObject.SetActive(false);
                    break;
                case TileEventEnum.NormalEnemy:
                    EventTitle.text = "普通敌人";
                    break;
                case TileEventEnum.EliteEnemy:
                    EventTitle.text = "精英敌人";
                    break;
                case TileEventEnum.RandomEvent:
                    EventTitle.text = "随机事件";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileEventType), tileEventType, null);
            }

            
            //如果是地图事件
            if (MapEventIndex != MapEventEnum.None)
            {
                DirectionImage.gameObject.SetActive(false);
                MapEventImage.gameObject.SetActive(true);
                TileDirections.Add(TileDirection.W);
                TileDirections.Add(TileDirection.E);
                TileDirections.Add(TileDirection.N);
                TileDirections.Add(TileDirection.S);
                
                switch (MapEventIndex)
                {
                    case MapEventEnum.Boss:
                        MapEventImage.sprite = IconAtlas.GetSprite("Icon_Store");
                        MapEventTitle.text = "Boss";
                        break;
                    case MapEventEnum.Elite:
                        MapEventImage.sprite = IconAtlas.GetSprite("Icon_Store");
                        MapEventTitle.text = "精英";
                        break;
                    case MapEventEnum.Store:
                        MapEventImage.sprite = IconAtlas.GetSprite("Icon_Store");
                        MapEventTitle.text = "商店";
                        break;
                    case MapEventEnum.Rest:
                        MapEventImage.sprite = IconAtlas.GetSprite("Icon_Rest");
                        MapEventTitle.text = "休息处";
                        break;
                    case MapEventEnum.Start:
                        MapEventImage.sprite = IconAtlas.GetSprite("Icon_Explored");
                        MapEventTitle.text = "起始处";
                        MapEventIndex = MapEventEnum.None;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mapEvent), mapEvent, null);
                }
                if (tileName != "")
                {
                    MapEventTitle.text = tileName;
                }
            }
        }



        public void OnBeginDrag(PointerEventData eventData)
        {
            
            if (IsFixed)
            {
                return;
            }

            oriPos = transform.position;
            oriTransform = transform.parent;
            IsDrag = true;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
        private TileDropper minDropper;
        private Vector3 oriPos;
        private Transform oriTransform;
        public void OnDrag(PointerEventData eventData)
        {
            if (IsFixed)
            {
                return;
            }
            Vector3 pos = UIKit.Root.Camera.ScreenToWorldPoint(Input.mousePosition);;
            pos.z = 0;
            transform.position = pos;

            
            //寻找离鼠标最近的地图块
            float min = float.MaxValue;
            minDropper = null;
            foreach (var tileDropper in  UIKit.GetPanel<UIMapPanel>().TileDroppers)
            {
                Vector2 dropPos = tileDropper.transform.position;
                Vector2 mousePos = pos;
                float dist = Vector2.Distance(dropPos, mousePos);
                if (dist < min && dist < .6f)
                {
                    min = dist;
                    minDropper = tileDropper;
                    //Debug.Log("最接近的地图块是 " + tileDropper.name);
                }
            }

            RandomEvent.GetComponent<CanvasGroup>().alpha = 0;

            //Debug.Log("#Position#" + pos);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsFixed)
            {
                return;
            }
            IsDrag = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (minDropper == null)
            {
                ReturnToStore();
            }
        }
        
        public void FixPosition()
        {
            IsFixed = true;
            DirectionImage.sprite = MapTileType.GetSprite("0" + _tileIndex);
            TileImage.ColorAlpha(0);
            IsDrag = false;
            this.GetSystem<MapSystem>().AddNewTileOnStore();
        }

        public void ReturnToStore()
        {
            transform.position = oriPos;
            transform.SetParent(oriTransform); 
            IsDrag = false;
  
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            RandomEvent.GetComponent<CanvasGroup>().alpha = 1;
        }


        public void DropTile()
        {
            IsFixed = true;
            DirectionImage.sprite = MapTileType.GetSprite("0" + _tileIndex);
            TileImage.ColorAlpha(0);
        }


        private Tween tween;
        private bool isRotating = false;
        void Update()
        {
            if (Input.GetKeyDown("t") && IsDrag && !isRotating)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            isRotating = true;
            tween = TileImage.transform.DORotate(TileImage.transform.rotation.eulerAngles + new Vector3(0, 0, -90), .3f)
                .OnComplete(() => { isRotating = false;});
            for (int i = 0; i < TileDirections.Count; i++)
            {
                TileDirection direction = TileDirections[i];
                TileDirections[i] = (TileDirection)(((int)direction + 1) % 4);
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

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}