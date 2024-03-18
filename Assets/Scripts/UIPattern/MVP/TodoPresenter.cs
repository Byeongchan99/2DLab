using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVP
{
    public class TodoPresenter
    {
        private TodoModel model = new TodoModel();
        private ITodoView view;

        public TodoPresenter(ITodoView view)
        {
            this.view = view;
            UpdateTodoListInView();
        }

        public void AddTodoItem(string item)
        {
            Debug.Log("TodoView에서 사용자의 입력을 TodoPresenter에게 전달");
            Debug.Log("TodoPresenter의 AddTodoItem 실행");
            Debug.Log("TodoModel에게 사용자의 입력 전달");
            model.AddTodoItem(item);
            Debug.Log("TodoModel의 변경사항을 TodoView에게 전달");
            UpdateTodoListInView();
            view.ClearInputField();
        }

        private void UpdateTodoListInView()
        {
            Debug.Log("TodoPresenter의 UpdateTodoListInView 실행");
            view.UpdateTodoList(model.Todos);
        }
    }
}