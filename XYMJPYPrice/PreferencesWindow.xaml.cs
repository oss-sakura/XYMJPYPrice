using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace XYMJPYPrice
{
    /// <summary>
    /// PreferencesWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public class MyFont
        {
            public FontFamily FontFamily { get; set; }
            public string LocalFontName { get; set; }
        }

        private MyFont[] GetFontList()
        {
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            MyFont[] fonts = Fonts.SystemFontFamilies.Select(i =>
                new MyFont() { FontFamily = i, LocalFontName = i.Source }).ToArray();

            fonts.Select(i => i.LocalFontName = i.FontFamily.FamilyNames
                  .FirstOrDefault(j => j.Key == this.Language).Value ?? i.FontFamily.Source).ToArray();

            return fonts;
        }

        private void Rendered()
        {
            // 設定済フォントがある場合、選択および表示されるようにスクロールする
            string selectFontName = Properties.Settings.Default.FontName;
            if (!string.IsNullOrEmpty(selectFontName))
            {
                int j = -1;
                int i = 0;
                foreach (MyFont item in lstFontList.Items)
                {
                    i++;
                    if (selectFontName.Equals(item.LocalFontName)) { j = i - 1; }
                }
                if (j > -1)
                {
                    lstFontList.SelectedIndex = j;
                    lstFontList.ScrollIntoView(lstFontList.Items[j]);
                }
            }
            // 設定済の取得間隔(秒)を選択
            string sec = Properties.Settings.Default.IntervalSec.ToString();
            int jj = -1;
            int ii = 0;
            foreach (ComboBoxItem item in cmbIntervalSec.Items)
            {
                ii++;
                if (sec.Equals(item.Content.ToString())) { jj = ii - 1; }
            }
            if (jj > -1) { cmbIntervalSec.SelectedIndex = jj; }
            // 色の選択
            if (Properties.Settings.Default.ColorType == 1)
            {
                radioUpGreenDownRed.IsChecked = true;
                radioUpRedDownGreen.IsChecked = false;
            }
            else
            {
                radioUpGreenDownRed.IsChecked = false;
                radioUpRedDownGreen.IsChecked = true;
            }
            // 色の選択
            if (Properties.Settings.Default.ColorType == 1)
            {
                radioUpGreenDownRed.IsChecked = true;
                radioUpRedDownGreen.IsChecked = false;
            }
            else
            {
                radioUpGreenDownRed.IsChecked = false;
                radioUpRedDownGreen.IsChecked = true;
            }
            // リトライ回数の選択
            string cnt = Properties.Settings.Default.MaxRetryCount.ToString();
            int jjj = -1;
            int iii = 0;
            foreach (ComboBoxItem item in cmbMaxRetryCount.Items)
            {
                iii++;
                if (cnt.Equals(item.Content.ToString())) { jjj = iii - 1; }
            }
            if (jjj > -1) { cmbMaxRetryCount.SelectedIndex = jjj; }
            // キャンセルボタンにフォーカスを移動
            btnCancel.Focus();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            if (lstFontList.SelectedIndex < 0)
            {
                Properties.Settings.Default.FontName = "";
            }
            else
            {
                MyFont mf = (MyFont)lstFontList.SelectedItem;
                Properties.Settings.Default.FontName = mf.LocalFontName;
            }

            // ColorType
            Properties.Settings.Default.ColorType = radioUpGreenDownRed.IsChecked.Value ? 1 : 2;

            // IntervalSec
            ComboBoxItem selectInterval = (ComboBoxItem)cmbIntervalSec.SelectedItem;
            if (!int.TryParse(selectInterval.Content.ToString(), out int sec)) { sec = 3; }
            Properties.Settings.Default.IntervalSec = sec;

            // 
            ComboBoxItem selectMaxRetryCount = (ComboBoxItem)cmbMaxRetryCount.SelectedItem;
            if (!int.TryParse(selectMaxRetryCount.Content.ToString(), out int cnt)) { cnt = 10; }
            Properties.Settings.Default.MaxRetryCount = cnt;

            // Save
            Properties.Settings.Default.Save();
        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FontName = SystemFonts.MessageFontFamily.ToString();
            Properties.Settings.Default.ColorType = 1;
            Properties.Settings.Default.IntervalSec = 3;
            Properties.Settings.Default.WindowWidth = 600;
            Properties.Settings.Default.WindowHeight = 450;
            Properties.Settings.Default.MaxRetryCount = 10;

            Properties.Settings.Default.Save();

            DialogResult = true;
            Close();
        }

        public PreferencesWindow()
        {
            InitializeComponent();

            DataContext = GetFontList();
            ContentRendered += (s, e) => { Rendered(); };
        }
    }
}
