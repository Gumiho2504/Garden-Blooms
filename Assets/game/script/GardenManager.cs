using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance;

    [Header("GameObject")]
    public GameObject gardenGridPanel; 
    public GameObject cellButtonPrefab;
    public GameObject lostPanel,settingPanel,infoPanel;

    [Header("Prefab")]
    public List<Flower> flowerPrefabs; 

    public UICell[,] cells;

    public int rows = 5;
    public int columns = 5;
    public int minMatchCount = 3;
   

    [Header("Image")]
    public Image random_flowerImage;
    public Image fadeImage;

    [Header("Text")]
    public Text score_text;
    public Text highscore_text;
    public Text scoretextAtlost;
    public Text highscore_text_atLost;

    int randomFlowerIndex = 0;
    int score = 0;
    int highscore = 0;

    string highscorekey = "high";

    bool isReady = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        FadeIn();

        randomFlowerIndex = Random.Range(0, flowerPrefabs.Count);
        random_flowerImage.sprite = flowerPrefabs[randomFlowerIndex].GetComponent<Image>().sprite;
        AudioController.Instance.PlaySFX("rand");
        LeanTween.scale(random_flowerImage.gameObject, Vector3.one, 0.5f).setEaseSpring().setOnComplete(()=> {
            isReady = true;
        });

        InitializeGrid();

        highscore_text.text = $"highscore : {PlayerPrefs.GetInt(highscorekey, 0)}";
        highscore = PlayerPrefs.GetInt(highscorekey, 0);
    }


    void InitializeGrid()
    {
        cells = new UICell[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
              
                GameObject newCellButton = Instantiate(cellButtonPrefab, gardenGridPanel.transform);
                UICell cellComponent = newCellButton.GetComponent<UICell>();
                cellComponent.SetCoordinates(i, j);
                cells[i, j] = cellComponent;
            }
        }
        //SetChildrenLayersAndOrder();
    }

    
    public void OnCellClicked(UICell cell)
    {
        gardenGridPanel.GetComponent<GridLayoutGroup>().enabled = false;
        if (cell.IsEmpty() && isReady)
        {
            AudioController.Instance.PlaySFX("drop");
            isReady = false;
            LeanTween.scale(random_flowerImage.gameObject, Vector3.zero, 0.5f).setEaseSpring();
            PlantFlower(cell);
        }

        CheckIfGameLost();
    }

    void PlantFlower(UICell cell)
    {
        
        Flower newFlower = Instantiate(flowerPrefabs[randomFlowerIndex], new Vector3(0,56f,0), Quaternion.identity);

        LeanTween.scale(newFlower.gameObject, Vector3.one * 2f, 0.3f).setEase(LeanTweenType.easeOutSine).setOnComplete(() =>
        {
            LeanTween.scale(newFlower.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutSine);
        });

        newFlower.transform.SetParent(cell.transform, false);
        cell.PlantFlower(newFlower);

       
        StartCoroutine(PlanFlowerAni(cell));
    }

    

    IEnumerator PlanFlowerAni(UICell cell)
    {
        yield return new WaitForSeconds(0.2f);

        CheckForMatches(cell);

        yield return new WaitForSeconds(0.5f);
        
        randomFlowerIndex = Random.Range(0, flowerPrefabs.Count);
        random_flowerImage.sprite = flowerPrefabs[randomFlowerIndex].GetComponent<Image>().sprite;
        AudioController.Instance.PlaySFX("rand");
        LeanTween.scale(random_flowerImage.gameObject, Vector3.one, 0.5f).setEaseSpring().setOnComplete(() => {
            isReady = true;
        });
    }

    void CheckForMatches(UICell cell)
    {
        List<UICell> matchingCells = FindMatchingNeighbors(cell);
        print($"lenght - {matchingCells.Count}");
        if (matchingCells.Count >= minMatchCount)
        {
            // Ensure all cells have flowers before attempting to merge
            if (cell.flower != null && matchingCells[0].flower != null)
            {
                //foreach (UICell match in matchingCells)
                //{
                //    if (match != cell) // Don't remove the base cell's flower
                //    {
                //        match.transform.GetChild(1).SetParent(matchingCells[0].transform, true);
                //        LeanTween.moveLocal(matchingCells[0].transform.GetChild(2).gameObject, matchingCells[0].transform.position, 0.5f)
                //            .setEaseInOutQuint()
                //            .setOnComplete(
                //                () =>
                //                {
                //                LeanTween.moveLocal(matchingCells[0].transform.GetChild(3).gameObject, matchingCells[0].transform.position, 0.5f)
                //                 .setEaseInOutQuint()
                //                 .setOnComplete(
                //                    () =>
                //                    {
                //                        match.RemoveFlower();
                //                    });


                //                }
                //            );

                //    }
                //}

                //// Update cell flower level and recheck neighbors
                //cell.flower.Merge(matchingCells[0].flower);
                //CheckForMatches(cell);
                ////matchingCells[1].RemoveFlower();
                ////matchingCells[2].RemoveFlower();



                ////cell.flower.Merge(matchingCells[0].flower) ;
                //score += 10 * matchingCells[0].flower.level;
                ////score_text.text = $"score : {score}";
                //// CheckForMatches(cell);
                //UpdateTextUI();

                AudioController.Instance.PlaySFX("collect");
                // Ensure all cells have flowers before attempting to merge
                if (cell.flower != null && matchingCells[0].flower != null)
                {
                    UICell baseCell = matchingCells[0]; 

                    foreach (UICell match in matchingCells)
                    {
                        if (match != baseCell) 
                        {
                            Transform flowerTransform = match.transform.GetChild(1); 
                            flowerTransform.SetParent(baseCell.transform, true);

                            LeanTween.moveLocal(flowerTransform.gameObject, Vector3.zero, 0.5f) 
                                .setEaseInOutQuint()
                                .setOnComplete(() =>
                                {
                                    match.RemoveFlower();
                                    Debug.Log($"Flower removed from cell at ({match.GetRow()}, {match.GetColumn()})");
                                });
                        }
                    }

                    // Update cell flower level and recheck neighbors

                    baseCell.flower.Merge(cell.flower);
                    Debug.Log($"Updated flower level to {baseCell.flower.level} at cell ({baseCell.GetRow()}, {baseCell.GetColumn()})");
                    //UpdateFlowerLevel(baseCell);

                    CheckForMatches(baseCell); // Recheck for more possible merges after an update

                    score += 10 * baseCell.flower.level;
                    UpdateTextUI();
                 }

                }
            else
            {
                Debug.LogError("Attempted to merge but one or more flowers were null.");
            }

        }
    }

    public List<UICell> FindMatchingNeighbors(UICell cell)
    {
        List<UICell> matchingNeighbors = new List<UICell>();
        Flower.FlowerType type = cell.flower.flowerType;
        int level = cell.flower.level;

        int row = cell.GetRow();
        int column = cell.GetColumn();

        // Initialize recursive check
        FindMatchingNeighborsRecursive(cell, type, matchingNeighbors,level);

        return matchingNeighbors;
    }

    // Recursive method to find all matching neighbors
    void FindMatchingNeighborsRecursive(UICell cell, Flower.FlowerType type, List<UICell> matchingNeighbors,int level)
    {
        // Get the cell's coordinates
        int row = cell.GetRow();
        int column = cell.GetColumn();

        // Add the current cell to the matching list if it's not already there
        if (!matchingNeighbors.Contains(cell))
        {
            matchingNeighbors.Add(cell);
        }

        // Define directions to check (N, S, E, W)
        int[,] directions = new int[,]
        {
            {-1, 0}, // North
            {1, 0},  // South
            {0, -1}, // West
            {0, 1}   // East
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = row + directions[i, 0];
            int newCol = column + directions[i, 1];

            // Check if the new coordinates are within the grid boundaries
            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < columns)
            {
                UICell neighborCell = cells[newRow, newCol];
                // Check if the neighbor has a flower of the same type
                if (neighborCell.flower != null && neighborCell.flower.flowerType == type && neighborCell.flower.level == level && !matchingNeighbors.Contains(neighborCell))
                {
                    FindMatchingNeighborsRecursive(neighborCell, type, matchingNeighbors,level);
                }
            }
        }
    }


    void CheckIfGameLost()
    {
        if (IsGridFull() && !HasPossibleMoves())
        {
            Debug.Log("Game Over! No more possible moves.");
            if(score > highscore)
            {
                highscore = score;
                PlayerPrefs.SetInt(highscorekey, highscore);
                UpdateTextUI();

                
            }

            lostPanel.SetActive(true);
            LeanTween.scale(lostPanel, Vector3.one, 1f).setEaseLinear();
            AudioController.Instance.PlaySFX("end");
            scoretextAtlost.text = $"score : {score}";
            highscore_text_atLost.text = $"highscore : {highscore}";
            // Implement additional game over logic, such as showing a game over screen
        }
    }

    // Check if the grid is full
    bool IsGridFull()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[i, j].IsEmpty())
                {
                    return false; // There's at least one empty cell
                }
            }
        }
        return true; // No empty cells found
    }

    // Check if there are any possible moves left
    bool HasPossibleMoves()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                UICell cell = cells[i, j];
                if (cell.flower != null)
                {
                    List<UICell> matchingNeighbors = FindMatchingNeighbors(cell);
                    if (matchingNeighbors.Count >= minMatchCount)
                    {
                        return true; // There's at least one possible match
                    }
                }
            }
        }
        return false; // No possible moves found
    }


    void UpdateTextUI()
    {
        score_text.text = $"score : {score}";
        highscore_text.text = $"highscore : {highscore}";
    }


    // button click

    public void OnClickReplay(GameObject g)
    {
        AudioController.Instance.PlaySFX("click");
        AnimateButtonPress(g);
        FadeOutAndLoadScene("game");
    }

    public void OnClickHomeOrMenu(GameObject g)
    {
        AudioController.Instance.PlaySFX("click");
        AnimateButtonPress(g);
        FadeOutAndLoadScene("menu");
    }


    public void onClickInfor(int i)
    {
        AudioController.Instance.PlaySFX("click");
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AnimateButtonPress(g);
        if (i == 1) LeanTween.scale(infoPanel, Vector3.one, 0.6f).setEaseLinear();
        else LeanTween.scale(infoPanel, Vector3.zero, 0.6f).setEaseLinear();

    }

    public void onClickSetting(int i)
    {
        AudioController.Instance.PlaySFX("click");
        GameObject g = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        AnimateButtonPress(g);
        if (i == 1) LeanTween.scale(settingPanel, Vector3.one, 0.6f).setEaseLinear();
        else LeanTween.scale(settingPanel, Vector3.zero, 0.6f).setEaseLinear();

    }


    // animation leantween


    public void FadeIn()
    {
        LeanTween.alpha(fadeImage.rectTransform, 0f, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false);
        });
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);

        LeanTween.alpha(fadeImage.rectTransform, 1f, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void AnimateButtonPress(GameObject button)
    {
        LeanTween.scale(button, Vector3.one * 0.9f, 0.1f).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>
        {
            LeanTween.scale(button, Vector3.one, 0.1f).setEase(LeanTweenType.easeInCubic);
        });
    }


    public GridLayoutGroup gridLayoutGroup; // Reference to the GridLayoutGroup
    public string sortingLayerName = "UI";  // Name of the sorting layer you want to use
    public int sortingOrder = 0;

    void SetChildrenLayersAndOrder()
    {
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                // Set the sorting layer and order
                Canvas childCanvas = child.GetComponent<Canvas>();
                if (childCanvas == null)
                {
                    childCanvas = child.gameObject.AddComponent<Canvas>();
                    child.gameObject.AddComponent<GraphicRaycaster>(); // Optional: if you need raycast interaction
                }

                childCanvas.overrideSorting = true;
                childCanvas.sortingLayerName = sortingLayerName;
                childCanvas.sortingOrder = sortingOrder;
            }
        }
    }



}// end of class
