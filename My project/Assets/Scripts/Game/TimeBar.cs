using System;
using System.Collections.Generic;
using System.Linq;
using Draconia.Controller;
using UnityEngine;
using QFramework;

namespace Draconia.ViewController
{
	/// <summary>
	/// 时间轴，控制角色的行动顺序
	/// </summary>
	public partial class TimeBar : QFramework.ViewController
	{
		public static int Length = 500;

		public Pointer PointerPrefab;
		public List<Pointer> Pointers;
		public List<Pointer> Players;
		public List<Pointer> Enemies;
		public bool IsInit, IsStart;


		public static float ToBarPosition(int k)
		{
			return k * TimeBarScale;
		}
		public void Init()
		{
			IsInit = true;
			Pointers = new List<Pointer>();
			Players = new List<Pointer>();
			Enemies = new List<Pointer>();
		}


		public Pointer AddCharacter(Player player)
		{
			Pointer pointer = Instantiate(PointerPrefab,PlayerSlot.transform);
			pointer.LocalPosition(0, 0, 0);
			pointer.Init(player,this);
			Pointers.Add(pointer);
			Players.Add(pointer);
			
			return pointer;
		}

		public Pointer AddEnemy(Enemy enemy)
		{
			Pointer pointer = Instantiate(PointerPrefab,EnemySlot.transform);
			pointer.LocalPosition(0, 0, 0);
			pointer.Init(enemy,this);
			Pointers.Add(pointer);
			Enemies.Add(pointer);
			return pointer;
		}

		public void RemoveCharacter(Player player)
		{
			Players.Remove(player.MyPointer);
			Pointers.Remove(player.MyPointer);
			player.MyPointer.gameObject.DestroySelf();
		}

		public void RemoveEnemy(Enemy enemy)
		{
			Players.Remove(enemy.MyPointer);
			Pointers.Remove(enemy.MyPointer);
			enemy.MyPointer.gameObject.DestroySelf();
		}

		public int GetPosition(Pointer pointer)
		{
			//获得角色的位置
			return 0;
		}

		public void FixedUpdate()
		{
			//调整transform顺序使得处在时间轴位置前面的人始终在transform前面
			Transform[] list = PlayerSlot.GetComponentsInChildren<Transform>();
			List<Transform> t = list.ToList().OrderBy(t => t.localPosition.x).ToList();
			for (int i = 0; i < t.Count; i++)
			{
				t[i].SetSiblingIndex(i);	
				//Debug.Log(t[i].GetComponent<Pointer>().name + " " + t[i].localPosition.x);
			}
			
			Transform[] enemyList = EnemySlot.GetComponentsInChildren<Transform>();
			List<Transform> enemyT = enemyList.ToList().OrderBy(t => t.localPosition.x).ToList();
			for (int i = 0; i < enemyT.Count; i++)
			{
				enemyT[i].SetSiblingIndex(i);	
				//Debug.Log(enemyT[i].GetComponent<Pointer>().name + " " + enemyT[i].localPosition.x);
			}
			
		}

		/// <summary>
		/// 时间刻度
		/// </summary>
		public const float TimeBarScale = 74f;

		public const float TimeBarEdge = 7 * TimeBarScale;

		/// <summary>
		/// 将时间轴刻度转译成游戏的X位置
		/// </summary>
		/// <param name="timePosition"></param>
		/// <param name="isEnemy"></param>
		/// <returns></returns>
		public float TransferPosition(float timePosition, bool isPlayer)
		{
			//Debug.LogFormat("#DEBUG#{0}, {1}",timePosition,EnemyActionPoint.position.x - timePosition * TimeBarScale);
			if (isPlayer)
			{
				return -(timePosition) * TimeBarScale;
			}
			else
			{
				return (timePosition) * TimeBarScale;
			}  
		}

		public QFramework.Tuple<int, int> TransferLocalPosition(Pointer pointer)
		{
			//获得绝对位置
			float h = Math.Abs(pointer.PositionX);
			int min = (int)h / (int)TimeBarScale;
			if ((int)h % (int)TimeBarScale == 0)
			{
				return new QFramework.Tuple<int, int>(min, min);
			}
			return new QFramework.Tuple<int, int>(min, min + 1);
		}



		public void MoveRelativeTimePosition(Pointer pointer, float cost)
		{
			if (pointer._isPlayer)
			{
				pointer.PositionX -= cost * TimeBarScale;
			}
			else
			{
				pointer.PositionX += cost * TimeBarScale;
			}
			pointer.Regulate();
		}
		
		
		public void MoveAbsoluteTimePosition(Pointer pointer,float pos)
		{
			pointer.PositionX = TransferPosition(pos, pointer._isPlayer);
		}

		public void Move()
		{
			
		}

		public bool IsValidMove(Pointer pointer, int cost)
		{
			if (TransferLocalPosition(pointer).Item2 + cost > 8)
			{
				return false;
			}
			return true;
		}

	
		
		
		
		/// <summary>
		/// 暂停游戏
		/// </summary>
		public void Stop()
		{
			//Debug.LogError("Stop");
			foreach (var pointer in Pointers)
			{
				pointer.IsStop = true;
			}
		}
		
		/// <summary>
		/// 继续游戏，时间轴上所有继续移动
		/// </summary>
		public void Continue()
		{
			foreach (var pointer in Pointers)
			{
				pointer.IsStop = false;
			}
		}
	}
}
