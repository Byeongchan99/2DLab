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

        void Start()
        {
            // ���� ���� �� �÷��̾� ���� �ʱ�ȭ�ϰ� UI�� �ݿ�
            InitModel();
        }

        private void InitModel()
        {
            Strength = 0; // Strength ������Ƽ�� setter�� ȣ��ǰ�, OnPropertyChanged�� ȣ��
        }
    }
}