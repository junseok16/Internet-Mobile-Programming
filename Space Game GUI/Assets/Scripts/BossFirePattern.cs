using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 열거체
 *  (0) 보스의 첫 번째 패턴으로 앞으로 미사일을 발사합니다.
 *  (1) 보스의 두 번째 패턴으로 원으로 미사일을 발사합니다.
 *  (2) 보스의 세 번째 패턴으로 좌우로 이동하며 미사일을 발사합니다.
 *  (3) 보스의 네 번째 패턴으로 두 번째 패턴과 세 번째 패턴으로 미사일을 발사합니다.
 */
public enum FirePattern {
    ForwardFire = 0,
    CircleFire = 1,
    CurveFire = 2,
    CircleCurveFire = 3
}

public class BossFirePattern : MonoBehaviour {
    [SerializeField] private GameObject[] projectilePrefab;

    public void StartFire(FirePattern pattern)
    {
        StartCoroutine(pattern.ToString());
    }

    public void StopFire(FirePattern pattern)
    {
        StopCoroutine(pattern.ToString());
    }

    private IEnumerator ForwardFire()
    {
        float attackRate = 0.6f;
        while (true)
        {
            GameObject cloneProjectile1 = Instantiate(projectilePrefab[2], transform.position + Vector3.down * 1.60f, Quaternion.Euler(0, 0, -90.0f));
            cloneProjectile1.GetComponent<Move>().MoveTo(Vector3.down);
            yield return new WaitForSeconds(attackRate);

            GameObject cloneProjectile0 = Instantiate(projectilePrefab[1], transform.position + Vector3.down * 1.25f + Vector3.left * 1.1f, Quaternion.Euler(0, 0, -90.0f));
            GameObject cloneProjectile2 = Instantiate(projectilePrefab[1], transform.position + Vector3.down * 1.25f + Vector3.right * 1.1f, Quaternion.Euler(0, 0, -90.0f));
            cloneProjectile0.GetComponent<Move>().MoveTo(Vector3.down);
            cloneProjectile2.GetComponent<Move>().MoveTo(Vector3.down);
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator CircleFire()
    {
        float attackRate = 3.0f;
        int count = 20;
        float internalAngle = 360 / count;
        float weightAngle = 0;

        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject cloneProjectile = Instantiate(projectilePrefab[0], transform.position, Quaternion.Euler(0, 0, -90.0f));
                float angle = weightAngle + internalAngle * i;
                float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
                cloneProjectile.GetComponent<Move>().MoveTo(new Vector2(x, y));
            }
            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < 3; ++i)
            {
                GameObject cloneProjectile1 = Instantiate(projectilePrefab[2], transform.position + Vector3.down * 1.60f, Quaternion.Euler(0, 0, -90.0f));
                cloneProjectile1.GetComponent<Move>().MoveTo(Vector3.down);
                yield return new WaitForSeconds(0.2f);
            }

            weightAngle++;
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator CurveFire()
    {
        Vector3 targetPosition = Vector3.zero + Vector3.down * 2.5f;
        float attackRate = 0.4f;

        while (true)
        {
            GameObject cloneProjectile = Instantiate(projectilePrefab[2], transform.position + Vector3.down * 1.5f, Quaternion.identity);
            Vector3 direction = (targetPosition - cloneProjectile.transform.position).normalized;
            cloneProjectile.GetComponent<Move>().MoveTo(direction);
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator CircleCurveFire()
    {
        Vector3 targetPosition = Vector3.zero + Vector3.down * 2.5f;
        float attackRate = 3.0f;
        int count = 20;
        float internalAngle = 360 / count;
        float weightAngle = 0;

        while (true)
        {
            for (int i = 0; i < 5; ++i)
            {
                GameObject cloneProjectile = Instantiate(projectilePrefab[2], transform.position + Vector3.down * 1.5f, Quaternion.identity);
                Vector3 direction = (targetPosition - cloneProjectile.transform.position).normalized;
                cloneProjectile.GetComponent<Move>().MoveTo(direction);
                yield return new WaitForSeconds(0.4f);
            }

            for (int i = 0; i < count; ++i)
            {
                GameObject cloneProjectile2 = Instantiate(projectilePrefab[0], transform.position, Quaternion.Euler(0, 0, -90.0f));
                float angle = weightAngle + internalAngle * i;
                float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
                cloneProjectile2.GetComponent<Move>().MoveTo(new Vector2(x, y));
            }
            yield return new WaitForSeconds(attackRate);
        }
    }
}
