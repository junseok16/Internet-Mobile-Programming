using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject[] mProjectilePrefab;    // 미사일을 발사할 때 생성할 프리팹을 불러옵니다.
    [SerializeField] private float mFireTime = 0.1f;    // 미사일의 재장전 시간을 선언합니다.
    private int mMaxProjectileLevel = 3;    // 미사일 업그레이드 최대 레벨을 선언합니다.
    private int mProjectileLevel = 1;
    private float cFireTime = 0.0f;

    public int projectileLevel
    {
        set => mProjectileLevel = Mathf.Clamp(value, 1, mMaxProjectileLevel);
        get => mProjectileLevel;
    }

    private void Update()
    {
        FireByProjectileLevel();
        Reload();
    }

    private void Reload()
    {
        cFireTime += Time.deltaTime;
    }

    private void FireByProjectileLevel()
    {
        if (cFireTime < mFireTime)
        {
            return;
        }
        GameObject cloneProjectile = null;

        switch (mProjectileLevel)
        {
            case 1:
                Instantiate(mProjectilePrefab[0], transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(mProjectilePrefab[1], transform.position + Vector3.up * 0.4f, Quaternion.identity);
                break;
            case 3:
                Instantiate(mProjectilePrefab[1], transform.position + Vector3.up * 0.4f, Quaternion.identity);
                cloneProjectile = Instantiate(mProjectilePrefab[2], transform.position + Vector3.left * 0.3f, Quaternion.identity);
                cloneProjectile.GetComponent<Move>().MoveTo(new Vector3(-0.15f, 1, 0));
                cloneProjectile = Instantiate(mProjectilePrefab[2], transform.position + Vector3.right * 0.3f, Quaternion.identity);
                cloneProjectile.GetComponent<Move>().MoveTo(new Vector3(0.15f, 1, 0));
                break;
        }
        cFireTime = 0.0f;
    }
}
