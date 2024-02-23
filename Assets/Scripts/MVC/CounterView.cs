using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    public class CounterView : MonoBehaviour
    {
        public Text countText;

        public void UpdateCount(int count)
        {
            countText.text = $"Count: {count}";
        }
    }
}