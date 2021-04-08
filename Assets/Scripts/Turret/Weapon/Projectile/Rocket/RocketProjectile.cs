using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile: MonoBehaviour, IProjectile
    {
        private float m_Speed;
        private float m_Damage;
        private float m_DamageRadius = 2f;
        private bool m_DidHit = false;
        private EnemyData m_Target;
        
        public void SetAsset(RocketProjectileAsset rocketProjectileAsset)
        {
            m_Speed = rocketProjectileAsset.Speed;
            m_Damage = rocketProjectileAsset.Damage;
        }
        
        public void TickApproaching()
        {
            Vector3 dir = (m_Target.View.transform.position - transform.position);
            Vector3 delta = dir.normalized * (m_Speed * Time.deltaTime);
            
            transform.Translate(delta, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            Vector3 position = transform.position;
            List<Node> reachableNodes = Game.Player.Grid.GetNodesInCircle(position, m_DamageRadius);
            foreach (Node reachableNode in reachableNodes)
            {
                foreach (EnemyData enemyData in reachableNode.EnemyDatas)
                {
                    enemyData.GetDamage(m_Damage);
                }
            }
            Destroy(gameObject);
        }

        public void SetTarget(EnemyData enemyData)
        {
            m_Target = enemyData;
        }
    }
}