using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A gameobject used to test class functionality that can't be easily 
/// tested within a scene.  
/// </summary>
public class Diagnostic : MonoBehaviour
{

    Inventory<string> drawer;
    
    // Start is called before the first frame update
    void Start()
    {
        InitTestInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Assert(string name, int expected, int received) {
        if (expected == received) {
            Debug.Log("Test " + name + " passed!");
        }
        else {
            Debug.LogError("Test " + name + " failed: Expected " + expected + ", but received: " + received);
        }
    }

    public void Assert(string name, object expected, object received) {

        if ((expected == null && received == null) || expected.Equals(received)) {
            Debug.Log("Test " + name + " passed!");
        }
        else {
            Debug.LogError("Test " + name + " failed: Expected " + expected + ", but received: " + received);
        }
    }

    public void InitTestInventory() {
        drawer = new Inventory<string>();

        TestInventoryAdd();
        TestInventoryClear();
        TestInventoryPop();
        TestInventoryPeek();
    }

    private void TestInventoryAdd() {
        Debug.Log("Testing Add: ");
        Assert("Add to empty inventory", 0, drawer.Add("crew"));
        Assert("Add same item ", 0, drawer.Add("crew"));
        Assert("Add different item ", 1, drawer.Add("ankle"));

        for (int i = 0; i < 62; i++) {
            drawer.Add("crew");
        }

        Assert("Add to full stack with empty slots ", 2, drawer.Add("crew"));

        drawer = new Inventory<string>();

        for (int i = 0; i < drawer.GetCapacity() * drawer.GetNumSlots(); i++) {
            drawer.Add("crew");
        }

        Assert("Adding to full inventory", -1, drawer.Add("crew"));

        for (int i = 0; i < drawer.GetNumSlots(); i++) {
            Assert("stack count of slot " + i, 64, drawer.CountOfSlot(i));
        }
    }

    private void TestInventoryClear() {
        Debug.Log(drawer);

        drawer.Clear();

        Debug.Log(drawer);

        Assert("Clearing then attemping to add", 0, drawer.Add("crew"));

        Debug.Log(drawer);

        drawer.ClearSlot(0);

        Debug.Log(drawer);

        Assert("Clearing first slot only", 0, drawer.CountOfSlot(0));
    }

    private void TestInventoryPop() {
        drawer.Add("crew");
        drawer.Add("ankle");
        drawer.Add("knee-hi");

        Debug.Log(drawer);

        Assert("Removing from inventory slot 0", "crew", drawer.Pop(0));

        Debug.Log(drawer);

        drawer.Add("crew");
        drawer.Add("fancy");
        drawer.Add("patterned");
        drawer.Add("fluffy");
        drawer.Add("clean");
        drawer.Add("dirty");

        Debug.Log(drawer);

        Assert("Removing from slot and then adding", "fancy", drawer.Pop(3));
    }

    private void TestInventoryPeek() {
        Assert("Peeking a non-existent slot", default(string), drawer.Peek(-1));
        Assert("Peeking a non-existent slot", default(string), drawer.Peek(8));

        Assert("Peeking an existent slot", "crew", drawer.Peek(0));
    }
}
