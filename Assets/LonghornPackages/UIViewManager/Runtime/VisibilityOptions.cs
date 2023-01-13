using System;
using System.Collections;
using System.Collections.Generic;
using Longhorn.UIViewManager;
using Sirenix.Utilities;
using UnityEngine;

public class VisibilityOptions
{
    private List<Type> viewsToShow= new List<Type>();
    private List<Type> viewsToHide= new List<Type>();

    public static VisibilityOptions New => new VisibilityOptions();

    public VisibilityOptions Show(List<Type> views) {
        viewsToShow = views;
        return this;
    }
    
    public VisibilityOptions Hide(List<Type> views)
    {
        viewsToHide = views;
        return this;
    }

    public bool ViewsToShowContains(Type type) {
        if (viewsToShow.IsNullOrEmpty()) {
            return false;
        }
        
        return viewsToShow.Contains(type); 
    }
    
    public bool ViewsToHideContains(Type type) {
        if (viewsToHide.IsNullOrEmpty()) {
            return false;
        }
        
        return viewsToHide.Contains(type); 
    }
}