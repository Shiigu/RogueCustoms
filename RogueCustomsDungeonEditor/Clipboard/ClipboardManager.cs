using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Clipboard
{
    public static class ClipboardManager
    {
        private static readonly Dictionary<string, object> clipboardData = new Dictionary<string, object>();

        public static event EventHandler ClipboardContentsChanged;

        public static void Copy(string key, object data)
        {
            try
            {
                if (data != null)
                {
                    clipboardData[key] = data;
                    RaiseClipboardContentsChanged();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error copying to clipboard: {ex.Message}");
            }
        }

        public static T Paste<T>(string key)
        {
            try
            {
                if (clipboardData.ContainsKey(key) && clipboardData[key] is T)
                {
                    return (T)clipboardData[key];
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error copying to clipboard: {ex.Message}");
            }

            return default;
        }

        public static bool ContainsData(string key)
        {
            return clipboardData.ContainsKey(key);
        }

        public static void Clear()
        {
            clipboardData.Clear();
        }

        public static void RemoveData(string key)
        {
            if (clipboardData.ContainsKey(key))
            {
                clipboardData.Remove(key);
                RaiseClipboardContentsChanged();
            }
        }

        private static void RaiseClipboardContentsChanged()
        {
            ClipboardContentsChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
