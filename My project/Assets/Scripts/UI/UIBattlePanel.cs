using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.Controller;
using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

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
			SettingBtn.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<UISettingPanel>();
			});

			
		}

		public void ChosenAll()
		{
			foreach (var character in Characters)
			{
				character.Chosen();
			}

			foreach (var enemy in Enemies)
			{
				enemy.Chosen();
			}
		}

		public void UnchosenAll()
		{
			foreach (var character in Characters)
			{
				character.Unchosen();
			}

			foreach (var enemy in Enemies)
			{
				enemy.Unchosen();
			}
		}

		public void ChoseAllAlly()
		{
			foreach (var character in Characters)
			{
				character.Chosen();
			}
		}
		
		public void ChoseAllEnemies()
		{
			foreach (var enemy in Enemies)
			{
				enemy.Chosen();
			}
		}
		GraphicRaycaster mRaycaster;
		PointerEventData _mPointerEventData;
		public Enemy ChosenEnemy;
		public Character ChosenCharacter;


		public void Update()
		{
			
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
