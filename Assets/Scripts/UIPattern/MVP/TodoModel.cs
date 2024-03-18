using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVP
{
    public class TodoModel
    {
        private List<string> todos = new List<string>();

        public IEnumerable<string> Todos => todos;

        // ������� �Է��� TodoModel�� �߰�
        public void AddTodoItem(string item)
        {
            Debug.Log("TodoModel ������Ʈ");
            if (!string.IsNullOrWhiteSpace(item))
            {
                todos.Add(item);
            }
        }
    }
}
