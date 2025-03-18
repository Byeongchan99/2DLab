using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "New TileEffect", menuName = "DiceKingdom/TileEffect")]
    public abstract class TileEffect : ScriptableObject
    {
        // ���Ͱ� ȿ���� �ޱ� ������ �� ȣ��
        public abstract void OnEnter(GameObject target);

        // ���Ͱ� ���� Ÿ�Ͽ� �ӹ��� ���� �� ������ ȣ�� (deltaTime�� ���ڷ� �޾� Ÿ�̸� ���ҵ� ����)
        public abstract void OnStay(GameObject target, float deltaTime);

        // ���Ͱ� Ÿ���� ��� �� ȣ��
        public abstract void OnExit(GameObject target);
    }
}
