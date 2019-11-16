using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    readonly Vector3 BULLET_IN_POOL_POSITION = new Vector3(1000,1000,1000);

    Dictionary<int, List<BulletBase>> bullets = new Dictionary<int, List<BulletBase>>();

    #region Mono
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region API
    public BulletBase TakeBullet(BulletBase _bullet)
    {
        if (instance.bullets.ContainsKey(_bullet.ID))
        {
            foreach (var b in instance.bullets[_bullet.ID])
            {
                if (b.state == BulletBase.State.Pooled)
                    return b;
            }
        }
        return ReturnBullet(_bullet);
    }

    public BulletBase ReturnBullet(BulletBase _bullet)
    {
        BulletBase bullet;

        if (_bullet.created)
        {
            bullet = _bullet;
            bullet.transform.position = BULLET_IN_POOL_POSITION;
        }
        else
        {
            bullet = Instantiate(_bullet, BULLET_IN_POOL_POSITION, Quaternion.identity);
            bullet.transform.SetParent(this.transform);
            bullet.created = true;
        }

        bullet.state = BulletBase.State.Pooled;

        if (instance.bullets.ContainsKey(bullet.ID))
        {
            instance.bullets[bullet.ID].Add(bullet);
        }
        else
        {
            instance.bullets.Add(bullet.ID, new List<BulletBase>());
            instance.bullets[bullet.ID].Add(bullet);
        }

        return bullet;
    }

    public void Shoot(BulletBase _bullet, Vector3 _shootPosition, Vector3 _direction, GameObject _shootable)
    {
        instance.TakeBullet(_bullet).Shoot(_shootPosition, _direction, _shootable);
    }
    #endregion
}