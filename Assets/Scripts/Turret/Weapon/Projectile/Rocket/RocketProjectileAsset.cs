using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAsset: ProjectileAssetBase
    {
        [SerializeField]
        private RocketProjectile m_RocketPrefab;
        
        public float Speed;
        public float Damage;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile rocketProjectile = Instantiate(m_RocketPrefab, origin, 
                Quaternion.LookRotation(originForward, Vector3.up));
            rocketProjectile.SetTarget(enemyData);
            rocketProjectile.SetAsset(this);
            return rocketProjectile;
        }
    }
}
