using Draconia.Controller;

namespace Draconia.ViewController.Event
{
    public struct BattleStartEvent
    {
        
    }
    
    public struct PlayerTurnStartEvent
    {
    
    }
    
    public struct EnterDangerAreaEvent
    {
        public Character Character;
    }


    public struct UseCardEvent
    {
        public Character Character;
        public Card UsedCard;
    }
}