using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Field
{
    public class TurretFieldWeapon: ITurretWeapon
    {
        private TurretFieldWeaponAsset m_Asset;
        private TurretView m_View;
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private GameObject m_Field;
        
        private float m_MaxDistance;
        private int m_Damage = 10; // per second

        private List<Node> m_ReachableNodes;

        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = m_Asset.MaxDistance;
            m_ReachableNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
            m_Field = Object.Instantiate(asset.FieldPrefab, m_View.transform.position, Quaternion.identity);
            m_Field.transform.localScale += Vector3.one * asset.MaxDistance;
        }
        
        public void TickShoot()
        {
            TickWeapon();
        }

        private void TickWeapon()
        {
            foreach (Node reachableNode in m_ReachableNodes)
            {
                foreach (EnemyData enemyData in reachableNode.EnemyDatas)
                {
                    enemyData.GetDamage(m_Damage * Time.deltaTime);
                }
            }
        }
    }
}