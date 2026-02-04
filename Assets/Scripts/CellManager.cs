using UnityEngine;
using System.Collections.Generic;
using System.Collections ;
using Image = UnityEngine.UI.Image;

public class CellManager : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private Color _aliveColor;
    [SerializeField] private Color _deadColor;
    private GameObject[,] _cells;
    private bool[,] _map;
    
    void Start()
    {
        MapCreateSize();
        _map[1, 1] = true;
        _map[1, 2] = true;
        _map[1, 3] = true;
        MapRender();
        StartCoroutine(FixedStep(_speed));
    }
    
    private IEnumerator FixedStep(float timeStep)
    {
        bool active = true;
        while (active)
        {
            Logic();
            MapRender();
            yield return new WaitForSeconds(timeStep);
        }
    }
    
    GameObject CreateCell(Vector2 _position)
    {
        GameObject cell = Instantiate<GameObject>(_cellPrefab);
        cell.transform.SetParent(gameObject.transform,false);
        cell.GetComponent<RectTransform>().localPosition = new Vector3(_size.x * _position.x , _size.y * _position.y, 0);
        cell.GetComponent<RectTransform>().sizeDelta = _size;
        cell.name = "Cell " + _position.ToString();
        return cell;
    }

    private void MapCreateSize()
    {
        _map = new bool[(int)_mapSize.x, (int)_mapSize.y];
        _cells = new GameObject[(int)_mapSize.x, (int)_mapSize.y];
        for (int j = 0; j < _mapSize.x; j++)
        {
            for (int i = 0; i < _mapSize.y; i++)
            {
                _map[j, i] = false;
                if (_cells[j, i] == null)
                {
                    _cells[j, i] = CreateCell(new  Vector2(j, i));
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
                if (_map[j, i])
                {
                    _cells[j, i].GetComponent<Image>().color = _aliveColor;
                }
                else
                {
                    _cells[j, i].GetComponent<Image>().color = _deadColor;
                }
            }
        }
    }

    private int CheckNeighbours(Vector2 _centerposition)
    {
        int _neighbours = 0;
        for (int x = (int)_centerposition.x-1 ; x < _centerposition.x+2; x++)
        {
            for (int y = (int)_centerposition.y-1 ; y < _centerposition.y+2; y++)
            {
                if (x >= 0 && x < _mapSize.x && y >= 0 && y < _mapSize.y && new Vector2(x , y) != _centerposition)
                {
                    if (_map[x, y] )
                    {
                        _neighbours++;
                    }
                }
            }
        }
        return _neighbours;
    }

    public void Logic()
    {
        bool[,] _tempmap =  new bool[(int)_mapSize.x, (int)_mapSize.y];
        for (int x = 0; x < _mapSize.x; x++)
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                int _neighbours = CheckNeighbours(new Vector2(x, y));
                if (_map[x, y])
                {
                    if (_neighbours == 0){ _tempmap[x, y] = false;}
                    if (_neighbours >= 2){ _tempmap[x, y] = true;}
                    if (_neighbours > 3){ _tempmap[x, y] = false;}
                }
                else
                {
                    if (_neighbours == 3){ _tempmap[x, y] = true;}
                }
            }
        }
        _map = _tempmap;
    }
}
