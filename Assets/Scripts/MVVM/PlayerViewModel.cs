using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MVVM
{
    public class PlayerViewModel : MonoBehaviour, INotifyPropertyChanged // 속성이 변경될 때 이벤트를 발생시키는 .NET 인터페이스
    {
        private PlayerModel playerModel = new PlayerModel();
        public event PropertyChangedEventHandler PropertyChanged; // 클래스의 속성 속성 중 하나가 변경될 때마다 발생하는 이벤트

        public int Strength
        {
            get => playerModel.Strength;
            set
            {
                if (playerModel.Strength != value)
                {
                    playerModel.Strength = value;
                    OnPropertyChanged(nameof(Strength));
                }
            }
        }

        public int Dexterity
        {
            get => playerModel.Dexterity;
            set
            {
                if (playerModel.Dexterity != value)
                {
                    playerModel.Dexterity = value;
                    OnPropertyChanged(nameof(Dexterity));
                }
            }
        }

        public int Intelligence
        {
            get => playerModel.Intelligence;
            set
            {
                if (playerModel.Intelligence != value)
                {
                    playerModel.Intelligence = value;
                    OnPropertyChanged(nameof(Intelligence));
                }
            }
        }

        public int Luck
        {
            get => playerModel.Luck;
            set
            {
                if (playerModel.Luck != value)
                {
                    playerModel.Luck = value;
                    OnPropertyChanged(nameof(Luck));
                }
            }
        }

        // 속성 이름을 매개변수로 받아 해당 속성이 변경되었음을 구독자(보통 UI 컴포넌트)에게 알림
        protected void OnPropertyChanged(string propertyName)
        {
            // PropertyChanged 이벤트가 null이 아닌 경우에만 이벤트를 발생시킴
            // Invoke는 대리자 또는 이벤트를 명시적으로 호출하는 데 사용
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IncreaseStrength()
        {
            Strength++;
        }

        public void IncreaseDexterity()
        {
            Dexterity++;
        }

        public void IncreaseIntelligence()
        {
            Intelligence++;
        }

        public void IncreaseLuck()
        {
            Luck++;
        }

        void Start()
        {
            // 게임 시작 시 플레이어 모델을 초기화하고 UI에 반영
            InitModel();
        }

        private void InitModel()
        {
            Strength = 0; // Strength 프로퍼티의 setter가 호출되고, OnPropertyChanged가 호출
            Dexterity = 0;
            Intelligence = 0;
            Luck = 0;
        }
    }
}