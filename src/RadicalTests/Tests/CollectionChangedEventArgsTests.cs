using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.ComponentModel;


namespace RadicalTests
{
    [TestClass]
    public class CollectionChangedEventArgsTests
    {
        [TestMethod]
        public void collectionChangedEventArgs_ctor_changeType_should_set_default_values()
        {
            var cType = CollectionChangeType.Reset;

            var target = new CollectionChangedEventArgs<Object>( cType );

            Assert.AreEqual(cType, target.ChangeType);
            Assert.AreEqual(-1, target.Index);
            Assert.IsNull(target.Item);
            Assert.AreEqual(-1, target.OldIndex);
        }

        [TestMethod]
        public void collectionChangedEventArgs_ctor_changeType_index_should_set_default_values()
        {
            var cType = CollectionChangeType.Reset;
            var index = 10;

            var target = new CollectionChangedEventArgs<Object>( cType, index );

            Assert.AreEqual(cType, target.ChangeType);
            Assert.AreEqual(index, target.Index);
            Assert.IsNull(target.Item);
            Assert.AreEqual(-1, target.OldIndex);
        }

        [TestMethod]
        public void collectionChangedEventArgs_ctor_changeType_index_oldIndex_item_should_set_default_values()
        {
            var item = new Object();
            var cType = CollectionChangeType.ItemMoved;
            var index = 10;
            var oldIndex = 1;

            var target = new CollectionChangedEventArgs<Object>( cType, index, oldIndex, item );

            Assert.AreEqual(cType, target.ChangeType);
            Assert.AreEqual(index, target.Index);
            Assert.AreEqual(item, target.Item);
            Assert.AreEqual(oldIndex,target.OldIndex);
        }
    }
}
