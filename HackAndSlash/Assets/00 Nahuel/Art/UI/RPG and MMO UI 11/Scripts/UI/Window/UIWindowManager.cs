using UnityEngine;
using System.Collections.Generic;

namespace DuloGames.UI
{
	public class UIWindowManager : MonoBehaviour {

        private static UIWindowManager m_Instance;

        /// <summary>
        /// Gets the current instance of the window manager.
        /// </summary>
        public static UIWindowManager Instance
        {
            get { return m_Instance; }
        }
        
        [SerializeField] private string m_EscapeInputName = "Cancel";
        private bool m_EscapeUsed = false;

        /// <summary>
        /// Gets the escape input name.
        /// </summary>
        public string escapeInputName
        {
            get { return this.m_EscapeInputName; }
        }

        /// <summary>
        /// Gets a value indicating whether the escape input was used to hide a window in this frame.
        /// </summary>
        public bool escapedUsed
        {
            get { return this.m_EscapeUsed; }
        }

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_Instance.Equals(this))
                m_Instance = null;
        }

       
	}
}
