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
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
	public partial class Enemy : MyViewController, ICharacter,IPointerEnterHandler, IPointerExitHandler
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
	
		public int Position 
		{
			get
			{
				List<Enemy> enemies = this.GetSystem<BattleSystem>().Enemies;
				int i = enemies.FindIndex(a => a = this);
				
				return i;
			}
		}

		public EnemyStrategy EnemyStrategy => _enemyStrategy;


		public Pointer MyPointer;
		//public SpriteAtlas EnemyAtlas;
		public EnemyAnimator _enemyAnimator;
		private EnemyStrategy _enemyStrategy;
		private int CurrHP;
		public EnemyAnimation EnemyAnimation;


		public void Init(EnemyInfo enemyInfo)
		{
			EnemyInfo = enemyInfo;
			_enemyStrategy = EnemyStrategy.GetEnemyStrategy(this);
			EnemyBar.Init(enemyInfo);
			EnemyAnimation = GetComponent<EnemyAnimation>();
			EnemyAnimation.Init(this);
			//EnemyImage.sprite = EnemyAtlas.GetSprite("dog01_Idle");
			EnemyImage.SetNativeSize();
			//HPBar.Init(EnemyInfo.InitialHP,EnemyInfo.InitialHP);
			CurrHP = EnemyInfo.InitialHP;
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddEnemy(this);

			_enemyAnimator.Init(this);
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
		
		public void IsHit(int damage)
		{
			//CharacterImage.sprite = CharacterAtlas.GetSprite("OnHit");
			GetComponent<EnemyAnimator>().HitText(damage.ToString());
			CurrHP -= damage;
			if (CurrHP <= 0)
			{
				Die();
			}
			EnemyBar.HPBar.GetComponent<HPBar>().SetHp(CurrHP);
			_enemyAnimator.IsHitAnimation();

		}

		public void Miss()
		{
			GetComponent<EnemyAnimator>().HitText("Miss");
		}
		public void Move(int position)
		{
			List<Enemy> enemies = BattleSystem.Enemies;
			int pos = enemies.FindIndex(a => a = this);
			int finalPos = pos + position;
            
			//TODO best practice of this
			if (finalPos < 0)
			{
				finalPos = 0;
			}

			if (finalPos >= enemies.Count)
			{
				finalPos = enemies.Count;
			}

			GetComponent<EnemyAnimator>().Move(enemies[finalPos]);
			(enemies[pos], enemies[finalPos]) = (enemies[finalPos], enemies[pos]);

		}
		public void Move(Enemy enemy)
		{
			List<Enemy> enemies = BattleSystem.Enemies;
			int pos = enemies.FindIndex(a => a = this);
			int finalPos = enemies.FindIndex(a => a = enemy);
            
			//TODO best practice of this
			if (finalPos < 0)
			{
				finalPos = 0;
			}

			if (finalPos >= enemies.Count)
			{
				finalPos = enemies.Count;
			}

			GetComponent<EnemyAnimator>().Move(enemies[finalPos]);
			(enemies[pos], enemies[finalPos]) = (enemies[finalPos], enemies[pos]);

		}


		public void Die()
		{
			BattleSystem.Enemies.Remove(this);
			Destroy(this);
		}

		public void AddBuff(string buffName, int stack)
		{
			GetComponent<BuffManager>().AddBuff(buffName, stack);
		}
	}
}
