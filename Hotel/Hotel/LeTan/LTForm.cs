﻿
using Hotel.DAO;
using Hotel.EMPLOYEE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel
{
    public partial class LTForm : Form
    {
        int eid;
        public LTForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            
        }
        public LTForm(int id)
        {
            InitializeComponent();
            this.idNhanVien = id;
            eid = id;
        }

        private Point lastClick;
        private Form currentForm;
        Assignment AssignmentSQL = new Assignment();
        private int idNhanVien = 1;
        const int WM_NCHITTEST = 0x0084;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;
        private bool isCollapsed = true;

        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    if (m.Result == (IntPtr)HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    break;
            }

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x0040000;
                return cp;
            }
        }

        private void LTForm_Load(object sender, EventArgs e)
        {
            try
            {
                EMPLOYEES Emp = new EMPLOYEES();
                DataTable dt = Emp.getEmployeeByID(idNhanVien);
                btnTenNhanVen.Text = "Nhân viên:" + dt.Rows[0][1].ToString();

                dtpDemo.Value = DateTime.Now;

                Form childForm = new LoadRoom();
                childForm.TopLevel = false;
                childForm.Size = this.pnlMain.Size;
                currentForm = childForm;
                this.lbTitle.Text = childForm.Text;
                pnlMain.Controls.Add(currentForm);
                currentForm.Show();
                pnlChoose.Tag = btnDSPhong;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Toang", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void OpenChildForm(Form childForm)
        {
            childForm.TopLevel = false;
            childForm.Size = this.pnlMain.Size;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.lbTitle.Text = childForm.Text;

            if (currentForm != null)
            {
                currentForm.Close();
                currentForm = childForm;
                this.pnlMain.Controls.Clear();
                this.pnlMain.Controls.Add(currentForm);
                currentForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picClose_MouseHover(object sender, EventArgs e)
        {
            ((PictureBox)sender).Image = global::Hotel.Properties.Resources.shutdownHover_40px; ;
        }

        private void picClose_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).Image = global::Hotel.Properties.Resources.shutdown_40px;
        }

        private void ReLocation_btnChoose(Button sender)
        {
            sender.Size = new Size(sender.Size.Width + 10, sender.Size.Height);
            sender.Location = new Point(sender.Location.X - 10, sender.Location.Y);

        }

        #region resize_drag_form

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            lastClick = e.Location;
        }

        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                this.Left += e.X - lastClick.X;
                this.Top += e.Y - lastClick.Y;
            }

        }

        #endregion resize_drag_form 

        private void btnDSPhong_Click(object sender, EventArgs e)
        {
            if ((Button)pnlChoose.Tag != btnDSPhong)
            {
                ReLocation_btnChoose((Button)pnlChoose.Tag);
                pnlChoose.Tag = btnDSPhong;
                pnlChoose.Location = btnDSPhong.Location;
                btnDSPhong.Location = new Point(pnlChoose.Width, btnDSPhong.Location.Y);
                btnDSPhong.Size = new Size(btnDSPhong.Size.Width - 10, btnDSPhong.Size.Height);

                Form childForm = new LoadRoom();
                OpenChildForm(childForm);
            }
        }
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            if ((Button)pnlChoose.Tag != this.btnKhachHang)
            {
                ReLocation_btnChoose((Button)pnlChoose.Tag);
                pnlChoose.Tag = this.btnKhachHang;
                pnlChoose.Location = this.btnKhachHang.Location;
                this.btnKhachHang.Location = new Point(pnlChoose.Width, this.btnKhachHang.Location.Y);
                this.btnKhachHang.Size = new Size(this.btnKhachHang.Size.Width - 10, this.btnKhachHang.Size.Height);

                Form childForm = new CustomerForm();
                OpenChildForm(childForm);
            }

        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            if ((Button)pnlChoose.Tag != this.btnDichVu)
            {
                ReLocation_btnChoose((Button)pnlChoose.Tag);
                pnlChoose.Tag = this.btnDichVu;
                pnlChoose.Location = this.btnDichVu.Location;
                this.btnDichVu.Location = new Point(pnlChoose.Width, this.btnDichVu.Location.Y);
                this.btnDichVu.Size = new Size(this.btnDichVu.Size.Width - 10, this.btnDichVu.Size.Height);
                ServiceForm childForm = new ServiceForm();
                childForm.btnAdd.Enabled = false;
                childForm.btnEdit.Enabled = false;
                childForm.btnImport.Enabled = false;
                childForm.btnRemove.Enabled = false;
                childForm.ThanhToanBT.Enabled = false;
                OpenChildForm(childForm);
            }
        }

        private void btnShowPanelTab_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void LTForm_Resize(object sender, EventArgs e)
        {
            pnlLeft.MinimumSize = new Size(pnlLeft.Width, this.Height - 70);
        }

        private void pnlHeader_DoubleClick(object sender, EventArgs e)
        {
            if (this.Size == this.MinimumSize)
            {
                Left = Top = -6;
                Width = Screen.PrimaryScreen.WorkingArea.Width + 12;
                Height = Screen.PrimaryScreen.WorkingArea.Height + 12;
            }
            else
            {
                Left = (Screen.PrimaryScreen.WorkingArea.Width - this.MinimumSize.Width) / 2;
                Top = (Screen.PrimaryScreen.WorkingArea.Height - this.MinimumSize.Height) / 2;
                this.Size = this.MinimumSize;

            }

        }

        
        private void timer_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                if (pnlLeft.Size.Width == pnlLeft.MaximumSize.Width)
                {
                    timer.Stop();
                    isCollapsed = false;
                }
                pnlLeft.Width += 15;
            }
            else
            {
                if (pnlLeft.Size.Width == pnlLeft.MinimumSize.Width)
                {
                    timer.Stop();
                    isCollapsed = true;
                }
                pnlLeft.Width -= 15;
            }
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            DateTime t = dtpDemo.Value;
            DateTime check = new DateTime(t.Year, t.Month, t.Day, 6, 30, t.Second);
            if ((check - t).TotalMinutes == 0)
            {
                MessageBox.Show("Tới giờ báo cáo");
            }
            dtpDemo.Value = dtpDemo.Value.AddMinutes(1);

        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            if ((Button)pnlChoose.Tag != this.btnBill)
            {
                ReLocation_btnChoose((Button)pnlChoose.Tag);
                pnlChoose.Tag = this.btnBill;
                pnlChoose.Location = this.btnBill.Location;
                this.btnBill.Location = new Point(pnlChoose.Width, this.btnBill.Location.Y);
                this.btnBill.Size = new Size(this.btnBill.Size.Width - 10, this.btnBill.Size.Height);
                ManageBillForm childForm = new ManageBillForm();
                childForm.btnAdd.Enabled = false;
                childForm.btnDelete.Enabled = false;
                childForm.btnEdit.Enabled = false;
                
                OpenChildForm(childForm);
            }
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            if (pnlCheckin.Tag.ToString() == "0")
            {
                pnlCheckin.Size = new Size(200, 60);
                btnExpand.BackgroundImage = global::Hotel.Properties.Resources.expand_arrow_96px3;
                pnlCheckin.Tag = "Expand";
            }
            else
            {
                pnlCheckin.Size = new Size(0, 60);
                btnExpand.BackgroundImage = global::Hotel.Properties.Resources.expand_arrow_96px2;
                pnlCheckin.Tag = "0";
            }

        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            WORKING WorkingSQL = new WORKING();
            Assignment AssignmentSQL = new Assignment();
            DataTable dt = AssignmentSQL.checkID(GlobalVar._id, DateTime.Now);//Lấy id phân công làm viêc
            if (dt.Rows.Count > 0)
            {
                int id = (int)(dt.Rows[0][0]);//id của bảng assignment
                if (WorkingSQL.ExistCheckin(GlobalVar._id, id))
                {
                    MessageBox.Show("Đã Checkin. Vui lòng không checkin lại", "Checkin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (WorkingSQL.UpdateCheckin(id, DateTime.Now))
                {
                    WorkingSQL.AddWorking(id, GlobalVar._id);
                    MessageBox.Show("CheckIn thành công");
                }
                else
                {
                    MessageBox.Show("CheckIn không thành công");
                }
            }
            else
            {
                //Viết hàm cho người dùng chọn ID của người cần làm thay
                Form checkin = new EMPLOYEE.CheckInForm();
                checkin.ShowDialog();
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            WORKING WorkingSQL = new WORKING();
            DataTable dt = WorkingSQL.GetIDWorking(GlobalVar._id);//Lấy id phân công làm viêc
            if (dt.Rows.Count > 0)
            {
                int id = (int)(dt.Rows[0][0]);//id của bảng assignment

                if (WorkingSQL.UpdateCheckout(id, DateTime.Now))
                {
                    if (WorkingSQL.DelWorking(id))
                        MessageBox.Show("CheckOut thành công");
                    else
                    {
                        MessageBox.Show("CheckOut không thành công");
                    }
                }
                else
                {
                    MessageBox.Show("CheckOut không thành công");
                }
            }
            else
            {
                MessageBox.Show("Đã Checkout. Vui lòng không checkout lại", "Checkin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if ((Button)pnlChoose.Tag != btn)
            {
                ReLocation_btnChoose((Button)pnlChoose.Tag);
                pnlChoose.Tag = btn;
                pnlChoose.Location = btn.Location;
                btn.Location = new Point(pnlChoose.Width, btn.Location.Y);
                this.btnSetting.Size = new Size(btn.Size.Width - 10, btn.Size.Height);
                Form childForm = new Setting(eid);
                OpenChildForm(childForm);
            }
        }

        private void Work_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if ((Button)pnlChoose.Tag != btn)
            {
                ReLocation_btnChoose((Button)pnlChoose.Tag);
                pnlChoose.Tag = btn;
                pnlChoose.Location = btn.Location;
                btn.Location = new Point(pnlChoose.Width, btn.Location.Y);
                this.btnSetting.Size = new Size(btn.Size.Width - 10, btn.Size.Height);
                ShowAssignmentFrom childForm = new ShowAssignmentFrom();
                childForm.btnDelete.Enabled = false;
                OpenChildForm(childForm);
            }
        }
    }
}
