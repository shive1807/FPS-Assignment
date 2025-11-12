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

        // Camera rotation state
        private float _yaw = 0f;   // horizontal
        private float _pitch = 20f; // vertical
        private float _distance = 8f;

        private Vector2 _previousMousePosition;

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

            foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<LocalPlayerTag>())
            {
                var targetPos = transform.ValueRO.Position;

                // --- Handle input for rotation ---
                Vector2 inputDelta = Vector2.zero;

#if UNITY_EDITOR || UNITY_STANDALONE
                // Mouse input
                if (Input.GetMouseButton(1)) // Right mouse button held
                {
                    Vector2 mouseDelta = (Vector2)Input.mousePosition - _previousMousePosition;
                    inputDelta = mouseDelta * 0.2f; // sensitivity
                }
                _previousMousePosition = Input.mousePosition;
#else
                // Touch input (single finger drag)
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                        inputDelta = touch.deltaPosition * 0.2f;
                }
#endif

                _yaw += inputDelta.x;
                _pitch -= inputDelta.y;
                _pitch = Mathf.Clamp(_pitch, -20f, 80f);

                // --- Calculate camera position ---
                Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
                Vector3 offset = rotation * new Vector3(0, 2f, -_distance); // 2 units above player
                Vector3 desiredPos = AddVector(targetPos, offset);

                _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, desiredPos, 0.1f);
                _mainCamera.transform.LookAt(AddVector(targetPos , (Vector3.up * 1.5f))); // look slightly above player
            }
        }

        private Vector3 AddVector(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
    }
}
