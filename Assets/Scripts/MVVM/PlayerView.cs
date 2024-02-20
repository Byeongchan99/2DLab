using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace MVVM
{
    public class PlayerView : MonoBehaviour
    {
        public PlayerViewModel viewModel;
        public Text strengthText; // Unity UI Text를 사용하여 스텟을 표시\
        public Button strengthButton; // 스텟을 증가시키는 버튼

        void Start()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            strengthButton.onClick.AddListener(viewModel.IncreaseStrength);

            // viewModel의 현재 Strength 값을 UI에 반영
            strengthText.text = viewModel.Strength.ToString();
        }

        // 스텟이 변경될 때마다 호출되는 이벤트 핸들러
        // PropertyChangedEventHandler 델리게이트를 사용하여 ViewModel의 PropertyChanged 이벤트에 바인딩
        // PropertyChangedEventArgs는 속성 변경 알림을 구독하는 리스너(예: UI 컨트롤)에게 어떤 속성이 변경되었는지 알려줌
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Strength")
            {
                strengthText.text = viewModel.Strength.ToString();
            }
        }
    }
}