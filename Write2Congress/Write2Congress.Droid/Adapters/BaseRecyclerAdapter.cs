using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Java.Lang;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Adapters
{
    public abstract class BaseRecyclerAdapter : RecyclerView.Adapter
    {
        protected Logger _logger;
        protected int _currentEnfOfListViewHolderHash;
        public event EndOfListReachedEventHandler OnEndOfListReached;
        public event EndOfListReachedEventHandler OnEndOfListElementRecycled;

        public BaseRecyclerAdapter()
        {
            _logger = new Logger(Class.SimpleName);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position == (ItemCount - 1))
            {
                RaiseEndOfListReached();
                _currentEnfOfListViewHolderHash = holder.GetHashCode();
            }
        }

        protected void RaiseEndOfListReached()
        {
            _logger.Info("End of list reached");
            OnEndOfListReached?.Invoke(this, null);
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            if(holder.GetHashCode() == _currentEnfOfListViewHolderHash)
            {
                _logger.Info("Current last element has been recycled");
                OnEndOfListElementRecycled?.Invoke(this, null);
            }

            base.OnViewRecycled(holder);
        }
    }

    public delegate void EndOfListReachedEventHandler(object sender, EventArgs e);
}
