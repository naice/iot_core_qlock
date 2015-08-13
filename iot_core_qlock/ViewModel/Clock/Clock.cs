using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_core_qlock.ViewModel.Clock
{
    public class ClockSegment : ViewModelBase
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }
        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (value != _IsActive)
                {
                    _IsActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }
    }
    public class ClockLine
    {
        public List<ClockSegment> Segments { get; set; }
        public ClockLine()
        {
            Segments = new List<ClockSegment>();
        }
    }
    public class ClockData: IEnumerable<ClockSegment>
    {
        const string _ClockString = "ESKISTLFÜNFZEHNZWANZIGDREIVIERTELTGNACHVORJMHALBQZWÖLFPZWEINSIEBENKDREIRHFÜNFELFNEUNVIERWACHTZEHNRSBSECHSFMUHR";
        static List<string> _ClockMatrix = new List<string>()
        {
            "ESKISTLFÜNF",
            "ZEHNZWANZIG",
            "DREIVIERTEL",
            "TGNACHVORJM",
            "HALBQZWÖLFP",
            "ZWEINSIEBEN",
            "KDREIRHFÜNF",
            "ELFNEUNVIER",
            "WACHTZEHNRS",
            "BSECHSFMUHR"
        };

        private List<ClockLine> _ClockItemSource;
        public List<ClockLine> ClockItemSource
        {
            get {

                if (_ClockItemSource == null)
                {
                    _ClockItemSource = new List<ClockLine>();

                    foreach (var str in _ClockMatrix)
                    {
                        var line = new ClockLine();
                        foreach (var c in str)
                        {
                            line.Segments.Add(new ClockSegment() { Text = c.ToString() });
                        }

                        _ClockItemSource.Add(line);
                    }


                }

                return _ClockItemSource; }
        }
        public bool IsActive { get; private set; }
        public string CurrentClockSentence { get; private set; }
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        public ClockData()
        {
            var cwnd = Windows.UI.Core.CoreWindow.GetForCurrentThread();
            if (cwnd != null)
                Dispatcher  = cwnd.Dispatcher;
        }
        
        public ClockSegment this[int key]
        {
            get
            {
                int segIndex = key % 11;
                int lineIndex = key / 11;
                return ClockItemSource[lineIndex].Segments[segIndex];
            }
        }

        public async void Run()
        { await RunAsync(); }
        public async Task RunAsync()
        {
            if (IsActive) return;
            IsActive = true;
            DateTime dtTest = DateTime.Now;
            await Task.Run(async () => {
                while (IsActive)
                {
                    string sentence = ClockSentenceGenerator.GetSentenceFor(dtTest);
                    if (CurrentClockSentence != sentence)
                    {
                        CurrentClockSentence = sentence;
                        List<ClockSegment> segments = GetSegmentsFor(sentence);
                        await UpdateSegments(GetOthers(segments), false);
                        await UpdateSegments(segments, true);
                    }
                    dtTest = dtTest.AddMinutes(1);
                    await Task.Delay(100);
                }
            });
            IsActive = false;
        }

        private List<ClockSegment> GetOthers(List<ClockSegment> s)
        {
            List<ClockSegment> retval = new List<ClockSegment>();

            foreach (var segment in this)
            {
                if (!s.Contains(segment))
                    retval.Add(segment);
            }

            return retval;
        }

        private async Task UpdateSegments(IEnumerable<ClockSegment> segments, bool isActive)
        {
            foreach (var segment in segments)
            {
                if (Dispatcher.HasThreadAccess)
                    segment.IsActive = isActive;
                else
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => segment.IsActive = isActive);
            }
        }

        private List<ClockSegment> GetSegmentsFor(string sentence)
        {
            List<ClockSegment> segments = new List<ClockSegment>();

            int currIdx = 0;
            foreach (var word in sentence.Split(' '))
            {
                currIdx = _ClockString.IndexOf(word, currIdx);
                if (currIdx == -1) throw new Exception(string.Format("SEGMENT '{0}' NOT FOUND, ERROR.", word));
                for (int i = currIdx; i < currIdx+word.Length; i++)
                    segments.Add(this[i]);
            }

            return segments;
        }

        public void Stop()
        {
            IsActive = false;
        }

        public IEnumerator<ClockSegment> GetEnumerator()
        {
            for (int i = 0; i < _ClockString.Length; i++)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class ClockSentenceGenerator
    {
        static string[] HOURNAMES = new string[]{
            "ZWÖLF", "EINS", "ZWEI", "DREI", "VIER", "FÜNF", "SECHS", "SIEBEN", "ACHT", "NEUN", "ZEHN", "ELF", 
        };

        public static string GetHourName(int hour)
        {
            return HOURNAMES[hour % 12];
        }
        public static string GetCurrentSentence()
        {
            return GetSentenceFor(DateTime.UtcNow.AddHours(2));
        }
        public static string GetSentenceFor(DateTime dt)
        {
            string s = "";

            DateTime dtFullHour = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, 0);

            TimeSpan t = dt - dtFullHour;

            string hourName = GetHourName(dt.Hour), nextHourName = GetHourName(dt.Hour + 1);

            if (t.TotalMinutes < 3)
                s = string.Format("ES IST {0} UHR", hourName);
            else if (t.TotalMinutes < 8)
                s = string.Format("ES IST FÜNF NACH {0}", hourName);
            else if (t.TotalMinutes < 13)
                s = string.Format("ES IST ZEHN NACH {0}", hourName);
            else if (t.TotalMinutes < 18)
                s = string.Format("ES IST VIERTEL NACH {0}", hourName);
            else if (t.TotalMinutes < 23)
                s = string.Format("ES IST ZWANZIG NACH {0}", hourName);
            else if (t.TotalMinutes < 28)
                s = string.Format("ES IST FÜNF VOR HALB {0}", hourName);
            else if (t.TotalMinutes < 33)
                s = string.Format("ES IST HALB {0}", hourName);
            else if (t.TotalMinutes < 38)
                s = string.Format("ES IST FÜNF NACH HALB {0}", nextHourName);
            else if (t.TotalMinutes < 43)
                s = string.Format("ES IST ZWANZIG VOR {0}", nextHourName);
            else if (t.TotalMinutes < 48)
                s = string.Format("ES IST VIERTEL VOR {0}", nextHourName);
            else if (t.TotalMinutes < 53)
                s = string.Format("ES IST ZEHN VOR {0}", nextHourName);
            else if (t.TotalMinutes < 58)
                s = string.Format("ES IST FÜNF VOR {0}", nextHourName);
            else 
                s = string.Format("ES IST {0} UHR", nextHourName);

            return s;
        }
    }
}
