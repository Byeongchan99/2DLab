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
            Debug.Log("TodoView의 UpdateTodoList 실행");
            Debug.Log("TodoView에서 사용자에게 결과 표시");
            foreach (Transform child in todoListContainer)
            {
                Destroy(child.gameObject);
            }

            // TodoListContainer에 TodoItemPrefab을 생성하여 TodoItem을 추가
            foreach (var todo in todos)
            {
                var item = Instantiate(todoItemPrefab, todoListContainer);
                item.GetComponentInChildren<Text>().text = todo;
            }
        }

        // 사용자의 입력을 지우는 메서드(inputField의 text를 비움)
        public void ClearInputField()
        {
            inputField.text = string.Empty;
        }
    }
}
