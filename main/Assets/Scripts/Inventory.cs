using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
///  A data structure consisting of an array of stacks,
///  where each stack is contains multiples of equivalent objects.
///  The number of slots cannot be changed once created.
///  The capacity per stack cannot be changed. 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Inventory<T>
{
    public static int DEF_NUMSLOTS = 8;
    public static int DEF_CAPACITY = 64;

    private int numSlots; //The number of inventory slots.
    private int capacity; //The number of items that each slot can contain. 

    private Stack<T>[] data; //The array of stacks containing the information. 
    private int[] sizes; //number of elements in each stack. 
    private List<int> emptySlots; //used to keep track of which slots are empty to ease it adding. 

    public Inventory() {
        init(DEF_NUMSLOTS, DEF_CAPACITY);
    }
    
    public Inventory(int numSlots, int capacity) {
        init(numSlots, capacity);
    }

    private void init(int numSlots, int capacity) {
        data = new Stack<T>[numSlots];
        emptySlots = new List<int>(numSlots);
        sizes = new int[numSlots];
        this.capacity = capacity;
        this.numSlots = numSlots;

        for (int i = 0; i < numSlots; i++) {
            data[i] = new Stack<T>(capacity);
            emptySlots.Add(i);
        }
    }

    /// <summary>
    /// Adds the given element into the next available slot. 
    /// If successful, the method will return the index that the item was placed in.
    /// Otherwise, the method returns -1. 
    /// </summary>
    /// <param name="item"></param>
    public int Add(T item) {

        int addIndex = -1;

        //Check for stacks holding equal objects. 
        for (int i = 0; i < data.Length; i++) {

            if (data[i].Count > 0 && data[i].Count < capacity && item.Equals(data[i].Peek())) {
                data[i].Push(item);
                addIndex = i;
            }
        }

        //Insert item into empty slot. 
        if (addIndex == -1 && emptySlots.Count > 0) {
            addIndex = emptySlots[0];
            emptySlots.RemoveAt(0);
            data[addIndex].Push(item);
        }

        return addIndex;
    }

    /// <summary>
    /// Removes an item from the given slot. 
    /// </summary>
    /// <param name="slot"></param>
    /// <returns>The item that was removed from the given slot.</returns>
    public T Pop(int slot) {
        if (data[slot].Count <= 0 || slot >= numSlots) {
            return default;
        }
        else {
            T ret = data[slot].Pop();

            if (data[slot].Count == 0) {
                emptySlots.Add(slot);
                emptySlots.Sort();
            }

            return ret;
        }
    }

    public T Peek(int slot) {
        if (slot >= numSlots || slot < 0 || data[slot].Count <= 0) {
            return default;
        }
        else {
            return data[slot].Peek();
        }
    }

    /// <summary>
    /// Clears the entire inventory. 
    /// </summary>
    public void Clear() {
        emptySlots.Clear(); //Clear emptySlots to avoid duplicate entries
        for(int i = 0; i < numSlots; i++) {
            data[i].Clear();
            emptySlots.Add(i);
        }
    }

    /// <summary>
    /// Clears the given inventory slot. 
    /// If slot is greater than or equal to the number of slots, the method silently fails. 
    /// </summary>
    /// <param name="slot"></param>
    public void ClearSlot(int slot) {
        if (slot >= numSlots) return;
        data[slot].Clear();
        emptySlots.Add(slot);
        emptySlots.Sort();
    }

    public int GetCapacity() {
        return capacity;
    }

    public int GetNumSlots() {
        return numSlots;
    }

    public int CountOfSlot(int slot) {
        if (slot >= numSlots) return -1;

        return data[slot].Count;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();

        sb.Append("[");
        for (int i = 0; i < numSlots - 1; i++) {
            if (data[i].Count == 0) {
                sb.Append("empty, ");
            }
            else {
                sb.Append(data[i].Peek() + " * " + data[i].Count + ", ");
            }
        }

        if (data[numSlots - 1].Count == 0) {
            sb.Append("empty]");
        }
        else {
            sb.Append(data[numSlots - 1].Peek() + " * " + data[numSlots - 1].Count + "]");
        }

        return sb.ToString();
    }
}
