using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using cfg;
using Draconia.Controller;
using Draconia.UI;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using QFramework;
using Unity.Mathematics;
using DG;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;
using NotImplementedException = System.NotImplementedException;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

namespace Draconia.System
{
    public class MapSystem : AbstractSystem, ICanSendEvent, ICanRegisterEvent
    {
        public readonly int Row = 5;
        public readonly int Col = 14;
        public readonly float OneDirectionRate = 0.1f;
        public readonly float TwoDirectionRate = 0.4f;
        public readonly float ThreeDirectionRate = 0.4f;
        public readonly float FourDirectionRate = 0.1f;


        public TileDropper[,] TileDroppers;
        public Tile[,] Tiles;
        public List<Tile> TileStore;
        public HashSet<TileDropper> OpenTileDroppers;
        private int _restTileNum;
        private UnityEngine.UI.Image _playerPointer;
        private Tile _playerCurrentTile;
        private List<Tile> _playerExploredTiles;

        private List<TileDropper> _exploredTileDroppers;

        public int RestTileNum
        {
            get => _restTileNum;
            set
            {
                _restTileNum = value;
                if (_uiMapPanel != null)
                    _uiMapPanel.RestTileTxt.text = _restTileNum.ToString();
            }
        }

        private UIMapPanel _uiMapPanel;


        //初始化地图 管理地图的事宜 根据关卡要求生成地图
        protected override void OnInit()
        {
            this.RegisterEvent<DropTileEvent>((e) => { });
        }


        /// <summary>
        /// 测试UIMapPanel
        /// </summary>
        public void TestInit()
        {
            
            _uiMapPanel = UIKit.OpenPanel<UIMapPanel>();
            RestTileNum = 25;
            _playerPointer = _uiMapPanel.CharacterPointer;
            StartNewMap();
        }

        /// <summary>
        /// 检查放置的区块是否合法
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tileDropper"></param>
        /// <returns></returns>
        public bool IsValidSpot(Tile tile, TileDropper tileDropper)
        {
            int row = tileDropper.row;
            int col = tileDropper.col;
            if (OpenTileDroppers.Contains(tileDropper))
            {
                foreach (var direction in tile.TileDirections)
                {
                    if (tileDropper.Directions.Contains(direction))
                        return true;
                }
            }

            return false;
        }

        public void StartNewMap()
        {
            Debug.Log("#DEBUG# TestInit On MapSystem");
            TileStore = new List<Tile>();
            _playerExploredTiles = new List<Tile>();
            TileDroppers =
                MyToolKit<TileDropper>.ConvertTo2DArray(_uiMapPanel.Grid.GetComponentsInChildren<TileDropper>(), 5, 14);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    TileDroppers[i, j].row = i;
                    TileDroppers[i, j].col = j;
                }
            }
            Tiles = new Tile[5, 14];
            OpenTileDroppers = new HashSet<TileDropper>();
            ConnectedTiles = new Dictionary<Tile, HashSet<Tile>>();



            PrepareTileOnMap(1);
            MapInfo mapInfo = this.GetSystem<ResLoadSystem>().Table.TbMapInfo[1];
            //Tiles[0, 0] = _uiMapPanel.AddTile(5, 0, 0, MapEventEnum.Start);
            //ConnectedTiles.Add(Tiles[0,0], new HashSet<Tile>());
            
            _playerCurrentTile = Tiles[0, 0];
            OpenTileDroppers.Add(TileDroppers[0, 1]);
            TileDroppers[0, 1].Directions.Add(TileDirection.W);
            OpenTileDroppers.Add(TileDroppers[1, 0]);
            TileDroppers[1, 0].Directions.Add(TileDirection.S);
            ConnectedTiles[Tiles[0, 0]] = new HashSet<Tile>();
            
            AddNewTileOnStore();
            //Debug.Log("DEBUG Add New Tile On Store");
            AddNewTileOnStore();
            AddNewTileOnStore();

