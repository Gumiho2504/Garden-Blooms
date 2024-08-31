using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenPlot : MonoBehaviour
{
    //public int rows = 5;  // Number of rows in the garden grid
    //public int columns = 5;  // Number of columns in the garden grid
    //public GameObject cellPrefab;  // Prefab for a garden cell
    //public float cellSpacing = 1.1f;  // Spacing between cells
    //public Cell[,] cells;  // 2D array to hold cell references

    //void Start()
    //{
    //    InitializeGrid();
    //}

    //// Method to initialize the garden grid
    //public void InitializeGrid()
    //{
    //    cells = new Cell[rows, columns];

    //    for (int i = 0; i < rows; i++)
    //    {
    //        for (int j = 0; j < columns; j++)
    //        {
    //            // Calculate position for each cell based on row, column, and spacing
    //            Vector3 cellPosition = new Vector3(i * cellSpacing, j, j * cellSpacing);  // Adjust Y to 0 for 3D or use X and Y for 2D
    //            // Instantiate the cell prefab at the calculated position
    //            GameObject newCell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

    //            // Get the Cell component and set its coordinates
    //            Cell cellComponent = newCell.GetComponent<Cell>();
    //            cellComponent.SetCoordinates(i, j);

    //            // Store the cell in the cells array
    //            cells[i, j] = cellComponent;
    //        }
    //    }
    //}

    //// Check if a cell at a specific row and column is empty
    //public bool IsCellEmpty(int row, int column)
    //{
    //    return cells[row, column].IsEmpty();
    //}

    //// Plant a flower in a specific cell
    //public void PlantFlower(int row, int column, Flower flower)
    //{
    //    if (IsCellEmpty(row, column))
    //    {
    //        cells[row, column].PlantFlower(flower);
    //    }
    //}
}
