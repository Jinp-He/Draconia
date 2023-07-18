using cfg;
using DG.Tweening;
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
		private CardInfo _cardInfo;
		void Start()
		{
			_cardState = CardState.Listen;
			_hands = UIKit.GetPanel<UIBattlePanel>().Hands;
		}

		public void Init(CardInfo cardInfo)
		{
			_cardInfo = cardInfo;
			CardName.text = _cardInfo.Name;
			CardDesc.text = _cardInfo.Desc;
			CardCost.text = _cardInfo.Cost.ToString();
			CardType.text = _cardInfo.SkillTargetType.ToString();
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
		
		public void OnDrag(PointerEventData eventData)
		{
			Vector2 pos;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_hands.GetComponent<RectTransform>(),
				    eventData.position, eventData.pressEventCamera, out pos))
			{
				transform.GetComponent<RectTransform>().anchoredPosition = pos;
			}
			if(_cardState == CardState.Listen)
				Play();
		}

		private int index;
		public void Hover()
		{
			
			transform.localScale = new Vector3(1.4f, 1.4f, 1.4f); 
			GlowEffect.gameObject.SetActive(true);
			index = transform.GetSiblingIndex();
			transform.SetAsLastSibling();
		}

		public void UnHover()
		{
			transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
			GlowEffect.gameObject.SetActive(false);
			transform.SetSiblingIndex(index);
		}

		public void Play()
		{
			
		}

		private Vector2 InitialPos;
		private Quaternion _rotation;
		public void OnBeginDrag(PointerEventData eventData)
		{
			InitialPos = transform.GetComponent<RectTransform>().anchoredPosition;
			_rotation = transform.rotation;
			transform.eulerAngles = new Vector3(0, 0, 0);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (true)
			{
				Destroy(this.gameObject);
				BattleSystem.Continue();
			}
			else
			{
				transform.GetComponent<RectTransform>().anchoredPosition = InitialPos;
				transform.SetSiblingIndex(index);
				transform.rotation = _rotation;
			}
			
		}
	}
}
