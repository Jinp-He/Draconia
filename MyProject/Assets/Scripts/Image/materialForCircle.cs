using UnityEngine;
using UnityEngine.Rendering;

namespace Draconia.Image
{
    public class materialForCircle : UnityEngine.UI.Image
    {
        public override Material materialForRendering
        {
            get
            {
                Material mat = new Material(base.materialForRendering);
                mat.SetInt("_StencilComp",(int)CompareFunction.NotEqual);
                return mat;
            }
            
        }
    }
}