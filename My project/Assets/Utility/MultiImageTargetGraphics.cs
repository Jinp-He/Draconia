using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class MultiImageTargetGraphics : MonoBehaviour
    {
        [SerializeField] private Graphic[] targetGraphics;
 
        public Graphic[] GetTargetGraphics => targetGraphics;
    }
}