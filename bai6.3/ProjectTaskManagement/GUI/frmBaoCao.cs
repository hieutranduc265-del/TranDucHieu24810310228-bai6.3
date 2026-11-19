using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;

namespace ProjectTaskManagement.GUI
{
    public class frmBaoCao : Form
    {
        private readonly CongViecBUS cvBus = new CongViecBUS();
        private readonly PhanCongBUS pcBus = new PhanCongBUS();
        private readonly DuAnBUS daBus = new DuAnBUS();

        private DataGridView dgvOverdue, dgvStats;
        private ComboBox cboDuAn;
        private Label lblTongPhanCong;

        public frmBaoCao()
        {
            BuildUI();
            LoadDuAnCombo();
            LoadReports();
        }

        private void BuildUI()
        {
            Controls.Add(new Label
            {
                Text = "CẢNH BÁO TASK TRỄ HẠN (HanNop < hiện tại & TrangThai <> 'Done')",
                Left = 20,
                Top = 15,
                Width = 550,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.Crimson
            });

            dgvOverdue = new DataGridView
            {
                Left = 20,
                Top = 40,
                Width = 870,
                Height = 200,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            Controls.Add(new Label { Text = "Chọn dự án thống kê:", Left = 20, Top = 258, Width = 135 });
            cboDuAn = new ComboBox { Left = 160, Top = 255, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDuAn.SelectedIndexChanged += (s, e) => LoadStats();

            lblTongPhanCong = new Label
            {
                Text = "Tổng lượt phân công dự án: 0",
                Left = 430,
                Top = 258,
                Width = 260,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };

            var btnRefresh = new Button { Text = "Tải lại", Left = 790, Top = 253, Width = 100, Height = 28 };
            btnRefresh.Click += (s, e) => LoadReports();

            dgvStats = new DataGridView
            {
                Left = 20,
                Top = 290,
                Width = 870,
                Height = 290,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            Controls.AddRange(new Control[] { dgvOverdue, cboDuAn, lblTongPhanCong, btnRefresh, dgvStats });
        }

        private void LoadDuAnCombo()
        {
            var list = daBus.GetAll();
            cboDuAn.DataSource = list;
            cboDuAn.DisplayMember = "TenDuAn";
            cboDuAn.ValueMember = "MaDuAn";
        }

        private void LoadReports()
        {
            dgvOverdue.DataSource = null;
            dgvOverdue.DataSource = cvBus.GetOverdueTasks();
            LoadStats();
        }

        private void LoadStats()
        {
            if (cboDuAn.SelectedValue is int maDuAn)
            {
                int tongPC = pcBus.GetTotalAssignedTasksByProject(maDuAn);
                lblTongPhanCong.Text = $"Tổng lượt phân công dự án: {tongPC}";
                dgvStats.DataSource = pcBus.GetCompletionStatsByProject(maDuAn);
            }
        }
    }
}