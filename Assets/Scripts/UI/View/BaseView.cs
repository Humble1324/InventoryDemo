using System;
using UnityEngine;

namespace View
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        public abstract string prefabPath
        {
            get;
        }
        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            AfterInit();
        }

        private void OnEnable()
        {
            AfterShow();
        }

        public void OnDisable()
        {
            AfterHide(); 
        }

        private void OnDestroy()
        {
            AfterClose();
            Release();
        }

        public abstract void Init();
        public abstract void AfterInit();
        public abstract void AfterShow();
        public abstract void AfterHide();
        public abstract void AfterClose();
        public abstract void Release();
    }
}