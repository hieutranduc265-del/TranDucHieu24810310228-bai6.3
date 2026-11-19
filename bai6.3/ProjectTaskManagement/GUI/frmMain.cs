using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectTaskManagement.GUI
{
    public partial class frmMain : Form
    {
        private Panel pnlContent;

        public frmMain()
        {
            BuildUI();
            OpenChildForm(new frmCongViec());
        }

        private void BuildUI()
        {
            Text = "Hệ thống Quản lý Công việc & Tiến độ Dự án";
            Size = new Size(1150, 720);
            StartPosition = FormStartPosition.CenterScreen;

            var pnlMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 210,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            var lblTitle = new Label
            {
                Text = "TASK & PROJECT",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnDuAn = CreateMenuButton("Quản lý Dự án", 65);
            var btnCongViec = CreateMenuButton("Task & Kanban", 115);
            var btnThanhVien = CreateMenuButton("Thành viên & Phân công", 165);
            var btnBaoCao = CreateMenuButton("Trễ hạn & Thống kê", 215);

            btnDuAn.Click += (s, e) => OpenChildForm(new frmDuAn());
            btnCongViec.Click += (s, e) => OpenChildForm(new frmCongViec());
            btnThanhVien.Click += (s, e) => OpenChildForm(new frmThanhVien());
            btnBaoCao.Click += (s, e) => OpenChildForm(new frmBaoCao());

            pnlMenu.Controls.AddRange(new Control[] { lblTitle, btnDuAn, btnCongViec, btnThanhVien, btnBaoCao });

            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 250, 252)
            };

            Controls.Add(pnlContent);
            Controls.Add(pnlMenu);
        }

        private Button CreateMenuButton(string text, int top)
        {
            return new Button
            {
                Text = text,
                Top = top,
                Left = 10,
                Width = 190,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void OpenChildForm(Form child)
        {
            pnlContent.Controls.Clear();
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(child);
            child.Show();
        }
    }
}