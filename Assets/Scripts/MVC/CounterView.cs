using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    public class CounterView : MonoBehaviour
    {
        public Text countText;

        private void Start()
        {
            countText.text = "Count: 0";
        }

        public void UpdateCount(int count)
        {
            Debug.Log("Model의 변경사항을 View에 반영");
            Debug.Log("UpdateCount 실행");
            countText.text = $"Count: {count}";
        }
    }
}