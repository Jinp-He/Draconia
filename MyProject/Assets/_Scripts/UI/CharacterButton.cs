using _Scripts.System;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class CharacterButton : Button, ICanGetSystem

    {
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        this.GetSystem<GameSystem>().OpenCharacter();
        
        Debug.LogFormat("#DEBUG# HI Bag Button");
    }


    public IArchitecture GetArchitecture()
    {
        return Draconia.Draconia.Interface;
    }
    }
}