using UnityEngine;
using System.Collections ;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class CellManager : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private Color _aliveColor;
    [SerializeField] private Color _deadColor;
    private Image[,] _cells;
    public bool[,] map;
    public bool active = false;
    
    void Start()
    {
        MapCreateSize();
        MapRender();
        StartCoroutine(FixedStep());
    }
    
    private IEnumerator FixedStep()
    {
        while (true)
        {
            if (active)
            {
                Logic();
                MapRender();
                yield return new WaitForSeconds(_speed);
            }
            yield return new WaitForSeconds(_speed);
        }
    }
    
    GameObject CreateCell(Vector2 position)
    {
        GameObject cell = Instantiate<GameObject>(_cellPrefab);
        cell.transform.SetParent(gameObject.transform,false);
        cell.GetComponent<RectTransform>().localPosition = new Vector3(_size.x * position.x , _size.y * position.y, 0);
        cell.GetComponent<RectTransform>().sizeDelta = _size;
        cell.GetComponent<CellIndividual>().position = position;
        cell.GetComponent<CellIndividual>().cellManager = this;
        cell.name = "Cell " + position.ToString();
        return cell;
    }

    private void MapCreateSize()
    {
        map = new bool[(int)_mapSize.x, (int)_mapSize.y];
        _cells = new Image[(int)_mapSize.x, (int)_mapSize.y];
        for (int j = 0; j < _mapSize.x; j++)
        {
            for (int i = 0; i < _mapSize.y; i++)
            {
                map[j, i] = false;
                if (_cells[j, i] == null)
                {
                    _cells[j, i] = CreateCell(new  Vector2(j, i)).GetComponent<Image>();
                }
            }
        }
    }
    public void MapRender()
    {
        for (int j = 0; j < _mapSize.x; j++)
        {
            for (int i = 0; i < _mapSize.y; i++)
            {
                if (map[j, i])
                {
                    _cells[j, i].color = _aliveColor;
                }
                else
                {
                    _cells[j, i].color = _deadColor;
                }
            }
        }
    }

    private int CheckNeighbours(Vector2 centerposition)
    {
        int neighbours = 0;
        for (int x = (int)centerposition.x-1 ; x < centerposition.x+2; x++)
        {
            for (int y = (int)centerposition.y-1 ; y < centerposition.y+2; y++)
            {
                if (x >= 0 && x < _mapSize.x && y >= 0 && y < _mapSize.y && new Vector2(x , y) != centerposition)
                {
                    if (map[x, y] )
                    {
                        neighbours++;
                    }
                }
            }
        }
        return neighbours;
    }

    public void Logic()
    {
        bool[,] tempmap =  new bool[(int)_mapSize.x, (int)_mapSize.y];
        for (int x = 0; x < _mapSize.x; x++)
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                int neighbours = CheckNeighbours(new Vector2(x, y));
                if (map[x, y])
                {
                    if (neighbours == 0){ tempmap[x, y] = false;}
                    if (neighbours >= 2){ tempmap[x, y] = true;}
                    if (neighbours > 3){ tempmap[x, y] = false;}
                }
                else
                {
                    if (neighbours == 3){ tempmap[x, y] = true;}
                }
            }
        }
        map = tempmap;
    }

    public void SetSpeed(Slider slider)
    {
        _speed = slider.value;
    }

    public void SetActive(bool tempActive)
    {
        active = tempActive;
    }

    public void MapReset()
    {
        map = new bool[(int)_mapSize.x, (int)_mapSize.y];
        MapRender();
    }

    public void MapRandomize(Slider slider)
    {
        for (int x = 0; x < _mapSize.x; x++)
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                if (Random.Range(0f, 1f)<=slider.value)
                {
                    map[x, y] = true;
                }
            }
        }
        MapRender();
    }
    
}
