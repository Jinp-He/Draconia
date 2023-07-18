using System.Collections.Generic;
using Draconia.Controller;
using UnityEngine;
using QFramework;

namespace Draconia.ViewController
{
	public partial class TimeBar : QFramework.ViewController
	{
		public static int Length = 500;

		public Pointer PointerPrefab;
		public List<Character> PlayerList;
		public List<Enemy> EnemyList;
		public List<Pointer> Pointers;
		public bool IsInit, IsStart;

		public void Init()
		{
			IsInit = true;
			PlayerList = new List<Character>();
			EnemyList = new List<Enemy>();
			Pointers = new List<Pointer>();
		}


		public Pointer AddCharacter(Character character)
		{
			Pointer pointer = Instantiate(PointerPrefab,PlayerSlot.transform);
			pointer.LocalPosition(0, 0, 0);
			pointer.Init(character);
			Pointers.Add(pointer);
			
			return pointer;
		}

		public Pointer AddEnemy(Enemy enemy)
		{
			Pointer pointer = Instantiate(PointerPrefab,EnemySlot.transform);
			pointer.LocalPosition(0, 0, 0);
			pointer.Init(enemy);
			Pointers.Add(pointer);
			return pointer;
		}
        
		public void RemoveCharacter(Character character)
		{}
        
		public void RemoveEnemy(Enemy enemy)
		{}

		public void FixedUpdate()
		{
			if (!IsInit || !IsStart)
			{
				return;
			}
			foreach (var player in PlayerList)
			{
                
			}
			foreach (var enemy in EnemyList)
			{
                
			}
		}

		public void Stop()
		{
			foreach (var pointer in Pointers)
			{
				pointer.IsStop = true;
			}
		}
		
		public void Continue()
		{
			foreach (var pointer in Pointers)
			{
				pointer.IsStop = false;
			}
		}
	}
}
