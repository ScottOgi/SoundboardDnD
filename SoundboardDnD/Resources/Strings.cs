using SoundboardDnD.Models;
using System;

namespace SoundboardDnD.Resources
{
    public static class Strings
    {
        public static string Placeholder = "[____]";
        public static string NewTrackTitle = "New track title";
        public static string EnterNewTrackTitle = "Enter a new title for this track";
        public static string NoTrackToRename = "No track to rename";
        public static string NoTrackToEdit = "No track loaded so no name to edit!";
        public static string NoTrackLoaded = "No track loaded for this button";
        public static string NoTrackToPlay = "No track to place";
    }

    public static class ControlGroups
    {
        public static string GroupBox = "gb";
        public static string TrackBar = "tb";
    }

    public static class StringExtensions
    {
        public static string GetGroupName(this string name) => name.Substring(name.IndexOf("-") + 1, name.Length - name.IndexOf("-") - 1);
        public static Group GetGroupFrom(this string name)
        {
            Group group = Group.None;
            Enum.TryParse(name, out group);
            return group;
        }
    }
}
