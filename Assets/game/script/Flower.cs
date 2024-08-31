using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Flower : MonoBehaviour
{
    public enum FlowerType { Rose, Sunflower, Lily, Tulip }
    public FlowerType flowerType;
    public int level = 1; // Flower growth level
    public List<Sprite> flowerLevelSprit = new List<Sprite>();
    public void Merge(Flower otherFlower)
    {
        if (otherFlower.flowerType == flowerType && otherFlower.level == level)
        {
            level++;
            UpdateAppearance();

            if (otherFlower.level == 4) StartCoroutine(DieAnimation(otherFlower.gameObject));


            PlayMergeEffect();
        }
    }

    IEnumerator DieAnimation(GameObject g)
    {
        yield return new WaitForSeconds(1f);
        LeanTween.scale(g, Vector3.one, 1f)
            .setEaseInOutSine()
            .setOnComplete(
                () =>
                {
                    Destroy(g);
                }
            );

    }

    private void UpdateAppearance()
    {
        // Placeholder: Update appearance based on level
        //transform.localScale = Vector3.one * (1.0f + 0.2f * level);
        LeanTween.scale(gameObject, Vector3.one * (1.0f + 0.5f * level), 1f).setEaseInBounce();
        gameObject.GetComponent<Image>().sprite = flowerLevelSprit[level-1];
    }

    private void PlayMergeEffect()
    {
        // Placeholder: Add particle effects or sounds
        Debug.Log("Merge Effect Played for " + flowerType + " at Level " + level);
    }

    public void ActivateSpecialAbility()
    {
        // Special abilities based on flower type
        switch (flowerType)
        {
            case FlowerType.Sunflower:
                BoostNearbyFlowers();
                break;
            case FlowerType.Lily:
                ClearWaterObstacles();
                break;
            case FlowerType.Rose:
                ProtectNearbyFlowers();
                break;
            case FlowerType.Tulip:
                AttractPollinators();
                break;
        }
    }

    // Increase the growth rate or level of nearby flowers
    private void BoostNearbyFlowers()
    {
        List<UICell> neighbors = GardenManager.Instance.FindMatchingNeighbors(GetComponentInParent<UICell>());
        foreach (UICell neighbor in neighbors)
        {
            if (neighbor.flower != null && neighbor.flower.level < this.level)
            {
                neighbor.flower.level++;
                neighbor.flower.UpdateAppearance();
                Debug.Log("Boosted flower at " + neighbor.GetRow() + "," + neighbor.GetColumn());
            }
        }
    }

    // Remove water or other obstacles from nearby cells
    private void ClearWaterObstacles()
    {
        int row = GetComponentInParent<UICell>().GetRow();
        int column = GetComponentInParent<UICell>().GetColumn();
        int[,] directions = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } }; // N, S, W, E

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = row + directions[i, 0];
            int newCol = column + directions[i, 1];

            if (newRow >= 0 && newRow < GardenManager.Instance.rows && newCol >= 0 && newCol < GardenManager.Instance.columns)
            {
                UICell neighborCell = GardenManager.Instance.cells[newRow, newCol];
                if (neighborCell.flower == null)
                {
                    // Assuming an obstacle is represented by no flower in a cell, clear it
                    Debug.Log("Cleared obstacle at " + newRow + "," + newCol);
                    // You could implement specific obstacle-clearing logic here
                }
            }
        }
    }

    // Make adjacent flowers temporarily invulnerable
    private void ProtectNearbyFlowers()
    {
        List<UICell> neighbors = GardenManager.Instance.FindMatchingNeighbors(GetComponentInParent<UICell>());
        foreach (UICell neighbor in neighbors)
        {
            if (neighbor.flower != null)
            {
                // Assume we set a protected state or flag
                // Here we could set a protected property, change color, etc.
                neighbor.flower.GetComponent<Image>().color = Color.green; // Example of visual feedback
                Debug.Log("Protected flower at " + neighbor.GetRow() + "," + neighbor.GetColumn());
            }
        }
    }

    // Increase the chance of adjacent flowers merging or leveling up
    private void AttractPollinators()
    {
        List<UICell> neighbors = GardenManager.Instance.FindMatchingNeighbors(GetComponentInParent<UICell>());
        foreach (UICell neighbor in neighbors)
        {
            if (neighbor.flower != null)
            {
                // Increase level or cause an immediate merge chance
                if (Random.value > 0.5f) // Example of a random event
                {
                    neighbor.flower.level++;
                    neighbor.flower.UpdateAppearance();
                    Debug.Log("Attracted pollinators to flower at " + neighbor.GetRow() + "," + neighbor.GetColumn());
                }
            }
        }
    }
}


// Here's a detailed description of the "Garden Blooms" game, including the game's objective, rules, and special abilities.

// Game Overview: Garden Blooms
// "Garden Blooms" is a relaxing yet engaging puzzle game where players create beautiful gardens by planting and merging various types of flowers. The game combines elements of strategy, matching, and garden management, offering a soothing experience while challenging players to think ahead.

// Objective
// The goal of "Garden Blooms" is to create the most vibrant and beautiful garden possible by strategically planting and merging flowers. Players aim to achieve high scores by growing flowers to higher levels through merging and utilizing special abilities. The game ends when there are no more valid moves or the garden grid is full and no matches can be made.

// How to Play
// Grid Layout: The game is played on a 5x5 grid, each cell representing a plot in the garden where flowers can be planted.

// Planting Flowers: At the bottom of the screen, a random flower is displayed. Players can drag and drop this flower onto any empty cell in the garden grid.

// Merging Flowers: When three or more flowers of the same type and level are placed adjacent to each other (either horizontally or vertically), they merge into a single flower of a higher level. The merging flower's level increases, and it becomes more vibrant and valuable.

// Scoring: Players earn points by merging flowers. The points awarded increase with the level of the flowers being merged. The score is displayed at the top of the screen.

// Special Abilities: Each flower type has a unique special ability that can influence nearby flowers or affect the game grid:

// Sunflower (Boost Growth): Increases the growth rate or level of nearby flowers.
// Lily (Clear Obstacles): Clears water or other obstacles from nearby cells.
// Rose (Protect Flowers): Makes nearby flowers temporarily invulnerable to negative effects.
// Tulip (Attract Pollinators): Increases the chance of nearby flowers merging or leveling up.
// Game Over Condition: The game ends when the grid is full and no more valid moves can be made. If the player cannot match three or more flowers and no empty cells remain, the game will display a "Game Over" message.

// Rules
// Flower Placement: Flowers can only be placed in empty cells. Players must plan their moves carefully to create opportunities for merging.

// Matching Criteria: Flowers must be of the same type and level to merge. Diagonal matches are not allowed; only horizontal and vertical matches count.

// Special Abilities: Special abilities are activated automatically when a flower reaches a specific level or when the conditions for the ability are met.

// Scoring System: Merging three level 1 flowers creates one level 2 flower and scores points. The higher the level of the flowers being merged, the more points are awarded. Players aim to keep merging to achieve the highest possible score.

// High Score Tracking: The game tracks the highest score achieved across sessions, displaying it at the top of the screen. Players can try to beat their high score with each new game.

// Tips for Players
// Plan Ahead: Think a few moves ahead to create opportunities for merging. Don't just focus on the immediate move.
// Use Corners Wisely: Utilize corners and edges of the grid to manage flower placement and avoid blocking yourself.
// Activate Special Abilities: Take advantage of each flower's special ability to enhance gameplay and create more merging opportunities.
// Keep an Eye on the Grid: Regularly check the grid to ensure you have room to plant new flowers and create matches.