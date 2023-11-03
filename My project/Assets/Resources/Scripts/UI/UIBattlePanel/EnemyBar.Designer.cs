/****************************************************************************
 * 2023.10 JAMES_WORKSPACE
 ****************************************************************************/

using Draconia.ViewController;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Draconia.UI
{
	public partial class EnemyBar
	{
		[SerializeField] public HPBar HPBar;
		[SerializeField] public RectTransform EnergyBar;

		public void Clear()
		{
			HPBar = null;
			EnergyBar = null;
		}

		public override string ComponentName
		{
			get { return "EnemyBar";}
		}
	}
}
