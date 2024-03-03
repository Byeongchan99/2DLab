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

        /// <summary> UI 창의 상태 프로퍼티 </summary>
        private VisibleState State { get; set; } = VisibleState.Disappeared;

        /****************************************************************************
                                 public Methods
        ****************************************************************************/
        /// <summary> UI 요소를 보여주는 메서드 </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            State = VisibleState.Appearing;
            // DoTween 애니메이션을 사용하여 등장 애니메이션 구현
            transform.DOScale(Vector3.one, 0.5f).OnComplete(() => State = VisibleState.Appeared);
        }

        /// <summary> UI 요소를 숨기는 메서드 </summary>
        public void Hide()
        {
            State = VisibleState.Disappearing;
            // 예: DoTween 애니메이션을 사용하여 사라짐 애니메이션 구현
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                State = VisibleState.Disappeared;
                gameObject.SetActive(false);
            });
        }
    }
}