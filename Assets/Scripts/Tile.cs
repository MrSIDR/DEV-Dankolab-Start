using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Color deletedColor = Color.clear;

    private SpriteRenderer render;

    private float rebro;

    public bool isFound = false;

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        rebro = transform.localScale.x;
    }

    private void OnMouseDown()
    {
        if(render.color != deletedColor)
            AudioManager.instance.ClickTile();

        DeleteAllIdentical();
    }

    /// <summary>
    /// Метод ищет идентичные тайлы.
    /// </summary>
    /// <param name="searchDir">Вектор поиска.</param>
    /// <returns></returns>
    private List<GameObject> FindIdenticalObjects(Vector2 searchDir)
    {
        List<GameObject> identicalObjects = new List<GameObject>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, searchDir, rebro);
        Debug.DrawRay(transform.position, searchDir * rebro, Color.black, 10, false);

        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().color == render.color && hit.collider.GetComponent<Tile>().isFound == false)
        {
            identicalObjects.Add(hit.collider.gameObject);
            identicalObjects.AddRange(hit.collider.GetComponent<Tile>().DeleteIdentical(adjacentDirections));
        }

        return identicalObjects;
    }

    /// <summary>
    /// Метод 
    /// </summary>
    /// <param name="direction">Массив векторов поиска тайлов.</param>
    /// <returns></returns>
    private List<GameObject> DeleteIdentical(Vector2[] direction)
    {
        isFound = true;

        List<GameObject> identicalObjects = new List<GameObject>();

        for (int i = 0; i < direction.Length; i++)
        {
            identicalObjects.AddRange(FindIdenticalObjects(direction[i]));
        }

        return identicalObjects;
    }

    /// <summary>
    /// Метод очищает тайлы и добавляет очки.
    /// </summary>
    public void DeleteAllIdentical()
    {
        if (render.color == deletedColor)
            return;

        List<GameObject> identicalObjects = DeleteIdentical(adjacentDirections);

        if (identicalObjects.Count >= 2)
        {
            for (int i = 0; i < identicalObjects.Count; i++)
            {
                identicalObjects[i].GetComponent<Tile>().isFound = false;
                identicalObjects[i].GetComponent<SpriteRenderer>().color = deletedColor;
            }

            FieldManager.instance.GotPoints(identicalObjects.Count + 1);

            render.color = deletedColor;

            FieldManager.instance.FindAllDeleted();

            isFound = false;
        }
        else isFound = false;
    }
}
