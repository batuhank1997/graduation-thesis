using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector;
#endif

namespace Longhorn.UIViewManager {
    [Serializable]
    public class ViewManager : Longhorn.Core.Singleton<ViewManager> {
        public List<ViewBase> views;

        [Header("Properties")]
        public List<ViewBase> initialView;

#if UNITY_EDITOR
        [Button]
        private void FindAllViews() {
            List<ViewBase> objectsInScene = new List<ViewBase>();
            
            foreach (ViewBase view in Resources.FindObjectsOfTypeAll(typeof(ViewBase)) as ViewBase[])
            {
                if (view.gameObject.hideFlags != HideFlags.None)
                    continue;
                if (PrefabUtility.GetPrefabType(view.gameObject) == PrefabType.Prefab || PrefabUtility.GetPrefabType(view.gameObject) == PrefabType.ModelPrefab)
                    continue;
                objectsInScene.Add(view);
            }
            
            views = objectsInScene;
        }
#endif
        
        private void Start() {
            if (!initialView.IsNullOrEmpty()) {
                foreach (var view in initialView)
                {
                    view.Show();
                }
            }
        }

        public T GetView<T>() where T : ViewBase { 
            for (int i = 0; i < views.Count; i++) {
                if (views[i] is T tView) {
                    return tView;
                }
            }

            return null;
        }
        
        public void Show(ViewBase view) {
            view.Show();
        }

        public void Show<T>() where T : ViewBase {
            for (int i = 0; i < views.Count; i++) {

                if (views[i] is T) {
                    Show(views[i]);
                }
            }
        }
        
        public void Hide(ViewBase view) {
            view.Hide(); 
            view.OnHidden();
        }
        
        public void Hide<T>() where T : ViewBase {
            for (int i = 0; i < views.Count; i++) {

                if (views[i] is T) {
                    Hide(views[i]);
                }
            }
        }
        
      public void SetVisibilityByOptions(VisibilityOptions options) {
          if (views == null) {
              return;
          }

          foreach (var t in views)
          {
              if (options.ViewsToShowContains(t.GetType()))
              {
                  Show(t);
              }
              
              if (options.ViewsToHideContains(t.GetType()))
              {
                  Hide(t);
              }
          }
      }
    }
}