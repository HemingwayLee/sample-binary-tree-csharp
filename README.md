# sample-binary-tree-csharp

This project is modified from https://github.com/HemingwayLee/sample-binary-tree-cpp and is rewrited in C#.

## The node in binary tree  
```csharp
public class TreeNode {
  public int val = 0;
  public TreeNode left = null;
  public TreeNode right = null;

  public TreeNode(int x) { 
    this.val = x; 
  }
}
```

## Result of running this sample code
```
After inserting val 10..
10

After inserting val 5..
 10
 /
5

After inserting val 15..
 10
 / \
5  15

After inserting vals 9, 13..
   10
   / \
  /   \
 /     \
5      15
 \     /
  9   13

After inserting vals 2, 6, 12, 14, ..
     10
     / \
    /   \
   /     \
  5      15
 / \     /
2   9   13
   /   / \
  6   /   \
     12   14


After deleting a node (14) with no child..
     10
     / \
    /   \
   /     \
  5      15
 / \     /
2   9   13
   /   /
  6   12


After deleting a node (13) with left child..
     10
     / \
    /   \
   /     \
  5      15
 / \     /
2   9   12
   /
  6


After deleting a node (5) with left and right children..
     10
     / \
    /   \
   /     \
  6      15
 / \     /
2   9   12


After deleting a node (10, root node) with left and right children..
   12
   / \
  6  15
 / \
2   9
```