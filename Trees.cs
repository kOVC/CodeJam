using System;
using System.Collections.Generic;
using System.Text;

namespace CJ {
    public class TreeNode<ItemType> {
        #region Protected Fields
        protected internal ItemType _value;
        protected internal TreeNode<ItemType> _parent;
        protected internal List<TreeNode<ItemType>> _children;
        protected internal int _index;
        #endregion
        #region Public Properties
        public ItemType Value { get { return _value; } set { _value = value; } }
        public TreeNode<ItemType> Parent { get { return _parent; } }
        public List<TreeNode<ItemType>> Children { get { return _children; } }
        public int Index { get { return _index; } }

        public TreeNode<ItemType> FirstChild { get { return _children != null && _children.Count != 0 ? _children[0] : null; } }
        public TreeNode<ItemType> LastChild { get { return _children != null && _children.Count != 0 ? _children[_children.Count - 1] : null; } }
        public TreeNode<ItemType> NextSibling { get { return _parent != null && _parent._children != null && _parent._children.Count > _index + 1 ? _parent._children[_index + 1] : null; } }
        public TreeNode<ItemType> PreviousSibling { get { return _parent != null && _parent._children != null && _index > 0 ? _parent._children[_index - 1] : null; } }
        public TreeNode<ItemType> FirstSibling { get { return _parent != null && _parent._children != null && _parent._children.Count > 0 ? _parent._children[0] : null; } }
        public TreeNode<ItemType> LastSibling { get { return _parent != null && _parent._children != null && _parent._children.Count > 0 ? _parent._children[_parent._children.Count - 1] : null; } }

        public int ChildrenCount { get { return _children == null ? -1 : _children.Count; } }
        public bool HasChildren { get { return _children != null && _children.Count > 0; } }
        public bool IsRoot { get { return _parent == null; } }
        public bool IsLeaf { get { return _children == null || _children.Count == 0; } }
        #endregion
        #region Constructors
        protected internal TreeNode() { _initCon(default(ItemType), null, new List<TreeNode<ItemType>>(), -1); }
        protected internal TreeNode(ItemType value) { _initCon(value, null, new List<TreeNode<ItemType>>(), -1); }
        protected internal TreeNode(ItemType value, TreeNode<ItemType> parent, int index) { _initCon(value, parent, new List<TreeNode<ItemType>>(), index); }
        protected internal TreeNode(ItemType value, TreeNode<ItemType> parent, int index, int childrenCapacity) { _initCon(value, parent, new List<TreeNode<ItemType>>(childrenCapacity), index); }
        protected internal TreeNode(ItemType value, TreeNode<ItemType> parent, int index, List<TreeNode<ItemType>> children) { _initCon(value, parent, children, index); }
        private void _initCon(ItemType value, TreeNode<ItemType> parent, List<TreeNode<ItemType>> children, int index) { _children = children; _index = index; _parent = parent; _value = value; }
        #endregion
        #region Public Methods
        public void AppendChild(ItemType value) {
            TreeNode<ItemType> n = new TreeNode<ItemType>(value, this, _children.Count);
            _children.Add(n);
        }
        public void AppendChild(TreeNode<ItemType> childNode) { if (childNode._parent != null) { childNode._parent.Remove(childNode); } childNode._parent = this; childNode._index = _children.Count; _children.Add(childNode); }
        public void AddChildAt(int index, ItemType value) {
            TreeNode<ItemType> n = new TreeNode<ItemType>(value, this, index);
            _children.Insert(index, n);
            for (int i = index + 1; i < _children.Count; i++) { _children[i]._index = i; }
        }
        public void AddChildAt(int index, TreeNode<ItemType> childNode) { 
            childNode._parent = this; childNode._index = index; 
            _children.Insert(index, childNode);
            for (int i = index + 1; i < _children.Count; i++) { _children[i]._index = i; }
        }
        public TreeNode<ItemType> Remove(int index) {
            TreeNode<ItemType> r = _children[index];
            _children.RemoveAt(index);
            for (int i = index; i < _children.Count; i++) { _children[i]._index = i; }
            r._parent = null; r._index = -1;
            return r;
        }
        public void Remove(TreeNode<ItemType> node) {
            int i = _children.IndexOf(node);
            if (i > -1) {
                _children[i]._parent = null; _children[i]._index = -1;
                _children.RemoveAt(i);
                for (; i < _children.Count; i++) { _children[i]._index = i; }
            }
        }
        public void Clear() {
            for (int i = 0; i < _children.Count; i++) { _children[i]._parent = null; }
            _children.Clear(); 
        }
        #endregion 
    }
    public class Tree<ItemType> {
        #region Protected Fields
        protected TreeNode<ItemType> _root;
        #endregion
        #region Public Properties
        public TreeNode<ItemType> Root { get { return _root; } }
        #endregion
        #region Constructors
        public Tree() { _root = new TreeNode<ItemType>(); }
        public Tree(ItemType value) { _root = new TreeNode<ItemType>(value); }
        public Tree(int childrenCapacity) { _root = new TreeNode<ItemType>(default(ItemType), null, -1, childrenCapacity); }
        public Tree(ItemType value, int childrenCapacity) { _root = new TreeNode<ItemType>(value, null, -1, childrenCapacity); }
        public Tree(ItemType value, IEnumerable<ItemType> childrenValues) { 
            _root = new TreeNode<ItemType>(value, null, -1);
            IEnumerator<ItemType> e = childrenValues.GetEnumerator();
            while (e.MoveNext()) { _root.AppendChild(e.Current); }
        }
        #endregion
    }
}
