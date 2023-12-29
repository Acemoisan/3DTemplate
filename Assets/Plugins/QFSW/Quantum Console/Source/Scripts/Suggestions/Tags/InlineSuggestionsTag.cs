using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC.Suggestors.Tags
{
    /// <summary>
    /// Object that holds all the inlined suggestions provided 
    /// to <see cref="SuggestionsAttribute"/>.
    /// </summary>
    public struct InlineSuggestionsTag : IQcSuggestorTag
    {
        public readonly IEnumerable<string> Suggestions;

        public InlineSuggestionsTag(IEnumerable<string> suggestions)
        {
            Suggestions = suggestions;
        }
    }

    /// <summary>
    /// Provides suggestions for the parameter.
    /// </summary>
    public sealed class SuggestionsAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags;

        public SuggestionsAttribute(params object[] suggestions)
        {
            IEnumerable<string> suggestionStrings;

            // Check if the first argument is an IEnumerable of strings
            if (suggestions.Length == 1 && suggestions[0] is IEnumerable<string> stringEnumerable)
            {
                suggestionStrings = stringEnumerable;
            }
            else
            {
                // Convert each suggestion to a string
                suggestionStrings = suggestions.Select(o => o.ToString());
            }

            InlineSuggestionsTag tag = new InlineSuggestionsTag(suggestionStrings);
            _tags = new IQcSuggestorTag[] { tag };
        }

        public override IQcSuggestorTag[] GetSuggestorTags() => _tags;
    }
}
