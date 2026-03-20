using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotionDiagnostics.UserControls
{
    /// <summary>
    /// WaferLoadingControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WaferLoadingControl : UserControl
    {
        public WaferLoadingControl()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 허용된 문자: 숫자, ., +, -
            Regex regex = new Regex("[^0-9.+-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
