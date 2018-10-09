// Scripted by Roberto Leogrande

using UnityEngine;
using VRTK;

public class DirectionReminder : MonoBehaviour
{
    // object to take position
    public GameObject				m_ReferenceOBJ					= null;

	// Pointer_Extender of arrows
    public VRTK_Pointer_Extension	m_PointerLeft					= null;
    public VRTK_Pointer_Extension	m_PointerRight					= null;

	// Transform of foot indicator
    public Transform				m_PositionIndicator				= null;

	[Tooltip(" Nomalized angle at which arrows are shown ") ]
    [Range(0f, 1f)]
    public float					m_NormalizedAngleToShowReminder	= 0.5f;

	[Tooltip(" the speed arrows will blink to ") ]
    [Range(0.2f, 10f)]
    public float					m_BlinkSpeed					= 1.0f;

	[Tooltip( "the speed at which arrows will rise up from ungerdround ") ]
    [Range(0.2f, 30f)]
    public float					m_RemindersSpeed				= 15.0f;

    // Arrows reference
    private Transform				m_ArrowLeft						= null;
    private Transform				m_ArrowRight					= null;

	// foot image transform reference
    private Transform				m_PositionIndicator_Circle		= null;

	// boolean value used to show or hide arrows
    private bool					 m_ShowReminder					= false;

	// boolean value used by arrows for blink effect
    private bool					m_PingPong						= false;

	// internal script status
    private bool					m_IsOK							= false;




	//////////////////////////////////////////////////////////////////////////
	// START
    void Start()
    {

		// sanity checks
        if (m_ReferenceOBJ == null) return;

        if ( ( m_ArrowLeft  = transform.GetChild(0) ) == null ) return;
        if ( ( m_ArrowRight = transform.GetChild(1) ) == null ) return;

		if ( ( m_PositionIndicator_Circle = m_PositionIndicator.Find("Circle") ) == null ) return;

        m_IsOK = true;
	}


	//////////////////////////////////////////////////////////////////////////
	// UpdateArrow
    private void UpdateArrow( Transform arrow ) {

        // set arrow position
        arrow.localPosition = Vector3.Lerp(
            arrow.localPosition,
            new Vector3( arrow.localPosition.x, ( m_ShowReminder ? 1f : -1f ), arrow.localPosition.z ),
            Time.deltaTime * m_RemindersSpeed
        );

        if (!m_ShowReminder) return;

        // blinking
		// 20180226 - Verza - Commentato dopo cambio modello freccia!
		/*
        for ( int i = 0; i < arrow.childCount; i++ )
        {

            Transform arrowChild = arrow.GetChild(i);

            MeshRenderer r = arrowChild.GetComponent<MeshRenderer>();
            Material m = r.material;
            Color c = m.color;

            if (!m_ShowReminder)
            {
                c.a = 1.0f;
                m.color = c;
                r.material = m;
                continue;
            }

            if (c.a == 1.0f)
            {
                m_PingPong = false;
            }
            if (c.a < 0.4f)
            {
                m_PingPong = true;
            }

            c.a += ((m_PingPong) ? 1f : -1f) * Time.deltaTime * m_BlinkSpeed;
            c.a = Mathf.Clamp01(c.a);

            m.color = c;
            r.material = m;

        }
		*/
	}


	//////////////////////////////////////////////////////////////////////////
	// UNITY
    private void Update() {

        if (!m_IsOK) enabled = false;

        // Reminder will follow player
        transform.position = new Vector3(m_ReferenceOBJ.transform.position.x, 0, m_ReferenceOBJ.transform.position.z);

		// foot transform update
        m_PositionIndicator.transform.localEulerAngles        = new Vector3(   0,  m_ReferenceOBJ.transform.localEulerAngles.y, 0 );
        m_PositionIndicator_Circle.transform.localEulerAngles = new Vector3( -90, -m_ReferenceOBJ.transform.localEulerAngles.y, 0 );

        // Show or hide reminder
        m_ShowReminder = Mathf.Abs(m_ReferenceOBJ.transform.localRotation.y) > m_NormalizedAngleToShowReminder;

        UpdateArrow( m_ArrowLeft  );
        UpdateArrow( m_ArrowRight );

    }

}
