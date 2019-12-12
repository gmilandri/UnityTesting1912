using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

	private GameObject _terrainGrid;

	[SerializeField]
	private GameObject _terrainPrefab;

	public const int GridSize = 15;
	private const float DistanceBetweenCubes = 0.1f;

	private void GenerateWorld()
	{
		_terrainGrid = new GameObject("Terrain Grid");

		Transform parentTransform = _terrainGrid.transform;

		for (int i = 0; i < GridSize; i++)
		{
			for (int j = 0; j < GridSize; j++)
			{
				GameObject.Instantiate(_terrainPrefab, new Vector3(i + i * DistanceBetweenCubes, 0f, j + j * DistanceBetweenCubes), Quaternion.identity, parentTransform);
			}
		}
	}

	private void Start()
	{
		GenerateWorld();
	}

}
