using System.Collections.Generic;
using UnityEngine;

namespace SmartStep.UI
{
    public class PanelsSwitcher : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _disablePanels;
        [SerializeField] private List<RectTransform> _enablePanels;

        public void DoSwitch()
        {
            foreach (var panel in _disablePanels)
            {
                panel.gameObject.SetActive(false);
            }

            foreach (var panel in _enablePanels)
            {
                panel.gameObject.SetActive(true);
            }
        }
    }
}
