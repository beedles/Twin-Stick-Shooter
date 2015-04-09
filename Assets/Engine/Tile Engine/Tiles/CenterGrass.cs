using UnityEngine;
using System.Collections;

public class CenterGrass : Block {

	public CenterGrass(): base() {
	}
	
	public override Tile TexturePosition() {
		Tile tile = new Tile();
		tile.x = 3;
		tile.y = 14;
		
		return tile;
	}
}
