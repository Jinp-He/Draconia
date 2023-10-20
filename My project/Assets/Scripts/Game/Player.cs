using System;
using System.Collections.Generic;
using cfg;
using Draconia.Controller;
using Draconia.UI;
using UnityEngine;
using QFramework;

using UnityEngine.EventSystems;
using UnityEngine.U2D;
using cfg;
using Draconia.Game.Buff;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.ViewController.Event;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.Controller
{
	public interface ICharacter
	{
		public virtual void Init()
		{
            
		}
	}
}

namespace Draconia.ViewController
{
	public partial class Player : MyViewController, ICanRegisterEvent, ICharacter, IPointerEnterHandler, IPointerExitHandler
	{
		public SpriteAtlas CharacterAtlas;
		public Pointer MyPointer;
		public PlayerInfo PlayerInfo;
		public List<CardInfo> Cards;


		public List<Card> Deck;
		public List<Card> Bin;

		

		private float _armorModifier;
		private float _magicResistModifier;
		private float _recoverModifier;
		private int _backNumModifier;
		public int BackNum => PlayerInfo.BackNum + _backNumModifier;
		private int _drawCardModifier;
		public int DrawCard => PlayerInfo.DrawCardNum + _drawCardModifier;
		private Action NextTurn;
		
		
		public int Position 
		{
			get
			{
				return BattleSystem.Characters.FindIndex(a => a = this);

			}
		}
		private string PlayerName;
		public PlayerAnimator PlayerAnimator;
		private int CurrHP;
		public void Init(PlayerInfo playerInfo)
		{
			
			PlayerInfo = playerInfo;
			Debug.Log(playerInfo.Name);
			CharacterAtlas = ResLoadSystem.LoadSync<SpriteAtlas>(playerInfo.Alias);
			CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
			CharacterImage.SetNativeSize();
			PlayerName = playerInfo.Name;
			CurrHP = playerInfo.InitialHP;
			HPBar.GetComponent<HPBar>().Init(playerInfo.InitialHP,playerInfo.InitialHP);
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
		
		private void Update()               
		{
			
		}

		private void RefreshCard()
		{
			Cards.AddRange(PlayerInfo.InitialCards_Ref);
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
			PlayerAnimator.IsHit();

		}

		public void Die()
		{
			BattleSystem.GameOver();
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			// Chosen();
			// UIKit.GetPanel<UIBattlePanel>().ChosenCharacter = this;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			// Unchosen();
			// UIKit.GetPanel<UIBattlePanel>().ChosenCharacter = null;
		}

		public void Chosen()
		{
			ChooseBracelet.gameObject.SetActive(true);
		} 

		public void Unchosen()
		{
			ChooseBracelet.gameObject.SetActive(false);
		}
		
		public void Move(int position)
		{
			List<Player> characters = BattleSystem.Characters;
			int pos = characters.FindIndex(a => a = this);
			int finalPos = pos + position;
            
			//TODO best practice of this
			if (finalPos < 0)
			{
				finalPos = 0;
			}

			if (finalPos >= characters.Count)
			{
				finalPos = characters.Count;
			}

			GetComponent<PlayerAnimator>().Move(characters[finalPos]);
			(characters[pos], characters[finalPos]) = (characters[finalPos], characters[pos]);

		}
		public void Move(Player player)
		{
			List<Player> characters = BattleSystem.Characters;
			int pos = characters.FindIndex(a => a = this);
			int finalPos = characters.FindIndex(a => a = player);
            
			//TODO best practice of this
			if (finalPos < 0)
			{
				finalPos = 0;
			}

			if (finalPos >= characters.Count)
			{
				finalPos = characters.Count;
			}

			GetComponent<PlayerAnimator>().Move(characters[finalPos]);
			(characters[pos], characters[finalPos]) = (characters[finalPos], characters[pos]);

		}
		
		public void Miss()
		{
			GetComponent<EnemyAnimator>().HitText("Miss");
		}

		public int Distance(Player player)
		{
			List<Player> characters = BattleSystem.Characters;
			return Math.Abs(characters.FindIndex(a => a = this)
			- characters.FindIndex(a => a = player));
		}

		public void Defense()
		{
			_magicResistModifier += 0.2f;
			_armorModifier += 0.2f;
			NextTurn += () =>
			{
				_magicResistModifier -= 0.2f;
				_armorModifier -= 0.2f;
			};
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
		

		public void Refresh()
		{
			MyPointer.Refresh();
		}


		public void OnTurnStart()
		{
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
	}
}
