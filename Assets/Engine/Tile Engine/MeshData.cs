using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData {
	public List<Vector3> vertices = new List<Vector3>();
	public List<int> triangles = new List<int>();
	public List<Vector2> uv = new List<Vector2>();
	
	public List<Vector3> col_vertices = new List<Vector3>();
	public List<int> col_triangles = new List<int>();
	
	public void Add_Quad_Triangles() {
		triangles.Add (vertices.Count - 4);
		triangles.Add (vertices.Count - 3);
		triangles.Add (vertices.Count - 2);
		
		triangles.Add (vertices.Count - 4);
		triangles.Add (vertices.Count - 2);
		triangles.Add (vertices.Count - 1);
	}
	
	/*public Mesh_Data() {
		
	}*/
}
