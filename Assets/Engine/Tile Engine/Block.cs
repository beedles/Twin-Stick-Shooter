using UnityEngine;
using System.Collections;

public class Block {
	//Constants for texture mapping
	const float texture_width = 968f;
	const float texture_height = 526f;
	const float tile_width = 16f/texture_width;
	const float tile_height = 16f/texture_height;
	const float padding_width = 1f/texture_width;
	const float padding_height = 1f/texture_height;

	private bool solid = false;
	public string block_name = "";

	public struct Tile {
		public int x;
		public int y;
		/*public int sortingLayerID;
		public int sortingOrder;*/
	}
	
	public virtual Tile TexturePosition() {
		Tile tile = new Tile();
		tile.x = 3;
		tile.y = 17;
		
		return tile;
	}

	public Block() {
	
	}
	
	public virtual MeshData Block_Data (Chunk chunk, int x, int y, int z, MeshData mesh_data) {
		mesh_data.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z));
		mesh_data.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z));
		mesh_data.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z));
		mesh_data.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z));
		
		mesh_data.Add_Quad_Triangles();
		
		mesh_data.uv.AddRange(Face_UVs());
		
		return mesh_data;
	}
	
	public virtual bool Is_Solid() {
		return solid;
	}
	
	public virtual Vector2[] Face_UVs() {
		Vector2[] UVs = new Vector2[4];
		Tile tile_pos = TexturePosition();
		
		//Start from bottom right corner and work counter-clockwise
		UVs[0] = new Vector2(tile_width * tile_pos.x + tile_pos.x * padding_width + tile_width,
		                     tile_height * tile_pos.y + tile_pos.y * padding_height);
		UVs[1] = new Vector2(tile_width * tile_pos.x + tile_pos.x * padding_width + tile_width,
		                     tile_height * tile_pos.y + tile_pos.y * padding_height + tile_height);		                     
		UVs[2] = new Vector2(tile_width * tile_pos.x + tile_pos.x * padding_width,
		                     tile_height * tile_pos.y + tile_pos.y * padding_height + tile_height);
		UVs[3] = new Vector2(tile_width * tile_pos.x + tile_pos.x * padding_width,
							 tile_height * tile_pos.y + tile_pos.y * padding_height);
		
		return UVs;
	}
}
