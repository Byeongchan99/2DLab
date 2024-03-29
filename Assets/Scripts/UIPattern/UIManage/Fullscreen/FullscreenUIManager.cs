using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIManage
{
    public class FullscreenUIManager : MonoBehaviour
    {
        /****************************************************************************
                                         public Fields
        ****************************************************************************/
        /// <summary> UI 스택 수 </summary>
        public Text stackCount;

        /****************************************************************************
                                         private Fields
        ****************************************************************************/
        /// <summary> UI 관리 스택 </summary>
        private Stack<FullscreenUI> _fullscreenStack = new Stack<FullscreenUI>();

        /// <summary> UI 리스트 </summary>
        [SerializeField] private List<FullscreenUI> _fullscreenList = new List<FullscreenUI>();

        /// <summary> Fullscreen UI 인스턴스들을 이름으로 관리하기 위한 딕셔너리 </summary>
        private Dictionary<string, FullscreenUI> _fullscreenDictionary = new Dictionary<string, FullscreenUI>();

        /// <summary> 현재 FullscreenUI를 반환하는 프로퍼티 </summary>
        private FullscreenUI _current => _fullscreenStack.Count > 0 ? _fullscreenStack.Peek() : null;

        /****************************************************************************
                                         Unity Callbacks
        ****************************************************************************/
        private void Awake()
        {
            // 초기화
            Init();
            // 메인화면
            Push("Fullscreen 1"); 
        }

        private void Update()
        {
            // 숫자 키 1을 누르면 Fullscreen 1을 보여줌
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                Push("Fullscreen 1");
            }
            // 숫자 키 2를 누르면 Fullscreen 2를 보여줌
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                Push("Fullscreen 2");
            }
            // 숫자 키 3을 누르면 Fullscreen 3를 보여줌
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                Push("Fullscreen 3");
            }
            // 숫자 키 0을 누르면 첫 번째 FullscreenUI가 나올 때까지 모두 Pop - 홈으로 가기
            else if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                PopToRoot();
            }
            // Esc 키를 누르면 현재 FullscreenUI를 숨기고 이전 FullscreenUI를 반환 - 뒤로가기
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pop();
            }

            // 스택 수 업데이트
            stackCount.text =  "현재 스택 수: " + _fullscreenStack.Count.ToString();
        }

        /****************************************************************************
                                         private Methods
        ****************************************************************************/
        /// <summary> 초기화 </summary>
        private void Init()
        {
            // 리스트의 FullscreenUI 인스턴스들을 딕셔너리에 등록 및 비활성화
            foreach (var fullscreen in _fullscreenList)
            {
                RegisterUI(fullscreen.gameObject.name, fullscreen);
                fullscreen.gameObject.SetActive(false);
            }
        }

        /// <summary> FullscreenUI 인스턴스들을 딕셔너리에 등록하는 메서드 </summary>
        private void RegisterUI(string name, FullscreenUI fullscreen)
        {
            if (!_fullscreenDictionary.ContainsKey(name))
            {
                _fullscreenDictionary.Add(name, fullscreen);
            }
        }

        /// <summary> UIName을 가진 FullscreenUI를 스택에 추가하고 반환 </summary>
        private FullscreenUI Push(string UIName)
        {
            if (_fullscreenDictionary.TryGetValue(UIName, out FullscreenUI fullscreen))
            {
                if (_current != null)
                {
                    _current.Hide();
                }
                _fullscreenStack.Push(fullscreen);
                fullscreen.Show();
                return fullscreen;
            }
            else
            {
                Debug.LogError($"Fullscreen with name {UIName} not found.");
                return null;
            }
        }

        /// <summary> 현재 FullscreenUI를 숨기고 이전 FullscreenUI를 반환 </summary>
        private void Pop()
        {
            if (_fullscreenStack.Count > 0)
            {
                _fullscreenStack.Pop().Hide();
            }

            if (_current != null)
            {
                _current.Show();
            }
        }

        /// <summary> 특정 이름의 FullscreenUI가 나올 때까지 Pop </summary>
        private void PopTo(string UIName)
        {
            while (_fullscreenStack.Count > 0 && _current.gameObject.name != UIName)
            {
                Pop();
            }
        }

        /// <summary> 첫 번째 FullscreenUI가 나올 때까지 모두 Pop </summary>
        private void PopToRoot()
        {
            while (_fullscreenStack.Count > 1) // Root만 남겨두기
            {
                Pop();
            }
        }

        /****************************************************************************
                                 public Methods
        ****************************************************************************/
        /// <summary> 뒤로가기 버튼 클릭 메서드 </summary>
        public void OnBackButtonClicked()
        {
            Pop();
        }

        /// <summary> 홈 버튼 클릭 메서드 </summary>
        public void OnHomeButtonClicked()
        {
            PopToRoot();
        }

        /// <summary> 스테이지 선택창 버튼 클릭 메서드 </summary>
        public void OnStageSelectButtonClicked()
        {
            Push("Fullscreen 2");
        }

        /// <summary> 파티 편성창 버튼 클릭 메서드 </summary>
        public void OnPartyOrganizeButtonClicked()
        {
            Push("Fullscreen 3");
        }
    }
}
