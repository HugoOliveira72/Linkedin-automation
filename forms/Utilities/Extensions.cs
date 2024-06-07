﻿namespace forms.Utilities
{
    public static class Extensions
    {
        public static List<string> ToList(this CheckedListBox.CheckedItemCollection collection)
        {
            return collection.Cast<object>().Select(item => item.ToString()).ToList();
        } 
    }
}
