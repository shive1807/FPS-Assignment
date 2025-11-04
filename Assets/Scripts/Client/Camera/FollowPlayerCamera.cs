using Common;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Client.Camera
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial class CameraFollowSystem : SystemBase
    {
        private UnityEngine.Camera _mainCamera;

        protected override void OnStartRunning()
        {
            _mainCamera = UnityEngine.Camera.main;
        }

        protected override void OnUpdate()
        {
            if (_mainCamera == null)
            {
                _mainCamera = UnityEngine.Camera.main;
                if (_mainCamera == null)
                {
                    Debug.LogWarning("❌ Main camera not found in scene!");
                    return;
                }
            }

            bool foundPlayer = false;

            foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<LocalPlayerTag>())
            {
                foundPlayer = true;
                var pos = transform.ValueRO.Position;
        
                _mainCamera.transform.position = Vector3.Lerp(
                    _mainCamera.transform.position,
                    new Vector3(pos.x, pos.y + 5f, pos.z - 8f),
                    0.1f
                );
                _mainCamera.transform.LookAt(pos);
            }

            if (!foundPlayer)
                Debug.LogWarning("⚠️ No entity found with LocalPlayerTag");
        }

    }
}