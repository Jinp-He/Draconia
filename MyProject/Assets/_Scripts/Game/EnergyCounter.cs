using _Scripts.UI;
using QFramework;

namespace _Scripts.Game
{
	public partial class EnergyCounter : QFramework.ViewController, ICanGetSystem
	{
		public UIBattlePanel UIBattlePanel;
		public float EnergySecond = 5;
		private float InitEnergy;
		private bool IsStop;
		public void Start()
		{
			UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
		}

		public void Stop()
		{
			IsStop = true;
		}

		public void Continue()
		{
			IsStop = false;
		}

		public void FixedUpdate()
		{
			// if (IsStop)
			// {
			// 	return;
			// 	
			// }
			// InitEnergy += Time.fixedDeltaTime;
			// EnergyCounterImage.fillAmount = InitEnergy / EnergySecond;
			// if (InitEnergy >= EnergySecond && this.GetSystem<BattleSystem>().Energy < this.GetSystem<BattleSystem>().MaxEnergy)
			// {
			// 	InitEnergy = 0f;
			// 	this.GetSystem<BattleSystem>().Energy.Value++;
			// }
		}

		public IArchitecture GetArchitecture()
		{
			return Draconia.Draconia.Interface;
		}
	}
}
