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
            Debug.Log("Model ������Ʈ");
            Debug.Log("Model���� ī��Ʈ ����");
            Count++;
            OnCountChanged?.Invoke(this, Count);
            Debug.Log("Model�� OnCountChanged�� ����Ǹ� View�� UpdateCount�� ����");
        }

        public event Action<CounterModel, int> OnCountChanged;
    }
}