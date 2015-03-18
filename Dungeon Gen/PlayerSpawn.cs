using UnityEngine;
using System.Collections;

public class PlayerSpawn : Entity {

    public GameObject Player;
    private GameManager GM;

	// Use this for initialization
	void Start () {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (Player == null)
        {
            Debug.LogWarning("No Prefab assigned to a Player Spawn object!");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnPlayer()
    {
        GameObject Pl = (GameObject)Instantiate(Player, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
    }
}
