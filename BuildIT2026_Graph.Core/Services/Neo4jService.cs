using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuildIT2026_Graph.Core.Services
{
    public class Neo4jService : INeo4jService
    {
        private readonly IDriver _driver;

        public Neo4jService(IConfiguration configuration)
        {
            var uri = configuration["Neo4j:Uri"] ?? throw new InvalidOperationException("Neo4j:Uri is missing");
            var user = configuration["Neo4j:User"] ?? throw new InvalidOperationException("Neo4j:User is missing");
            var password = configuration["Neo4j:Password"] ?? throw new InvalidOperationException("Neo4j:Password is missing");

            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task<object?> CreateNodeAsync(string label, Dictionary<string, object?> properties)
        {
            var safeLabel = ValidateIdentifier(label);
            var normalizedProps = NormalizeDictionary(properties);

            var cypher = $@"
                CREATE (n:{safeLabel})
                SET n += $props
                RETURN n";

            await using var session = _driver.AsyncSession();

            var result = await session.ExecuteWriteAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new { props = normalizedProps });
                var record = await cursor.SingleAsync();
                return record["n"];
            });

            return ConvertNeo4jValue(result);
        }

        public async Task<List<Dictionary<string, object?>>> UpdateNodeAsync(string cypher, Dictionary<string, object?> parameters)
        {
            var normalizedParams = NormalizeDictionary(parameters);

            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, normalizedParams);
                var records = await cursor.ToListAsync();

                return records.Select(record =>
                {
                    var dict = new Dictionary<string, object?>();

                    foreach (var key in record.Keys)
                    {
                        dict[key] = ConvertNeo4jValue(record[key]);
                    }

                    return dict;
                }).ToList();
            });
        }

        public async Task<object?> CreateRelationshipAsync(
            string fromLabel,
            string fromKey,
            object? fromValue,
            string toLabel,
            string toKey,
            object? toValue,
            string relationshipType,
            Dictionary<string, object?> relationshipProperties)
        {
            var safeFromLabel = ValidateIdentifier(fromLabel);
            var safeFromKey = ValidateIdentifier(fromKey);
            var safeToLabel = ValidateIdentifier(toLabel);
            var safeToKey = ValidateIdentifier(toKey);
            var safeRelType = ValidateIdentifier(relationshipType);
            var normalizedRelProps = NormalizeDictionary(relationshipProperties);

            var cypher = $@"
                MATCH (a:{safeFromLabel})
                WHERE a.{safeFromKey} = $fromValue
                MATCH (b:{safeToLabel})
                WHERE b.{safeToKey} = $toValue
                CREATE (a)-[r:{safeRelType}]->(b)
                SET r += $relProps
                RETURN a, r, b";

            await using var session = _driver.AsyncSession();

            var result = await session.ExecuteWriteAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new
                {
                    fromValue = NormalizeValue(fromValue),
                    toValue = NormalizeValue(toValue),
                    relProps = normalizedRelProps
                });

                var record = await cursor.SingleAsync();

                return new Dictionary<string, object?>
                {
                    ["from"] = ConvertNeo4jValue(record["a"]),
                    ["relationship"] = ConvertNeo4jValue(record["r"]),
                    ["to"] = ConvertNeo4jValue(record["b"])
                };
            });

            return result;
        }

        public async Task<List<object?>> FindNodesAsync(string label, string propertyName, object? propertyValue)
        {
            var safeLabel = ValidateIdentifier(label);
            var safePropertyName = ValidateIdentifier(propertyName);

            var cypher = $@"
                MATCH (n:{safeLabel})
                WHERE n.{safePropertyName} = $value
                RETURN n";

            await using var session = _driver.AsyncSession();

            return await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, new { value = NormalizeValue(propertyValue) });
                var records = await cursor.ToListAsync();

                return records
                    .Select(r => ConvertNeo4jValue(r["n"]))
                    .ToList();
            });
        }

        public async Task<List<Dictionary<string, object?>>> QueryAsync(string cypher, Dictionary<string, object?> parameters)
        {
            var normalizedParams = NormalizeDictionary(parameters);

            await using var session = _driver.AsyncSession();

            return await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(cypher, normalizedParams);
                var records = await cursor.ToListAsync();

                return records.Select(record =>
                {
                    var dict = new Dictionary<string, object?>();

                    foreach (var key in record.Keys)
                    {
                        dict[key] = ConvertNeo4jValue(record[key]);
                    }

                    return dict;
                }).ToList();
            });
        }

        private static string ValidateIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier cannot be empty.");

            if (!Regex.IsMatch(identifier, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                throw new ArgumentException($"Invalid identifier: {identifier}");

            return identifier;
        }

        private static Dictionary<string, object?> NormalizeDictionary(Dictionary<string, object?> input)
        {
            return input.ToDictionary(k => k.Key, v => NormalizeValue(v.Value));
        }

        private static object? NormalizeValue(object? value)
        {
            if (value is null)
                return null;

            if (value is JsonElement json)
                return NormalizeJsonElement(json);

            if (value is Dictionary<string, object?> dict)
                return dict.ToDictionary(k => k.Key, v => NormalizeValue(v.Value));

            if (value is IEnumerable<object?> enumerable && value is not string)
                return enumerable.Select(NormalizeValue).ToList();

            return value;
        }

        private static object? NormalizeJsonElement(JsonElement json)
        {
            return json.ValueKind switch
            {
                JsonValueKind.String => json.GetString(),
                JsonValueKind.Number when json.TryGetInt64(out var l) => l,
                JsonValueKind.Number when json.TryGetDouble(out var d) => d,
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Object => json.EnumerateObject()
                    .ToDictionary(p => p.Name, p => NormalizeJsonElement(p.Value)),
                JsonValueKind.Array => json.EnumerateArray()
                    .Select(NormalizeJsonElement)
                    .ToList(),
                _ => json.ToString()
            };
        }

        private static object? ConvertNeo4jValue(object? value)
        {
            if (value is null)
                return null;

            return value switch
            {
                INode node => new
                {
                    id = node.ElementId,
                    labels = node.Labels,
                    properties = node.Properties.ToDictionary(k => k.Key, v => ConvertNeo4jValue(v.Value))
                },

                IRelationship rel => new
                {
                    id = rel.ElementId,
                    type = rel.Type,
                    startNodeId = rel.StartNodeElementId,
                    endNodeId = rel.EndNodeElementId,
                    properties = rel.Properties.ToDictionary(k => k.Key, v => ConvertNeo4jValue(v.Value))
                },

                IReadOnlyDictionary<string, object> dict => dict.ToDictionary(k => k.Key, v => ConvertNeo4jValue(v.Value)),

                IEnumerable enumerable when value is not string =>
                    enumerable.Cast<object?>().Select(ConvertNeo4jValue).ToList(),

                _ => value
            };
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
