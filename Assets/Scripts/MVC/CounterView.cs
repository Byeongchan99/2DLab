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
            Debug.Log("Model�� ��������� View�� �ݿ�");
            Debug.Log("UpdateCount ����");
            countText.text = $"Count: {count}";
        }
    }
}