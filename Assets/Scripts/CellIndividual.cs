using UnityEngine;

public class CellIndividual : MonoBehaviour
{
    public Vector2 position;
    public CellManager cellManager;

    public void Switch()
    {
        bool selfState = cellManager.map[(int)position.x, (int)position.y];
        if (selfState)
        {
            cellManager.map[(int)position.x, (int)position.y] = false;
        }
        else
        {
            cellManager.map[(int)position.x, (int)position.y] = true;
        }
        cellManager.MapRender();
    }
}
