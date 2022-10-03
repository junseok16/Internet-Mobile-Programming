using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFire : MonoBehaviour {
    [SerializeField] private GameObject mProjectilePrefab = null;// 미사일을 발사할 때 생성할 프리팹을 불러옵니다.
    public float mFireTime = 0.0f;// 미사일의 재장전 시간을 선언합니다.
    private float cFireTime = 0.0f;

    private void Update() {
        FireByProjectileLevel();
        Reload();
    }

    private void Reload() {
        cFireTime += Time.deltaTime;
    }

    private void FireByProjectileLevel() {
        if (cFireTime < mFireTime || mFireTime == 0) {
            return;
        }
        Instantiate(mProjectilePrefab, transform.position + Vector3.down * 0.4f, Quaternion.Euler(0, 0, -90.0f));
        cFireTime = 0.0f;
    }
}
