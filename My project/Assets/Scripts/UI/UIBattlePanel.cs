using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Draconia.Controller;
using Draconia.System;
using Draconia.ViewController;
using Draconia.ViewController.Event;
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
	public partial class UIBattlePanel : UIPanel, ICanSendEvent, ICanGetSystem
	{
		public Player PlayerPrefab;
		public Enemy EnemyPrefab;
		public List<Player> Characters;
		public List<Enemy> Enemies;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBattlePanelData ?? new UIBattlePanelData();
			Characters = new List<Player>();
			Enemies = new List<Enemy>();
			//please add init code here
			foreach (var playerInfo in mData.StageInfo.CharacterList_Ref)
			{
				Player player = Instantiate(PlayerPrefab, PlayerArea.transform, true);
				player.LocalScale(1);
				player.LocalPosition(0, 0, 0);                                                                                                                                       
				player.Init(playerInfo);
				Characters.Add(player);
			}
			foreach (var enemyInfo in mData.StageInfo.EnemyList_Ref)
			{
				Debug.Log(enemyInfo.Name);
				ResKit.Init();
				Enemy enemyPrefab = this.GetSystem<ResLoadSystem>().LoadSync<GameObject>(enemyInfo.Name).GetComponent<Enemy>();
				Enemy enemy = Instantiate(enemyPrefab, EnemyArea.transform, true);
				enemy.LocalScale(1);
				enemy.LocalPosition(0, 0, 0);
				enemy.Init(enemyInfo);
				Enemies.Add(enemy);
			}
			SettingBtn.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<UISettingPanel>();
			});
			EndTurnButton.onClick.AddListener(() =>
			{
				this.GetSystem<BattleSystem>().PlayerTurnEnd();
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
				enemy.EnemyAnimation.Chosen();
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
				enemy.EnemyAnimation.Unchosen();
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
				enemy.EnemyAnimation.Chosen();
			}
		}
		GraphicRaycaster mRaycaster;
		PointerEventData _mPointerEventData;
		public Enemy ChosenEnemy;
		public Player ChosenPlayer;


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

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}
