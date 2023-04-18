using Items.Data;

namespace Items.Core
{
    public abstract class Item
    {
        protected Item(ItemDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public ItemDescriptor Descriptor { get; }
        public abstract int Amount { get; }
        public abstract void Use();
    }
}