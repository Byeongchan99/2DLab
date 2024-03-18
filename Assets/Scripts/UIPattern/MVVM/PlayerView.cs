using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MVVM
{
    public class PlayerView : MonoBehaviour
    {
        public PlayerViewModel viewModel;
        public Text strengthText, dexterityText, intelligenceText, luckText; // Unity UI Text를 사용하여 스텟을 표시
        public Button strengthButton, dexterityButton, intelligenceButton, luckButton; // 스텟을 증가시키는 버튼

        void Start()
        {
            // ViewModel을 초기화하고 PropertyChanged 이벤트에 대한 리스너를 등록
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            // 버튼 클릭 이벤트에 대한 리스너를 등록
            strengthButton.onClick.AddListener(viewModel.IncreaseStrength);
            dexterityButton.onClick.AddListener(viewModel.IncreaseDexterity);
            intelligenceButton.onClick.AddListener(viewModel.IncreaseIntelligence);
            luckButton.onClick.AddListener(viewModel.IncreaseLuck);

            // UI 텍스트를 초기화
            InitText();
        }

        private void InitText()
        {
            // viewModel의 현재 값들을 UI에 반영
            strengthText.text = viewModel.Strength.ToString();
            dexterityText.text = viewModel.Dexterity.ToString();
            intelligenceText.text = viewModel.Intelligence.ToString();
            luckText.text = viewModel.Luck.ToString();
        }

        // 스텟이 변경될 때마다 호출되는 이벤트 핸들러
        // PropertyChangedEventHandler 델리게이트를 사용하여 ViewModel의 PropertyChanged 이벤트에 바인딩
        // PropertyChangedEventArgs는 속성 변경 알림을 구독하는 리스너(예: UI 컨트롤)에게 어떤 속성이 변경되었는지 알려줌
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Log("PlayerView의 ViewModel_PropertyChanged 실행");
            Debug.Log("PlayerModel의 변경 사항이 PlayerView에 반영");
            // swith문을 사용하여 어떤 속성이 변경되었는지 확인
            switch (e.PropertyName)
            {
                case "Strength":
                    strengthText.text = viewModel.Strength.ToString();
                    break;
                case "Dexterity":
                    dexterityText.text = viewModel.Dexterity.ToString();
                    break;
                case "Intelligence":
                    intelligenceText.text = viewModel.Intelligence.ToString();
                    break;
                case "Luck":
                    luckText.text = viewModel.Luck.ToString();
                    break;
            }
        }
    }
}