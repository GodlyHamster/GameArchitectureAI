using System.Collections.Generic;

public static class LinkedListNodeExtension
{
    public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> item, LinkedList<T> list)
    {
        if (item.Next == null)
        {
            return list.First;
        }
        return item.Next;
    }
}
