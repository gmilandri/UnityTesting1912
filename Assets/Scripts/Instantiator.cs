using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Instantiator : Singleton<Instantiator>
{
	public void WorldInstantiate(ISpawnable spawn)
	{
		if (WorldManager.Instance.EmptyGridCells.Count != 0)
		{
			int randomIndex;
			while (true)
			{
				randomIndex = UnityEngine.Random.Range(0, WorldManager.Instance.EmptyGridCells.Count);
				if (WorldManager.Instance.EmptyGridCells[randomIndex].ThisTileType != TileType.Water)
					break;

			}

			var myNewGridCell = WorldManager.Instance.EmptyGridCells[randomIndex];

			var pos = new Vector3(myNewGridCell.gameObject.transform.position.x - 2.5f, myNewGridCell.gameObject.transform.position.y, myNewGridCell.gameObject.transform.position.z - 2.5f);

			spawn.ThisGameObject().transform.position = pos;

			if (!WorldManager.Instance.WorldObjects.Contains(spawn))
			{
				WorldManager.Instance.WorldObjects.Add(spawn);
				if (spawn is IResource)
					WorldManager.Instance.Resources.Add((IResource)spawn);
				if (spawn is Human)
					spawn.ThisGameObject().GetComponent<NavMeshAgent>().enabled = true;
			}

			if (spawn is IStatic)
			{
				IStatic staticSpawn = (IStatic)spawn;
				myNewGridCell.GridObject = staticSpawn;

				staticSpawn.SetCell(myNewGridCell);

				WorldManager.Instance.EmptyGridCells.Remove(myNewGridCell);
				WorldManager.Instance.OccupiedGridCells.Add(myNewGridCell);
			}

		}
		else
		{
			Debug.LogError("No valid position found for " + spawn.ThisGameObject().name);
		}
	}
}
