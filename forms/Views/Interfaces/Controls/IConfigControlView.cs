﻿namespace forms.Views.Interfaces.Control
{
    public interface IConfigControlView
    {
        public string? ResolutionType { get; set; }
        public int ResolutionTypeSelectedIndex { get; set; }
        public string? Resolution { get; set; }

        //Events /acitions
        event EventHandler ConfigFormLoaded;
        event EventHandler SaveConfigEvent;
        event EventHandler UpdateComponentsEvent;
    }
}
