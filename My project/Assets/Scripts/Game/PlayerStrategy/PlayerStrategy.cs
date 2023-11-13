
using System;
using System.Collections.Generic;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.Game.Buff;
using Draconia.UI;
using Draconia.ViewController.Event;
using QFramework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Draconia.ViewController
{
	public class PlayerStrategy : ICanRegisterEvent, ICanGetSystem
	{
		protected Player _player;
		public PlayerInfo PlayerInfo;
		public List<CardInfo> Cards;
		public List<Card> Deck;
		public List<Card> Hands;
		public List<Card> Bin;
		protected float _armorModifier;
		protected float _magicResistModifier;
		protected float _recoverModifier;
		protected int _backNumModifier;
		protected int _drawCardModifier;
		public Action NextTurn;
		public string PlayerName;


		protected bool _isFirstTimeDangerArea;
		protected bool _isInPose;
		protected string _prevPoseId;
		

		public int BackNum => PlayerInfo.BackNum + _backNumModifier;
		public int CardDrawNum => PlayerInfo.DrawCardNum + _drawCardModifier;
		public int Position => _player.transform.GetSiblingIndex();

		public virtual void Init(Player player)
		{
			Cards = new List<CardInfo>();
			Deck = new List<Card>();
			Hands = new List<Card>();
			Bin = new List<Card>();

			_player = player;
			PlayerInfo = player.PlayerInfo;
			
			Cards.AddRange(PlayerInfo.InitialCards_Ref);
			PlayerName = PlayerInfo.Name;

			this.RegisterEvent<PlayerTurnStartEvent>((e) =>
			{
				NextTurn?.Invoke();
				NextTurn = () => { };
			});
		}
		

		public static PlayerStrategy GetStrategy(string alias, Player player)
		{
			switch (alias)
			{
				case "Zhouzhou":
					return new Zhouzhou(player);
				case "Timbuktu":
					return new Timbuktu(player);
				default:
					Debug.LogErrorFormat("没有找到对应角色：{0}",alias);
					return new Zhouzhou(player);
			}
		}
		


		public void Die()
		{
			_player.HpBar.SetHp(0);
			_player.BattleSystem.PlayerDie(_player);

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
				_player.BattleSystem.DrawCard(_player, 1);
			}

			_player.IsHit(_player.DangerAreaDamageNum, AttackType.Physical);
			_isFirstTimeDangerArea = false;
		}

		/// <summary>
		/// 进入姿态
		/// </summary>
		public void EnterPose(string poseId)
		{
			if (_isInPose)
			{
				LeavePose();
			}

			_player.StartCoroutine(_player.PlayerAnimator.SendNotificationText("进入姿态"));
			switch (poseId)
			{
				case "飞鸟式":
					_player.BattleSystem.TimeBar.DangerAreaEnemy += 1;
					break;
				case "拿云式":

					break;

			}

			_player.StartCoroutine(_player.PlayerAnimator.SendNotificationText(poseId));
			_isInPose = true;
			_prevPoseId = poseId;
		}

		public void LeavePose()
		{
			_player.StartCoroutine(_player.PlayerAnimator.SendNotificationText("退出姿态"));
			switch (_prevPoseId)
			{
				case "飞鸟式":
					_player.BattleSystem.TimeBar.DangerAreaEnemy -= 1;
					break;
				case "拿云式":

					break;

			}

			_isInPose = false;
		}

		public void Move(Player player)
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(player.transform.DOLocalMoveX(_player.transform.localPosition.x, 1f))
				.Join(_player.transform.DOLocalMoveX(player.transform.localPosition.x, 1f))
				.OnComplete(() =>
				{
					int tempPos = player.transform.GetSiblingIndex();
					player.transform.SetSiblingIndex(Position);
					_player.transform.SetSiblingIndex(tempPos);
				});
		}

		public int Distance(Player player)
		{
			return Math.Abs(player.PlayerStrategy.Position - Position);
		}

		public bool ValidCard(Card card)
		{
			if (_player.BattleSystem.TimeBar.IsValidMove(_player.MyPointer, card.Cost))
				return true;
			return false;
		}

		/// <summary>
		/// 时间轴上移动相应的数值
		/// </summary>
		public void PayCost(int cost)
		{
			_player.MyPointer.Move(cost);
		}

		public void OnTurnEnd()
		{
			_player.PlayerAnimator.EndChosen();
			_player.BattleSystem.Hands.OnEndTurn(_player);
		}

		public void AddBuff(string buffName, int stack)
		{
			_player.GetComponent<BuffManager>().AddBuff(buffName, stack);
		}

		public void MoveInTime(int i)
		{
			_player.MyPointer.Move(i);
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Interface;
		}
	}
}

