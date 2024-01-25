using Draconia.System;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Utility;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
{
	public class UIMapPanelData : UIPanelData
	{
	}
	public partial class UIMapPanel : UIPanel, ICanGetSystem
	{
		public TileDropper[,] TileDroppers;
		public Tile TilePrefab;
		public PlayerViewController PlayerPrefab;
		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIMapPanelData ?? new UIMapPanelData();
			TileDroppers = MyToolKit<TileDropper>.ConvertTo2DArray(Grid.GetComponentsInChildren<TileDropper>(),5,14);
	
			SettingBtn.GetComponent<Button>().onClick.AddListener(() =>
			{
				UIKit.OpenPanel<UISettingPanel>();
			});

			foreach (var player in this.GetSystem<GameSystem>().Players)
			{
				PlayerViewController p = Instantiate(PlayerPrefab, TeamBar);
				p.Init(player,false);
			}
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		public Tile AddTileOnTileStore(int tileType, TileEventEnum tileEventType = TileEventEnum.None)
		{
			Tile t = Instantiate(TilePrefab, TileStore.transform);
			t.Init(tileType,tileEventType);
			return t;
		}
		
		public Tile AddTile(int tileType, int row, int col, MapEventEnum mapEvent = MapEventEnum.None, string tileName = "")
		{
			Tile t = Instantiate(TilePrefab, TileDroppers[row,col].transform);
			TileDroppers[row,col].DropTile(t);
			t.Init(tileType, TileEventEnum.None,mapEvent);
			t.DropTile();
			return t;
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}
