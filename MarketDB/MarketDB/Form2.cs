using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketDB
{
    public partial class Form2 : Form
    {
        

        DataSet1TableAdapters.MARKETUSERTableAdapter marketuserTableAdapter1 = new DataSet1TableAdapters.MARKETUSERTableAdapter();
        DataSet1TableAdapters.MARKETTableAdapter marketTableAdapter1 = new DataSet1TableAdapters.MARKETTableAdapter();
        public Form2(string ftype)
        {

            InitializeComponent();
            marketuserTableAdapter1.Fill(dataSet11.MARKETUSER);
            marketTableAdapter1.Fill(dataSet11.MARKET);
            if (ftype == "drop")
            {
                label1.Visible = false;
                Registerbutton.Visible = false;
                groupBox1.Visible = false;
                panel15.Visible = false;
                
            }

        }

        private void Registerbutton_Click(object sender, EventArgs e)
        {
            DataTable marketusertbl = dataSet11.Tables["MARKETUSER"];
            DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(textBox1.Text.ToString());
            if (marketuserRow != null)
            {
                MessageBox.Show("중복 아이디가 존재합니다!");
            }
            else
            {
                DataRow u_dataRow = marketusertbl.NewRow();
                u_dataRow["U_ID"] = textBox1.Text;
                u_dataRow["U_PW"] = textBox2.Text;
                u_dataRow["U_NAME"] = textBox3.Text;
                if (radioButton1.Checked)               //admin 회원가입
                {
                    u_dataRow["U_TYPE"] = 1;
                    u_dataRow["U_CONNECTION"] = 0;
                    
                    marketusertbl.Rows.Add(u_dataRow);
                    marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                    MessageBox.Show("ID : " + u_dataRow["U_ID"] + " 회원가입 완료!");
                    Form2.ActiveForm.Close();
                }
                else if (radioButton2.Checked && comboBox1.SelectedItem != null)        //seller 회원가입
                {
                    u_dataRow["U_TYPE"] = 2;
                    u_dataRow["U_MNAME"] = marketTableAdapter1.ScalarQuery(comboBox1.SelectedItem.ToString());
                    u_dataRow["U_CONNECTION"] = 0;
                    
                    marketusertbl.Rows.Add(u_dataRow);
                    marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                    MessageBox.Show("ID : " + u_dataRow["U_ID"] + " 회원가입 완료!");
                    Form2.ActiveForm.Close();
                }
                else if (radioButton3.Checked)      
                {
                    u_dataRow["U_TYPE"] = 3;
                    u_dataRow["U_MNAME"] = marketTableAdapter1.ScalarQuery(comboBox1.SelectedItem.ToString());
                    u_dataRow["U_CONNECTION"] = 0;
                    u_dataRow["U_RATING"] = 50;
                    //u_dataRow["U_V_ID"] = 0;
                    marketusertbl.Rows.Add(u_dataRow);
                    marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                    MessageBox.Show("ID : " + u_dataRow["U_ID"] + " 회원가입 완료!");
                    Form2.ActiveForm.Close();
                }
                else
                {
                    MessageBox.Show("데이터 입력이 정확한지 확인하세요");
                }



            }
        }

        private void Dropbutton_Click(object sender, EventArgs e)
        {
            DataTable marketusertbl = dataSet11.Tables["MARKETUSER"];
            DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(textBox1.Text.ToString());

            if (marketuserRow == null)
            {
                MessageBox.Show("존재하지 않는 아이디입니다!");
            }
            else
            {
                if (marketuserRow.U_PW == textBox2.Text.ToString() && marketuserRow.U_NAME == textBox3.Text.ToString())
                {
                    marketuserRow.Delete();
                    marketuserTableAdapter1.Update(dataSet11.MARKETUSER);

                    MessageBox.Show("회원탈퇴 완료!");
                    Form2.ActiveForm.Close();
                }
                else
                {
                    MessageBox.Show("(보안) 패스워드 혹은 이름이 올바르지 않습니다!");
                }

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataTable markettbl = dataSet11.Tables["MARKET"];

            foreach (DataRow mydataRow in markettbl.Rows)       // 마트 combobox
            {
                comboBox1.Items.Add(mydataRow["M_NAME"].ToString());
            }
        }
    }
}
