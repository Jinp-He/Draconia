using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cfg;
using Draconia.MyComponent;

using QFramework;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Common = cfg.Common;

namespace Utility
{
    public class Tooltip
    {
        public string Desc;
        public string Name;
        public string Type;
    }
    
    public class MyTooltipManager : MyViewController, IPointerEnterHandler, IPointerExitHandler
    {
        public MyTooltip TooltipPrefab;
        private RectTransform TooltipGroup => UIRoot.Instance.TooltipContainer;
        [HideInInspector]public GameObject TooltipContainer;
        public RectTransform TooltipPosition;
        public MyTooltip Tooltip;
        [HideInInspector]public List<MyTooltip> Tooltips;
        public bool LagDisplay;
        public float MouseOverSec;
        private bool IsMouseOver;

        public bool IsInit { get; private set; } = false;

        private void InitTooltip(List<Tooltip> tooltips)
        {
            Tooltips = new List<MyTooltip>();
            IsInit = true;
            TooltipContainer = new GameObject();
            TooltipContainer.transform.parent = TooltipGroup;
            TooltipContainer.transform.position = TooltipPosition.transform.position;
            TooltipContainer.transform.localScale = new Vector3(1, 1, 1);
            TooltipContainer.AddComponent<VerticalLayoutGroup>();
          
            TooltipContainer.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
            TooltipContainer.GetComponent<VerticalLayoutGroup>().childControlWidth = true;
            TooltipContainer.AddComponent<ContentSizeFitter>();
            TooltipContainer.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;

            foreach (var lists in tooltips)
            {
                Tooltip = Instantiate(TooltipPrefab, TooltipContainer.transform);
                Tooltips.Add(Tooltip);
                Tooltip.Initialize(lists);
            }
            TooltipContainer.gameObject.SetActive(false);
        }

        public void ChangeTooltip(Tooltip tooltip)
        {
            Tooltips[0].Initialize(tooltip);
        }

        public void InitTooltip(Tooltip tooltip)
        {
            
            string desc = tooltip.Desc;
            MatchCollection m = Regex.Matches(desc, "\\{(.*?)\\}");
            List<string> commons = new List<string>();
            foreach (Match match in m)
            {
                commons.Add(match.ToString().Trim('{').Trim('}'));
            }
            
            tooltip.Desc = desc.Replace("{", "<color=yellow>").Replace("}", "</color>");
            if (IsInit)
                ChangeTooltip(tooltip);
            else
                InitTooltip(new List<Tooltip>(){tooltip});
            AddCommons(commons);
        }
        
        public void InitWithCommons(List<string> commons)
        {
            IsInit = true;
            var res = new List<Tooltip>();
            foreach (var noun in commons)
            {
                Common c = ResLoadSystem.Table.TbCommon[noun];
                Tooltip list = new Tooltip(){Name = c.Name,Desc = c.Desc,Type = c.Type};
                res.Add(list);
            }

            InitTooltip(res);
        }

        public void AddTooltip(List<Tooltip> tooltips, Color titleColor)
        {
            foreach (var lists in tooltips)
            {
                Tooltip = Instantiate(TooltipPrefab, TooltipContainer.transform);
                Tooltip.transform.position = TooltipPosition.transform.position;
                Tooltip.Title.color = titleColor;
                Tooltips.Add(Tooltip);            
                Tooltip.Initialize(lists);
            }
        }

        /// <summary>
        /// 添加common
        /// </summary>
        /// <param name="commons"></param>
        public void AddCommons(IEnumerable<string> commons)
        {
            IsInit = true;
            var res = new List<Tooltip>();
            foreach (var noun in commons)
            {
                Common c = ResLoadSystem.Table.TbCommon[noun];
                Tooltip list = new Tooltip(){Name = c.Name,Desc = c.Desc,Type = c.Type}; 
                res.Add(list);
            }

            AddTooltip(res, Color.yellow);
        }

        private float _mouseEnterTime;
        public void Update()
        {
            if (!IsInit)
            {
                return;
            }

            if (!IsMouseOver || !LagDisplay) return;
            _mouseEnterTime += Time.deltaTime;
            if (_mouseEnterTime > MouseOverSec)
            {
                TooltipContainer.gameObject.SetActive(true);
                _mouseEnterTime = 0;
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TooltipContainer.transform.position != TooltipPosition.transform.position)
                TooltipContainer.transform.position = TooltipPosition.transform.position;
            if (!IsInit)
            {
                return;
            }

            if(!LagDisplay)
                TooltipContainer.gameObject.SetActive(true);
            IsMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsInit)
            {
                return;
            }
            
            Invoke(nameof(DelayExit), .1f);

            IsMouseOver = false;
            _mouseEnterTime = 0;
        }

        public void Active()
        {
            TooltipContainer.gameObject.SetActive(true);
        }

        public void DelayExit()
        {
            TooltipContainer.gameObject.SetActive(false);
        }
    }
}