using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIManage
{
    public class UINavigation : MonoBehaviour
    {
        /****************************************************************************
                                         private Fields
        ****************************************************************************/
        /// <summary> UI 관리 스택 </summary>
        private Stack<UIView> _viewStack = new Stack<UIView>();

        /// <summary> UI 리스트 </summary>
        [SerializeField] private List<UIView> _viewList = new List<UIView>();

        /// <summary> UI View 인스턴스들을 이름으로 관리하기 위한 딕셔너리 </summary>
        private Dictionary<string, UIView> _viewDictionary = new Dictionary<string, UIView>();

        /// <summary> 현재 UI View를 반환하는 프로퍼티 </summary>
        private UIView _current => _viewStack.Count > 0 ? _viewStack.Peek() : null;

        /****************************************************************************
                                         Unity Callbacks
        ****************************************************************************/
        void Start()
        {
            foreach (var view in _viewList)
            {
                RegisterView(view.gameObject.name, view);
            }
        }

        public void Update()
        {
            // 숫자 키 1을 누르면 view1을 보여줌
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                Push("View 1");
            }
            // 숫자 키 2를 누르면 view2를 보여줌
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                Push("View 2");
            }
            // 숫자 키 3을 누르면 view3를 보여줌
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                Push("View 3");
            }
        }

        /****************************************************************************
                                         private Methods
        ****************************************************************************/
        /// <summary> UIView 인스턴스들을 등록하는 메서드 </summary>
        private void RegisterView(string name, UIView view)
        {
            if (!_viewDictionary.ContainsKey(name))
            {
                _viewDictionary.Add(name, view);
            }
        }

        /// <summary> viewName을 가진 UIView를 스택에 추가하고 반환 </summary>
        private UIView Push(string viewName)
        {
            if (_viewDictionary.TryGetValue(viewName, out UIView view))
            {
                if (_current != null)
                {
                    _current.Hide();
                }
                _viewStack.Push(view);
                view.Show();
                return view;
            }
            else
            {
                Debug.LogError($"UIView with name {viewName} not found.");
                return null;
            }
        }

        /// <summary> 현재 UIView를 숨기고 이전 UIView를 반환 </summary>
        private void Pop()
        {
            if (_viewStack.Count > 0)
            {
                _viewStack.Pop().Hide();
            }

            if (_current != null)
            {
                _current.Show();
            }
        }

        /// <summary> 특정 이름의 UIView가 나올 때까지 Pop </summary>
        private void PopTo(string viewName)
        {
            while (_viewStack.Count > 0 && _current.gameObject.name != viewName)
            {
                Pop();
            }
        }

        /// <summary> 첫 번째 UIView가 나올 때까지 모두 Pop </summary>
        private void PopToRoot()
        {
            while (_viewStack.Count > 1) // Root만 남겨두기
            {
                Pop();
            }
        }
    }
}
