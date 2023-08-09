using System.Collections.Generic;
using cfg;
using Draconia.Controller;
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
		public int Energy;
		public int Position 
		{
			get
			{
				return this.GetSystem<BattleSystem>().Enemies.FindIndex(a => a = this);

			}
		}

		public EnemyStrategy EnemyStrategy
		{
			get { return _enemyStrategy; }
		}

		public BindableProperty<int> EnergyCount;
		public Pointer MyPointer;
		public SpriteAtlas EnemyAtlas;
		public EnemyAnimator _enemyAnimator;
		public Image EnergyBulbPrefab;
		private readonly List<Image> _energyBulbs = new List<Image>();
		private EnemyStrategy _enemyStrategy;

		public Enemy()
		{
			
		}

		public void Init(EnemyInfo enemyInfo)
		{
			_enemyStrategy = new EnemyStrategy(this);
			EnemyInfo = enemyInfo;
			Debug.Log(enemyInfo.Speed);
			EnemyImage.sprite = EnemyAtlas.GetSprite("dog01_Idle");
			EnemyImage.SetNativeSize();
			HPBar.Init(EnemyInfo.InitialHP,EnemyInfo.InitialHP);
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddEnemy(this);
			EnergyCount = new BindableProperty<int>
			{
				Value = 0
			};
			for (int i = 0; i < enemyInfo.MaxEnergy; i++)
			{
				Image energyBulb = Instantiate(EnergyBulbPrefab, EnergyBar.transform);
				_energyBulbs.Add(energyBulb);
				energyBulb.gameObject.SetActive(true);
			}
			EnergyCount.Register(e =>
			{
				foreach (var bulb in _energyBulbs)
				{
					bulb.color = Color.white;
					
				}
				foreach (var bulb in _energyBulbs.GetRange(0,e))
				{
					bulb.color = Color.blue;
				}
				
			});
			_enemyAnimator.Init(this);
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

		public void Chosen()
		{
			ChooseBracelet.gameObject.SetActive(true);
		}

		public void Unchosen()
		{
			ChooseBracelet.gameObject.SetActive(false);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Chosen();
			UIKit.GetPanel<UIBattlePanel>().ChosenEnemy = this;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Unchosen();
			UIKit.GetPanel<UIBattlePanel>().ChosenEnemy = null;
		}
	}
}
