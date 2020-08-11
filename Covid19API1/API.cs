using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
//using System.Web.UI;
//using System.Web.Script.Serialization;
/// <summary>
/// Backend Test Task, implemented by Carl Stermann-Lücke, August 2020
/// </summary>
namespace Covid19API1
{
    public class API
    {
        private static List<Entry> entries = new List<Entry>();//TODO: Should I use a smarter data structure than a list?
        /// <summary>
        /// This method creates a new entry for a patient of the specified state, where the time is set to the point where the method was called.
        /// It returns an id that can be used to find a certain patient again. The id is just counting upwards from 0.
        /// </summary>
        /// <param name="state">An enum. Can be Infected (0), Recovered (1), or Deceased (2).</param>
        /// <returns>The index of the new entry in all the data collected.</returns>
        public static long CreateEntry(State state)
        {
            return CreateEntry(state, null);
            //Default parameters have to be compile-time-constant.
            //This is why I overload the method instead of using a default parameter for the time.
        }
        /// <summary>
        /// This method creates a new entry for a patient of the specified state for the specified time.
        /// It returns an id that can be used to find a certain patient again. The id is just counting upwards from 0.
        /// </summary>
        /// <param name="state">An enum. Can be Infected (0), Recovered (1), or Deceased (2).</param>
        /// <param name="time">The timestamp associated to the entry, such as time of infection or first test. Optional, "today" by default.</param>
        /// <returns>The index of the new entry in all the data collected.</returns>
        public static long CreateEntry(State state, DateTime? time)
        {
            DateTime t = DateTime.Now.Date;
            if(time.HasValue)
            {
                t = time.Value;
            }
            long id = entries.Count;
            entries.Add(new Entry(id, state, t));
            return id;
        }
        /// <summary>
        /// This method updates the first entry that it finds that matches the specified timestamp and current state.
        /// Since no further personal data are collected and the times of the update are not stored, it does not matter which exact entry is updated, as they are equivalent.
        /// Note that this update function does not perform consistency checks: It is possible to update an already deceased patient to be recovered or infected again.
        /// </summary>
        /// <param name="time">The timestamp of the entry to be updated.</param>
        /// <param name="oldState">The current state of the entry to be updated.</param>
        /// <param name="newState">The new state that you want to update the entry to.</param>
        /// <param name="updateTime">The timestamp when the update occurred (optional, "today" by default).</param>
        /// <returns></returns>
        public static bool UpdateEntry(DateTime time, State oldState, State newState, DateTime? updateTime)
        {
            var allMatchingEntries = entries.FindAll(e => e.Time == time && e.State == oldState);
            if(allMatchingEntries.Count > 0)
            {
                allMatchingEntries[0].State = newState;
                return true;
            }
            return false;
        }
        /// <summary>
        /// This method updates an entry by id.
        /// If you stored the id returned by the CreateEntry method, you can update that exact entry using this method.
        /// Note that this update function does not perform consistency checks: It is possible to update an already deceased patient to be recovered or infected again.
        /// </summary>
        /// <param name="id">The ID of the entry to be updated.</param>
        /// <param name="state">The state that you want to update the entry to.</param>
        /// <param name="time">The timestamp when the update occurred (optional, "today" by default).</param>
        /// <returns></returns>
        public static bool UpdateEntry(long id, State state, DateTime? time)
        {
            var allMatchingEntries = entries.FindAll(e => e.ID == id);
            int numEntries = allMatchingEntries.Count;
            if(numEntries == 1)
            {
                allMatchingEntries[0].State = state;
                if (time.HasValue)
                {
                    allMatchingEntries[0].Time = time.Value;
                }
                else
                {
                    allMatchingEntries[0].Time = DateTime.Now.Date;
                }
                return true;
            } else if(numEntries == 0)
            {
                return false;
            }
            Debug.Write("ID was given multiple times.");
            return false;
        }
        /// <summary>
        /// This method finds all entries that were originally reported on a certain time.
        /// Optionally, they can be filtered by their current state.
        /// </summary>
        /// <param name="timeOfFirstReport">The original timestamp of the entries.</param>
        /// <param name="currentState">The current state of the entries (optional).</param>
        /// <returns></returns>
        public static List<Entry> FindCases(DateTime timeOfFirstReport, State? currentState)
        {
            List<Entry> allCasesOfSpecifiedTime = entries.FindAll(e => e.OriginalTime == timeOfFirstReport);
            List<Entry> filteredByState = currentState.HasValue ? allCasesOfSpecifiedTime.FindAll(e => e.State == currentState.Value) : allCasesOfSpecifiedTime;
            return filteredByState;
        }
        public static void SaveToFile(string filename = "./savedEntries.dat")
        {
            File.WriteAllText(filename, JsonSerializer.Serialize(entries));
        }
        public static void LoadFromFile(string filename = "./savedEntries.dat")
        {
            string jsonString = File.ReadAllText(filename);
            entries = JsonSerializer.Deserialize<List<Entry>>(jsonString);
        }
    }
}
