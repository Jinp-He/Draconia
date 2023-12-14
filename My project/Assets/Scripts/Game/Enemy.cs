using System.Collections.Generic;
using cfg;
using Draconia.Controller;
using Draconia.Game.Buff;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.UI;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using Utility;
using DOTween = DG.Tweening.DOTween;
using DG.Tweening;
using Draconia.ViewController.Event;
using NotImplementedException = System.NotImplementedException;
using Sequence = DG.Tweening.Sequence;

namespace Draconia.ViewController
{
	public partial class Enemy : Character,IPointerEnterHandler, IPointerExitHandler
	{
		public EnemyInfo EnemyInfo;
		public int _energy;
		public int MaxEnergy;
		public BuffManager BuffManager;
		public int Energy
		{
			get => _energy;
			set
			{
				_energy = value;
				if (_energy > MaxEnergy)
				{
					_energy = MaxEnergy;
				}

				for (int i = 0; i < MaxEnergy; i++)
				{
					if(i < _energy)
						EnemyBar._energyBulbs[i].color = Color.blue;
					else
					{
						EnemyBar._energyBulbs[i].color = Color.white;
					}
				}

			}

		}
	
		public int Position => transform.GetSiblingIndex();

		public EnemyStrategy EnemyStrategy => _enemyStrategy;
		
		//public SpriteAtlas EnemyAtlas;
		public EnemyAnimator _enemyAnimator;
		private EnemyStrategy _enemyStrategy;
		public EnemyAnimation EnemyAnimation;


		public void Init(EnemyInfo enemyInfo)
		{
			base.Init();
			BuffManager = GetComponent<BuffManager>();
			EnemyInfo = enemyInfo;
			_enemyStrategy = EnemyStrategy.GetEnemyStrategy(this);

			MaxEnergy = enemyInfo.MaxEnergy;

			CharacterAtlas = ResLoadSystem.LoadSync<SpriteAtlas>(enemyInfo.Alias);
			CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
			CharacterImage.SetNativeSize();
			
			HpBar.Init(enemyInfo.InitialHP,enemyInfo.InitialHP);
			EnemyBar.Init(enemyInfo);
			EnemyAnimation = GetComponent<EnemyAnimation>();
			EnemyAnimation.Init(this);
			CurrHP = EnemyInfo.InitialHP;
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddEnemy(this);
			_enemyAnimator.Init(this);
			
			TriggerDanger = () => { };
			TriggerDanger += EnterDangerArea;

			this.RegisterEvent<BattleStartEvent>(e => _enemyStrategy.ChooseNextTurnAction());
		}

		public override void TurnStart()  
		{
			//OnTurnStart.Invoke();
			//调整位置
			BattleSystem.TimeBar.MoveAbsoluteTimePosition(MyPointer, EnemyInfo.BackPos);
			_enemyAnimator.Attack();
			EnemyStrategy.Action();
		}



		public void EnemyTurnEnd()
		{
			MyPointer.Refresh();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			EnemyAnimation.OnPointerEnter(eventData);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			EnemyAnimation.OnPointerExit(eventData);
		}
		

		
		public void Move(int position)
		{
			int pos = transform.GetSiblingIndex() + position;
			if (pos < 0 || pos >= BattleSystem.Enemies.Count)
			{
				Debug.LogError("Move in Wrong Direction");
				return;
			}
			Move(transform.parent.GetChild(transform.GetSiblingIndex() + position).GetComponent<Enemy>());
		}
		public void Move(Enemy enemy)
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(enemy.transform.DOLocalMoveX(transform.localPosition.x, 1f))
				.Join(transform.DOLocalMoveX(enemy.transform.localPosition.x, 1f))
				.OnComplete(() =>
				{
					int tempPos = enemy.transform.GetSiblingIndex();
					enemy.transform.SetSiblingIndex(Position);
					transform.SetSiblingIndex(tempPos);
				});
		}


		public override void Die()
		{
			BattleSystem.EnemyDie(this);
			Destroy(this);
		}

		public void AddBuff(string buffName, int stack)
		{
			GetComponent<BuffManager>().AddBuff(buffName, stack);
		}
		
		public void RefreshBuff(string buffName, int stack)
		{
			GetComponent<BuffManager>().RefreshBuff(buffName, stack);
		}


		public void MoveInTime(int i)
		{
			MyPointer.Move(i);
		}

		private void EnterDangerArea()
		{
			IsHit(DangerAreaDamageNum, AttackType.Physical);
		}
	}
}
