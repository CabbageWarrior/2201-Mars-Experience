using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public enum WaterColor
    {
        RED,
        GREEN,
        BLUE,
        ORANGE,
        NONE
    }

    public enum WaterOperation
    {
        ADD,
        REMOVE,
        CHECK
    }

    public int redMeter = 0; //Counter of water for Red Plants
    public int greenMeter = 0; //Counter of water for Green Plants
    public int blueMeter = 0; //Counter of water for Blue Plants
    public int orangeMeter = 0; //Counter of water for Orange Plants

    public int redCorrect = 4;
    public int greenCorrect = 1;
    public int blueCorrect = 3;
    public int orangeCorrect = 2;

    public int waterMeter = 10; //Max amount of water, increase or decrease everytime. Can't go below 0 and go up 10.

    //Arrays of cubes in the scene, divided by color
    public GameObject[] redCubes;
    public GameObject[] greenCubes;
    public GameObject[] blueCubes;
    public GameObject[] orangeCubes;
    public GameObject[] waterCubes;

	//[Header("Audio Effects")]
	//public AudioSource positiveResolutionAudio;
	//public AudioSource negativeResolutionAudio;

    GreenHousePlantManager m_GreenHousePlantManager;

    private void Start()
    {
        m_GreenHousePlantManager = FindObjectOfType<GreenHousePlantManager>();
    }

    // Update is called once per frame
    void UpdateTotalWater()
    {
        for (int i = 0; i < waterMeter; i++)
        {
            //waterCubes[i].GetComponent<Renderer>().material.color = Color.cyan;
            waterCubes[i].SetActive(true);
        }

        for (int i = 9; i >= waterMeter; i--)
        {
            //waterCubes[i].GetComponent<Renderer>().material.color = Color.grey;
            waterCubes[i].SetActive(false);
        }

    }

    //if the condition is true, I increase or decrease an unit of water and color the relative cube in the scene

    public void SetWaterColorOperation(WaterColor color, WaterOperation operation)
    {
        int thisColorMeter = -1;
        GameObject[] thisColorBlocks = { };

        switch (color)
        {
            case WaterColor.RED:
                thisColorMeter = redMeter;
                thisColorBlocks = redCubes;
                break;
            case WaterColor.GREEN:
                thisColorMeter = greenMeter;
                thisColorBlocks = greenCubes;
                break;
            case WaterColor.BLUE:
                thisColorMeter = blueMeter;
                thisColorBlocks = blueCubes;
                break;
            case WaterColor.ORANGE:
                thisColorMeter = orangeMeter;
                thisColorBlocks = orangeCubes;
                break;
        }

        if (m_GreenHousePlantManager.turnedOnCubes)
        {

            if (
                (operation == WaterOperation.ADD && thisColorMeter < 4 && waterMeter > 0) ||

                (operation == WaterOperation.REMOVE && thisColorMeter > 0 && waterMeter < 10)
            )
            {
                thisColorMeter += (operation == WaterOperation.ADD ? 1 : -1);
                waterMeter += (operation == WaterOperation.ADD ? -1 : 1);



                //for (int i = 0; i < thisColorMeter; i++)
                //{
                //    redCubes[i].GetComponent<Renderer>().material.color = Color.red;
                //}


                for (int i = 0; i < 4; i++)
                {
                    thisColorBlocks[i].SetActive(i < thisColorMeter);
                }

                switch (color)
                {
                    case WaterColor.RED:
                        redMeter = thisColorMeter;
                        break;
                    case WaterColor.GREEN:
                        greenMeter = thisColorMeter;
                        break;
                    case WaterColor.BLUE:
                        blueMeter = thisColorMeter;
                        break;
                    case WaterColor.ORANGE:
                        orangeMeter = thisColorMeter;
                        break;
                }

              //  Debug.Log("Operation completed.");
            }
        }
        else
        {
          //  Debug.Log("Can't do that operation, please check.");
        }
        UpdateTotalWater();
    }

    #region RED
    public void RedPlus()
    {
        SetWaterColorOperation(WaterColor.RED, WaterOperation.ADD);
    }

    public void RedMinus()
    {
        SetWaterColorOperation(WaterColor.RED, WaterOperation.REMOVE);
    }
    #endregion

    #region GREEN
    public void GreenPlus()
    {
        SetWaterColorOperation(WaterColor.GREEN, WaterOperation.ADD);
    }

    public void GreenMinus()
    {
        SetWaterColorOperation(WaterColor.GREEN, WaterOperation.REMOVE);
    }
    #endregion

    #region BLUE
    public void BluePlus()
    {
        SetWaterColorOperation(WaterColor.BLUE, WaterOperation.ADD);
    }

    public void BlueMinus()
    {
        SetWaterColorOperation(WaterColor.BLUE, WaterOperation.REMOVE);
    }
    #endregion

    #region ORANGE
    public void OrangePlus()
    {
        SetWaterColorOperation(WaterColor.ORANGE, WaterOperation.ADD);
    }

    public void OrangeMinus()
    {
        SetWaterColorOperation(WaterColor.ORANGE, WaterOperation.REMOVE);
    }
    #endregion


    public void Check()
    {
        if (m_GreenHousePlantManager.turnedOnCubes)
        {
            if (redMeter == redCorrect && greenMeter == greenCorrect && blueMeter == blueCorrect && orangeMeter == orangeCorrect && waterMeter == 0)
            {
               // positiveResolutionAudio.Play();

                //Debug.Log("Key correct!");


                //  completed callback
                m_GreenHousePlantManager.OnCompletion();
            }
            else
            {
                //negativeResolutionAudio.Play();

               // Debug.Log("Incorrect key! Retry");
                m_GreenHousePlantManager.StartCoroutine(m_GreenHousePlantManager.WaterError());
				m_GreenHousePlantManager.OnError();


            }
        }
       

    }
}
