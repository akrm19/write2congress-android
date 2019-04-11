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
        public event EndOfListReachedEventHandler OnEndOfListReached;

        public BaseRecyclerAdapter()
        {
            _logger = new Logger(Class.SimpleName);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position == (ItemCount - 1))
                RaiseEndOfListReached();
        }

        protected void RaiseEndOfListReached()
        {
            _logger.Info("End of list reached");
            OnEndOfListReached?.Invoke(this, null);
        }
    }

    public delegate void EndOfListReachedEventHandler(object sender, EventArgs e);
}
