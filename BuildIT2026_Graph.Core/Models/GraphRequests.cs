using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildIT2026_Graph.Core.Models
{
    public class CreateNodeRequest
    {
        public string Label { get; set; } = string.Empty;
        public Dictionary<string, object?> Properties { get; set; } = new();
    }

    public class CreateRelationshipRequest
    {
        public string FromLabel { get; set; } = string.Empty;
        public string FromKey { get; set; } = string.Empty;
        public object? FromValue { get; set; }

        public string ToLabel { get; set; } = string.Empty;
        public string ToKey { get; set; } = string.Empty;
        public object? ToValue { get; set; }

        public string RelationshipType { get; set; } = string.Empty;
        public Dictionary<string, object?> Properties { get; set; } = new();
    }

    public class QueryRequest
    {
        public string Cypher { get; set; } = string.Empty;
        public Dictionary<string, object?> Parameters { get; set; } = new();
    }

    public class FindNodesRequest
    {
        public string Label { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public object? PropertyValue { get; set; }
    }
}
