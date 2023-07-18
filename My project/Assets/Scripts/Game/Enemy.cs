using cfg;
using Draconia.Controller;
using Draconia.MyComponent;
using Draconia.UI;
using UnityEngine;
using QFramework;

namespace Draconia.ViewController
{
	public partial class Enemy : MyViewController
	{
		public EnemyInfo EnemyInfo;
		public Pointer MyPointer;
		public void Init(EnemyInfo enemyInfo)
		{
			EnemyInfo = enemyInfo;
			// EnemyImage.sprite = CharacterAtlas.GetSprite("Idle");
			// CharacterImage.SetNativeSize();
			// CharacterName = playerInfo.Name;
			HPBar.Init(EnemyInfo.InitialHP,EnemyInfo.InitialHP);
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddEnemy(this);
		}
        
		public void Action()
		{
            BattleSystem.EnemyTurnStart(this);
		}
	}
}
