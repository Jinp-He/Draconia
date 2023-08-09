using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Utility.BelzierArrows
{
    public class BezierArrows : MonoBehaviour
    {
        [Tooltip("Prefab of arrow Head")]
        public GameObject ArrowHeadPrefab;
        [Tooltip("Prefab of arrow Node")]
        public GameObject ArrowNodePrefab;
        public int arrowNodeNum;
        public float scaleFactor = 1f;

        private RectTransform origin;
        private List<RectTransform> arrowNodes = new List<RectTransform>();
        private List<Vector2> controlPoints = new List<Vector2>();
        private readonly List<Vector2> controlPointFactors = new List<Vector2>(){new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f)};

        private void Awake()
        {
            this.origin = this.GetComponent<RectTransform>();

            for (int i = 0; i < this.arrowNodeNum; ++i)
            {
                this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab, this.transform).GetComponent<RectTransform>());
            }
            
            this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<RectTransform>());
            
            this.arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

            for (int i = 0; i < 4; i++)
            {
                controlPoints.Add(Vector2.zero);
            }
        }

        private bool _isActivate = false;
        public void Activate()
        {
            _isActivate = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActivate = false;
            gameObject.SetActive(false);
        }
        private void Update()
        {
            if (!_isActivate)
            {
                return;
            }

            this.controlPoints[0] = new Vector2(origin.position.x, origin.position.y);
            Vector2 mousePos =
               
                    UIKit.Root.Camera.ScreenToWorldPoint(Input.mousePosition);
            controlPoints[3] = new Vector2(mousePos.x, mousePos.y);

            controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[0];
            controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[1];

            for (int i = 0; i < arrowNodes.Count(); ++i)
            {
                var t = Mathf.Log(1f * i / (arrowNodes.Count-1) + 1f, 2f);

                arrowNodes[i].position = Mathf.Pow(1 - t, 3) * controlPoints[0]
                                         + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1]
                                         + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2]
                                         + Mathf.Pow(t, 3) * controlPoints[3];

                if (i > 0)
                {
                    var euler = new Vector3(0, 0,
                        Vector2.SignedAngle(Vector2.up, arrowNodes[i].position - this.arrowNodes[i - 1].position));
                    arrowNodes[i].rotation = Quaternion.Euler(euler);
                    
                }

                var scale = scaleFactor * (1f - 0.03f * (arrowNodes.Count() - 1 - i));
                arrowNodes[i].localScale = new Vector3(scale, scale, 1f);

                
            }

            arrowNodes[0].transform.rotation = arrowNodes[1].transform.rotation;
        }
    }
}