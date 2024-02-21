using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MVVM
{
    public class PlayerViewModel : MonoBehaviour, INotifyPropertyChanged // �Ӽ��� ����� �� �̺�Ʈ�� �߻���Ű�� .NET �������̽�
    {
        private PlayerModel playerModel = new PlayerModel();
        public event PropertyChangedEventHandler PropertyChanged; // Ŭ������ �Ӽ� �Ӽ� �� �ϳ��� ����� ������ �߻��ϴ� �̺�Ʈ

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

        // �Ӽ� �̸��� �Ű������� �޾� �ش� �Ӽ��� ����Ǿ����� ������(���� UI ������Ʈ)���� �˸�
        protected void OnPropertyChanged(string propertyName)
        {
            // PropertyChanged �̺�Ʈ�� null�� �ƴ� ��쿡�� �̺�Ʈ�� �߻���Ŵ
            // Invoke�� �븮�� �Ǵ� �̺�Ʈ�� ��������� ȣ���ϴ� �� ���
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
            // ���� ���� �� �÷��̾� ���� �ʱ�ȭ�ϰ� UI�� �ݿ�
            InitModel();
        }

        private void InitModel()
        {
            Strength = 0; // Strength ������Ƽ�� setter�� ȣ��ǰ�, OnPropertyChanged�� ȣ��
            Dexterity = 0;
            Intelligence = 0;
            Luck = 0;
        }
    }
}