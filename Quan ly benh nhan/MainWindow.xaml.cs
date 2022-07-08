using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using TestAppWithDB.Models;

namespace TestAppWithDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string error = "";
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        truonghocdb2Context db = new truonghocdb2Context(); 
        private void showData()
        {
            var data = from sv in db.Sinhviens select sv;
            dtlist.ItemsSource = data.ToList();
        }
        private void showCombobox()
        {
            var data = from Lophoc in db.Lophocs select Lophoc;
            loptxt.ItemsSource = data.ToList();
            loptxt.DisplayMemberPath = "Tenlop";
            loptxt.SelectedValuePath = "Malop";
            loptxt.SelectedIndex = 0;

            var data2 = from sv in db.Sinhviens select sv;
            diachitxt.ItemsSource = data2.ToList();
            diachitxt.DisplayMemberPath = "Diachi";
            diachitxt.SelectedValuePath = "Masv";
            diachitxt.SelectedIndex = 0;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool checkData()
        {
            bool check = true;
            string masv = msvtext.Text;
            string hoten = nametxt.Text;
            string diem = diemtxt.Text;
            
            if (!Regex.IsMatch(masv, "^\\d{1,}$"))
            {
                check = false;
                error += "\nMa sinh vien phai la so";
            }
            if (!Regex.IsMatch(hoten, "^[a-zA-Z]+$"))
            {
                check = false;
                error += "\nHo ten phai la chuoi ky tu";
            }
            if(!Regex.IsMatch(diem, "^\\d{1,}$"))
            {
                check = false;
                error += "\nDiem phai la so";
            }

            return check;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bool t = checkData();
            if (t)
            {
                try
                {
                    Sinhvien s = new Sinhvien();
                    s.Masv = int.Parse(msvtext.Text);
                    s.Hoten = nametxt.Text;
                    s.Diachi = diachitxt.Text;
                    s.Diem = byte.Parse(diemtxt.Text);
                    s.Malop = int.Parse(loptxt.SelectedValue.ToString());
                    db.Sinhviens.Add(s);
                    db.SaveChanges();
                    clearData();
                    showData();

                }
                catch (Exception x)
                {
                    MessageBox.Show("Da xay ra loi" + x.Message);
                    db.ChangeTracker.Clear();
                }
            }
        }

        private void Update_click(object sender, RoutedEventArgs e)
        {
            try
            {
                var svsua = db.Sinhviens.SingleOrDefault(sv => sv.Masv == int.Parse(msvtext.Text));
                if(svsua != null)
                {
                    if (checkData())
                    {
                        svsua.Hoten = nametxt.Text;
                        svsua.Diem = byte.Parse(diemtxt.Text);
                        svsua.Diachi = diachitxt.Text;
                        svsua.Malop = int.Parse(loptxt.SelectedValue.ToString());

                        db.SaveChanges();
                        showData();
                        //clearData();
                    }
                }
            }
            catch(Exception x)
            {
                MessageBox.Show("Da xay ra loi \n" + x.Message);
            }
        }

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            try
            {
                var svxoa = db.Sinhviens.SingleOrDefault(sv => sv.Masv == int.Parse(msvtext.Text));
                db.Sinhviens.Remove(svxoa);
                db.SaveChanges();
                showData();
                clearData();
            }
            catch(Exception x)
            {
                MessageBox.Show("Da xay ra loi\n" + x.Message);
                db.ChangeTracker.Clear();
            }
        }

        private void search_click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            showData();
            showCombobox();
        }
        private void clearData()
        {
            msvtext.Clear();
            diemtxt.Clear();
            nametxt.Clear();
            diachitxt.SelectedIndex = 0;
            loptxt.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Object obj = dtlist.SelectedItem;
            if(obj != null)
            {
                try 
                {
                    Type t = obj.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    int msv = int.Parse(p[0].GetValue(dtlist.SelectedItem).ToString());
                    MessageBoxResult ms = MessageBox.Show("Ban co chac muon xoa khong","Thong bao", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ms == MessageBoxResult.Yes)
                    {
                        var xoa = db.Sinhviens.SingleOrDefault(sv => sv.Masv == msv);
                        db.Sinhviens.Remove(xoa);
                        db.SaveChanges();
                        showData();
                        clearData();
                    }
                }
                catch(Exception x)
                {
                    MessageBox.Show("Ban chua chon sinh vien nao ca\n" + x.Message);
                    
                }
            }
        }

        private void dtlist_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Object obj = dtlist.SelectedItem;
            if(obj != null)
            {
                try
                {
                    Type c = obj.GetType();
                    PropertyInfo[] p = c.GetProperties();
                    msvtext.Text = p[0].GetValue(dtlist.SelectedItem).ToString();
                    nametxt.Text = p[1].GetValue(dtlist.SelectedItem).ToString();
                    string dc = p[2].GetValue(dtlist.SelectedItem).ToString();
                    if (dc.Equals("Hà Nội"))
                    {
                        diachitxt.SelectedIndex = 0;

                    }
                    else if (dc.Equals("Thái Bình"))
                    {
                        diachitxt.SelectedIndex = 1;
                    }
                    diemtxt.Text = p[3].GetValue(dtlist.SelectedItem).ToString();
                    string l = p[4].GetValue(dtlist.SelectedItem).ToString();
                    if (l.Equals("Công nghệ thông tin"))
                    {
                        loptxt.SelectedIndex = 0;
                    }
                    else
                    {
                        loptxt.SelectedIndex = 1;
                    }

                }
                catch(Exception t)
                {
                    MessageBox.Show("Ban chua chon sinh vien nao ca\n" + t.Message);

                }
            }
        }
    }
}
