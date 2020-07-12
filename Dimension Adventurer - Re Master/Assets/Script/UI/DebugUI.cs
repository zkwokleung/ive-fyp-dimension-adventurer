using DimensionAdventurer.Players;
using DimensionAdventurer.Players.Abilities;
using DimensionAdventurer.Players.Modifiers;
using DimensionAdventurer.Players.Controls;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DimensionAdventurer.UI
{
    public class DebugUI : MonoBehaviour
    {
        public Text txtField_1;
        public Text txtField_2;
        public Text txtField_3;

        private Player player;
        private PlayerAbility ability;
        private Teleport indicator;

        private void Update()
        {
            //if (manager != null)
            //    txtField_1.text = string.Format("adress: {0}\nport: {1}\nmax connection: {2}\nconnects: {3}", NetworkManager.singleton.networkAddress, manager.networkPort, manager.maxConnections, NetworkServer.connections.Count);

            if (player != null)
            {
                txtField_2.text = string.Format("Running on: {0}\nFloor: {1}", player.WorldPosition.Track, player.WorldPosition.Plane);
                txtField_3.text = string.Format("TeleportMode: {0}\nTeleport floor: {1}\nTeleport track: {2}", ability.TeleportMode, indicator.TeleportPlane, indicator.TeleportTrack);
            }
        }

        private void OnEnable()
        {
            player = GameManager.GetPlayer("LocalPlayer");
            ability = player.GetComponent<PlayerAbility>();
            indicator = player.GetComponent<Teleport>();
        }
    }
}