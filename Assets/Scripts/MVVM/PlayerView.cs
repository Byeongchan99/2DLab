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
        public Text strengthText, dexterityText, intelligenceText, luckText; // Unity UI Text�� ����Ͽ� ������ ǥ��
        public Button strengthButton, dexterityButton, intelligenceButton, luckButton; // ������ ������Ű�� ��ư

        void Start()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            strengthButton.onClick.AddListener(viewModel.IncreaseStrength);
            dexterityButton.onClick.AddListener(viewModel.IncreaseDexterity);
            intelligenceButton.onClick.AddListener(viewModel.IncreaseIntelligence);
            luckButton.onClick.AddListener(viewModel.IncreaseLuck);

            InitText();
        }

        private void InitText()
        {
            // viewModel�� ���� Strength ���� UI�� �ݿ�
            strengthText.text = viewModel.Strength.ToString();
            dexterityText.text = viewModel.Dexterity.ToString();
            intelligenceText.text = viewModel.Intelligence.ToString();
            luckText.text = viewModel.Luck.ToString();
        }

        // ������ ����� ������ ȣ��Ǵ� �̺�Ʈ �ڵ鷯
        // PropertyChangedEventHandler ��������Ʈ�� ����Ͽ� ViewModel�� PropertyChanged �̺�Ʈ�� ���ε�
        // PropertyChangedEventArgs�� �Ӽ� ���� �˸��� �����ϴ� ������(��: UI ��Ʈ��)���� � �Ӽ��� ����Ǿ����� �˷���
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // swith���� ����Ͽ� � �Ӽ��� ����Ǿ����� Ȯ��
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