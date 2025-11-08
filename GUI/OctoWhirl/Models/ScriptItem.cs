using Microsoft.CodeAnalysis;

namespace OctoWhirl.GUI.Models
{
    public class ScriptItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DocumentId DocumentId { get; set; }
    }
}
