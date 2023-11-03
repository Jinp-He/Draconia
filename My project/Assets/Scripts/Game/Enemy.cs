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
using NotImplementedException = System.NotImplementedException;
using Sequence = DG.Tweening.Sequence;

namespace Draconia.ViewController
{
	public partial class Enemy : Character,IPointerEnterHandler, IPointerExitHandler
	{
		public EnemyInfo EnemyInfo;
		private int _energy;

		public int Energy
		{
			get => _energy;
			set
			{
				_energy = value;
				if (_energy > EnemyBar._energyBulbs.Count)
				{
					_energy = EnemyBar._energyBulbs.Count;
				}
				foreach (var bulb in EnemyBar._energyBulbs)
				{
					bulb.color = Color.white;

				}

				foreach (var bulb in EnemyBar._energyBulbs.GetRange(0, value))
				{
					bulb.color = Color.blue;
				}

			}

		}
	
		public int Position => transform.GetSiblingIndex();

		public EnemyStrategy EnemyStrategy => _enemyStrategy;


		public Pointer MyPointer;
		//public SpriteAtlas EnemyAtlas;
		public EnemyAnimator _enemyAnimator;
		private EnemyStrategy _enemyStrategy;
		public EnemyAnimation EnemyAnimation;


		public void Init(EnemyInfo enemyInfo)
		{
			//base.Init();
			EnemyInfo = enemyInfo;
			_enemyStrategy = EnemyStrategy.GetEnemyStrategy(this);

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
		}

		public override void OnTurnStart()  
		{
			base.OnTurnStart();
			//调整位置
			BattleSystem.TimeBar.MoveAbsoluteTimePosition(MyPointer, 4);
			Debug.Log(MyPointer.PositionX);
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
		
		public override void IsHit(int damage, AttackType attackType)
		{
			//CharacterImage.sprite = CharacterAtlas.GetSprite("OnHit");
			//GetComponent<EnemyAnimator>().HitText(damage.ToString());
			base.IsHit(damage, attackType);
			//_enemyAnimator.IsHit();

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
			BattleSystem.Enemies.Remove(this);
			Destroy(this);
		}

		public void AddBuff(string buffName, int stack)
		{
			GetComponent<BuffManager>().AddBuff(buffName, stack);
		}

		public bool IsOnDangerArea()
		{
			return true;
		}
		
		public void MoveInTime(int i)
		{
			MyPointer.Move(i);
		}
	}
}
