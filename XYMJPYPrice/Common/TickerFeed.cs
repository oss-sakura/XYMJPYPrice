using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using XYMJPYPrice.Base;

namespace XYMJPYPrice.Common
{
    public class TickerFeed
    {
        private Label LabelExchangeName { get; set; }
        private Label LabelAsk { get; set; }
        private Label LabelBid { get; set; }
        private Label LabelLast { get; set; }
        private Label LabelSpred { get; set; }
        private Label LabelAskDiffValue { get; set; }
        private Label LabelBidDiffValue { get; set; }
        private Label LabelLastDiffValue { get; set; }

        private BaseTickerAPI TickerAPI { get; set; }
        private IntervalTimer _intervalTimer;
        private string _exchangeName;

        private float _prevBidValue = 0;
        private float _prevAskValue = 0;
        private float _prevLastValue = 0;

        private int _colorType;
        private Brush _upColor;
        private Brush _downColor;
        private Brush _defaultColor = Brushes.Black;

        private int _failCount = 0;

        public TickerFeed(
            BaseTickerAPI tickerApi,
            string exchangeName,
            Label lblExchangeName,
            Label lblAsk,
            Label lblBid,
            Label lblLast,
            Label lblSpread,
            Label lblAskDiffValue,
            Label lblBidDiffValue,
            Label lblLastDiffValue,
            IntervalTimer timer)
        {
            _exchangeName = exchangeName;
            _intervalTimer = timer;
            _intervalTimer.Add(_exchangeName);

            TickerAPI = tickerApi;

            LabelExchangeName = lblExchangeName;
            LabelAsk = lblAsk;
            LabelBid = lblBid;
            LabelLast = lblLast;
            LabelSpred = lblSpread;

            LabelAskDiffValue = lblAskDiffValue;
            LabelBidDiffValue = lblBidDiffValue;
            LabelLastDiffValue = lblLastDiffValue;

            _defaultColor = LabelAsk.Foreground;
            _colorType = Properties.Settings.Default.ColorType;
            SetUpDownColor();

            SetExchangeName(_exchangeName);
        }

        private void SetExchangeName(string name)
        {
            LabelExchangeName.Content = name;
        }

        private void SetUpDownColor()
        {
            if (_colorType == 1)
            {
                _upColor = Brushes.Green;
                _downColor = Brushes.Red;
            }
            else if (_colorType == 2)
            {
                _upColor = Brushes.Red;
                _downColor = Brushes.Green;
            }
            else
            {
                _upColor = _downColor = _defaultColor;
            }
        }

        public void SetFeedValues(float ask, float bid, float last)
        {
            if (_prevAskValue == 0 && _prevBidValue == 0 && _prevLastValue == 0)
            {
                _prevAskValue = ask;
                _prevBidValue = bid;
                _prevLastValue = last;
            }

            LabelAsk.Foreground = LabelBid.Foreground = LabelLast.Foreground = _defaultColor;
            LabelAskDiffValue.Foreground = LabelBidDiffValue.Foreground = LabelLastDiffValue.Foreground = _defaultColor;
            LabelAskDiffValue.Content = LabelBidDiffValue.Content = LabelLastDiffValue.Content = "";

            // Price UP/DOWN color
            if (_colorType != Properties.Settings.Default.ColorType)
            {
                _colorType = Properties.Settings.Default.ColorType;
                SetUpDownColor();
            }

            // Ask
            if (_prevAskValue < ask)
            {
                LabelAsk.Foreground = _upColor;
                LabelAskDiffValue.Foreground = _upColor;
                LabelAskDiffValue.Content = (ask - _prevAskValue).ToString("0.0000");
            }
            else if (_prevAskValue > ask)
            {
                LabelAsk.Foreground = _downColor;
                LabelAskDiffValue.Foreground = _downColor;
                LabelAskDiffValue.Content = (ask - _prevAskValue).ToString("0.0000");
            }

            // Bid
            if (_prevBidValue < bid)
            {
                LabelBid.Foreground = _upColor;
                LabelBidDiffValue.Foreground = _upColor;
                LabelBidDiffValue.Content = (bid - _prevBidValue).ToString("0.0000");
            }
            else if (_prevBidValue > bid)
            {
                LabelBid.Foreground = _downColor;
                LabelBidDiffValue.Foreground = _downColor;
                LabelBidDiffValue.Content = (bid - _prevBidValue).ToString("0.0000");
            }

            // Last
            if (_prevLastValue < last)
            {
                LabelLast.Foreground = _upColor;
                LabelLastDiffValue.Foreground = _upColor;
                LabelLastDiffValue.Content = (last - _prevLastValue).ToString("0.0000");
            }
            else if (_prevLastValue > last)
            {
                LabelLast.Foreground = _downColor;
                LabelLastDiffValue.Foreground = _downColor;
                LabelLastDiffValue.Content = (last - _prevLastValue).ToString("0.0000");
            }

            _prevAskValue = ask;
            _prevBidValue = bid;
            _prevLastValue = last;

            LabelAsk.Content = ask.ToString("0.0000");
            LabelBid.Content = bid.ToString("0.0000");
            LabelLast.Content = last.ToString("0.0000");
            LabelSpred.Content = (ask - bid).ToString("0.0000");
        }

        private void Failed()
        {
            if (_failCount == 0)
            {
                LabelAsk.Foreground = LabelBid.Foreground = LabelLast.Foreground = Brushes.Red;
                LabelAsk.Content = LabelBid.Content = LabelLast.Content = "failed";
                LabelSpred.Content = "";
            }
            _failCount++;
        }

        private void Stopped()
        {
            LabelAsk.Foreground = LabelBid.Foreground = LabelLast.Foreground = Brushes.Red;
            LabelAsk.Content = LabelBid.Content = LabelLast.Content = "Stopped";
            LabelSpred.Content = "";
        }

        public async void Run()
        {
            for (; ; )
            {
                if (_intervalTimer.OverIntervalTime[_exchangeName])
                {
                    TickerAPI.GetTicker();
                    if (TickerAPI.GetStatusCode() != -1 && (HttpStatusCode)TickerAPI.GetStatusCode() == HttpStatusCode.OK)
                    {
                        SetFeedValues(TickerAPI.GetAsk(), TickerAPI.GetBid(), TickerAPI.GetLast());
                    }
                    else
                    {
                        Failed();
                        if (_failCount > Properties.Settings.Default.MaxRetryCount)
                        {
                            Stopped();
                            break;
                        }
                    }
                    _intervalTimer.Reset(_exchangeName);
                }
                await Task.Delay(100);
            }
        }
    }
}
