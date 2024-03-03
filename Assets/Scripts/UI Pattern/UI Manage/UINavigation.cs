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
        /// <summary> UI ���� ���� </summary>
        private Stack<UIView> _viewStack = new Stack<UIView>();

        /// <summary> UI View �ν��Ͻ����� �̸����� �����ϱ� ���� ��ųʸ� </summary>
        private Dictionary<string, UIView> _views = new Dictionary<string, UIView>();

        /// <summary> ���� UI View�� ��ȯ�ϴ� ������Ƽ </summary>
        private UIView _Current => _viewStack.Count > 0 ? _viewStack.Peek() : null;

        /****************************************************************************
                                         private Methods
        ****************************************************************************/
        /// <summary> UIView �ν��Ͻ����� ����ϴ� �޼��� </summary>
        private void RegisterView(string name, UIView view)
        {
            if (!_views.ContainsKey(name))
            {
                _views.Add(name, view);
            }
        }

        /// <summary> viewName�� ���� UIView�� ���ÿ� �߰��ϰ� ��ȯ </summary>
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

        /// <summary> ���� UIView�� ����� ���� UIView�� ��ȯ </summary>
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

        /// <summary> Ư�� �̸��� UIView�� ���� ������ Pop </summary>
        private void PopTo(string viewName)
        {
            while (_viewStack.Count > 0 && _Current.gameObject.name != viewName)
            {
                Pop();
            }
        }

        /// <summary> ù ��° UIView�� ���� ������ ��� Pop </summary>
        private void PopToRoot()
        {
            while (_viewStack.Count > 1) // Root�� ���ܵα�
            {
                Pop();
            }
        }
    }
}
