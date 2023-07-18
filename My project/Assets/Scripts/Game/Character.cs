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
	public partial class Character : QFramework.ViewController, ICharacter, IPointerEnterHandler, IPointerExitHandler
	{
		public SpriteAtlas CharacterAtlas;
		public Pointer MyPointer;
		public PlayerInfo PlayerInfo;
		public List<CardInfo> Cards;
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
			
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			
		}
	}
}
