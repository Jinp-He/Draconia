
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.Game.Buff;
using Draconia.System;
using Draconia.UI;
using Draconia.ViewController.Event;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Draconia.ViewController
{
	public class Player : SerializedComponent, ICanRegisterEvent, ICanGetSystem
	{
		public PlayerViewController PlayerViewController;
		public PlayerInfo PlayerInfo;
		public List<CardInfo> Cards;
		public List<CardVC> Deck;
		public List<CardVC> Hands;
		public List<CardVC> Bin;
		protected float _armorModifier;
		protected float _magicResistModifier;
		protected float _recoverModifier;
		protected int _backNumModifier;
		protected int _drawCardModifier;
		public Action NextTurn;
		public string PlayerName;
		public string Alias;

		public int Mastery;


		protected bool _isFirstTimeDangerArea;
		protected bool _isInPose;
		protected string _prevPoseId;

		protected BattleSystem BattleSystem => this.GetSystem<BattleSystem>();
		

		public int BackNum => PlayerInfo.BackNum + _backNumModifier;
		public int CardDrawNum => PlayerInfo.DrawCardNum + _drawCardModifier;
		public int Position => PlayerViewController.transform.GetSiblingIndex();

		public Player()
		{
			
		}



		public virtual void Init(PlayerInfo playerInfo)
		{
			Alias = playerInfo.Alias;
			Cards = new List<CardInfo>();
			
			Deck = new List<CardVC>();
			Hands = new List<CardVC>();
			Bin = new List<CardVC>();

			PlayerInfo = playerInfo;
			
			Cards.AddRange(PlayerInfo.InitialCards_Ref);
			PlayerName = PlayerInfo.Name;

			this.RegisterEvent<PlayerTurnStartEvent>((e) =>
			{
				NextTurn?.Invoke();
				NextTurn = () => { };
			});
		}
		


		
		public static Player GetPlayer(PlayerInfo playerInfo)
		{
			switch (playerInfo.Alias)
			{
				case "Zhouzhou":
					return new Zhouzhou();
				case "Timbuktu":
					return new Timbuktu();
				default:
					Debug.LogErrorFormat("没有找到对应角色：{0}",playerInfo.Alias);
					return new Zhouzhou();
			}
		}
		


		public void Die()
		{
			PlayerViewController.HpBar.SetHp(0);
			PlayerViewController.BattleSystem.PlayerDie(PlayerViewController);

		}

		public virtual void OnTurnStart()
		{
			_isFirstTimeDangerArea = true;
		}


		/// <summary>
		/// 进入危险区域自动触发
		/// </summary>
		public void EnterDangerArea()
		{

			if (_isFirstTimeDangerArea)
			{
				PlayerViewController.BattleSystem.DrawCard(PlayerViewController, 1);
			}

			PlayerViewController.IsHit(PlayerViewController.DangerAreaDamageNum, AttackType.Physical);
			_isFirstTimeDangerArea = false;
		}


		public void Move(PlayerViewController playerViewController)
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(playerViewController.transform.DOLocalMoveX(PlayerViewController.transform.localPosition.x, 1f))
				.Join(PlayerViewController.transform.DOLocalMoveX(playerViewController.transform.localPosition.x, 1f))
				.OnComplete(() =>
				{
					int tempPos = playerViewController.transform.GetSiblingIndex();
					playerViewController.transform.SetSiblingIndex(Position);
					PlayerViewController.transform.SetSiblingIndex(tempPos);
				});
		}

		public int Distance(PlayerViewController playerViewController)
		{
			return Math.Abs(playerViewController.Player.Position - Position);
		}

		public bool ValidCard(CardVC cardVc)
		{
			if (PlayerViewController.BattleSystem.TimeBar.IsValidMove(PlayerViewController.MyPointer, cardVc.Cost))
				return true;
			return false;
		}

		/// <summary>
		/// 时间轴上移动相应的数值
		/// </summary>
		public void PayCost(int cost)
		{
			PlayerViewController.MyPointer.Move(cost);
		}

		public void OnTurnEnd()
		{
			PlayerViewController.PlayerAnimator.EndChosen();
			PlayerViewController.BattleSystem.Hands.OnEndTurn(PlayerViewController);
		}

		public void AddBuff(string buffName, int stack)
		{
			PlayerViewController.GetComponent<BuffManager>().AddBuff(buffName, stack);
		}

		public void MoveInTime(int i)
		{
			PlayerViewController.MyPointer.Move(i);
		}

		public void Charge(UnityAction<int> chargeAction)
		{
			PlayerViewController.BuffManager.AddBuff("蓄力",0);
			PlayerViewController.BuffManager.GetBuff("蓄力").OnEnd += chargeAction;
		}

		public virtual void InvokePassive()
		{
			
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}

