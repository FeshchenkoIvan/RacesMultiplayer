using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float minX, minZ, maxX, maxZ;

    // Update is called once per frame
    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 1, Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }
}
