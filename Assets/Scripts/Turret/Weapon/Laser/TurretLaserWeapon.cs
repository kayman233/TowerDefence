using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Laser
{
    public class TurretLaserWeapon: ITurretWeapon
    {
        private TurretLaserWeaponAsset m_Asset;
        private TurretView m_View;
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private LineRenderer m_LineRenderer;
        
        private float m_MaxDistance;
        private int m_Damage = 10; // per second

        private List<Node> m_ReachableNodes;

        public TurretLaserWeapon(TurretLaserWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = m_Asset.MaxDistance;
            m_ReachableNodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
            m_LineRenderer = Object.Instantiate(
                asset.LineRendererPrefab, m_View.transform.position, Quaternion.identity);
            m_LineRenderer.SetPosition(0, Vector3.zero);
            m_LineRenderer.gameObject.SetActive(false);
        }
        
        public void TickShoot()
        {
            TickWeapon();
            TickTower();
        }

        private void TickWeapon()
        {
            m_ClosestEnemyData = 
                EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_ReachableNodes);

            if (m_ClosestEnemyData == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
            }
            else
            {
                Vector3 projectilePosition = m_View.ProjectileOrigin.position;
                m_LineRenderer.transform.position = projectilePosition;
                m_LineRenderer.SetPosition(1,
                    m_ClosestEnemyData.View.transform.position - projectilePosition);
                m_LineRenderer.gameObject.SetActive(true);
                
                m_ClosestEnemyData.GetDamage(m_Damage * Time.deltaTime);
            }
            TickTower();
        }

        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }
    }
}