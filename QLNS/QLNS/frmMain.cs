using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QLNS
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
        }
        private bool Handling()
        {

            bool checking = true;
            if (this.txtAddress.Text == "")
            {
                MessageBox.Show("Địa chỉ đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtBirthday.Text == "")
            {
                MessageBox.Show("Ngày sinh đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtDegree.Text == "")
            {
                MessageBox.Show("Bằng cấp đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtGender.Text == "")
            {
                MessageBox.Show("Giới tính đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtJoinDate.Text == "")
            {
                MessageBox.Show("Ngày vào đảng đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtLanguage.Text == "")
            {
                MessageBox.Show("Ngoại ngữ đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtName.Text == "")
            {
                MessageBox.Show("Họ và tên đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtOfficialDate.Text == "")
            {
                MessageBox.Show("Ngày chính thức đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtSpecialize.Text == "")
            {
                MessageBox.Show("Chuyên môn đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtTypeEducation.Text == "")
            {
                MessageBox.Show("Hình thức đào tạo đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            else if (this.txtChuyenNganh.Text == "")
            {
                MessageBox.Show("Chuyên môn cao nhất đang bỏ trống, vui lòng điền thông tin này");
                checking = false;
            }
            return checking;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Handling())
                {
                    string queryAdd = string.Format("" +
                        "INSERT INTO USER (hoten,ngaysinh,gioitinh,diachi,ngayvaodang," +
                        "ngaychinhthuc,chuyenmon,hinhthucdaotao,ngoaingu,bangcap,chuyennganh) " +
                        "VALUES('" +
                            txtName.Text + "','" + txtBirthday.Text + "','" +
                            txtGender.Text + "','" + txtAddress.Text + "','" +
                            txtJoinDate.Text + "','" + txtOfficialDate.Text + "','" +
                            txtSpecialize.Text + "','" + txtTypeEducation.Text + "','" +
                            txtLanguage.Text + "','" + txtDegree.Text + "','" + txtChuyenNganh.Text + "')");
                    Console.WriteLine(queryAdd);
                    connectDB con = new connectDB(Application.StartupPath);
                    con.InsertUsers(queryAdd);
                    loadData();
                    MessageBox.Show("Thành công!");
                }
            } catch (Exception ee)
            {
                MessageBox.Show("Xãy ra lỗi!!!", ee.ToString());
            }
        }
        private int convertNameColumn(string item)
        {
            if (item.Length == 1)
            {
                return (int)item[0] - (int)('A') + 1;
            }
            else
            {
                return 30; // colum AD9
            }

        }
        private void ImortFile()
        {

            string filePath = "";
            // tạo SaveFileDialog để lưu file excel
            OpenFileDialog dialog = new OpenFileDialog();

            // chỉ lọc ra các file có định dạng Excel

            // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePath = dialog.FileName;
            }

            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Đường dẫn báo cáo không hợp lệ");
                return;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                try
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    // tên: 		            "Nguyễn Hoàng Tân" 			        [T7]
                    this.txtName.Text = worksheet.Cells[7, convertNameColumn("T")].Value.ToString();

                    //ngày sinh: 		        "06"                                [L9]
                    //tháng sinh: 	            "01"                                [P9]
                    //năm sinh: 		        "1963"                              [T9]
                    string day = worksheet.Cells[9, convertNameColumn("L")].Value.ToString();
                    string month = worksheet.Cells[9, convertNameColumn("P")].Value.ToString();
                    string year = worksheet.Cells[9, convertNameColumn("T")].Value.ToString();
                    this.txtBirthday.Text = day + "/" + month + "/" + year;

                    // giới tính: 		        "Nam" 				                [AD9]
                    this.txtGender.Text = worksheet.Cells[9, convertNameColumn("AD")].Value.ToString();

                    // địa chỉ:		        "39/13 Vườn chuối,p4,q3,TP.HCM"     	[I15]
                    this.txtAddress.Text = worksheet.Cells[15, convertNameColumn("I")].Value.ToString();

                    //chuyên ngành cao nhất:  "299-Cử nhân" 			            [O27]
                    this.txtChuyenNganh.Text = worksheet.Cells[27, convertNameColumn("O")].Value.ToString();

                    //Ngày vào đảng: 	        ".../.../.../"				        [O34]
                    this.txtJoinDate.Text = worksheet.Cells[34, convertNameColumn("O")].Value.ToString();

                    //Ngày chính thức:	        ".../.../.../"			            [Z34]
                    this.txtOfficialDate.Text = worksheet.Cells[34, convertNameColumn("Z")].Value.ToString();

                    //Hình thức đào tạo:	    "285-Tập trung"			            [W53]
                    this.txtTypeEducation.Text = worksheet.Cells[53, convertNameColumn("W")].Value.ToString();

                    //Ngoại ngữ:		        "129-Tiếng Anh"			            [H88]
                    this.txtLanguage.Text = worksheet.Cells[88, convertNameColumn("H")].Value.ToString();


                    this.txtDegree.Text = "";
                    var temp = "";
                    //Trình độ 1:	            "1361-NN-Tiếng Anh B1"		        [Z88]
                    if (worksheet.Cells[88, convertNameColumn("Z")].Value != null)
                    {
                        temp += worksheet.Cells[88, convertNameColumn("Z")].Value.ToString() + ", ";
                    }
                    //Trình độ 2:	            "1368-TH-Sơ cấp tin học"	    	[Y104]
                    if (worksheet.Cells[104, convertNameColumn("Y")].Value != null)
                    {
                        temp += worksheet.Cells[104, convertNameColumn("Y")].Value.ToString() + ", ";
                    }
                    //Trình độ 3:	            ""				                    [Y116]
                    if (worksheet.Cells[116, convertNameColumn("Y")].Value != null)
                    {
                        temp += worksheet.Cells[116, convertNameColumn("Y")].Value.ToString() + ", ";
                    }
                    //Trình độ 4:	            "3123-QLNN ngạch chuyên viên"	    [Y127]
                    if (worksheet.Cells[127, convertNameColumn("Y")].Value != null)
                    {
                        temp += worksheet.Cells[127, convertNameColumn("Y")].Value.ToString() + ".";
                    }
                    //Trình độ 5:	            "3123-QLNN ngạch chuyên viên"	    [Y141]
                    if (worksheet.Cells[141, convertNameColumn("Y")].Value != null)
                    {
                        temp += worksheet.Cells[141, convertNameColumn("Y")].Value.ToString() + ".";
                    }
                    this.txtDegree.Text = temp;

                    this.txtSpecialize.Text = worksheet.Cells[53, convertNameColumn("H")].Value.ToString();
                    /*                                                                                                                                    
                    Chuyên môn	            "Nghiệp vụ"			                [H53]                                                           
                    */
                }
                catch (Exception err)
                {
                    MessageBox.Show("Error", err.ToString());
                }
            }

        }
        public DataView view = new DataView();
        public BindingSource bdsrc = new BindingSource();
        public void ShowDataUser(DataTable dt)
        {
            view.Table = dt;
            bdsrc.DataSource = view;
            gridview.DataSource = bdsrc;

        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            ImortFile();
        }
        private void loadData()
        {
            string query = "SELECT * FROM USER";
            connectDB con = new connectDB(Application.StartupPath);
            
            ShowDataUser(con.SelectUsers(query));
        }

        private void btnShowInfo_Click(object sender, EventArgs e)
        {
            
        }

        private void gridview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if(e.RowIndex >=0)
            {
                MessageBox.Show(gridview.Rows[e.RowIndex].Cells[0].Value.ToString());
                MessageBox.Show(gridview.ColumnCount.ToString());
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            connectDB con = new connectDB(Application.StartupPath);
            DataView view = new DataView();
            BindingSource bdsrc = new BindingSource();
            try
            {
                int convertID = Convert.ToInt16(txtSearch.Text);
                view.Table = con.SelectUsers("SELECT * FROM USER  WHERE id = " + convertID.ToString());
                bdsrc.DataSource = view;
                gridview.DataSource = bdsrc;
            } catch
            {
                try
                {   
                    List<DataGridViewRow> rows = new List<DataGridViewRow>();
                    if (txtSearch.Text.Length >0)
                    {
                        
                        view.Table = con.SelectUsers("SELECT * FROM USER  WHERE instr(hoten, '" + txtSearch.Text + "') > 0");
                        bdsrc.DataSource = view;
                        gridview.DataSource = bdsrc;     
                    }
                    else
                    {
                        gridview.DataSource = this.bdsrc;
                    }
                }
                catch
                {
                    gridview.DataSource = this.bdsrc;
                }
            }
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {

        }
    }
}
