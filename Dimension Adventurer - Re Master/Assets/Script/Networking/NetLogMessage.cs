using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Networking
{
    public class NetLogMessage
    {
        public const string CONNECTING = "Connecting . . .";
        public const string CONNECTED_TO_SERVER = "Connected to server";
        public const string CONNECT_FAIL = "Failed connect to server";
        public const string DISCONNECTED_FROM_SERVER = "Disconnected from server";

        public const string JOINING_MATCH = "Finding match . . .";
        public const string JOIN_RANDOM_FAIL = "Failed to find a match";
        public const string JOIN_PASSWORD_FAIL = "Failed to join a match";

        public const string CREATING_MATCH = "Creating a new match . . .";
        public const string CREATING_PASSWORD_MATCH = "Creating a new password match . . .";
        public const string WAITING_PLAYER = "Waiting for other players to join . . .";

        public const string OTHER_JOIN_ROOM = "New Player has joined the match";
        public const string OTHER_LEAVE_ROOM = "A player has lefted";
    }
}