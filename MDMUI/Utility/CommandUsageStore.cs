using System;
using System.Collections.Generic;
using System.Linq;

namespace MDMUI.Utility
{
    public static class CommandUsageStore
    {
        private static readonly object Gate = new object();

        public static Dictionary<string, CommandUsageEntry> LoadIndex()
        {
            lock (Gate)
            {
                AppPreferences prefs = AppPreferencesStore.Load();
                if (prefs.CommandUsage == null)
                {
                    prefs.CommandUsage = new List<CommandUsageEntry>();
                }

                return prefs.CommandUsage
                    .Where(entry => !string.IsNullOrWhiteSpace(entry.FormName))
                    .GroupBy(entry => entry.FormName, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(group => group.Key, group => group.OrderByDescending(e => e.LastUsedUtc).First(), StringComparer.OrdinalIgnoreCase);
            }
        }

        public static void RecordUse(string formName, string title, string group)
        {
            if (string.IsNullOrWhiteSpace(formName)) return;

            lock (Gate)
            {
                AppPreferences prefs = AppPreferencesStore.Load();
                if (prefs.CommandUsage == null)
                {
                    prefs.CommandUsage = new List<CommandUsageEntry>();
                }

                CommandUsageEntry entry = prefs.CommandUsage.FirstOrDefault(e => string.Equals(e.FormName, formName, StringComparison.OrdinalIgnoreCase));
                if (entry == null)
                {
                    entry = new CommandUsageEntry
                    {
                        FormName = formName,
                        Title = title ?? string.Empty,
                        Group = group ?? string.Empty,
                        UseCount = 0,
                        LastUsedUtc = DateTime.MinValue
                    };
                    prefs.CommandUsage.Add(entry);
                }

                entry.Title = title ?? entry.Title;
                entry.Group = group ?? entry.Group;
                entry.UseCount += 1;
                entry.LastUsedUtc = DateTime.UtcNow;

                AppPreferencesStore.Save(prefs);
            }
        }

        public static void TogglePin(string formName)
        {
            if (string.IsNullOrWhiteSpace(formName)) return;

            lock (Gate)
            {
                AppPreferences prefs = AppPreferencesStore.Load();
                if (prefs.CommandUsage == null)
                {
                    prefs.CommandUsage = new List<CommandUsageEntry>();
                }

                CommandUsageEntry entry = prefs.CommandUsage.FirstOrDefault(e => string.Equals(e.FormName, formName, StringComparison.OrdinalIgnoreCase));
                if (entry == null)
                {
                    entry = new CommandUsageEntry
                    {
                        FormName = formName,
                        Title = string.Empty,
                        Group = string.Empty,
                        UseCount = 0,
                        LastUsedUtc = DateTime.UtcNow,
                        Pinned = true
                    };
                    prefs.CommandUsage.Add(entry);
                }
                else
                {
                    entry.Pinned = !entry.Pinned;
                    entry.LastUsedUtc = DateTime.UtcNow;
                }

                AppPreferencesStore.Save(prefs);
            }
        }
    }
}
