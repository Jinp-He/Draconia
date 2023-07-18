using System.Collections.Generic;
using cfg;
using Draconia.Controller;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	public class UIBattlePanelData : UIPanelData
	{
		public StageInfo StageInfo;
	}
	public partial class UIBattlePanel : UIPanel
	{
		public Character PlayerPrefab;
		public Enemy EnemyPrefab;
		public List<Character> Characters;
		public List<Enemy> Enemies;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBattlePanelData ?? new UIBattlePanelData();
			Characters = new List<Character>();
			Enemies = new List<Enemy>();
			//please add init code here
			foreach (var playerInfo in mData.StageInfo.CharacterList_Ref)
			{
				Character player = Instantiate(PlayerPrefab, PlayerArea.transform, true);
				player.LocalScale(1);
				player.LocalPosition(0, 0, 0);                                                                                                                                       
				player.Init(playerInfo);
				Characters.Add(player);
			}
			foreach (var enemyInfo in mData.StageInfo.EnemyList_Ref)
			{
				Enemy enemy = Instantiate(EnemyPrefab, EnemyArea.transform, true);
				enemy.LocalScale(1);
				enemy.LocalPosition(0, 0, 0);
				enemy.Init(enemyInfo);
				Enemies.Add(enemy);
			}

			
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
	}
}
