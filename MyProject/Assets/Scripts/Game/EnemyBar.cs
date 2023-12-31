using System.Collections.Generic;
using cfg;
using Draconia.UI;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace Draconia.ViewController
{
	public partial class EnemyBar : QFramework.ViewController
	{
		public Image EnergyBulbPrefab1,EnergyBulbPrefab2;
		public readonly List<Image> _energyBulbs = new List<Image>();
		void Start()
		{
			// Code Here
		}

		public void Init(EnemyInfo enemyInfo)
		{
			for (int i = 0; i < enemyInfo.MaxEnergy; i++)
			{
				Image energyBulb;
				if (i % 2 == 0)
					energyBulb = Instantiate(EnergyBulbPrefab2, EnergyBar.transform);
				else
				{
					energyBulb = Instantiate(EnergyBulbPrefab1, EnergyBar.transform);
				}
				_energyBulbs.Add(energyBulb);
				energyBulb.gameObject.SetActive(true);

			}
		}
	}
}
