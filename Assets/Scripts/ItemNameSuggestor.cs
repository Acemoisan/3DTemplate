using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using System.Linq;

public struct ItemNameTag : IQcSuggestorTag
{

}

public struct EntityNameTag : IQcSuggestorTag
{

}

public struct PrefabNameTag : IQcSuggestorTag
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

public sealed class EntityNameAttribute : SuggestorTagAttribute
{
    private readonly IQcSuggestorTag[] _tags = { new EntityNameTag() };

    public override IQcSuggestorTag[] GetSuggestorTags()
    {
        return _tags;
    }
}

public sealed class PrefabNameAttribute : SuggestorTagAttribute
{
    private readonly IQcSuggestorTag[] _tags = { new PrefabNameTag() };

    public override IQcSuggestorTag[] GetSuggestorTags()
    {
        return _tags;
    }
}

public class ItemNameSuggestor : BasicCachedQcSuggestor<string>
{
    public static ItemDatabase itemDatabase { get; set; }
    //public static EntityDatabase entityDatabase { get; set; }
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

public class EntityNameSuggestor : BasicCachedQcSuggestor<string>
{
    //public static ItemDatabase itemDatabase { get; set; }
    public static EntityDatabase entityDatabase { get; set; }
    protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
    {
        return context.HasTag<EntityNameTag>();
    }

    protected override IQcSuggestion ItemToSuggestion(string itemName)
    {
        return new RawSuggestion(itemName, true);
    }

    protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
    {
        if (entityDatabase != null)
        {
           return entityDatabase.entities.Select(entity => entity.name);
        }
        else
        {
           return new string[0];
        }
    }
}

public class PrefabNameSuggestor : BasicCachedQcSuggestor<string>
{
    //public static ItemDatabase itemDatabase { get; set; }
    public static EntityDatabase entityDatabase { get; set; }
    protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
    {
        return context.HasTag<PrefabNameTag>();
    }

    protected override IQcSuggestion ItemToSuggestion(string itemName)
    {
        return new RawSuggestion(itemName, true);
    }

    protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
    {
        if (entityDatabase != null)
        {
            return entityDatabase.prefabs.Select(prefab => prefab.name);
        }
        else
        {
            return new string[0];
        }
    }
}
