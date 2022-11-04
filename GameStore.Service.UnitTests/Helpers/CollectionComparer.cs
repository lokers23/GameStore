namespace GameStore.Service.UnitTests.Helpers;

public static class CollectionComparer
{
    public static bool CompareCollection<T>(ICollection<T> firstCollection, ICollection<T> secondCollection)
    {
        var firstLength = firstCollection.Count;
        var secondLength = secondCollection.Count;
        if (firstLength != secondLength)
        {
            return false;
        }

        foreach (var first in firstCollection)
        {

        }
        
        return true;
    }
}