using _Scripts.Game.Card;
using _Scripts.Game.Event;
using _Scripts.UI;
using cfg;
using QFramework;
using UnityEngine;

namespace _Scripts.Game.Player
{
    public partial class PlayerViewController : CharacterViewController
    {
        //public SpriteAtlas CharacterAtlas;
        public PlayerAnimator PlayerAnimator;
        public PlayerInfo PlayerInfo;

        
        
        

        public void Init(PlayerStrategy.Player player, bool isBattle = true)
        {
            PlayerInfo playerInfo = player.PlayerInfo;
            IsPlayer = true;
            base.Init();
            Player = player;
            Player.PlayerViewController = this;
            PlayerInfo = playerInfo;
            Alias = playerInfo.Alias;
            
            //CharacterAtlas = ResLoadSystem.LoadSync<SpriteAtlas>("Zhouzhou");
            //CharacterImage.sprite = CharacterAtlas.GetSprite("Idle");
            //CharacterImage.SetNativeSize();
            //CardImageSprite = ResLoadSystem.LoadSync<Sprite>("CardImage_Zhouzhou");
            
            CurrHP = player.Hp;
            HpBar.Init(player.MaxHp, player.Hp);

            PlayerAnimator.Init(this);

            
            if (isBattle)
            {
                MyPointer = UIKit.GetPanel<UIBattlePanel>().TimeBar.AddCharacter(this);
                TriggerDanger = () => { };
                TriggerDanger += Player.EnterDangerArea;
                CharacterDisplayBar.gameObject.SetActive(false);
                BattleStart();
                Debug.Log("#BattleSystem# StartInitialize3 DisplayBar");
            }
            else
            {
                
                transform.localScale = new Vector3(.9f, .9f, 1);
            }
            
            CharacterDisplayBar.Init(player);

            
        }

        public void UpdateDisplayArea()
        {
            
        }


        public void BattleStart()
        {
            foreach (var cardInfo in Player.PlayerInfo.InitialCards_Ref)
            {
                var cardPrefab = ResLoadSystem.LoadSync<GameObject>("CardPrefab").GetComponent<CardVC>();
                CardVC cardVc = Instantiate(cardPrefab, CardDeck);
                cardVc.Init(cardInfo, this);
                Player.Deck.Add(cardVc);
            }
        }

        public void Chosen()
        {
            ChooseBracelet.gameObject.SetActive(true);
        }

        public void Unchosen()
        {
            ChooseBracelet.gameObject.SetActive(false);
        }


        public override void Die()
        {
            _player.Die();
        }

       


        //判断是否可以释放该卡


        public override void TurnStart()
        {
            _player.OnTurnStart();
            this.SendEvent(new PlayerTurnStartEvent() { Player = this.Player });
            StartCoroutine(PlayerAnimator.SendNotificationText("回合开始了"));
            //TODO 避免错误
            UIKit.GetPanel<UIBattlePanel>().TimeBar.MoveAbsoluteTimePosition(MyPointer, _player.BackNum,true);
            PlayerAnimator.IsChosen();
        }
    }
}