using UnityEngine;
using UnityEngine.Networking;

public class TestNetworkManager : NetworkManager
{
    public ui canvas;
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        var count_players = NetworkServer.connections.Count;
        if (count_players <= startPositions.Count) {
            if (count_players == 2) {
                canvas.createBall();
                NetworkServer.Spawn(canvas.GetBall());
            }
            string player_tag = "racket_up";
            if (count_players == 1) {
                player_tag = "racket_down";
            }
            GameObject player = Instantiate(playerPrefab, startPositions[count_players - 1].position, Quaternion.identity);
            player.tag = player_tag;
            platform platform = player.GetComponent<platform>();
            platform.canvas = canvas;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        } else {
            conn.Disconnect();
        }
    }
}
