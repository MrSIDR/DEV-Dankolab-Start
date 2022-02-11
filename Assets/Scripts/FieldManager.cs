using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance = null;
    public static Action GettingPoints;

    public GameObject tile;
    public GameObject BackgroundField = null;

    [HideInInspector]
    public List<Color> characters = new List<Color>();

    public int SizeX = 16;
    public int SizeY = 10;
    public int StartColor = 3;
    [Range (0, 0.03f)]
    public float TileOffset = 0.01f;
    public int TileCost = 100;


    private int points = 0;

    private float backX;
    private float backY;

    private Vector3 Field;
    private Vector2 sizeField;

    private GameObject[,] tiles;
    private SpriteRenderer[,] tilesSpriteRend;

    private Color[] color = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };
    private Color deletedColor = Color.clear;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    //Запуск игры с настройками по умолчанию.
    void Start()
    {
        Field = BackgroundField.gameObject.transform.localScale;
        characters.Clear();
        characters.AddRange(color);

        CreateField(SizeX, SizeY, StartColor);
    }

    /// <summary>
    /// Метод задает размер тайла, заполняет игровое поле.
    /// </summary>
    /// <param name="rebro">Размер ребра тайла.</param>
    /// <param name="rangeColor">Количесвто цветов.</param>
    private void CreateField(int size_X, int size_Y, int rangeColor)
    {
        UpdatePoints();

        sizeField = new Vector2(size_X+1, size_Y+1);
        Vector2 sizeBackgroundField = Field;

        Vector2 offset = sizeBackgroundField / sizeField;
        float rebro = Mathf.Min(offset.x, offset.y) / 1.06f;
        tile.transform.localScale = new Vector3(rebro, rebro, 0);

        tiles = new GameObject[size_X, size_Y];
        tilesSpriteRend = new SpriteRenderer[size_X, size_Y];

        float startPosX = transform.position.x + tile.transform.localScale.x / 2 + TileOffset;
        float startPosY = transform.position.y + tile.transform.localScale.y / 2 + TileOffset;

        for (int x = 0; x < size_X; x++)  
            for(int y = 0; y < size_Y; y++)
            {
                tiles[x, y] = Instantiate(tile, new Vector3((startPosX + (rebro * x) + TileOffset * x), (startPosY + (rebro * y) + TileOffset * y), 0f), Quaternion.identity, transform);

                tilesSpriteRend[x, y] = tiles[x, y].GetComponent<SpriteRenderer>();
                tilesSpriteRend[x, y].color = characters[UnityEngine.Random.Range(0, rangeColor)];

                
            }

        backX = ((rebro * size_X) + TileOffset * size_X + TileOffset);
        backY = ((rebro * size_Y) + TileOffset * size_Y + TileOffset);

        FieldCentering();
    }

    /// <summary>
    /// Метод изменяет размер фонового изображения и центрирует игровое поле.
    /// </summary>
    private void FieldCentering()
    {
        BackgroundField.transform.localScale = new Vector3(backX, backY, 0);
        transform.position = new Vector3(BackgroundField.transform.position.x - (backX / 2), BackgroundField.transform.position.y - (backY / 2), 0);
    }

    /// <summary>
    /// Метод очищает игровое поле.
    /// </summary>
    private void ClearField()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    /// <summary>
    /// Метод описывает на нажатие кнопки Старт.
    /// </summary>
    /// <param name="x">Количество тайлов по ширине.</param>
    /// <param name="y">Количество тайлов по высоте.</param>
    /// <param name="color">Количество цветов.</param>
    public void ClickOnButton(int x, int y, int color)
    {
        ClearField();

        CreateField(x, y, color);
    }

    /// <summary>
    /// Метод ищет все прозрачные тайлы и "сдвигает" непрозрачные тайлы вниз.
    /// </summary>
    public void FindAllDeleted()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if(tilesSpriteRend[x,y].color == deletedColor)
                {
                    for (int i = y; i < tiles.GetLength(1); i++)
                        if(tilesSpriteRend[x, i].color != deletedColor)
                        {
                            tilesSpriteRend[x, y].color = tilesSpriteRend[x, i].color;
                            tilesSpriteRend[x, i].color = deletedColor;
                            break;
                        }
                }
            }
    }

    /// <summary>
    /// Метод суммирует количество очков.
    /// </summary>
    /// <param name="number">Количество тайлов.</param>
    public void GotPoints(int number)
    {
        points += number * TileCost;

        GettingPoints.Invoke();
    }

    /// <summary>
    /// Метод возвращает количество очков.
    /// </summary>
    /// <returns></returns>
    public int GivePoints()
    {
        return points;
    }

    /// <summary>
    /// Метод обнуляет количество очков
    /// </summary>
    private void UpdatePoints()
    {
        points = 0;

        GettingPoints.Invoke();
    }
}
