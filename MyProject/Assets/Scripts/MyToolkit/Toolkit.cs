using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace Utility
{
   public class MyToolKit
   {
      public static Vector3 GetRelativePosition(Transform origin, Vector3 position) {
         Vector3 distance = position - origin.position;
         Vector3 relativePosition = Vector3.zero;
         relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
         relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
         relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
	
         return relativePosition;
      }
      
      
      public static async void StartTimer(float delay, Action action)
      {
         await Task.Delay((int)delay * 1000);
         action?.Invoke();
      }

   }
   
   }