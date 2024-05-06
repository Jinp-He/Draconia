using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.UI;
using Draconia.ViewController.Event;
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

    public enum CardPosition
    {
        Battle,
        View,
        Store,
        Bought
    }

    public partial class CardVC : MyViewController, IPointerEnterHandler, IPointerExitHandler, IDragHandler,
        ICanSendEvent,
        IBeginDragHandler, IEndDragHandler
    {
        protected CardState _cardState;
        protected Hands _hands;
        public CardPosition CardPosition;
        public Card Card;
        public CardInfo _cardInfo;
        public PlayerViewController CardUser;
        private UIBattlePanel UIBattlePanel;
        public List<EnumCardProperty> Properties;
        protected int index;
        private Vector3 _localScale, _localPos;
        public bool IsChosen;
        public bool IsBasicCard;
        public bool IsViewMode = true;
        public int Cost => Card.Cost;

        void Start()
        {


        }

        public void ShowMode(bool isUsed = false)
        {
            IsViewMode = true;
            if (isUsed)
            {
                GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }

        public void BeingBought()
        {
            CardPosition = CardPosition.Bought;
            CardName.color = Color.gray;
            CardBorder.gameObject.SetActive(false);
        }

        public void DOShakePosition()
        {
            ImageGroup.transform.DOShakePosition(0.5f, 20, 20, 90);
        }



        public void Init(CardInfo cardInfo, PlayerViewController cardUser = null, bool isBasic = false, CardPosition 
        cardPosition = CardPosition.Battle)
        {
            UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
            if (UIBattlePanel != null) _hands = UIKit.GetPanel<UIBattlePanel>().Hands;
            CardPosition = cardPosition;

            _cardInfo = cardInfo;
            Properties = cardInfo.Properties;
            IsBasicCard = isBasic;
            _cardState = CardState.Listen;
            CardUser = cardUser;

            ChangeDesc();
            CardName.text = _cardInfo.Name;
            if (!IsBasicCard && cardUser!=null)
                CardImage.sprite = CardUser.CardImageSprite;
            //基础卡的标题是竖着
            if (IsBasicCard)
                CardName.text = "<rotate=90>" + _cardInfo.Name;
            CardImage.SetNativeSize();


            Card = Card.GetCard(this, cardInfo);
            
            CardCost.text = Card.Cost.ToString();
            //CardType.text = _cardInfo.SkillTargetType.ToString();





            //更改角色描述
            void ChangeDesc()
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
                GetComponent<MyTooltipManager>().InitWithCommons(commons);
            }
        }


        public void SetDescription()
        {
            var desc = _cardInfo.Desc;
            MatchCollection m = Regex.Matches(desc, "\\{(.*?)\\}");
            List<string> commons = new List<string>();
            foreach (Match match in m)
            {
                commons.Add(match.ToString().Trim('{').Trim('}'));
            }

            desc = desc.Replace("{", "<color=yellow>").Replace("}", "</color>");
            //TODO 为添加的卡牌特效增加相应的解释
            foreach (var prop in Properties)
            {
                commons.Add(ResLoadSystem.Table.TbCardBuffInfo.DataList.Find(e => e.Properties == prop).BuffName);
            }

            CardDesc.text = desc;

            GetComponent<MyTooltipManager>().InitWithCommons(commons);
        }



        protected List<CharacterViewController> Target;
        protected List<Enemy> _enemies;
        protected List<PlayerViewController> _allies;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen && CardPosition == CardPosition.Battle)
                Hover();
            if (IsViewMode && CardPosition != CardPosition.Bought)
            {
                CardBorder.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen && CardPosition == CardPosition.Battle)
                UnHover();
            if (IsViewMode && CardPosition != CardPosition.Bought)
            {
                CardBorder.gameObject.SetActive(false);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player || IsViewMode)
            {
                return;
            }

            PlayTimeBarAnimation(_cardInfo.Cost, CardUser);
            InitialPos = transform.localPosition;
            _rotation = transform.rotation;
            transform.eulerAngles = new Vector3(0, 0, 0);
            UIKit.GetPanel<UIBattlePanel>().Bezier.Activate();
            UIKit.GetPanel<UIBattlePanel>().Bezier.GetComponent<RectTransform>().position =
                transform.GetComponent<RectTransform>().position;

            Target = new List<CharacterViewController>();
            _enemies = new List<Enemy>();
            _allies = new List<PlayerViewController>();
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
            UIKit.GetPanel<UIBattlePanel>().transform.GetChild(0).GetComponent<GraphicRaycaster>()
                .Raycast(eventData, res);
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

        protected void ChooseTarget(List<RaycastResult> res)
        {
            switch (_cardInfo.SkillTargetType)
            {
                case SkillTarget.Self:
                    CardUser.Chosen();
                    Target.Add(CardUser);
                    _allies.Add(CardUser);
                    break;

                case SkillTarget.SingleAlly:
                    if (res[0].gameObject.CompareTag("PlayerRaycast"))
                    {
                        PlayerViewController playerViewController =
                            res[0].gameObject.GetComponentInParent<PlayerViewController>();
                        if (_allies.Count > 0 && playerViewController != _allies[0])
                        {
                            _allies[0].Unchosen();
                            _allies.Clear();
                        }

                        playerViewController.Chosen();
                        _allies.Add(playerViewController);
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
                case SkillTarget.DoubleEnemy:
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
                        if (BattleSystem.Enemies.Count > enemy.Position + 1)
                        {
                            BattleSystem.Enemies[enemy.Position + 1].EnemyAnimation.Chosen();
                            _enemies.Add(BattleSystem.Enemies[enemy.Position + 1]);
                        }
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
                        PlayerViewController playerViewController =
                            res[0].gameObject.GetComponentInParent<PlayerViewController>();
                        if (playerViewController.Player.Distance(CardUser) == 1)
                        {
                            playerViewController.Chosen();
                            _allies.Add(playerViewController);
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
            if (InThePlayerArea() && IsRightTarget() && CardUser.Player.ValidCard(this))
            {
                CardUser.Player.PayCost(Card.Cost);
                if (Properties.Exists(e => e == EnumCardProperty.Double))
                {
                    Play();
                }

                Play();
                Discard();
                this.SendEvent(new UseCardEvent()
                    { UsedCardVc = this, CharacterViewController = CardUser });
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


        protected Sequence _sequence;
        protected float _pointerPosition;

        /// <summary>
        /// 演示使用这张牌在时间轴上会发生什么效果
        /// </summary>
        /// <param name="cardInfoCost"></param>
        /// <param name="cardPlayerViewController"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected void PlayTimeBarAnimation(int cardInfoCost, PlayerViewController cardPlayerViewController)
        {
            _pointerPosition = cardPlayerViewController.MyPointer.PosX.Value;
            _sequence = DOTween.Sequence();
            _sequence.Append(
                    cardPlayerViewController.MyPointer.transform.DOLocalMoveX(
                        _pointerPosition - TimeBar.ToBarPosition(cardInfoCost), .9f))
                .Join(cardPlayerViewController.MyPointer.PointerImage.DOFade(0, .9f))
                .OnComplete(() =>
                {
                    cardPlayerViewController.MyPointer.PosX.Value = _pointerPosition;
                    cardPlayerViewController.MyPointer.PointerImage.ChangeAlpha(1);
                })
                .SetLoops(-1);
        }

        protected void StopTimeBarAnimation()
        {
            CardUser.MyPointer.PosX.Value = _pointerPosition;
            CardUser.MyPointer.PointerImage.ChangeAlpha(1);
            _sequence.Pause();
        }

        protected void MoveToMiddle()
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
                CardUser.Player.Hands.Remove(this);
                CardUser.Player.Bin.Add(this);
            }
            else
            {
                CardUser.Player.Hands.Remove(this);
            }

            //TODO 正确的移除basic card
            transform.SetParent(CardUser.CardBin);
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
        public virtual void Play()
        {
            Card.Play(_enemies,_allies);

            //CardPlayer.PlayerTurnEnd();
        }



        protected Vector2 InitialPos;
        protected Quaternion _rotation;


        protected bool InThePlayerArea()
        {
            return Input.mousePosition.y > UIRoot.Instance.Canvas.GetComponent<RectTransform>().rect.height * 0.175f;
        }

        /// <summary>
        /// Choose the right target to activate;
        /// </summary>
        /// <returns></returns>
        protected bool IsRightTarget()
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
}