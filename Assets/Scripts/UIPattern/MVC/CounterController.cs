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
            // 람다식을 사용하여 이벤트 핸들러를 추가
            model.OnCountChanged += (sender, count) => view.UpdateCount(count);
        }

        public void OnIncrementButtonClicked()
        {
            Debug.Log("Controller에서 사용자의 입력을 받음");
            Debug.Log("OnIncrementButtonClicked 실행");
            Debug.Log("Model에게 입력 전달");
            model.IncrementCount();
        }
    }
}