using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadWorld : MonoBehaviour {
	public World world;
	
	public bool do_map = false;
	
	int timer = 0;
	
	List<WorldPos> chunk_update_list = new List<WorldPos>();
	List<WorldPos> chunk_build_list = new List<WorldPos>();
	
	static WorldPos[] chunk_positions = { 
		new WorldPos(-4, -4, 0), new WorldPos(-4, -3, 0), new WorldPos(-4, -2, 0), new WorldPos(-4, -1, 0), new WorldPos(-4, 0, 0), 
		new WorldPos(-4, 1, 0), new WorldPos(-4, 2, 0), new WorldPos(-4, 3, 0), new WorldPos(-4, 4, 0), new WorldPos(-3, -4, 0), 
		new WorldPos(-3, -3, 0), new WorldPos(-3, -2, 0), new WorldPos(-3, -1, 0), new WorldPos(-3, 0, 0), new WorldPos(-3, 1, 0), 
		new WorldPos(-3, 2, 0), new WorldPos(-3, 3, 0), new WorldPos(-3, 4, 0), new WorldPos(-2, -4, 0), new WorldPos(-2, -3, 0), 
		new WorldPos(-2, -2, 0), new WorldPos(-2, -1, 0), new WorldPos(-2, 0, 0), new WorldPos(-2, 1, 0), new WorldPos(-2, 2, 0), 
		new WorldPos(-2, 3, 0), new WorldPos(-2, 4, 0), new WorldPos(-1, -4, 0), new WorldPos(-1, -3, 0), new WorldPos(-1, -2, 0), 
		new WorldPos(-1, -1, 0), new WorldPos(-1, 0, 0), new WorldPos(-1, 1, 0), new WorldPos(-1, 2, 0), new WorldPos(-1, 3, 0), 
		new WorldPos(-1, 4, 0), new WorldPos(0, -4, 0), new WorldPos(0, -3, 0), new WorldPos(0, -2, 0), new WorldPos(0, -1, 0), 
		new WorldPos(0, 0, 0), new WorldPos(0, 1, 0), new WorldPos(0, 2, 0), new WorldPos(0, 3, 0), new WorldPos(0, 4, 0), 
		new WorldPos(1, -4, 0), new WorldPos(1, -3, 0), new WorldPos(1, -2, 0), new WorldPos(1, -1, 0), new WorldPos(1, 0, 0), 
		new WorldPos(1, 1, 0), new WorldPos(1, 2, 0), new WorldPos(1, 3, 0), new WorldPos(1, 4, 0), new WorldPos(2, -4, 0), 
		new WorldPos(2, -3, 0), new WorldPos(2, -2, 0), new WorldPos(2, -1, 0), new WorldPos(2, 0, 0), new WorldPos(2, 1, 0), 
		new WorldPos(2, 2, 0), new WorldPos(2, 3, 0), new WorldPos(2, 4, 0), new WorldPos(3, -4, 0), new WorldPos(3, -3, 0), 
		new WorldPos(3, -2, 0), new WorldPos(3, -1, 0), new WorldPos(3, 0, 0), new WorldPos(3, 1, 0), new WorldPos(3, 2, 0), 
		new WorldPos(3, 3, 0), new WorldPos(3, 4, 0), new WorldPos(4, -4, 0), new WorldPos(4, -3, 0), new WorldPos(4, -2, 0), 
		new WorldPos(4, -1, 0), new WorldPos(4, 0, 0), new WorldPos(4, 1, 0), new WorldPos(4, 2, 0), new WorldPos(4, 3, 0), 
		new WorldPos(4, 4, 0)};
	
	void Update() {
		Delete_Chunks();
		Find_Chunks_To_Load();		
		Load_And_Render_Chunks();
	}
	
	void Find_Chunks_To_Load() {
		WorldPos player_pos = new WorldPos(
			Mathf.FloorToInt(transform.position.x / Chunk.chunk_size) * Chunk.chunk_size,
			Mathf.FloorToInt(transform.position.y / Chunk.chunk_size) * Chunk.chunk_size,
			Mathf.FloorToInt(transform.position.z / Chunk.chunk_size) * Chunk.chunk_size);
		
		if (chunk_build_list.Count == 0) {
			for(int i = 0; i < chunk_positions.Length; i++) {
				WorldPos new_chunk_pos = new WorldPos(
					chunk_positions[i].x * Chunk.chunk_size + player_pos.x, 
					chunk_positions[i].y * Chunk.chunk_size + player_pos.y,
					0);
				Chunk new_chunk = world.Get_Chunk(new_chunk_pos.x, new_chunk_pos.y, new_chunk_pos.z);
				
				if (new_chunk != null && (new_chunk.rendered || chunk_update_list.Contains(new_chunk_pos))) {
					continue;
				}
				
				chunk_build_list.Add(new WorldPos(new_chunk_pos.x, new_chunk_pos.y, new_chunk_pos.z));
				
				return;
			}
		}
	}
	
	void Build_Chunk(WorldPos pos) {
		for(int x = pos.x - Chunk.chunk_size; x <= pos.x + Chunk.chunk_size; x += Chunk.chunk_size) {
			for(int y = pos.y - Chunk.chunk_size; y <= pos.y + Chunk.chunk_size; y += Chunk.chunk_size) {
				if (world.Get_Chunk(x, y, 0) == null) {
					world.Create_Chunk(x, y, 0);
				}
			}
		}
		
		chunk_update_list.Add(pos);
	}
	
	void Load_And_Render_Chunks() {
		for(int i = 0; i < 4; ++i) {
			if(chunk_build_list.Count != 0) {
				Build_Chunk(chunk_build_list[0]);
				chunk_build_list.RemoveAt (0);
			}
		}
		
		for(int i = 0; i < chunk_update_list.Count; i++) {
			Chunk chunk = world.Get_Chunk(chunk_update_list[0].x, chunk_update_list[0].y, chunk_update_list[0].z);
			if(chunk != null) {
				chunk.update = true;
			}
			chunk_update_list.RemoveAt(0);
		}
	}
	
	void Delete_Chunks() {
		if(timer == 10) {
			var chunks_to_delete = new List<WorldPos>();
			foreach(var chunk in world.chunks) {
				float distance = Vector3.Distance (
					new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, 0),
					new Vector3(transform.position.x, transform.position.y, 0));
					
				if(distance > 256) {
					chunks_to_delete.Add(chunk.Key);
				}
			}
			
			foreach(var chunk in chunks_to_delete) {
				world.Destroy_Chunk(chunk.x, chunk.y, chunk.z);
			}
			
			timer = 0;
		}
		
		timer++;
	}
}
