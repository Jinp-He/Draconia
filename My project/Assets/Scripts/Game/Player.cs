using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using cfg;
using Draconia.Controller;
using Draconia.UI;
using UnityEngine;
using QFramework;

using UnityEngine.EventSystems;
using UnityEngine.U2D;
using cfg;
using DG.Tweening;
using Draconia.Game.Buff;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.ViewController;
using Draconia.ViewController.Event;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;
using Sequence = DG.Tweening.Sequence;

namespace Draconia.Controller
{
	public class Character : MyViewController
	{
		public HPBar HpBar;
		public int CurrHP;
		private int _armor;
		public SpriteAtlas CharacterAtlas;
		public Image CharacterImage;
		public CharacterAnimator CharacterAnimator;
		public RectTransform DamageTextField;
		public RectTransform NotificationTextField;
		public int Armor
		{
			get => _armor;
			set => _armor = value;
		}

		public virtual void Init() 
		{
            
		}

		public virtual bool IsOnDangerArea()
		{
			return true;
		}


		public virtual void MoveInTime(int i)
		{
			
		}

		public virtual void IsHit(int damage,AttackType attackType)
		{
			if (Armor >= damage)
			{
				Armor -= damage;
				HpBar.SetArmor(Armor);
				CharacterAnimator.IsHit(0, attackType, damage);
			}
			else
			{
				int tempArmor = Armor;
				damage -= Armor;
				HpBar.SetArmor(Armor);
				CurrHP -= damage;
				HpBar.SetHp(CurrHP);
				CharacterAnimator.IsHit(damage, attackType, tempArmor);
			}
			
			if (CurrHP <= 0)
			{
				Die();
			}
			
		}
		
		public void Defense(int value)
		{
			Armor += value; 
			HpBar.SetArmor(value);
		}
		
		public virtual void OnTurnStart()
		{
			Armor = 0;
			HpBar.SetArmor(Armor);
		}

		public virtual void Die()
		{
			
		}
		
		public void Miss()
		{
			StartCoroutine(GetComponent<CharacterAnimator>().Miss());
		}

	}
}

namespace Draconia.ViewController
{
	public partial class Player : Character, ICanRegisterEvent
	{
		//public SpriteAtlas CharacterAtlas;
		public Pointer MyPointer;
		public PlayerInfo PlayerInfo;
		public List<CardInfo> Cards;


		public List<Card> Deck;
		public List<Card> Hands;
		public List<Card> Bin;

		

		private float _armorModifier;
		private float _magicResistModifier;
		private float _recoverModifier;
		private int _backNumModifier;
		public int BackNum => PlayerInfo.BackNum + _backNumModifier;
		private int _drawCardModifier;
		public int DrawCard => PlayerInfo.DrawCardNum + _drawCardModifier;
		private Action NextTurn;

		public Sprite CardImageSprite;

		
		
		public int Position => transform.GetSiblingIndex();
		private string PlayerName;
		public PlayerAnimator PlayerAnimator;
		public void Init(PlayerInfo playerInfo)
		{
			
			PlayerInfo = playerInfo;
			//Debug.Log(playerInfo.Name);
			CharacterAtlas = ResLoadSystem.LoadSync<SpriteAtlas>(playerInfo.Alias);
			CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
			CharacterImage.SetNativeSize();
			CardImageSprite = ResLoadSystem.LoadSync<Sprite>("CardImage_" + playerInfo.Alias);
			PlayerName = playerInfo.Name;
			CurrHP = playerInfo.InitialHP;
			HpBar.Init(playerInfo.InitialHP,playerInfo.InitialHP);
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddCharacter(this);
			Cards = new List<CardInfo>();
			//初始牌库
			Cards.AddRange(PlayerInfo.InitialCards_Ref);
			PlayerAnimator.Init(this);
			this.RegisterEvent<PlayerTurnStartEvent>((e) =>
			{
				NextTurn?.Invoke();
				NextTurn = new Action(() => { });
			});
			BattleStart();
		}



		public void BattleStart()
		{
			foreach (var cardInfo in PlayerInfo.InitialCards_Ref)
			{
				var cardPrefab = ResLoadSystem.LoadSync<GameObject>("CardPrefab").GetComponent<Card>();
				Card card = Instantiate(cardPrefab, CardDeck);
				card.Init(cardInfo, this);
				Deck.Add(card);
			}
		}
		

		public override void Die()
		{
			HpBar.SetHp(0);
			BattleSystem.PlayerDie(this);
			
		}

		public void Chosen()
		{
			ChooseBracelet.gameObject.SetActive(true);
		} 

		public void Unchosen()
		{
			ChooseBracelet.gameObject.SetActive(false);
		}

		/// <summary>
		/// 进入姿态
		/// </summary>
		public void EnterPose()
		{
			
		}
		
		public void Move(Player player)
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(player.transform.DOLocalMoveX(transform.localPosition.x, 1f))
				.Join(transform.DOLocalMoveX(player.transform.localPosition.x, 1f))
				.OnComplete(() =>
				{
					int tempPos = player.transform.GetSiblingIndex();
					player.transform.SetSiblingIndex(Position);
					transform.SetSiblingIndex(tempPos);
				});
		}
		
		

		public int Distance(Player player)
		{
			List<Player> characters = BattleSystem.Characters;
			return Math.Abs(player.Position - Position);
		}
		

		//判断是否可以释放该卡
		public bool ValidCard(Card card)
		{
			if(BattleSystem.TimeBar.IsValidMove(MyPointer, card._cardInfo.Cost))
				return true;
			return false;
		}

		/// <summary>
		/// 时间轴上移动相应的数值
		/// </summary>
		public void PayCost(int cost)
		{
			MyPointer.Move(cost);
		}
		

		public override void OnTurnStart()
		{
			base.OnTurnStart();
			StartCoroutine(PlayerAnimator.SendNotificationText("回合开始了"));
			UIKit.GetPanel<UIBattlePanel>().TimeBar.MoveAbsoluteTimePosition(MyPointer, BackNum);
			PlayerAnimator.IsChosen();
		}
		public void OnTurnEnd()
		{
			PlayerAnimator.EndChosen();
			BattleSystem.Hands.OnEndTurn(this);
		}

		public void AddBuff(string buffName, int stack)
		{
			GetComponent<BuffManager>().AddBuff(buffName, stack);
		}

		public bool IsOnDangerArea()
		{
			return true;
		}

		public void MoveInTime(int i)
		{
			MyPointer.Move(i);
		}
	}
}
