using System.Collections.Generic;
using System.Text.RegularExpressions;
using cfg;
using Draconia.Controller;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.UI;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

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
        private Player CardPlayer;
        private UIBattlePanel UIBattlePanel;
        private List<EnumCardProperty> _properties;
        private int index;
        private Vector3 _localScale, _localPos; 
        public bool IsChosen;
        
        void Start()
        {
            _cardState = CardState.Listen;
            _hands = UIKit.GetPanel<UIBattlePanel>().Hands;
            UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
        }

        public void Init(CardInfo cardInfo, Player player)
        {
            _cardInfo = cardInfo;
            _properties = cardInfo.Properties;
            
            var desc = _cardInfo.Desc;
            MatchCollection m = Regex.Matches(desc, "\\{(.*?)\\}");
            List<string> commons = new List<string>();
            foreach (Match match in m)
            {
                commons.Add(match.ToString().Trim('{').Trim('}'));
            }
            desc = desc.Replace("{", "<color=yellow>").Replace("}", "</color>");

            CardName.text = _cardInfo.Name;
            CardDesc.text = desc;
            CardCost.text = _cardInfo.Cost.ToString();
            //CardType.text = _cardInfo.SkillTargetType.ToString();
            CardPlayer = player;
            
            GetComponent<MyTooltipManager>().InitWithCommons(commons);
        }

  

        private List<ICharacter> Target;
        private List<Enemy> _enemies;
        private List<Player> _characters;
      public void OnPointerEnter(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen)
                Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen)
                UnHover();
        }
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

            InitialPos = transform.localPosition;
            _rotation = transform.rotation;
            transform.eulerAngles = new Vector3(0, 0, 0);
            UIKit.GetPanel<UIBattlePanel>().Bezier.Activate();
            UIKit.GetPanel<UIBattlePanel>().Bezier.GetComponent<RectTransform>().position =
                transform.GetComponent<RectTransform>().position;

            Target = new List<ICharacter>();
            _enemies = new List<Enemy>();
            _characters = new List<Player>();
        }

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
            MoveToMiddle();

            List<RaycastResult> res = new List<RaycastResult>();
            UIRoot.Instance.Canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, res);
            if (res.Count == 0)
            {
                return;
            }

            switch (_cardInfo.SkillTargetType)
            {
                case SkillTarget.Self:
                    CardPlayer.Chosen();
                    Target.Add(CardPlayer);
                    _characters.Add(CardPlayer);
                    break;

                case SkillTarget.SingleAlly:
                    if (res[0].gameObject.CompareTag("PlayerRaycast"))
                    {
                        Player player = res[0].gameObject.GetComponentInParent<Player>();
                        if (_characters.Count > 0 && player != _characters[0])
                        {
                            _characters[0].Unchosen();
                            _characters.Clear();
                        }

                        player.Chosen();
                        _characters.Add(player);
                    }

                    break;
                case SkillTarget.AllAlly:
                    foreach (var character in BattleSystem.Characters)
                    {
                        character.Chosen();
                    }

                    _characters.AddRange(BattleSystem.Characters);
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
                        if (player.Distance(CardPlayer) == 1)
                        {
                            player.Chosen();
                            _characters.Add(player);
                        }
                        
                    }
                    break;
                //TODO For other Condition;
            }

            if (!InThePlayerArea())
            {
                UIKit.GetPanel<UIBattlePanel>().UnchosenAll();
            }

            // if(_cardState == CardState.Listen)
            // 	Play();
        }

        private void MoveToMiddle()
        {
            transform.parent = _hands.DisplayArea;
            transform.localPosition = Vector3.zero;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player)
            {
                return;
            }
            
            UIKit.GetPanel<UIBattlePanel>().Bezier.Deactivate();
            if (InThePlayerArea() && IsRightTarget())
            {
                Play();
            }
            else
            {
                transform.parent = _hands.transform;
                transform.localPosition = InitialPos;
                transform.SetSiblingIndex(index);
                transform.rotation = _rotation;
                _hands.RecView();
            }

            UIKit.GetPanel<UIBattlePanel>().UnchosenAll();
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
            transform.parent = CardPlayer.CardBin;
            _hands.Refresh();
        }

       
        public void Hover()
        {
            var transform1 = transform;
            _localScale = transform1.localScale;
            _localPos = transform1.localPosition;
            transform1.localScale = new Vector3(.8f, .8f, 1f);
            
            
            //ChosenEffect.gameObject.SetActive(true);
            index = transform.GetSiblingIndex();
            transform1.parent = _hands.DisplayArea;
            transform1.localPosition = new Vector3(transform1.localPosition.x, 0, 0);
            transform1.SetAsLastSibling();
            IsChosen = true;
            //_hands.RecView();
        }

        public void UnHover()
        {
            transform.parent = _hands.transform;
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
            switch (_cardInfo.Id)
            {
                case 100:
                    BattleSystem.Attack(CardPlayer, _enemies[0],1f, AttackType.Physical);
                    break;
                case 101:
                    CardPlayer.Move(_characters[0]);
                    break;
                case 102:
                    CardPlayer.Defense();
                    break;
                case 103:
                    BattleSystem.Attack(CardPlayer, _enemies[0],.9f, AttackType.Physical);
                    _enemies[0].Move(1);
                    _enemies[0].AddBuff("重心不稳",1);
                    break;
                case 104:
                    CardPlayer.AddBuff("必定闪避", 1);
                    break;
                case 105:
                    BattleSystem.Attack(CardPlayer, _enemies[0],1.3f, AttackType.Physical);
                    CardPlayer.AddBuff("清风", 1);
                    break;
                case 106:
                    _characters[0].Refresh();
                    CardPlayer.AddBuff("加速", 1);
                    break;
                case 107:
                    BattleSystem.Attack(CardPlayer, _enemies[0],1.8f, AttackType.Physical);
                    Card card = BattleSystem.DrawRandom(CardPlayer, 1)[0];
                    //card._properties.Add(EnumCardProperty.Virtual);
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
            if (_characters.Count == 0 && _enemies.Count == 0)
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