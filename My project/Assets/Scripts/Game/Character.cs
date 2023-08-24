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
using Draconia.MyComponent;
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
	public partial class Character : MyViewController, ICharacter, IPointerEnterHandler, IPointerExitHandler
	{
		public SpriteAtlas CharacterAtlas;
		public Pointer MyPointer;
		public PlayerInfo PlayerInfo;
		public List<CardInfo> Cards;
		
		public int Position 
		{
			get
			{
				return BattleSystem.Characters.FindIndex(a => a = this);

			}
		}
		private string CharacterName;
		public CharacterAnimator _characterAnimator;
		private int CurrHP;
		public void Init(PlayerInfo playerInfo)
		{
			
			PlayerInfo = playerInfo;
			CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
			CharacterImage.SetNativeSize();
			CharacterName = playerInfo.Name;
			CurrHP = playerInfo.InitialHP;
			HPBar.GetComponent<HPBar>().Init(playerInfo.InitialHP,playerInfo.InitialHP);
			MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddCharacter(this);
			Cards = new List<CardInfo>();
			Cards.AddRange(PlayerInfo.InitialCards_Ref);
			_characterAnimator.Init(this);
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
			_characterAnimator.IsHit();

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
			List<Character> characters = BattleSystem.Characters;
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

			GetComponent<CharacterAnimator>().Move(characters[finalPos]);
			(characters[pos], characters[finalPos]) = (characters[finalPos], characters[pos]);

		}
		public void Move(Character character)
		{
			List<Character> characters = BattleSystem.Characters;
			int pos = characters.FindIndex(a => a = this);
			int finalPos = characters.FindIndex(a => a = character);
            
			//TODO best practice of this
			if (finalPos < 0)
			{
				finalPos = 0;
			}

			if (finalPos >= characters.Count)
			{
				finalPos = characters.Count;
			}

			GetComponent<CharacterAnimator>().Move(characters[finalPos]);
			(characters[pos], characters[finalPos]) = (characters[finalPos], characters[pos]);

		}

		public int Distance(Character character)
		{
			List<Character> characters = BattleSystem.Characters;
			return Math.Abs(characters.FindIndex(a => a = this)
			- characters.FindIndex(a => a = character));
		}
	}
}
