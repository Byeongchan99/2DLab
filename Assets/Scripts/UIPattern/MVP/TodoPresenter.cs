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
            Debug.Log("TodoView���� ������� �Է��� TodoPresenter���� ����");
            Debug.Log("TodoPresenter�� AddTodoItem ����");
            Debug.Log("TodoModel���� ������� �Է� ����");
            model.AddTodoItem(item);
            Debug.Log("TodoModel�� ��������� TodoView���� ����");
            UpdateTodoListInView();
            view.ClearInputField();
        }

        private void UpdateTodoListInView()
        {
            Debug.Log("TodoPresenter�� UpdateTodoListInView ����");
            view.UpdateTodoList(model.Todos);
        }
    }
}