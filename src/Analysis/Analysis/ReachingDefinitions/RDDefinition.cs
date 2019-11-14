using System;
using Analysis.AST;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDDefinition : IEquatable<RDDefinition>
    {
        private int _identId;
        private int? _label;
        private string _identName;

        public string IdentityID => _identId.ToString();
        public string Label => _label?.ToString() ?? "?";
        public string IdentityName => _identName;


        public RDDefinition(int identId, int label, string name)
        {
            _identId = identId;
            _label = label;
            _identName = name;
        }

        public RDDefinition(int identId, string name)
        {
            _identId = identId;
            _label = null;
            _identName = name;
        }

        public override string ToString() => $"(#{_identId} {_identName}), L{_label?.ToString() ?? "?"})";

        public bool Equals(RDDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _identId == other._identId && _label == other._label;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RDDefinition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_identId * 397) ^ _label.GetHashCode();
            }
        }
    }
}