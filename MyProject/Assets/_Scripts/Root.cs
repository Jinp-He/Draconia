using System.Collections;
using System.Collections.Generic;
using Draconia.System;
using Draconia.UI;
using QFramework;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Draconia
{


    public class Root : MonoBehaviour, ICanGetSystem
    {
        public Texture2D Cursor;

        public Texture2D Cursor_Click;
        // Start is called before the first frame update
        void Start()
        {
            UnityEngine.Cursor.SetCursor(Cursor, Vector2.zero, CursorMode.Auto);
            
            ResKit.Init();
            UIKit.OpenPanel<UIStartPanel>();
            
            //GetComponent<ChangeAllFont>().ChangeAllFonts();
        }

        private bool isClick;

        private float timer;
        // Update is called once per frame
        void Update()
        {
            if (isClick)
            {
                timer += Time.deltaTime;
                if (timer >= .1f && !Input.GetMouseButton(0))
                {
                    timer = 0f;
                    isClick = false;
                    UnityEngine.Cursor.SetCursor(Cursor, Vector2.zero, CursorMode.Auto);
                }
            }

            if (Input.GetMouseButtonDown(0) && !isClick)
            {
                isClick = true;
                UnityEngine.Cursor.SetCursor(Cursor_Click, Vector2.zero, CursorMode.Auto);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return Draconia.Interface;
        }
    }
}
