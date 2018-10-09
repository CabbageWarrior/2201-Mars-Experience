using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Slots : MonoBehaviour {
	#region OLDCODE
	/*
    class ChipSlot
    {
        public string correctChipName;
        public int correctAngleDeg;

        public ChipSlot(string correctChipName, int correctAngleDeg)
        {
            this.correctChipName = correctChipName;
            this.correctAngleDeg = correctAngleDeg;
        }
    }

    public bool isCorrect;

    OpenClozeManager openClozeManager;

    Dictionary<string, ChipSlot> chipSlots = new Dictionary<string, ChipSlot>();

    private void Awake()
    {
        chipSlots.Add("EmptySlot1", new ChipSlot("ChipO", 0));
        chipSlots.Add("EmptySlot2", new ChipSlot("ChipCL", 180));
        chipSlots.Add("EmptySlot3", new ChipSlot("ChipE", 0));
        chipSlots.Add("EmptySlot4", new ChipSlot("ChipZ", 90));
    }

    void Start()
    {
        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += new SnapDropZoneEventHandler(ObjectSnappedToDropZone);
        GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone += new SnapDropZoneEventHandler(ObjectUnsnappedToDropZone);
        openClozeManager = FindObjectOfType<OpenClozeManager>();
    }

    void ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.tag == "Cubes")
        {
			// DICTIONARY WRONG KEY
			/*
            if (e.snappedObject.name == chipSlots[this.gameObject.name].correctChipName &&
                (int)e.snappedObject.transform.eulerAngles.z == chipSlots[this.gameObject.name].correctAngleDeg)
            {
                isCorrect = true;
                openClozeManager.correctChips += 1;
                Debug.Log(openClozeManager.correctChips);
            }
			*/
	#endregion

	public bool chipSnapped;
	public bool isCorrect;
	OpenClozeManager openClozeManager;

	void Start()
	{
		GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += new SnapDropZoneEventHandler( ObjectSnappedToDropZone );
		GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone += new SnapDropZoneEventHandler( ObjectUnsnappedToDropZone );
		openClozeManager = FindObjectOfType<OpenClozeManager>();
	}

	void ObjectSnappedToDropZone( object sender, SnapDropZoneEventArgs e )
	{
		if ( e.snappedObject.tag == "Cubes" )
		{
			this.chipSnapped = true;
			switch ( this.gameObject.name )
			{
				case "SnapEmptySlot1":
					if ( e.snappedObject.name == "ChipO" )
					{
						if ( Mathf.RoundToInt( this.transform.localEulerAngles.z) == 0 || Mathf.RoundToInt( this.transform.localEulerAngles.z) == 180)
						{
							AddCorrectChip();
						} else
						{
							Debug.Log( this.transform.localEulerAngles.z );
						}
					}
					break;

				case "SnapEmptySlot2":
					if ( e.snappedObject.name == "ChipCL" && Mathf.RoundToInt(this.transform.localEulerAngles.z) ==  180)
					{
						AddCorrectChip();
					} else
					{
						Debug.Log( this.transform.localEulerAngles.z );
					}
					break;

				case "SnapEmptySlot3":
					if ( e.snappedObject.name == "ChipE" && Mathf.RoundToInt( this.transform.localEulerAngles.z) == 0)
					{
						AddCorrectChip();
					} else
					{
						Debug.Log( this.transform.localEulerAngles.z );
					}
					break;

				case "SnapEmptySlot4":
					if ( e.snappedObject.name == "ChipZ" )
					{
						if ( Mathf.RoundToInt( this.transform.localEulerAngles.z) == 90 || Mathf.RoundToInt( this.transform.localEulerAngles.z) == 270)
						{
							AddCorrectChip();
						} else
						{
							Debug.Log( this.transform.localEulerAngles.z );
						}
					}
					break;
			}
		}
	}

	void ObjectUnsnappedToDropZone( object sender, SnapDropZoneEventArgs i )
	{

		this.chipSnapped = false;


		if ( this.isCorrect == true )
		{
			this.isCorrect = false;
			openClozeManager.correctChips -= 1;
		}
	}

	void AddCorrectChip()
	{
		this.isCorrect = true;
		openClozeManager.correctChips += 1;
		openClozeManager.CheckPuzzle();
	}
}








