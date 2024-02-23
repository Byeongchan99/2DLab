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
            Count++;
            OnCountChanged?.Invoke(this, Count);
        }

        public event Action<CounterModel, int> OnCountChanged;
    }
}