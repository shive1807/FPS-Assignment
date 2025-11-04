using System;
using Common;
using TMPro;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Connection
{
    public class ClientConnectionManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField usernameInputField;
        
        [SerializeField]
        private TMP_Dropdown joinTypeDropdown;
        
        [SerializeField]
        private Button joinButton;

        private void OnEnable()
        {
            joinButton.onClick.AddListener(OnButtonConnect);
        }

        private void OnDisable()
        {
            joinButton.onClick.RemoveAllListeners();
        }

        private void OnButtonConnect()
        {
            DestroyLocalSimulationWorld();

            SceneManager.LoadScene(1);

            switch (joinTypeDropdown.value)
            {
                case 0 :
                    StartServer();
                    StartClient();
                    break;
                case 1:
                    StartClient();
                    break;
                default:
                    Debug.LogError($"Unknown join type: {joinTypeDropdown.value}");
                    break;
            }
        }

        private void DestroyLocalSimulationWorld()
        {
            foreach (var world in World.All)
            {
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                    break;
                }
            }
        }

        private void StartServer()
        {
            var serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");

            var serverEndpoint = NetworkEndpoint.AnyIpv4.WithPort(7979);
            {
                using var networkDriverQuery =
                    serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(serverEndpoint);
            }
        }

        private void StartClient()
        {
            var clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
            var connectionEndpoint = NetworkEndpoint.Parse("127.0.0.1", 7979);
            {
                using var networkDriverQuery = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, connectionEndpoint);
            }
            
            World.DefaultGameObjectInjectionWorld = clientWorld;

            var teamRequestEntity = clientWorld.EntityManager.CreateEntity();
            clientWorld.EntityManager.AddComponentData(teamRequestEntity, new ClientTeamRequest
            {
                Value = TeamType.Blue
            });
        }
    }
}
