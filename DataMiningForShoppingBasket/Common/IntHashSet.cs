using System.Collections.Generic;
using System;
using System.Linq;

namespace DataMiningForShoppingBasket.Common;

public class IntHashSet : HashSet<int>, IEquatable<IntHashSet>, IEqualityComparer<IntHashSet>
{
    public IntHashSet()
        : base(EqualityComparer<int>.Default)
    {
    }

    public IntHashSet(IEnumerable<int> collection)
        : base(collection, EqualityComparer<int>.Default)
    {
    }

    public bool Equals(IntHashSet other)
        => Equals(this, other);

    public bool Equals(IntHashSet x, IntHashSet y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.Count != y.Count) return false;

        return !x.Except(y).Any();
    }

    public int GetHashCode(IntHashSet obj)
        => obj.Aggregate(obj.Count, (current, x) => (current * 397) ^ x);
}