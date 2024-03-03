using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UIManage
{
    public class UIView : MonoBehaviour
    {
        /****************************************************************************
                                 private Fields
        ****************************************************************************/
        private enum VisibleState
        {
            Appearing,
            Appeared,
            Disappearing,
            Disappeared
        }

        /// <summary> UI â�� ���� ������Ƽ </summary>
        private VisibleState State { get; set; } = VisibleState.Disappeared;

        /****************************************************************************
                                 public Methods
        ****************************************************************************/
        /// <summary> UI ��Ҹ� �����ִ� �޼��� </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            State = VisibleState.Appearing;
            // DoTween �ִϸ��̼��� ����Ͽ� ���� �ִϸ��̼� ����
            transform.DOScale(Vector3.one, 0.5f).OnComplete(() => State = VisibleState.Appeared);
        }

        /// <summary> UI ��Ҹ� ����� �޼��� </summary>
        public void Hide()
        {
            State = VisibleState.Disappearing;
            // ��: DoTween �ִϸ��̼��� ����Ͽ� ����� �ִϸ��̼� ����
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                State = VisibleState.Disappeared;
                gameObject.SetActive(false);
            });
        }
    }
}