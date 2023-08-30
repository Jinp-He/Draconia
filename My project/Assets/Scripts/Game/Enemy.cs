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
				foreach (var bulb in _energyBulbs)
				{
					bulb.color = Color.white;

				}

				foreach (var bulb in _energyBulbs.GetRange(0, value))
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


		public BindableProperty<int> EnergyCount;
		public Pointer MyPointer;
		public SpriteAtlas EnemyAtlas;
		public EnemyAnimator _enemyAnimator;
		public Image EnergyBulbPrefab;
		private readonly List<Image> _energyBulbs = new List<Image>();
		private EnemyStrategy _enemyStrategy;
		private int CurrHP;
		public EnemyAnimation EnemyAnimation;


		public void Init(EnemyInfo enemyInfo)
		{
			EnemyInfo = enemyInfo;
			_enemyStrategy = EnemyStrategy.GetEnemyStrategy(this);
			EnemyAnimation = GetComponent<EnemyAnimation>();
			EnemyAnimation.Init(this);
			EnemyImage.sprite = EnemyAtlas.GetSprite("dog01_Idle");
			EnemyImage.SetNativeSize();
			HPBar.Init(EnemyInfo.InitialHP,EnemyInfo.InitialHP);
			CurrHP = EnemyInfo.InitialHP;
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddEnemy(this);
			// EnergyCount = new BindableProperty<int>
			// {
			// 	Value = 0
			// };
			for (int i = 0; i < enemyInfo.MaxEnergy; i++)
			{
				Image energyBulb = Instantiate(EnergyBulbPrefab, EnergyBar.transform);
				_energyBulbs.Add(energyBulb);
				energyBulb.gameObject.SetActive(true);
			}
			// EnergyCount.Register(e =>
			// {
			// 	foreach (var bulb in _energyBulbs)
			// 	{
			// 		bulb.color = Color.white;
			// 		
			// 	}
			// 	foreach (var bulb in _energyBulbs.GetRange(0,e))
			// 	{
			// 		bulb.color = Color.blue;
			// 	}
			// 	
			// });
			_enemyAnimator.Init(this);
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
			CurrHP -= damage;
			if (CurrHP <= 0)
			{
				Die();
			}
			HPBar.GetComponent<HPBar>().SetHp(CurrHP);
			_enemyAnimator.IsHit();

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
