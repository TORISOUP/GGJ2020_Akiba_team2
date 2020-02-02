using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanGanKamen
{
    public class DebugShippingButton : MonoBehaviour
    {
        [SerializeField] private GGJ2020.Stages.ShippingButton shippingButton;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                shippingButton.TryHold();
            }
        }
    }
}

