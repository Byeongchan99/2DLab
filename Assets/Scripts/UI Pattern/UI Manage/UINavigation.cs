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

        /// <summary> UI View 인스턴스들을 이름으로 관리하기 위한 딕셔너리 </summary>
        private Dictionary<string, UIView> _views = new Dictionary<string, UIView>();

        /// <summary> 현재 UI View를 반환하는 프로퍼티 </summary>
        private UIView _Current => _viewStack.Count > 0 ? _viewStack.Peek() : null;

        /****************************************************************************
                                         private Methods
        ****************************************************************************/
        /// <summary> UIView 인스턴스들을 등록하는 메서드 </summary>
        private void RegisterView(string name, UIView view)
        {
            if (!_views.ContainsKey(name))
            {
                _views.Add(name, view);
            }
        }

        /// <summary> viewName을 가진 UIView를 스택에 추가하고 반환 </summary>
        private UIView Push(string viewName)
        {
            if (_views.TryGetValue(viewName, out UIView view))
            {
                if (_Current != null)
                {
                    _Current.Hide();
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

            if (_Current != null)
            {
                _Current.Show();
            }
        }

        /// <summary> 특정 이름의 UIView가 나올 때까지 Pop </summary>
        private void PopTo(string viewName)
        {
            while (_viewStack.Count > 0 && _Current.gameObject.name != viewName)
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
