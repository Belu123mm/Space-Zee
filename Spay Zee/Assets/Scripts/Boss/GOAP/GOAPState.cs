using System.Collections.Generic;
using System.Linq;

public class GOAPState
{
    public Dictionary<BossState, object> valuesDictionary = new Dictionary<BossState, object>();
    public GOAPAction generatingAction = null;
    public int step = 0;

    #region CONSTRUCTOR
    public GOAPState(GOAPAction gen = null)
    {
        generatingAction = gen;
    }

    public GOAPState(GOAPState source, GOAPAction gen = null)
    {
        foreach (var elem in source.valuesDictionary)
        {
            if (valuesDictionary.ContainsKey(elem.Key))
                valuesDictionary[elem.Key] = elem.Value;
            else
                valuesDictionary.Add(elem.Key, elem.Value);
        }
        generatingAction = gen;
    }
    #endregion

    public override bool Equals(object obj)
    {
        var other = obj as GOAPState;
        var result =
            other != null
            && other.generatingAction == generatingAction       //Very important to keep! TODO: REVIEW
            && other.valuesDictionary.Count == valuesDictionary.Count
            && other.valuesDictionary.All(kv => kv.In(valuesDictionary));
        //&& other.values.All(kv => values.Contains(kv));
        return result;
    }

    public override int GetHashCode()
    {
        //Better hashing but slow.
        //var x = 31;
        //var hashCode = 0;
        //foreach(var kv in values) {
        //	hashCode += x*(kv.Key + ":" + kv.Value).GetHashCode);
        //	x*=31;
        //}
        //return hashCode;

        //Heuristic count+first value hash multiplied by polynomial primes
        return valuesDictionary.Count == 0 ? 0 : 31 * valuesDictionary.Count + 31 * 31 * valuesDictionary.First().GetHashCode();
    }

    public override string ToString()
    {
        var str = "";
        foreach (var kv in valuesDictionary.OrderBy(x => x.Key))
        {
            str += $"{kv.Key.ToString():12} : {kv.Value}\n";
        }
        return "--->" + (generatingAction != null ? generatingAction.name : "NULL") + "\n" + str;
    }
}
