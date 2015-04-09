using UnityEngine;
using System.Collections;

public class GrassFlower : Block {

	public GrassFlower(): base() {
	}
	
	public override Tile TexturePosition() {
		Tile tile = new Tile();
		tile.x = 3;
		tile.y = 17;
		this.block_name = "grass_flower";
		
		return tile;
	}
}
