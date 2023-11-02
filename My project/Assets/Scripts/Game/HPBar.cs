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
			HPText.gameObject.SetActive(true);
			ArmorImage.gameObject.SetActive(false);
			ArmorText.text = "0";
		}

		public void SetHp(int t)
		{
			_Hp = t;
			HPBarSlider.value = (float)_Hp / _maxHp;
			HPText.text = _Hp + "/" + _maxHp;
		}

		public void SetArmor(int t)
		{
			if (t == 0)
			{
				ArmorImage.gameObject.SetActive(false);
			}
			else
			{
				ArmorImage.gameObject.SetActive(true);
				ArmorText.text = t.ToString();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			HPText.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			//HPText.gameObject.SetActive(false);
		}
	}
}
