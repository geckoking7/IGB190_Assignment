using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class MyListSearchProvider : ScriptableObject, ISearchWindowProvider
{
    private string[] listItems;
    private UnityAction<int> callback;
    public MyListSearchProvider(string[] items, UnityAction<int> callback)
    {
        Setup(items, callback);
    }

    public void Setup(string[] items, UnityAction<int> callback)
    {
        listItems = items;
        this.callback = callback;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        searchList.Add(new SearchTreeGroupEntry(new GUIContent("Select a Function", "Test Tooltip"), 0));
        List<string> groups = new List<string>();
        int id = 0;
        foreach (string item in listItems)
        {
            string[] entryTitle = item.Split('/');
            string groupName = "";
            for (int i = 0; i < entryTitle.Length - 1; i++)
            {
                groupName += entryTitle[i];
                if (!groups.Contains(groupName))
                {
                    searchList.Add(new SearchTreeGroupEntry(new GUIContent("" + entryTitle[i], "Test Tooltip"), i + 1));
                    groups.Add(groupName);
                }
                groupName += "/";
            }
            SearchTreeEntry entry = new SearchTreeEntry(new GUIContent("   " + entryTitle.Last(), "Please work."));
            entry.level = entryTitle.Length;
            entry.userData = id;
            id++;
            searchList.Add(entry);
        }
        return searchList;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        callback.Invoke((int)SearchTreeEntry.userData);
        return true;
    }
}
