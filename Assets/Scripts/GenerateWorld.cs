using UnityEngine;
using System.Collections;

public class GenerateWorld : MonoBehaviour {

	public Transform player;
	public Transform gold; // gold prefab
	public Transform speedUp; // speedUp prefab
	public Transform speedDown; // speedDown prefab
	public Transform platform; // platform prefab
	public Transform block;
	private float randomRangeY = 2.0f; // how much up and down platforms will be.
	private float platformSpawnedTo = 0.0f; // position of last spawned platform
	private float nextCheck; // when to check.
	private ArrayList platforms = new ArrayList(); // spawned platforms
	private ArrayList collectables = new ArrayList(); // spawned collectables
	private ArrayList blocks = new ArrayList(); // spawned blocks

	// Update is called once per frame
	void Update () {
		float playerX = player.position.x;
		if(playerX > nextCheck)
		{
			maintenance(playerX);
		}
	}

	/* Checks if anything needed, for game world.
	 * If needed, deletes game objects which are behind the player and screen.
	 * If needed, spawns new platforms.
	 */ 
	private void maintenance(float playerX)
	{
		nextCheck = playerX + 30; // this 30, may be smaller or greater but not smaller than screen size.
		for (int i = platforms.Count-1; i >= 0; i--) 
		{
			Transform p = (Transform)platforms[i];
			if(p.position.x < (transform.position.x - 30)) // checks if game objects behind.
			{
				Destroy(p.gameObject);
				platforms.RemoveAt(i);
				collectables.RemoveAt(i);
				blocks.RemoveAt(i);
			}
		}
		spawnPlatforms(5); // why 5, because it is enough, player can't see more than 3 platforms at the same time.
	}

	/* Spawns platforms both randomly on x and on y.
	 * Also spawns a collectable and a block for each new platform.
	 * All positions calculated by using player's position.
	 */
	private void spawnPlatforms(int howMany)
	{
		float x = platformSpawnedTo;
		for(int i = 0; i<howMany; i++)
		{
			float y = Random.Range(-randomRangeY,randomRangeY);
			x += 8.0f;
			Vector3 pos = new Vector3(x, y, 0);
			Transform p = (Transform)Instantiate(platform, pos, Quaternion.identity);
			platforms.Add(p);
			Vector3 posCollectable = new Vector3(x+Random.Range(-3.0f,3.0f), y+Random.Range(0.9f,2.2f), 0);
			spawnCollectable(posCollectable);
			spawnBlock(pos);
		}
		platformSpawnedTo = x;
	}

	/* Spawns a block, on a platform.
	 */
	private void spawnBlock(Vector3 pos)
	{
		float rnd = Random.Range(-1.8f,1.8f);
		pos.x+=rnd;
		pos.y+=0.35f; 
		Transform b = (Transform)Instantiate(block,pos,Quaternion.identity);
		blocks.Add(b);

	}
	/* Spawns a collectable
	 * It is 80% Gold, 10% SpeedUp, 10% SpeedDown. 
	 */
	private void spawnCollectable(Vector3 pos)
	{
		int random = Random.Range(1,10);
		Transform collectable;
		if(random<8)
			collectable = (Transform)Instantiate(gold,pos, Quaternion.identity);
		else if(random<9)
			collectable = (Transform)Instantiate(speedUp,pos, Quaternion.identity);
		else
			collectable = (Transform)Instantiate(speedDown,pos, Quaternion.identity);
		collectables.Add(collectable);
	}
}
