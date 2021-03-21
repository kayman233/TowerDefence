using Assets;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        public readonly EnemyAsset m_Asset;

        public EnemyView View => m_View;

        public EnemyData(EnemyAsset asset)
        {
            m_Asset = asset;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}