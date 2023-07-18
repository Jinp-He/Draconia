using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
	public partial class HPBar : QFramework.ViewController, IPointerEnterHandler, IPointerExitHandler
	{

		private int _maxHp;
		private int _Hp;

		public void Init(int maxHp, int currHp)
		{
			_maxHp = maxHp;
			_Hp = currHp;
			HPBarSlider.value = (float)_Hp / _maxHp;
			HPText.text = _Hp + "/" + _maxHp;
		}

		public void SetHp(int t)
		{
			_Hp = t;
			HPBarSlider.value = (float)_Hp / _maxHp;
			HPText.text = _Hp + "/" + _maxHp;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			HPText.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			HPText.gameObject.SetActive(false);
		}
	}
}
