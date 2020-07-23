using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System.Data;
using UnityEngine.UIElements;
using UnityEditor;

public class PopulateGrid : MonoBehaviour
{
    private string jsonString;
    private JsonData itemData;
    private int rows;
    private int columns;

    public GameObject prefab;
    
    void Start()
    {
        ReadData();
        Populate();
    }
    
    void Update(){}

    void Populate()
    {
        GameObject newObj;
        
        // Set headers
        for (int col = 0; col < columns; col++)
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Text>().text = itemData["ColumnHeaders"][col].ToString();
            newObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
            newObj.GetComponent<Text>().fontSize = 20;
        }
        
        // Set data
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                string field = itemData["ColumnHeaders"][col].ToString();

                newObj = (GameObject)Instantiate(prefab, transform);
                newObj.GetComponent<Text>().text = itemData["Data"][row][field].ToString();
            }
        }
    }

    void ReadData()
    {
        // Get data from Json
        jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/JsonChallenge.json");
        itemData = JsonMapper.ToObject(jsonString);

        // Get number of rows and columns
        rows = itemData["Data"].Count;
        columns = itemData["ColumnHeaders"].Count;

        Debug.Log("Columnas: " + columns + " - Filas: " + rows );

        // Set columns
        this.GetComponent<GridLayoutGroup>().constraintCount = columns;

        // Set Title
        GameObject title = GameObject.FindGameObjectWithTag("Title");
        title.GetComponent<Text>().text = itemData["Title"].ToString();
        title.GetComponent<Text>().fontStyle = FontStyle.Bold;
        title.GetComponent<Text>().fontSize = 20;
    }

    public void Reload()
    {
        var Cells = GameObject.FindGameObjectsWithTag("Cell");

        foreach (var item in Cells)
        {
            Destroy(item);
        }

        ReadData();
        Populate();
    }
}
