using DimensionAdventurer.Players;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DimensionAdventurer.Networking
{
    public class PlayerNet : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject model;
        [SerializeField] private GameObject[] nonRemoteObjects;
        [SerializeField] private MonoBehaviour[] nonRemoteScripts;
        public float Health { get => player.Health; set => player.Health = value; }
        public float Score { get => player.Score; set => player.Score = value; }
        public int Collected { get => player.Collected; set => player.Collected = value; }

        private void Awake()
        {
            // Not Multiplayer, disable this script
            if (!GamePreference.IsMultiplayer)
            {
                this.enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (!photonView.IsMine)
            {
                GetComponent<Collider>().enabled = false;
                foreach (GameObject go in nonRemoteObjects)
                    go.SetActive(false);
                foreach (MonoBehaviour mb in nonRemoteScripts)
                    mb.enabled = false;
            }
            else
            {
                model.SetActive(false);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.Health);
                stream.SendNext(this.Score);
                stream.SendNext(this.Collected);
            }
            else
            {
                // Network player, receive data
                this.Health = (float)stream.ReceiveNext();
                this.Score = (float)stream.ReceiveNext();
                this.Collected = (int)stream.ReceiveNext();
            }
        }
    }
}