            //HelpFunction();
        }


        /// <summary>
        /// 添加新的Tile到商店
        /// </summary>
        public void AddNewTileOnStore()
        {
            int index = MyToolKit<int>.ChooseFromRandom(new List<float>()
            {
                OneDirectionRate, TwoDirectionRate / 2, TwoDirectionRate / 2, ThreeDirectionRate, FourDirectionRate
            }) + 1;

            TileInfo tileInfo = this.GetSystem<ResLoadSystem>().Table.TbTileInfo[index];
            int randomEventIndex = MyToolKit<int>.ChooseFromRandom(new List<float>()
            {
                tileInfo.NoEventRate, tileInfo.NormalEnemyRate, tileInfo.EliteEnemyRate, tileInfo.RandomEventRate
            });
            TileEventEnum tileEvent = (TileEventEnum)randomEventIndex;

            TileStore.Add(_uiMapPanel.AddTileOnTileStore(index, tileEvent));
            RestTileNum--;
            if (_restTileNum == 0)
            {
                StartBossBattle();
            }
        }

        /// <summary>
        /// 创建地图中，添加Tile到地图上,根据不同Map变化
        /// </summary>
        private void PrepareTileOnMap(int mapIndex = 1)
        {
            MapInfo mapInfo = this.GetSystem<ResLoadSystem>().Table.TbMapInfo[mapIndex];
            
            //Add Start Tile
            Tiles[0, 0] = _uiMapPanel.AddTile(5, 0, 0, MapEventEnum.Start);
            ConnectedTiles.Add(Tiles[0,0], new HashSet<Tile>());
            
            //Add Map Event
            foreach (var mapEvent in mapInfo.SpecialEvents)
            {
                if (mapEvent.EventType != EventTypeEnum.Conversation)
                {
                    int row = Random.Range(mapEvent.StartingRow, mapEvent.EndingRow);
                    int col = Random.Range(mapEvent.StartingCol, mapEvent.EndingCol);
                    Tiles[row, col] =
                        _uiMapPanel.AddTile(5, row, col, MapEventEnum.Elite, mapEvent.Name);
                    ConnectedTiles.Add(Tiles[row, col], new HashSet<Tile>());
                }
            }
            
            //Add Walls
            foreach (var wallPos in mapInfo.Walls)
            {
                Tiles[wallPos[0], wallPos[1]] =
                    _uiMapPanel.AddTile(0, wallPos[0], wallPos[1]);
                ConnectedTiles.Add(Tiles[wallPos[0], wallPos[1]], new HashSet<Tile>());
            }
            
            
            //Add Boss Tile
            int bossRow = mapInfo.BossLocation[0];
            int bossCol = mapInfo.BossLocation[1];
            Tiles[bossRow, mapInfo.BossLocation[1]] = _uiMapPanel.AddTile(5, bossRow, bossCol, MapEventEnum.BossConnect,mapInfo.BossLocationName);

            for(int i = bossRow; i > bossRow - 2; i--)
            {
                for(int j = bossCol; j < bossCol + 2; j++)
                {
                    Tiles[i, j] = _uiMapPanel.AddTile(5, i, j, MapEventEnum.BossConnect,mapInfo.BossLocationName);
                    ConnectedTiles.Add(Tiles[i, j], new HashSet<Tile>());
                }
            }
            
            //Add Store and Rest Area
            List<TileDropper> selectedTiles = new List<TileDropper>();
            for(int i = 0; i < mapInfo.StoreNum; i++)
            {
                TileDropper t = GetRandomTileDropper();
                Tiles[t.row, t.col] = _uiMapPanel.AddTile(5, t.row, t.col, MapEventEnum.Store);
                ConnectedTiles.Add(Tiles[t.row, t.col], new HashSet<Tile>());
            }
            for(int i = 0; i < mapInfo.RestNum; i++)
            {
                TileDropper t = GetRandomTileDropper();
                Tiles[t.row, t.col] = _uiMapPanel.AddTile(5, t.row, t.col, MapEventEnum.Rest);
                ConnectedTiles.Add(Tiles[t.row, t.col], new HashSet<Tile>());
            }
            
            

            TileDropper GetRandomTileDropper()
            {
                selectedTiles.Clear();
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 14; j++)
                    {
                        if (Tiles[i, j] == null)
                        {
                            if (i - 1 >= 0 && Tiles[i - 1, j] != null &&
                                Tiles[i - 1, j].MapEventIndex != MapEventEnum.None)
                                continue;
                            if (i + 1 < Row && Tiles[i + 1, j] != null &&
                                Tiles[i + 1, j].MapEventIndex != MapEventEnum.None)
                                continue;
                            if (j - 1 >= 0 && Tiles[i, j - 1] != null &&
                                Tiles[i, j - 1].MapEventIndex != MapEventEnum.None)
                                continue;
                            if (j + 1 < Col && Tiles[i, j + 1] != null &&
                                Tiles[i, j + 1].MapEventIndex != MapEventEnum.None)
                                continue;
                            selectedTiles.Add(TileDroppers[i, j]);
                        }
                    }
                }

                int randomTileIndex = Random.Range(0, selectedTiles.Count);
                TileDropper t = selectedTiles[randomTileIndex];
                return t;
            }

        }
        

        public void AddTileOnMap(Tile tile)
        {
            
            Tiles[tile.Row, tile.Col] = tile;
            AddTileOnOpenTileDroppers(tile);

            MovePlayer(tile);

            void AddTileOnOpenTileDroppers(Tile tile)
            {
                _playerExploredTiles.Add(tile);
                if (tile.Row - 1 >= 0 && tile.TileDirections.Contains(TileDirection.S))
                {
                    
                    TileDroppers[tile.Row - 1, tile.Col].Directions.Add(TileDirection.N);
                    if (Tiles[tile.Row - 1, tile.Col] != null && !_playerExploredTiles.Contains(Tiles[tile.Row - 1, tile.Col]))
                    {
                        AddTileOnOpenTileDroppers(Tiles[tile.Row - 1, tile.Col]);
                    }
                    else
                    {
                        OpenTileDroppers.Add(TileDroppers[tile.Row - 1, tile.Col]);
                    }
                    
                }

                if (tile.Row + 1 < Row && tile.TileDirections.Contains(TileDirection.N))
                {
                    
                    TileDroppers[tile.Row + 1, tile.Col].Directions.Add(TileDirection.S);
                    if (Tiles[tile.Row + 1, tile.Col] != null && !_playerExploredTiles.Contains(Tiles[tile.Row + 1, tile.Col]))
                    {
                        AddTileOnOpenTileDroppers(Tiles[tile.Row + 1, tile.Col]);
                    }
                    else
                    {
                        OpenTileDroppers.Add(TileDroppers[tile.Row + 1, tile.Col]);
                    }
                }

                if (tile.Col - 1 >= 0 && tile.TileDirections.Contains(TileDirection.W))
                {
                    
                    TileDroppers[tile.Row, tile.Col - 1].Directions.Add(TileDirection.E);
                    if (Tiles[tile.Row, tile.Col - 1] != null&& !_playerExploredTiles.Contains(Tiles[tile.Row, tile.Col - 1]))
                    {
                        AddTileOnOpenTileDroppers(Tiles[tile.Row, tile.Col - 1]);
                    }
                    else
                    {
                        OpenTileDroppers.Add(TileDroppers[tile.Row, tile.Col - 1]);
                    }
                }

                if (tile.Col + 1 < Col && tile.TileDirections.Contains(TileDirection.E))
                {
                    
                    TileDroppers[tile.Row, tile.Col + 1].Directions.Add(TileDirection.W);
                    if (Tiles[tile.Row, tile.Col + 1] != null&& !_playerExploredTiles.Contains(Tiles[tile.Row, tile.Col + 1]))
                    {
                        AddTileOnOpenTileDroppers(Tiles[tile.Row, tile.Col + 1]);
                    }
                    else
                    {
                        OpenTileDroppers.Add(TileDroppers[tile.Row, tile.Col + 1]);
                    }
                }
            }
        }
        
        

        /// <summary>
        /// 检查在事件结束后是否有地图事件
        /// </summary>
        public void CheckMapEvent()
        {
            //Debug.Log("DEBUG Checked");
            if (_playerExploredTiles.Any(tile => tile.MapEventIndex != MapEventEnum.None && tile.MapEventIndex != MapEventEnum.Explored))
            {
                Tile t = _playerExploredTiles.Where(tile => tile.MapEventIndex != MapEventEnum.None).ToArray()[0];
                Debug.Log("DEBUG Find Map Event Tile" + t.Row + " " + t.Col);
                MovePlayer(t);
                
            }

        }

        private void HelpFunction()
        {
            foreach (var tileDropper in OpenTileDroppers)
            {
                //tileDropper.GetComponent<Image>().color = Color.green;
            }
        }


        private bool isPlayerMoving;
        private Dictionary<Tile, HashSet<Tile>> ConnectedTiles = new Dictionary<Tile, HashSet<Tile>>();

        /// <summary>
        /// Move Player through A* Algorithm
        /// </summary>
        private void MovePlayer(Tile targetTile)
        {
            
            //Debug.Log("DEBUG Move to Tile" + targetTile.Row + " " + targetTile.Col);
            //Dictionary<Tile, List<Tile>> connectedTiles = new Dictionary<Tile, List<Tile>>();
            Tile playerPrevTile = _playerCurrentTile;
            _playerCurrentTile = targetTile;
            if(!ConnectedTiles.ContainsKey(targetTile))
                ConnectedTiles.Add(targetTile, new HashSet<Tile>());
            int row = targetTile.Row;
            int col = targetTile.Col;
            //Get Connected Tiles
            if (targetTile.Row - 1 >= 0 && Tiles[targetTile.Row - 1, targetTile.Col] != null
                                          && targetTile.TileDirections.Contains(TileDirection.S)
                                          && Tiles[targetTile.Row - 1, targetTile.Col].TileDirections
                                              .Contains(TileDirection.N))
            {
                ConnectedTiles[targetTile].Add(Tiles[targetTile.Row - 1, targetTile.Col]);
                ConnectedTiles[Tiles[targetTile.Row - 1, targetTile.Col]].Add(Tiles[row, col]);
            }

            if (targetTile.Row + 1 < Row && Tiles[targetTile.Row + 1, targetTile.Col] != null
                                           && targetTile.TileDirections.Contains(TileDirection.N)
                                           && Tiles[targetTile.Row + 1, targetTile.Col].TileDirections
                                               .Contains(TileDirection.S))
            {
                ConnectedTiles[targetTile].Add(Tiles[targetTile.Row + 1, targetTile.Col]);
                ConnectedTiles[Tiles[targetTile.Row + 1, targetTile.Col]].Add(Tiles[row, col]);
            }

            if (targetTile.Col - 1 >= 0 && Tiles[targetTile.Row, targetTile.Col - 1] != null
                                          && targetTile.TileDirections.Contains(TileDirection.W)
                                          && Tiles[targetTile.Row, targetTile.Col - 1].TileDirections
                                              .Contains(TileDirection.E))
            {
                ConnectedTiles[targetTile].Add(Tiles[targetTile.Row, targetTile.Col - 1]);
                ConnectedTiles[Tiles[targetTile.Row, targetTile.Col - 1]].Add(Tiles[row, col]);
            }

            if (targetTile.Col + 1 < Col && Tiles[targetTile.Row, targetTile.Col + 1] != null
                                           && targetTile.TileDirections.Contains(TileDirection.E)
                                           && Tiles[targetTile.Row, targetTile.Col + 1].TileDirections
                                               .Contains(TileDirection.W))
            {
                ConnectedTiles[targetTile].Add(Tiles[targetTile.Row, targetTile.Col + 1]);
                ConnectedTiles[Tiles[targetTile.Row, targetTile.Col + 1]].Add(Tiles[row, col]);
            }

            targetTile.ConnectedTiles = ConnectedTiles[targetTile];

            //Debug.Log("DEBUG " + Tiles[row,col].Row + " " + Tiles[row,col].Col + " " + ConnectedTiles[Tiles[row,col]].Count);
            //A* algorithm
            List<Tile> openList = new List<Tile>();
            List<Tile> closeList = new List<Tile>();
            openList.Add(targetTile);
            foreach (var tile in Tiles)
            {
                if (tile != null)
                {
                    tile.F = 0;
                    tile.G = 0;
                    tile.H = 0;
                }
            }
            while (openList.Count != 0)
            {
                Tile currentTile = openList[0];
                foreach (var tile in openList)
                {
                    if (tile.F < currentTile.F)
                        currentTile = tile;
                }

                openList.Remove(currentTile);
                closeList.Add(currentTile);
                if (currentTile.Row == playerPrevTile.Row && currentTile.Col == playerPrevTile.Col)
                {
                    //TODO Move Player
                    //Debug.Log("DEBUG Find Path " + targetTile.Row + " " + targetTile.Col);
                    List<Tile> path = new List<Tile>();
                    while (currentTile != targetTile)
                    {
                        path.Add(currentTile);
                        currentTile = currentTile.Parent;
                    }
                   

                    //path.Reverse();
                    path.Add(targetTile);
                    Sequence seq = DOTween.Sequence();
                    foreach (var tile in path)
                    {
                        seq.Append(_playerPointer.transform.DOLocalMove(
                            TileDroppers[tile.Row, tile.Col].transform.localPosition + new Vector3(0, 60, 0),
                            0.1f));
                    }

                    isPlayerMoving = true;
                    seq.AppendCallback(() =>
                    {
                        PlayerMovingEnd();
                    });

                    return;
                }

                foreach (var tile in currentTile.ConnectedTiles.Where(tile => !closeList.Contains(tile)))
                {
                    if (!openList.Contains(tile))
                    {
                        tile.G = currentTile.G + 1;
                        tile.H = math.abs(tile.Row - playerPrevTile.Row) + math.abs(tile.Col - playerPrevTile.Col);
                        tile.F = tile.G + tile.H;
                        tile.Parent = currentTile;
                        openList.Add(tile);
                    }
                    else
                    {
                        if (tile.G > currentTile.G + 1)
                        {
                            tile.G = currentTile.G + 1;
                            tile.F = tile.G + tile.H;
                            tile.Parent = currentTile;
                        }
                    }
                }
                
            }


            foreach (var pair in _playerExploredTiles)
            {
                pair.TestText.text = "G" + pair.G + "F" + pair.F + "H" + pair.H;

            }
        }

        private void PlayerMovingEnd()
        {
            isPlayerMoving = false;

            ExecuteEvent(_playerCurrentTile);
            if (_playerCurrentTile.EventIndex != 0)
            {
                ExecuteEvent(_playerCurrentTile);
            }
        }

        private void ExecuteEvent(Tile tile)
        {
            Debug.Log("#DEBUG# ExecuteEvent");
            this.GetSystem<BattleSystem>().InitBattle();
            switch (tile.EventIndex)
            {
                case TileEventEnum.None:
                    Debug.Log("None");
                    break;
                case TileEventEnum.NormalEnemy:
                    Debug.Log("NormalEnemy");
                    //this.GetSystem<BattleSystem>().InitBattle();
                    CheckMapEvent();
                    break;
                case TileEventEnum.EliteEnemy:
                    Debug.Log("EliteEnemy");
                    CheckMapEvent();
                    break;
                case TileEventEnum.RandomEvent:
                    Debug.Log("RandomEvent");
                    CheckMapEvent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (tile.MapEventIndex)
            {
                case MapEventEnum.None:
                    Debug.Log("Nothing");
                    break;
                case MapEventEnum.Boss:
                    Debug.Log("Boss");
                    CheckMapEvent();
                    break;
                case MapEventEnum.BossConnect:
                    Debug.Log("Boss");
                    CheckMapEvent();
                    break;
                case MapEventEnum.Elite:
                    Debug.Log("Elite");
                    break;
                case MapEventEnum.Store:
                    Debug.Log("Store");
                    
                    break;
                case MapEventEnum.Rest:
                    Debug.Log("Rest");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
            if (tile.MapEventIndex != MapEventEnum.None && tile.MapEventIndex != MapEventEnum.Explored)
            {
                tile.MapEventIndex = MapEventEnum.None;
                //tile.DirectionImage.gameObject.SetActive(true);
                tile.MapEventImage.sprite = tile.IconAtlas.GetSprite("Icon_Explored");
                //tile.Map.sprite = tile.IconAtlas.GetSprite("Icon_Explored");
                //tile.MapEventImage.gameObject.SetActive(false); 
            }


            CheckMapEvent();
        }

        private void StartBossBattle()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 设置板块
        /// </summary>
        public void SetTile()
        {
        }
    }
}