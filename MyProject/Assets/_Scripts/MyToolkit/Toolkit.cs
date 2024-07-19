using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility
{
   public class MyToolKit<T>
   {
      public static Vector3 GetRelativePosition(Transform origin, Vector3 position)
      {
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

      public static T[,] ConvertTo2DArray<T>(T[] inputArray, int rows, int cols)
      {
         if (inputArray.Length != rows * cols)
         {
            throw new ArgumentException("The length of the input array must match the product of rows and columns.");
         }

         T[,] result = new T[rows, cols];
         for (int i = 0; i < inputArray.Length; i++)
         {
            int row = rows - 1 - (i / cols);
            int col = i % cols;
            result[row, col] = inputArray[i];
         }

         return result;
      }

      
      /// <summary>
      /// 返回一个int，出现的数字概率等同于提供列表中对应index的概率
      /// </summary>
      /// <param name="rate"> 概率表</param>
      /// <returns></returns>
      public static int ChooseFromRandom(List<float> rate)
      {
         float f = Random.Range(0f, 1f);
         int index = 0;
         float sum = 0;
         foreach (var r in rate)
         {

            sum += r;
            if (f <= sum)
            {
               return index;
            }
            index++;
           
         }

         throw new Exception("所给列表点数和不足1");
      }


   }
}
   