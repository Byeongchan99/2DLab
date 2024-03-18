using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVP
{
    public interface ITodoView
    {
        void UpdateTodoList(IEnumerable<string> todos);
        void ClearInputField();
    }

    public class TodoView : MonoBehaviour, ITodoView
    {
        public InputField inputField;
        public Button addButton;
        public GameObject todoItemPrefab;
        public Transform todoListContainer;
        public TodoPresenter presenter;

        private void Start()
        {
            presenter = new TodoPresenter(this);
            addButton.onClick.AddListener(() => presenter.AddTodoItem(inputField.text));
        }

        public void UpdateTodoList(IEnumerable<string> todos)
        {
            Debug.Log("TodoView�� UpdateTodoList ����");
            Debug.Log("TodoView���� ����ڿ��� ��� ǥ��");
            foreach (Transform child in todoListContainer)
            {
                Destroy(child.gameObject);
            }

            // TodoListContainer�� TodoItemPrefab�� �����Ͽ� TodoItem�� �߰�
            foreach (var todo in todos)
            {
                var item = Instantiate(todoItemPrefab, todoListContainer);
                item.GetComponentInChildren<Text>().text = todo;
            }
        }

        // ������� �Է��� ����� �޼���(inputField�� text�� ���)
        public void ClearInputField()
        {
            inputField.text = string.Empty;
        }
    }
}
