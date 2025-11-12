using Unity.Entities;
using UnityEngine;

namespace Common
{
    public class MobaPrefabsAuthoring : MonoBehaviour
    {
        public GameObject Champion;
        public GameObject Bullet;
        public class MobaPrefabsBaker : Baker<MobaPrefabsAuthoring>
        {
            public override void Bake(MobaPrefabsAuthoring authoring)
            {
                var prefabContainerEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(prefabContainerEntity, new MobaPrefabs
                {
                    ChampionPrefabEntity = GetEntity(authoring.Champion, TransformUsageFlags.Dynamic),
                    BulletPrefabEntity = GetEntity(authoring.Bullet, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}