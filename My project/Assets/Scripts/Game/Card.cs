using System.Collections.Generic;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.UI;
using UnityEngine;
using QFramework;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
	public enum CardState
	{
		Idle, Listen
	}
	
	public partial class Card : MyViewController, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		private CardState _cardState;
		private Hands _hands;
		public CardInfo _cardInfo;
		private Character CardPlayer;
		private UIBattlePanel UIBattlePanel;
		/// <summary>
		/// IF not in player state, unactivate, but can reveiw
		/// IF in player state, cards that can be played will be light
		/// 
		/// </summary>
		void Start()
		{
			_cardState = CardState.Listen;
			_hands = UIKit.GetPanel<UIBattlePanel>().Hands;
			UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
		}

		public void Init(CardInfo cardInfo, Character character)
		{
			_cardInfo = cardInfo;
			CardName.text = _cardInfo.Name;
			CardDesc.text = _cardInfo.Desc;
			CardCost.text = _cardInfo.Cost.ToString();
			CardType.text = _cardInfo.SkillTargetType.ToString();
			CardPlayer = character;
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			if(_cardState == CardState.Listen)
				_hands.Choose(this);
		}
		
		public void OnPointerExit(PointerEventData eventData)
		{
			if(_cardState == CardState.Listen)
				UnHover();
		}

		private List<ICharacter> Target;
		public void OnDrag(PointerEventData eventData)
		{
			if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player)
			{
				return;
			}
			if (_cardInfo.Cost > BattleSystem.Energy)
			{
				return;
			}
			//Debug.LogFormat("{0},{1}",Input.mousePosition.x,Input.mousePosition.y);
			
			UnHover();
			
			Target = new List<ICharacter>();
			switch (_cardInfo.SkillTargetType)
			{
				case SkillTarget.Self:
					CardPlayer.Chosen();
					Target.Add(CardPlayer);
					break;
				case SkillTarget.SingleEnemy:
					if (UIKit.GetPanel<UIBattlePanel>().ChosenEnemy != null)
					{
						Target.Add(UIKit.GetPanel<UIBattlePanel>().ChosenEnemy);
					}
					break;
				//TODO For other Condition;
			}
			
			// if(_cardState == CardState.Listen)
			// 	Play();
		}

		private int index;
		public void Hover()
		{
			
			transform.localScale = new Vector3(1.4f, 1.4f, 1.4f); 
			ChosenEffect.gameObject.SetActive(true);
			index = transform.GetSiblingIndex();
			transform.SetAsLastSibling();
		}

		public void UnHover()
		{
			transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
			ChosenEffect.gameObject.SetActive(false);
			transform.SetSiblingIndex(index);
		}

		//TODO For Example Use only
		public void Play()
		{
		
		}

		private Vector2 InitialPos;
		private Quaternion _rotation;
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player)
			{
				return;
			}
			if (_cardInfo.Cost > BattleSystem.Energy)
			{
				return;
			}
			InitialPos = transform.GetComponent<RectTransform>().anchoredPosition;
			_rotation = transform.rotation;
			transform.eulerAngles = new Vector3(0, 0, 0);
			UIKit.GetPanel<UIBattlePanel>().Bezier.Activate();
			UIKit.GetPanel<UIBattlePanel>().Bezier.GetComponent<RectTransform>().position =
				transform.GetComponent<RectTransform>().position;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player)
			{
				return;
			}
			if (_cardInfo.Cost > BattleSystem.Energy)
			{
				return;
			}
			UIKit.GetPanel<UIBattlePanel>().Bezier.Deactivate();
			if (InThePlayerArea() && IsRightTarget())
			{
				Play();
				Destroy(this.gameObject);
				BattleSystem.Energy.Value -= _cardInfo.Cost;
				BattleSystem.BattleState = BattleState.Enemy;
				BattleSystem.Continue();
				_hands.Refresh();
				
			}
			else
			{
				transform.GetComponent<RectTransform>().anchoredPosition = InitialPos;
				transform.SetSiblingIndex(index);
				transform.rotation = _rotation;
			}
			UIKit.GetPanel<UIBattlePanel>().UnchosenAll();
			
		}

		private bool InThePlayerArea()
		{
			return Input.mousePosition.y > 200;
		}

		/// <summary>
		/// Choose the right target to activate;
		/// </summary>
		/// <returns></returns>
		private bool IsRightTarget()
		{
			return true;
		}
	}
}
