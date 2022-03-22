using KMCCC.Authentication;
using KMCCC.Launcher;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace T_MinecraftLauncher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static LauncherCore Core = LauncherCore.Create();
        ArrayList java_path = new ArrayList();

        public MainWindow()
        {


            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            java_path.Add(Environment.GetEnvironmentVariable("JAVA_HOME") + @"\bin\javaw.exe");
            //javapath.Text = java_path[0].ToString();
            var versions = Core.GetVersions().ToArray();//定义变量获取版本列表
            comboBox1.ItemsSource = versions;//绑定数据源
            comboBox1.DisplayMemberPath = "Id";//设置comboBox显示的为版本Id
        }
        public void gamestart()
        {
            try
            {
                Core.JavaPath = @"javapath.Text";
                var ver = (KMCCC.Launcher.Version)comboBox1.SelectedItem;
                LaunchOptions options = new LaunchOptions
                {
                    Version = ver, //Ver为Versions里你要启动的版本名字
                    MaxMemory = Convert.ToInt32(MemoryTextbox.Text), //最大内存，int类型
                    MinMemory = Convert.ToInt32(minMemoryTextbox.Text),
                    //Authenticator = new OfflineAuthenticator(IdTextbox.Text), //离线启动，ZhaiSoul那儿为你要设置的游戏名
                    Mode = LaunchMode.MCLauncher, //启动模式，这个我会在后面解释有哪几种
                    Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
                };
                if ((bool)zb.IsChecked)
                {
                    options.Authenticator = new YggdrasilLogin(youxiang.Text, mima.Text, false);
                }
                else
                {
                    options.Authenticator = new OfflineAuthenticator(IdTextbox.Text); //离线启动
                }

                var result = Core.Launch(options);


                if (!result.Success)
                {
                    switch (result.ErrorType)
                    {
                        case ErrorType.AuthenticationFailed:
                            MessageBox.Show(this, "正版验证失败！请检查你的账号密码", "账号错误\n详细信息：" + result.ErrorMessage);
                            break;
                        case ErrorType.NoJAVA:
                            MessageBox.Show(result.ErrorMessage + "你没有java哦");
                            break;
                        case ErrorType.UncompressingFailed:
                            MessageBox.Show(result.ErrorMessage + "文件损坏了呢");
                            break;
                        default:
                            MessageBox.Show(result.ErrorMessage + "启动错误");
                            break;
                    }
                }

            }
            catch
            {
                MessageBox.Show("启动失败", "错误");
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Core.JavaPath = @"javapath.Text";
            gamestart();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
