using System.Collections.Generic;
using System.Text.RegularExpressions;
using cfg;
using DG.Tweening;
using Draconia.Controller;
using Draconia.MyComponent;
using Draconia.System;
using Draconia.UI;
using UnityEngine;
using QFramework;
using Unity.VisualScripting;
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
        private Character CardPlayer;
        private UIBattlePanel UIBattlePanel;
        private List<EnumCardProperty> _properties;
        
        void Start()
        {
            _cardState = CardState.Listen;
            _hands = UIKit.GetPanel<UIBattlePanel>().Hands;
            UIBattlePanel = UIKit.GetPanel<UIBattlePanel>();
        }

        public void Init(CardInfo cardInfo, Character character)
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
            CardType.text = _cardInfo.SkillTargetType.ToString();
            CardPlayer = character;
            
            GetComponent<MyTooltipManager>().InitWithCommons(commons);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen)
                _hands.Choose(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_cardState == CardState.Listen)
                UnHover();
        }

        private List<ICharacter> Target;
        private List<Enemy> _enemies;
        private List<Character> _characters;

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

            InitialPos = transform.GetComponent<RectTransform>().anchoredPosition;
            _rotation = transform.rotation;
            transform.eulerAngles = new Vector3(0, 0, 0);
            UIKit.GetPanel<UIBattlePanel>().Bezier.Activate();
            UIKit.GetPanel<UIBattlePanel>().Bezier.GetComponent<RectTransform>().position =
                transform.GetComponent<RectTransform>().position;

            Target = new List<ICharacter>();
            _enemies = new List<Enemy>();
            _characters = new List<Character>();
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
                        Character character = res[0].gameObject.GetComponentInParent<Character>();
                        if (_characters.Count > 0 && character != _characters[0])
                        {
                            _characters[0].Unchosen();
                            _characters.Clear();
                        }

                        character.Chosen();
                        _characters.Add(character);
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
                        Character character = res[0].gameObject.GetComponentInParent<Character>();
                        if (character.Distance(CardPlayer) == 1)
                        {
                            character.Chosen();
                            _characters.Add(character);
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
        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.GetSystem<BattleSystem>().BattleState != BattleState.Player)
            {
                return;
            }

            if (_cardInfo.Cost > BattleSystem.Energy)
            {
                return;
            }

            UIKit.GetPanel<UIBattlePanel>().Bezier.Deactivate();
            if (InThePlayerArea() && IsRightTarget())
            {
                Play();
                if (_properties.Contains(EnumCardProperty.Flash))
                {
                    _hands.RemoveCard(this);
                }
               
                BattleSystem.Energy.Value -= _cardInfo.Cost;
                BattleSystem.BattleState = BattleState.Enemy;
                BattleSystem.Continue();
                
                _hands.RemoveCard(this);
                CardPlayer.OnEndTurn();
                Destroy(gameObject);
                _hands.Refresh();
            }
            else
            {
                transform.GetComponent<RectTransform>().anchoredPosition = InitialPos;
                transform.SetSiblingIndex(index);
                transform.rotation = _rotation;
            }

            UIKit.GetPanel<UIBattlePanel>().UnchosenAll();
        }

        private int index;

        public void Hover()
        {
            transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            ChosenEffect.gameObject.SetActive(true);
            index = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
        }

        public void UnHover()
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ChosenEffect.gameObject.SetActive(false);
            transform.SetSiblingIndex(index);
        }

        //TODO For Example Use only
        public void Play()
        {
            switch (_cardInfo.Id)
            {
                case 100:
                    BattleSystem.Attack(CardPlayer, _enemies[0],1);
                    break;
                case 101:
                    CardPlayer.Move(_characters[0]);
                    break;
                case 102:
                    CardPlayer.Defense();
                    break;
                case 103:
                    BattleSystem.Attack(CardPlayer, _enemies[0],.9f);
                    _enemies[0].Move(1);
                    _enemies[0].AddBuff("重心不稳",1);
                    break;
                case 104:
                    CardPlayer.AddBuff("必定闪避", 1);
                    break;
                case 105:
                    BattleSystem.Attack(CardPlayer, _enemies[0],1.3f);
                    CardPlayer.AddBuff("清风", 1);
                    break;
                case 106:
                    _characters[0].Refresh();
                    CardPlayer.AddBuff("加速", 1);
                    break;
                case 107:
                    BattleSystem.Attack(CardPlayer, _enemies[0],1.8f);
                    Card card = BattleSystem.DrawRandom(CardPlayer, 1)[0];
                    //card._properties.Add(EnumCardProperty.Virtual);
                    break;
            }
        }

        public void OnEndTurn()
        {
            if (_properties.Contains(EnumCardProperty.Virtual))
            {
                _hands.RemoveCard(this);
            }
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
}