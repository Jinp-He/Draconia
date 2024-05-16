using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.System;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Utility;

namespace Draconia.UI
{
	public class UIBattlePanelData : UIPanelData
	{
		public StageInfo StageInfo;
	}
	public partial class UIBattlePanel : UIPanel, ICanSendEvent, ICanGetSystem
	{
		public GameSystem GameSystem => this.GetSystem<GameSystem>();
		public PlayerViewController PlayerViewControllerPrefab;
		public Enemy EnemyPrefab;
		public List<PlayerViewController> Characters;
		public List<Enemy> Enemies;
		public Animation OpenAnimation;
		
		protected override void OnInit(IUIData uiData = null)
		{
			OpenAnimation.Play();
			mData = uiData as UIBattlePanelData ?? new UIBattlePanelData();
			Characters = new List<PlayerViewController>();
			Enemies = new List<Enemy>();
			//please add init code here
			foreach (var player in GameSystem.Players)
			{
				PlayerViewController playerViewController = Instantiate(PlayerViewControllerPrefab, PlayerArea.transform, true);
				playerViewController.LocalScale(1f);
				playerViewController.LocalPosition(0, 0, 0);                                                                                                                                       
				playerViewController.Init(player);
				Characters.Add(playerViewController);
			}
			foreach (var enemyInfo in mData.StageInfo.EnemyList_Ref)
			{
				Debug.Log(enemyInfo.Name);
				ResKit.Init();
				Enemy enemy = Instantiate(EnemyPrefab, EnemyArea.transform, true);
				enemy.LocalScale(.8f);
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
			ItemToggle.onValueChanged.AddListener(e =>
			{
				if (e)
				{
					Hands.DisplayItem();
				}
			});
			HandsToggle.onValueChanged.AddListener(e =>
			{
				if (e)
				{
					Hands.DisplayHands();
				}
			});
			
			CardBinButton.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<CardDisplayPanel>(new CardDisplayPanelData()
				{
					OnGoingPlayerViewController = this.GetSystem<BattleSystem>().OngoingPlayerViewController,
				});
			});
			
			//this.SendEvent<BattleStartEvent>();
			

			
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
		public PlayerViewController ChosenPlayerViewController;


		public void Update()
		{
			
		}
		
		
		public void FocusOn(CharacterViewController characterViewController, float animationTime)
		{
			Rect rect = GetComponent<RectTransform>().rect;
			Vector2 relativePos = characterViewController.GetComponent<RectTransform>().position;
			//Debug.LogFormat("RelativePos is {0}",transform.position);
			Debug.Log(relativePos);
			//relativePos -= new Vector2(rect.width / 2, rect.height / 2);
			
			Vector2 pivot = new Vector2(0.5f + relativePos.x / rect.width, 0.5f+ relativePos.y / rect.height);
			Debug.LogFormat("pivot is {0}",pivot);
			GetComponent<RectTransform>().pivot = pivot;
			transform.DOScale(2.5f, animationTime);
			
		}

		public void FocusOut()
		{
			transform.DOScale(1, .1f);
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
