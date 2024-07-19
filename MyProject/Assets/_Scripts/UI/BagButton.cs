using Draconia.System;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace Draconia.UI
{
    public class BagButton : Button, ICanGetSystem

    {
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        this.GetSystem<GameSystem>().OpenBag();
        Debug.LogFormat("#DEBUG# HI Bag Button");
    }


    public IArchitecture GetArchitecture()
    {
        return Draconia.Interface;
    }
    }
}