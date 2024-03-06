using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManage
{
    public class UINavigation : MonoBehaviour
    {
        /****************************************************************************
                                         public Fields
        ****************************************************************************/
        /// <summary> UI ���� �� </summary>
        public Text stackCount;

        /****************************************************************************
                                         private Fields
        ****************************************************************************/
        /// <summary> UI ���� ���� </summary>
        private Stack<UIView> _viewStack = new Stack<UIView>();

        /// <summary> UI ����Ʈ </summary>
        [SerializeField] private List<UIView> _viewList = new List<UIView>();

        /// <summary> UI View �ν��Ͻ����� �̸����� �����ϱ� ���� ��ųʸ� </summary>
        private Dictionary<string, UIView> _viewDictionary = new Dictionary<string, UIView>();

        /// <summary> ���� UI View�� ��ȯ�ϴ� ������Ƽ </summary>
        private UIView _current => _viewStack.Count > 0 ? _viewStack.Peek() : null;

        /****************************************************************************
                                         Unity Callbacks
        ****************************************************************************/
        void Start()
        {
            // �ʱ�ȭ
            Init();
            // ����ȭ��
            Push("View 1"); 
        }

        public void Update()
        {
            // ���� Ű 1�� ������ view1�� ������
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                Push("View 1");
            }
            // ���� Ű 2�� ������ view2�� ������
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                Push("View 2");
            }
            // ���� Ű 3�� ������ view3�� ������
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                Push("View 3");
            }
            // ���� Ű 0�� ������ ù ��° UIView�� ���� ������ ��� Pop - Ȩ���� ����
            else if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                PopToRoot();
            }
            // Esc Ű�� ������ ���� UIView�� ����� ���� UIView�� ��ȯ - �ڷΰ���
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pop();
            }

            // ���� �� ������Ʈ
            stackCount.text =  "���� ���� ��: " + _viewStack.Count.ToString();
        }

        /****************************************************************************
                                         private Methods
        ****************************************************************************/
        /// <summary> �ʱ�ȭ </summary>
        private void Init()
        {
            foreach (var view in _viewList)
            {
                RegisterView(view.gameObject.name, view);
                view.gameObject.SetActive(false);
            }
        }


        /// <summary> UIView �ν��Ͻ����� ����ϴ� �޼��� </summary>
        private void RegisterView(string name, UIView view)
        {
            if (!_viewDictionary.ContainsKey(name))
            {
                _viewDictionary.Add(name, view);
            }
        }

        /// <summary> viewName�� ���� UIView�� ���ÿ� �߰��ϰ� ��ȯ </summary>
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

        /// <summary> ���� UIView�� ����� ���� UIView�� ��ȯ </summary>
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

        /// <summary> Ư�� �̸��� UIView�� ���� ������ Pop </summary>
        private void PopTo(string viewName)
        {
            while (_viewStack.Count > 0 && _current.gameObject.name != viewName)
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

        /****************************************************************************
                                 public Methods
        ****************************************************************************/
        /// <summary> �ڷΰ��� ��ư Ŭ�� �޼��� </summary>
        public void OnBackButtonClicked()
        {
            Pop();
        }

        /// <summary> Ȩ ��ư Ŭ�� �޼��� </summary>
        public void OnHomeButtonClicked()
        {
            PopToRoot();
        }

        /// <summary> �������� ����â ��ư Ŭ�� �޼��� </summary>
        public void OnStageSelectButtonClicked()
        {
            Push("View 2");
        }

        /// <summary> ��Ƽ ����â ��ư Ŭ�� �޼��� </summary>
        public void OnPartyOrganizeButtonClicked()
        {
            Push("View 3");
        }
    }
}