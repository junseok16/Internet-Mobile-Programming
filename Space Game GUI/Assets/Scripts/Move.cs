using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // 오브젝트의 속력을 설정합니다.
    [SerializeField] private float mSpeed = 0.0f;
    // 오브젝트의 방향을 설정합니다.
    [SerializeField] private Vector3 mDirection = Vector3.zero;

    // Update is called once per frame
    private void Update()
    {
        // 속력과 방향에 따라서 오브젝트를 움직입니다.
        transform.position += mDirection * mSpeed * Time.deltaTime;
    }

    // 스크립트 외부에서 호출하여 방향을 재설정합니다.
    public void MoveTo(Vector3 direction)
    {
        mDirection = direction;
    }
}
