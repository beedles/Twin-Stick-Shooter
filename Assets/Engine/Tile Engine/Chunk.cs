using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour {
	
	public static int chunk_size = 16;
	public static int depth = 1;
	public bool update = false;
	public bool rendered;
	public World world;
	public WorldPos pos;
	
	Block [ , , ] blocks = new Block[chunk_size, chunk_size, 1];
	
	MeshFilter filter;
	MeshCollider coll;
	
	// Use this for initialization
	void Start () {
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();
		
		/*blocks = new Block[chunk_size, chunk_size, depth];
		
		for(int x = 0; x < chunk_size; ++x) {
			for(int y = 0; y < chunk_size; ++y) {
				for(int z = 0; z < depth; ++z) {
					blocks[x, y, z] = new CenterGrass();
				}
			}
		}
		
		int number_of_flowers = Random.Range(1, 5);
		for(int i = 0; i < number_of_flowers; ++i) {
			bool not_placed = true;
			while(not_placed) {			
				int x = Random.Range(0, chunk_size);
				int y = Random.Range(0, chunk_size);
				
				if(blocks[x, y, 0].block_name != "grass_flower") {
					blocks[x, y, 0] = new GrassFlower();
					not_placed = false;
				}
			}
		}
		
		
		Update_Chunk();*/
	}
	
	// Update is called once per frame
	void Update () {
		if (update) {
			update = false;
			Update_Chunk();
		}
	}
	
	public void Set_Block(int x, int y, int z, Block block) {
		if (In_Range(x) && In_Range(y) && In_Range(z)) {
			blocks[x, y, z] = block;
		} else {
			world.Set_Block (pos.x + x, pos.y + y, pos.z + z, block);
		}
	}
	
	public Block Get_Block(int x, int y, int z) {
		if(In_Range(x) && In_Range(y) && In_Range(z)) {
			return blocks[x, y, z];
		}
		return world.Get_Block(pos.x + x, pos.y + y, pos.z + z);
	}
	
	public static bool In_Range(int index) {
		if(index < 0 || index > chunk_size) {
			return false;
		}
		
		return true;
	}
	
	void Update_Chunk() {
		rendered = true;
		MeshData mesh_data = new MeshData();
		for(int x = 0; x < chunk_size; ++x) {
			for(int y = 0; y < chunk_size; ++y) {
				for(int z = 0; z < depth; ++z) {
					mesh_data = blocks[x, y, z].Block_Data(this, x, y, z, mesh_data);
				}
			}
		}
		
		Render_Mesh(mesh_data);
	}
	
	void Render_Mesh(MeshData mesh_data) {
		filter.mesh.Clear();
		filter.mesh.vertices = mesh_data.vertices.ToArray();
		filter.mesh.triangles = mesh_data.triangles.ToArray();
		
		filter.mesh.uv = mesh_data.uv.ToArray();
		filter.mesh.RecalculateNormals();
	}
}
