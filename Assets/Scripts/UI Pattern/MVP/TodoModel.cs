using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVP
{
    public class TodoModel
    {
        private List<string> todos = new List<string>();

        public IEnumerable<string> Todos => todos;

        // 사용자의 입력을 TodoModel에 추가
        public void AddTodoItem(string item)
        {
            Debug.Log("TodoModel 업데이트");
            if (!string.IsNullOrWhiteSpace(item))
            {
                todos.Add(item);
            }
        }
    }
}
