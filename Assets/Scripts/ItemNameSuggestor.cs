using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using System.Linq;

public struct ItemNameTag : IQcSuggestorTag
{

}

public sealed class ItemNameAttribute : SuggestorTagAttribute
{
    private readonly IQcSuggestorTag[] _tags = { new ItemNameTag() };

    public override IQcSuggestorTag[] GetSuggestorTags()
    {
        return _tags;
    }
}

public class ItemNameSuggestor : BasicCachedQcSuggestor<string>
{
    public static ItemDatabase itemDatabase { get; set; }
    protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
    {
        return context.HasTag<ItemNameTag>();
    }

    protected override IQcSuggestion ItemToSuggestion(string itemName)
    {
        return new RawSuggestion(itemName, true);
    }

    protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
    {
        if (itemDatabase != null)
        {
            return itemDatabase.items.Select(item => item.name);
        }
        else
        {
            return new string[0];
        }    
    }
}
