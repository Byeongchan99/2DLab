using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    public class CounterController : MonoBehaviour
    {
        public CounterModel model;
        public CounterView view;

        void Start()
        {
            model = new CounterModel();
            // ���ٽ��� ����Ͽ� �̺�Ʈ �ڵ鷯�� �߰�
            model.OnCountChanged += (sender, count) => view.UpdateCount(count);
        }

        public void OnIncrementButtonClicked()
        {
            model.IncrementCount();
        }
    }
}