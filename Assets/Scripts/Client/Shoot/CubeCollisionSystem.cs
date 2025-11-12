using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Client.Shoot
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial class CubeCollisionSystem : SystemBase
    {
        private StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnCreate()
        {
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

            var triggerJob = new TriggerJob
            {
                ECB = entityCommandBuffer
            };

            this._stepPhysicsWorld.PhysicsWorld
                .Simulation
                .ScheduleTriggerEventsJob(triggerJob, this.Dependency)
                .Complete();

            entityCommandBuffer.Playback(EntityManager);
            entityCommandBuffer.Dispose();
        }

        [BurstCompile]
        struct TriggerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer ECB;

            public void Execute(TriggerEvent triggerEvent)
            {
                // Example: CubeA collides with CubeB
                ECB.AddComponent(triggerEvent.EntityA, new Game.CubeCollisionEvent { OtherEntity = triggerEvent.EntityB });
                ECB.AddComponent(triggerEvent.EntityB, new Game.CubeCollisionEvent { OtherEntity = triggerEvent.EntityA });
            }
        }

    }
}