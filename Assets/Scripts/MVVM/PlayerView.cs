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
        public Text strengthText; // Unity UI Text�� ����Ͽ� ������ ǥ��\
        public Button strengthButton; // ������ ������Ű�� ��ư

        void Start()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            strengthButton.onClick.AddListener(viewModel.IncreaseStrength);

            // viewModel�� ���� Strength ���� UI�� �ݿ�
            strengthText.text = viewModel.Strength.ToString();
        }

        // ������ ����� ������ ȣ��Ǵ� �̺�Ʈ �ڵ鷯
        // PropertyChangedEventHandler ��������Ʈ�� ����Ͽ� ViewModel�� PropertyChanged �̺�Ʈ�� ���ε�
        // PropertyChangedEventArgs�� �Ӽ� ���� �˸��� �����ϴ� ������(��: UI ��Ʈ��)���� � �Ӽ��� ����Ǿ����� �˷���
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Strength")
            {
                strengthText.text = viewModel.Strength.ToString();
            }
        }
    }
}