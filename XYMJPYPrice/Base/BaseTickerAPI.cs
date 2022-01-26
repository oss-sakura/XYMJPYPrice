namespace XYMJPYPrice.Base
{
    public abstract class BaseTickerAPI
    {
        public abstract float GetAsk();
        public abstract float GetBid();
        public abstract float GetLast();
        public abstract int GetStatusCode();
        public abstract void GetTicker();
    }
}
