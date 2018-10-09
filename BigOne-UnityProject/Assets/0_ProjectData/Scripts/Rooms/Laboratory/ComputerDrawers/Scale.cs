using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
    public Text textScale;

    public GameEvent RocksWeighted;

    [Header("Rock Names")]
    public string rockNameQuarzo = "Quarzo_Grabbabile";
    public string rockNameTridimite = "Trimidite_Grabbabile";
    public string rockNameCristobalite = "Cristobalite_Grabbabile";

    Collider scaleCollider;

    bool quartzWeighted;
    bool tridimiteWeighted;
    bool cristobaliteWeighted;

    bool allRocksWeighted = false;

    private void Start()
    {
        scaleCollider = GetComponent<Collider>();
        ResetWeight();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rocks")
        {
            var rb = other.GetComponent<Rigidbody>();
            if (rb == null) return;

            float mass = rb.mass;
            GetWeight(mass);
            if (other.gameObject.name == rockNameQuarzo)
            {
                quartzWeighted = true;
            }
            if (other.gameObject.name == rockNameTridimite)
            {
                tridimiteWeighted = true;
            }
            if (other.gameObject.name == rockNameCristobalite)
            {
                cristobaliteWeighted = true;
            }
            UpdateState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rocks")
        {
            ResetWeight();
        }
    }

    private void GetWeight(float mass)
    {
        textScale.text = "" + mass;
    }

    private void ResetWeight()
    {
        textScale.text = "0";
    }

    void UpdateState()
    {
        if (!allRocksWeighted && quartzWeighted && tridimiteWeighted && cristobaliteWeighted && RocksWeighted != null)
        {
            allRocksWeighted = true;

            RocksWeighted.Invoke();
        }
    }
}