using System;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class frmThanhVien : Form
    {
        private readonly ThanhVienBUS tvBus = new ThanhVienBUS();
        private readonly CongViecBUS cvBus = new CongViecBUS();
        private readonly PhanCongBUS pcBus = new PhanCongBUS();

        private TextBox txtHoTen, txtEmail, txtChuyenMon;
        private ComboBox cboTask, cboThanhVien;
        private DataGridView dgvThanhVien, dgvPhanCong;

        public frmThanhVien()
        {
            BuildUI();
            LoadAll();
        }

        private void BuildUI()
        {
            Controls.Add(new Label { Text = "Họ tên:", Left = 20, Top = 20, Width = 75 });
            txtHoTen = new TextBox { Left = 100, Top = 17, Width = 180 };

            Controls.Add(new Label { Text = "Email:", Left = 295, Top = 20, Width = 50 });
            txtEmail = new TextBox { Left = 350, Top = 17, Width = 180 };

            Controls.Add(new Label { Text = "Chuyên môn:", Left = 545, Top = 20, Width = 80 });
            txtChuyenMon = new TextBox { Left = 630, Top = 17, Width = 160 };

            var btnThemTV = new Button { Text = "Thêm TV", Left = 805, Top = 15, Width = 85, Height = 28 };
            btnThemTV.Click += BtnThemTV_Click;

            dgvThanhVien = new DataGridView
            {
                Left = 20,
                Top = 55,
                Width = 870,
                Height = 190,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            Controls.Add(new Label { Text = "Giao Task:", Left = 20, Top = 262, Width = 75 });
            cboTask = new ComboBox { Left = 100, Top = 259, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };

            Controls.Add(new Label { Text = "Thành viên:", Left = 395, Top = 262, Width = 80 });
            cboThanhVien = new ComboBox { Left = 480, Top = 259, Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };

            var btnPhanCong = new Button { Text = "Phân công", Left = 725, Top = 257, Width = 100, Height = 28 };
            btnPhanCong.Click += BtnPhanCong_Click;

            dgvPhanCong = new DataGridView
            {
                Left = 20,
                Top = 298,
                Width = 870,
                Height = 290,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            Controls.AddRange(new Control[]
            {
                txtHoTen, txtEmail, txtChuyenMon, btnThemTV, dgvThanhVien,
                cboTask, cboThanhVien, btnPhanCong, dgvPhanCong
            });
        }

        private void LoadAll()
        {
            var tvs = tvBus.GetAll();
            dgvThanhVien.DataSource = null;
            dgvThanhVien.DataSource = tvs;

            cboThanhVien.DataSource = tvs;
            cboThanhVien.DisplayMember = "HoTen";
            cboThanhVien.ValueMember = "MaThanhVien";

            var tasks = cvBus.GetTasks();
            cboTask.DataSource = tasks;
            cboTask.DisplayMember = "TenTask";
            cboTask.ValueMember = "MaTask";

            dgvPhanCong.DataSource = null;
            dgvPhanCong.DataSource = pcBus.GetAll();
        }

        private void BtnThemTV_Click(object sender, EventArgs e)
        {
            var tv = new ThanhVienDTO
            {
                HoTen = txtHoTen.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                ChuyenMon = txtChuyenMon.Text.Trim()
            };

            var res = tvBus.Insert(tv);
            MessageBox.Show(res.msg);
            if (res.ok)
            {
                txtHoTen.Clear();
                txtEmail.Clear();
                txtChuyenMon.Clear();
                LoadAll();
            }
        }

        private void BtnPhanCong_Click(object sender, EventArgs e)
        {
            if (cboTask.SelectedValue is int maTask && cboThanhVien.SelectedValue is int maTV)
            {
                var res = pcBus.AssignTask(maTask, maTV);
                MessageBox.Show(res.msg);
                if (res.ok) LoadAll();
            }
        }
    }
}