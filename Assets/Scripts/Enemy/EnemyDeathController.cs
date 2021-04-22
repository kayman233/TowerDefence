using System.Collections.Generic;
using Runtime;

namespace Enemy
{
    public class EnemyDeathController : IController
    {
        private List<EnemyData> n_DiedEnemyDatas = new List<EnemyData>();
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            foreach (EnemyData enemyData in Game.Player.EnemyDatas)
            {
                if (enemyData.IsDead)
                {
                    n_DiedEnemyDatas.Add(enemyData);
                    Game.Player.TurretMarket.GetReward(enemyData);
                    enemyData.Die();
                }
            }
            
            foreach (EnemyData enemyData in n_DiedEnemyDatas)
            {
                Game.Player.EnemyDied(enemyData);
            }
            
            n_DiedEnemyDatas.Clear();
        }
    }
}