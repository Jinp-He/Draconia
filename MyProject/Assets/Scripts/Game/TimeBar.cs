using System;
using System.Collections.Generic;
using System.Linq;
using Draconia.Controller;
using Draconia.ViewController.Event;
using UnityEngine;
using QFramework;

namespace Draconia.ViewController
{
	/// <summary>
	/// 时间轴，控制角色的行动顺序
	/// </summary>
	public partial class TimeBar : QFramework.ViewController, ICanRegisterEvent, ICanSendEvent
	{
		public static int Length = 500;

		public Pointer PointerPrefab;
		public List<Pointer> Pointers;
		public List<Pointer> Players;
		public List<Pointer> Enemies;
		public bool IsInit, IsStart;

		public int DangerAreaPlayer, DangerAreaEnemy;


		public static float ToBarPosition(int k)
		{
			return k * TimeBarScale;
		}
		public void Init()
		{
			// Pointers = new List<Pointer>();
			// Players = new List<Pointer>();
			// Enemies = new List<Pointer>();

			DangerAreaPlayer = 5;
			DangerAreaEnemy = 5;
		}


		public Pointer AddCharacter(PlayerViewController playerViewController)
		{
			Pointer pointer = Instantiate(PointerPrefab,PlayerSlot.transform);
			pointer.LocalPosition(0, 0, 0);
			pointer.Init(playerViewController,this);
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

		public void RemoveCharacter(PlayerViewController playerViewController)
		{
			Players.Remove(playerViewController.MyPointer);
			Pointers.Remove(playerViewController.MyPointer);
			playerViewController.MyPointer.gameObject.DestroySelf();
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
		private float TransferPosition(float timePosition, bool isPlayer)
		{
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
			//Debug.LogFormat("h is {0} TimeBarS擦了 is {1}",h,TimeBarScale);
			int min = (int)h / (int)TimeBarScale;
			//Debug.LogFormat("获取的位置为 {0} {1}",min,min+1);
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
			if(IsInDangerArea(pointer))
				pointer.CharacterViewController.TriggerDanger.Invoke();
		}


		public void MoveAbsoluteTimePosition(Pointer pointer, float pos, bool isInit = false)
		{
			pointer.PositionX = TransferPosition(pos, pointer._isPlayer);
			if (isInit) return;
			pointer.Regulate();
			if(IsInDangerArea(pointer))
				pointer.CharacterViewController.TriggerDanger.Invoke();
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


		public bool IsInDangerArea(Pointer pointer)
		{
			QFramework.Tuple<int, int> range = TransferLocalPosition(pointer);
			//Debug.LogFormat("位置为{0} {1}", range.Item1, range.Item2);
			//Debug.LogFormat("Dangerarea {0}", DangerAreaPlayer);
			if (pointer._isPlayer)
			{
				if (range.Item1 > DangerAreaPlayer)
				{
					this.SendEvent(new EnterDangerAreaEvent() { CharacterViewController = pointer.CharacterViewController });
					return true;
				}
				else if (range.Item1 == DangerAreaPlayer && range.Item1 != range.Item2)
				{
					this.SendEvent(new EnterDangerAreaEvent() { CharacterViewController = pointer.CharacterViewController });
					return true;
				}
					
			}
			else
			{
				if (range.Item1 > DangerAreaEnemy)
				{
					this.SendEvent(new EnterDangerAreaEvent() { CharacterViewController = pointer.CharacterViewController });
					return true;
				}
				else if (range.Item1 == DangerAreaEnemy && range.Item1 != range.Item2)
				{
					this.SendEvent(new EnterDangerAreaEvent() { CharacterViewController = pointer.CharacterViewController });
					return true;
				}
			}

			
			return false;
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

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}
