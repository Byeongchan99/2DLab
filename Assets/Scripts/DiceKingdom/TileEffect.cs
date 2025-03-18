using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "New TileEffect", menuName = "DiceKingdom/TileEffect")]
    public abstract class TileEffect : ScriptableObject
    {
        // 몬스터가 효과를 받기 시작할 때 호출
        public abstract void OnEnter(GameObject target);

        // 몬스터가 같은 타일에 머무는 동안 매 프레임 호출 (deltaTime을 인자로 받아 타이머 역할도 가능)
        public abstract void OnStay(GameObject target, float deltaTime);

        // 몬스터가 타일을 벗어날 때 호출
        public abstract void OnExit(GameObject target);
    }
}
