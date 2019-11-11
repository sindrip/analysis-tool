using System;
using Analysis.AST;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDDefinition : IEquatable<RDDefinition>
    {
        private int _identId;
        private int? _label;

        public RDDefinition(int identId, int label)
        {
            _identId = identId;
            _label = label;
        }

        public RDDefinition(int identId)
        {
            _identId = identId;
            _label = null;
        }

        public override string ToString() => $"({_identId}, {_label?.ToString() ?? "?"})";

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