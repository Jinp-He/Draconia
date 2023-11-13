using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.UI;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.ViewController
{
    public enum CardState
    {
        Idle,
        Listen
    }

    public partial class Card : MyViewController, IPointerEnterHandler, IPointerExitHandler, IDragHandler,
        IBeginDragHandler, IEndDragHandler
    {
        private CardState _cardState;
        private Hands _hands;
        public CardInfo _cardInfo;
        public CardDragger CardDragger;
        public Player CardPlayer;
        private UIBattlePanel UIBattlePanel;
        private List<EnumCardProperty> _properties;
        private int index;
        private Vector3 _localScale, _localPos; 
        public bool IsChosen;
        public bool IsBasicCard;
        public bool IsViewMode = true;

        private int tempCostModifier;
        public int TempCostModifier {
            get => tempCostModifier;
            set
            {
                tempCostModifier = value;
                SetCost();
            }
        }
        
        private int battleCostModifier;
        public int BattleCostModifier {
            get => battleCostModifier;
            set
            {
                battleCostModifier = value;
                SetCost();
                
            }
        }

        public int Cost { set; get; }

        void Start()
        {
            _cardState = CardState.Listen;
           
            UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
            if(UIBattlePanel != null) _hands = UIKit.GetPanel<UIBattlePanel>().Hands;

        }

        public void ShowMode(bool isUsed = false)
        {
            IsViewMode = true;
            if (isUsed)
            {
                GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }

        public void Init(CardInfo cardInfo, Player player, bool isBasic = false)
        {
            IsViewMode = false;
            
            _cardInfo = cardInfo;
            _properties = cardInfo.Properties;
            IsBasicCard = isBasic;
            _cardState = CardState.Listen;
            CardPlayer = player;
            
            var commons = ChangeDesc();
            CardName.text = _cardInfo.Name;
            if (!IsBasicCard)
                CardImage.sprite = CardPlayer.CardImageSprite;
            //基础卡的标题是竖着
            if (IsBasicCard)
                CardName.text = "<rotate=90>" + _cardInfo.Name;
            CardImage.SetNativeSize();
            CardCost.text = Cost.ToString();
            //CardType.text = _cardInfo.SkillTargetType.ToString();
            
            
            GetComponent<MyTooltipManager>().InitWithCommons(commons);

            
            //更改角色描述
            List<string> ChangeDesc()
            {
                var desc = _cardInfo.Desc;
                MatchCollection m = Regex.Matches(desc, "\\{(.*?)\\}");
                List<string> commons = new List<string>();
                foreach (Match match in m)
                {
                    commons.Add(match.ToString().Trim('{').Trim('}'));
                }

                desc = desc.Replace("{", "<color=yellow>").Replace("}", "</color>");
                CardDesc.text = desc;
                return commons;
            }
        }

        public void SetCost()
        {
            Cost = _cardInfo.Cost + TempCostModifier + BattleCostModifier;
            CardCost.text = Cost.ToString();
        }

  

        private List<Character> Target;
        private List<Enemy> _enemies;
        private List<Player> _allies;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen && !IsViewMode)
                Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen&& !IsViewMode)
                UnHover();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player || IsViewMode)
            {
                return;
            }

            PlayTimeBarAnimation(_cardInfo.Cost, CardPlayer);
            InitialPos = transform.localPosition;
            _rotation = transform.rotation;
            transform.eulerAngles = new Vector3(0, 0, 0);
            UIKit.GetPanel<UIBattlePanel>().Bezier.Activate();
            UIKit.GetPanel<UIBattlePanel>().Bezier.GetComponent<RectTransform>().position =
                transform.GetComponent<RectTransform>().position;

            Target = new List<Character>();
            _enemies = new List<Enemy>();
            _allies = new List<Player>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player || IsViewMode)
            {
                return;
            }

            
            index = transform.GetSiblingIndex();
            //Debug.LogFormat("{0},{1}",Input.mousePosition.x,Input.mousePosition.y);

            UnHover();
            
            MoveToMiddle();

            List<RaycastResult> res = new List<RaycastResult>();
            //UIRoot.Instance.Canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, res);
            UIKit.GetPanel<UIBattlePanel>().transform.GetChild(0).GetComponent<GraphicRaycaster>().Raycast(eventData,res);
            if (res.Count == 0)
            {
                //Debug.Log("Cannot find raycast trarget");
                return;
            }

            ChooseTarget(res);

            if (!InThePlayerArea())
            {
                UIKit.GetPanel<UIBattlePanel>().UnchosenAll();
            }

            // if(_cardState == CardState.Listen)
            // 	Play();
        }

        private void ChooseTarget(List<RaycastResult> res)
        {
            switch (_cardInfo.SkillTargetType)
            {
                case SkillTarget.Self:
                    CardPlayer.Chosen();
                    Target.Add(CardPlayer);
                    _allies.Add(CardPlayer);
                    break;

                case SkillTarget.SingleAlly:
                    if (res[0].gameObject.CompareTag("PlayerRaycast"))
                    {
                        Player player = res[0].gameObject.GetComponentInParent<Player>();
                        if (_allies.Count > 0 && player != _allies[0])
                        {
                            _allies[0].Unchosen();
                            _allies.Clear();
                        }

                        player.Chosen();
                        _allies.Add(player);
                    }

                    break;
                case SkillTarget.AllAlly:
                    foreach (var character in BattleSystem.Players)
                    {
                        character.Chosen();
                    }

                    _allies.AddRange(BattleSystem.Players);
                    break;
                case SkillTarget.SingleEnemy:
                    if (res[0].gameObject.CompareTag("EnemyRaycast"))
                    {
                        Enemy enemy = res[0].gameObject.GetComponentInParent<Enemy>();
                        if (_enemies.Count > 0 && enemy != _enemies[0])
                        {
                            _enemies[0].EnemyAnimation.Unchosen();
                            _enemies.Clear();
                        }

                        enemy.EnemyAnimation.Chosen();
                        _enemies.Add(enemy);
                    }

                    break;
                case SkillTarget.AllEnemy:
                    foreach (var enemy in BattleSystem.Enemies)
                    {
                        enemy.EnemyAnimation.Chosen();
                    }

                    _enemies.AddRange(BattleSystem.Enemies);
                    break;
                case SkillTarget.AroundSelf:
                    if (res[0].gameObject.CompareTag("PlayerRaycast"))
                    {
                        Player player = res[0].gameObject.GetComponentInParent<Player>();
                        if (player.PlayerStrategy.Distance(CardPlayer) == 1)
                        {
                            player.Chosen();
                            _allies.Add(player);
                        }
                    }

                    break;
                //TODO For other Condition;
            }
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player || IsViewMode)
            {
                return;
            }

            StopTimeBarAnimation();
            
            UIKit.GetPanel<UIBattlePanel>().Bezier.Deactivate();
            if (InThePlayerArea() && IsRightTarget() && CardPlayer.PlayerStrategy.ValidCard(this))
            {
                CardPlayer.PlayerStrategy.PayCost(Cost);
                Play();
            }
            else
            {
                transform.parent = _hands.OngoingPlayerHands.transform;
                transform.localPosition = InitialPos;
                transform.SetSiblingIndex(index);
                transform.rotation = _rotation;
                _hands.Refresh();
            }

            UIKit.GetPanel<UIBattlePanel>().UnchosenAll();
        }


        private Sequence _sequence;
        private float _pointerPosition;
        /// <summary>
        /// 演示使用这张牌在时间轴上会发生什么效果
        /// </summary>
        /// <param name="cardInfoCost"></param>
        /// <param name="cardPlayer"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PlayTimeBarAnimation(int cardInfoCost, Player cardPlayer)
        {
            _pointerPosition = cardPlayer.MyPointer.PositionX;
            _sequence = DOTween.Sequence();
            _sequence.Append(cardPlayer.MyPointer.transform.DOLocalMoveX(_pointerPosition-TimeBar.ToBarPosition(cardInfoCost),.9f))
                .Join(cardPlayer.MyPointer.PointerImage.DOFade(0,.9f))
                .OnComplete(()=>
                {
                    cardPlayer.MyPointer.PositionX = _pointerPosition;
                    cardPlayer.MyPointer.PointerImage.ChangeAlpha(1);
                })
                .SetLoops(-1);
        }
        
        private void StopTimeBarAnimation()
        {
            CardPlayer.MyPointer.PositionX = _pointerPosition;
            CardPlayer.MyPointer.PointerImage.ChangeAlpha(1);
            _sequence.Pause();
        }

        private void MoveToMiddle()
        {
            transform.parent = _hands.DisplayArea;
            transform.localPosition = Vector3.zero;
        }


        /// <summary>
        /// 丢弃到角色的弃牌堆
        /// </summary>
        public void Discard()
        {
            // if (_cardInfo.Properties.Contains(EnumCardProperty.Virtual))
            // {
            //     Destroy(this.gameObject,0.1f);
            //     return;
            // }
            if (!IsBasicCard)
            {
                CardPlayer.PlayerStrategy.Hands.Remove(this);
                CardPlayer.PlayerStrategy.Bin.Add(this);
            }
            else
            {
                CardPlayer.PlayerStrategy.Hands.Remove(this);
            }
            //TODO 正确的移除basiccard
            transform.SetParent(CardPlayer.CardBin);
            _hands.Refresh();
        }

       
        public void Hover()
        {
            if (IsViewMode) return;
            var transform1 = transform;
            _localScale = transform1.localScale;
            _localPos = transform1.localPosition;
            transform1.localScale = new Vector3(.6f, .6f, 1f);
            
            
            //ChosenEffect.gameObject.SetActive(true);
            index = transform.GetSiblingIndex();
            //transform1.parent = _hands.DisplayArea;
            //transform1.localPosition = new Vector3(0, 50f, 0);
            transform1.SetAsLastSibling();
            IsChosen = true;
            _hands.RecView();
        }

        public void UnHover()
        {
            if (IsViewMode) return;
                transform.SetParent(_hands.OngoingPlayerHands.transform);
                transform.localScale = _localScale;
                transform.localPosition = _localPos;
                transform.SetSiblingIndex(index);
                IsChosen = false;
                _hands.RecView();
                //ChosenEffect.gameObject.SetActive(false);
        }

        //TODO For Example Use only
        public void Play()
        {
            int id = _cardInfo.Id;
            if (id > 200)
            {
                id -= 100;
            }
            switch (id)
            {
                case 100:
                    BattleSystem.Attack(CardPlayer, _enemies[0], AttackType.Physical, 2);
                    break;
                case 101:
                    CardPlayer.PlayerStrategy.Move(_allies[0]);
                    break;
                case 102:
                    CardPlayer.Defense(2);
                    break;
                case 103:
                    BattleSystem.Attack(CardPlayer, _enemies[0], AttackType.Physical, 3);
                    //TODO cost modifieir 
                    CardPlayer.PlayerStrategy.Hands.Where(card => card.IsBasicCard).ToList()
                        .ForEach(card => card.tempCostModifier -= 1);
                    break;
                case 104:
                    BattleSystem.Attack(CardPlayer, _enemies[0], AttackType.Physical, 2);
                    break;
                case 105:
                    CardPlayer.PlayerStrategy.EnterPose("飞鸟式");
                    break;
                case 106:
                    CardPlayer.PlayerStrategy.EnterPose("拿云式");
                    break;
                case 107:
                    _allies[0].PlayerStrategy.MoveInTime(2);
                    break;
                case 108:
                    BattleSystem.Attack(CardPlayer, _enemies[0], AttackType.Physical, 3);
                    _enemies[0].MoveInTime(2);
                    break;
                case 109:
                    BattleSystem.Attack(CardPlayer, _enemies[0], AttackType.Physical, 5);
                    break;
                case 110:
                    CardPlayer.Defense(3);
                    break;
            }
            Discard();
            //CardPlayer.PlayerTurnEnd();
        }


        
        private Vector2 InitialPos;
        private Quaternion _rotation;

    
        private bool InThePlayerArea()
        {
            return Input.mousePosition.y > UIRoot.Instance.Canvas.GetComponent<RectTransform>().rect.height * 0.175f;
        }

        /// <summary>
        /// Choose the right target to activate;
        /// </summary>
        /// <returns></returns>
        private bool IsRightTarget()
        {
            if (_allies.Count == 0 && _enemies.Count == 0)
            {
                return false;
            }

            return true;
        }
        

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }

    public class CardDragger : MyViewController
    {
        
    }
}