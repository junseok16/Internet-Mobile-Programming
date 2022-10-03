using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPLocater : MonoBehaviour
{
    // 몬스터 오브젝트와 Slider UI 사이의 거리를 선언합니다.
    [SerializeField] private Vector3 mDistance = Vector3.down * 50.0f;
    private Transform mTransform;
    private RectTransform mRectTransform;

    public void SetUp(Transform transform)
    {
        // Slider UI가 따라다닐 오브젝트를 설정합니다.
        mTransform = transform;
        mRectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (mTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 worldPosition = Camera.main.WorldToScreenPoint(mTransform.position);
        mRectTransform.position = worldPosition + mDistance;
    }
}
