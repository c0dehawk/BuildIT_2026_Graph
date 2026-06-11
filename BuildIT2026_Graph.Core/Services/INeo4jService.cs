using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildIT2026_Graph.Core.Services
{
    public interface INeo4jService
    {
        Task<object?> CreateNodeAsync(string label, Dictionary<string, object?> properties);
        Task<object?> CreateRelationshipAsync(
            string fromLabel,
            string fromKey,
            object? fromValue,
            string toLabel,
            string toKey,
            object? toValue,
            string relationshipType,
            Dictionary<string, object?> relationshipProperties);

        Task<List<Dictionary<string, object?>>> UpdateNodeAsync(string cypher, Dictionary<string, object?> parameters);

        Task<List<Dictionary<string, object?>>> QueryAsync(string cypher, Dictionary<string, object?> parameters);
        Task<List<object?>> FindNodesAsync(string label, string propertyName, object? propertyValue);
    }
}
