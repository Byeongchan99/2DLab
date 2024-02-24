using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    public class CounterModel
    {
        public int Count { get; private set; }

        public void IncrementCount()
        {
            Debug.Log("Model 업데이트");
            Debug.Log("Model에서 카운트 증가");
            Count++;
            OnCountChanged?.Invoke(this, Count);
            Debug.Log("Model의 OnCountChanged가 실행되면 View의 UpdateCount가 실행");
        }

        public event Action<CounterModel, int> OnCountChanged;
    }
}