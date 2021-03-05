using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Asset Root", fileName = "Asset Root")]
    public class AssetRoot : ScriptableObject
    {
        public List<LevelAsset> Levels;
    }
}