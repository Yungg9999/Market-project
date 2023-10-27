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
    public partial class Form1 : Form
    {        
        DataSet1TableAdapters.MARKETTableAdapter marketTableAdapter1 = new DataSet1TableAdapters.MARKETTableAdapter();
        DataSet1TableAdapters.MARKETUSERTableAdapter marketuserTableAdapter1 = new DataSet1TableAdapters.MARKETUSERTableAdapter();
        DataSet1TableAdapters.PRODUCTTableAdapter productTableAdapter1 = new DataSet1TableAdapters.PRODUCTTableAdapter();
        DataSet1TableAdapters.PRODUCT_TYPETableAdapter productTypeTableadapter1 = new DataSet1TableAdapters.PRODUCT_TYPETableAdapter();
        DataSet1TableAdapters.STOCKLISTTableAdapter stocklistTableAdapter1 = new DataSet1TableAdapters.STOCKLISTTableAdapter();
        DataSet1TableAdapters.VASKETTableAdapter vasketTableAdapter1 = new DataSet1TableAdapters.VASKETTableAdapter();
        DataSet1TableAdapters.VASKET_LISTTableAdapter vasketListTableAdapter1 = new DataSet1TableAdapters.VASKET_LISTTableAdapter();
        DataSet1TableAdapters.PURCHASETableAdapter purchaseTableAdapter1 = new DataSet1TableAdapters.PURCHASETableAdapter();
        DataSet1TableAdapters.PURCHASE_LISTTableAdapter purchaseListTableAdapter1 = new DataSet1TableAdapters.PURCHASE_LISTTableAdapter();
        DataSet1TableAdapters.REVIEWTableAdapter reviewTableAdapter1 = new DataSet1TableAdapters.REVIEWTableAdapter();
        string connectedId;
        string connectedType;
        string selectMarket;
        int vasketNum;
        int vaskettotal;
        int RFtotalprice;
        public Form1()
        {
            InitializeComponent();
            marketTableAdapter1.Fill(dataSet11.MARKET);
            marketuserTableAdapter1.Fill(dataSet11.MARKETUSER);
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            productTypeTableadapter1.Fill(dataSet11.PRODUCT_TYPE);
            stocklistTableAdapter1.Fill(dataSet11.STOCKLIST);
            vasketTableAdapter1.Fill(dataSet11.VASKET);
            vasketListTableAdapter1.Fill(dataSet11.VASKET_LIST);
            purchaseTableAdapter1.Fill(dataSet11.PURCHASE);
            purchaseListTableAdapter1.Fill(dataSet11.PURCHASE_LIST);
            Adminpanel.Visible = false;
            Customerpanel.Visible = false;
            Sellerpanel.Visible = false;

            
            DataTable markettbl = dataSet11.Tables["MARKET"];

            foreach (DataRow mydataRow in markettbl.Rows)       // 마트 combobox
            {
                comboBox2.Items.Add(mydataRow["M_ID"].ToString());
                comboBox4.Items.Add(mydataRow["M_NAME"].ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            marketTableAdapter1.Fill(dataSet11.MARKET);
            marketuserTableAdapter1.Fill(dataSet11.MARKETUSER);
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            productTypeTableadapter1.Fill(dataSet11.PRODUCT_TYPE);
            stocklistTableAdapter1.Fill(dataSet11.STOCKLIST);
            vasketTableAdapter1.Fill(dataSet11.VASKET);
            vasketListTableAdapter1.Fill(dataSet11.VASKET_LIST);
            purchaseTableAdapter1.Fill(dataSet11.PURCHASE);
            purchaseListTableAdapter1.Fill(dataSet11.PURCHASE_LIST);
            reviewTableAdapter1.Fill(dataSet11.REVIEW);

            DataTable marketusertbl = dataSet11.Tables["MARKETUSER"];
            
            
            DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(textBox1.Text.ToString());

            if (marketuserRow == null)
            {
                MessageBox.Show("존재하지 않는 아이디입니다!");
            }
            else
            {
                if (radioButton1.Checked)       //admin 로그인 시
                {
                    if (marketuserRow.U_PW == textBox2.Text.ToString() && marketuserRow.U_TYPE == "1")
                    {
                        
                        marketuserRow.U_CONNECTION = 1;
                        
                        marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                        MessageBox.Show("로그인 성공!");
                        connectedId = marketuserRow.U_ID;
                        connectedType = marketuserRow.U_TYPE;

                        Loginpanel.Visible = false;
                        Adminpanel.Visible = true;
                        label24.Text = "사용자: " + connectedId;
                    }
                    else
                    {
                        MessageBox.Show("(보안) 패스워드가 올바르지 않습니다!");
                    }
                }
                else if(radioButton2.Checked)                     //seller 로그인 시
                {
                    if (marketuserRow.U_PW == textBox2.Text.ToString() && marketuserRow.U_TYPE == "2")
                    {                       
                        marketuserRow.U_CONNECTION = 1;
                        
                        marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                        MessageBox.Show("로그인 성공!");
                        connectedId = marketuserRow.U_ID;
                        connectedType = marketuserRow.U_TYPE;
                        selectMarket = marketuserRow.U_MNAME;
                        purchaseTableAdapter1.FillBy(dataSet11.PURCHASE, marketuserRow.U_MNAME);
                        
                        Loginpanel.Visible = false;
                        Sellerpanel.Visible = true;
                        label30.Text = "사용자: " + connectedId;
                        label36.Text = "마트: " + marketuserRow.U_MNAME;
                        textBox6.Text = "0";
                    }
                    else
                    {
                        MessageBox.Show("(보안) 패스워드가 올바르지 않습니다!");
                    }
                }
                else if(radioButton3.Checked)                                        //customer 로그인 시
                {
                    if (marketuserRow.U_PW == textBox2.Text.ToString() && marketuserRow.U_TYPE == "3")
                    {
                        if(comboBox1.SelectedIndex == -1)
                        {
                            MessageBox.Show("마트를 선택하세요!");
                        }
                        else
                        {
                            vasketTableAdapter1.FillBy(dataSet11.VASKET, comboBox1.SelectedItem.ToString());
                            if(dataSet11.VASKET.Count == 0)
                            {
                                MessageBox.Show("최대 입장 인원 초과입니다.");
                            }
                            else
                            {
                                
                                selectMarket = comboBox1.SelectedItem.ToString();

                                if(marketTableAdapter1.ScalarQuery1(selectMarket.ToString()).ToString() != null)
                                {
                                    textBox5.Text = marketTableAdapter1.ScalarQuery1(selectMarket.ToString()).ToString();
                                }
                                   //공지사항 띄우기

                                DataTable dt = dataSet11.VASKET;        // vasket dataset을 Table로 변환 -> 첫번째 Row -> Row의 첫 행 내용가져오기
                                DataRow firstRow = dt.Rows[0];

                                vasketNum = int.Parse(firstRow[0].ToString());

                                marketuserRow.U_MNAME = selectMarket;
                                marketuserRow.U_CONNECTION = 1;
                                

                                marketuserRow.U_RATING++;   //고객 로그인 시 Rating +1

                                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                                stocklistTableAdapter1.FillBy(dataSet11.STOCKLIST, selectMarket);

                                

                                MessageBox.Show("로그인 성공!");
                                connectedId = marketuserRow.U_ID;
                                connectedType = marketuserRow.U_TYPE;

                                purchaseTableAdapter1.FillBy2(dataSet11.PURCHASE, connectedId, selectMarket);

                                Loginpanel.Visible = false;
                                Customerpanel.Visible = true;
                                dataGridView12.Visible = false;
                                label17.Text = "사용자: " + connectedId;
                                label10.Text = "마트: " + selectMarket + "마트";
                                label32.Text = "장바구니: " + vasketNum;
                                DataSet1.VASKETRow vASKETRows = dataSet11.VASKET.FindByV_ID(vasketNum);

                                vASKETRows.V_AVAIL = 0;
                                vasketTableAdapter1.Update(dataSet11.VASKET);

                                textBox7.Text = "0";
                                textBox8.Text = "0";
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("(보안) 패스워드가 올바르지 않습니다!");
                    }
                }
                else
                {
                    MessageBox.Show("사용자 타입을 선택해주세요!!");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet11.PURCHASE_LIST' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.pURCHASE_LISTTableAdapter.Fill(this.dataSet11.PURCHASE_LIST);



            DataTable markettbl = dataSet11.Tables["MARKET"];
            
            foreach (DataRow mydataRow in markettbl.Rows)
            {
                comboBox1.Items.Add(mydataRow["M_ID"].ToString());
            }

            
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(connectedType == "1" || connectedType == "2")            //관리자 or 판매자인 경우 로그아웃 진행
            {
                DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(connectedId);
                marketuserRow.U_CONNECTION = 0;           //로그아웃 시 connection 0으로 초기화
                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
            }
            else                                    //고객인 경우 로그아웃 진행
            {
                DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(connectedId);
                marketuserRow.U_CONNECTION = 0;           //로그아웃 시 connection 0으로 초기화
                
                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);

                DataSet1.VASKETRow vASKETRows = dataSet11.VASKET.FindByV_ID(vasketNum);
                vASKETRows.V_AVAIL = 1;
                if(dataGridView7.RowCount>0)
                {                    
                    vASKET_LISTTableAdapter.DeleteQuery(vasketNum);     //장바구니 내용 삭제
                }
                vasketTableAdapter1.Update(dataSet11.VASKET);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel21.Visible = false;
            panel20.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel21.Visible = false;
            panel20.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            panel21.Visible = true;
            panel20.Visible = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2("register");
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2("drop");
            f2.Show();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            string pid = marketTableAdapter1.ScalarQuery(comboBox4.SelectedItem.ToString());
            DataTable market = dataSet11.Tables["MARKET"];
            DataSet1.MARKETRow marketRow = dataSet11.MARKET.FindByM_ID(pid);

            marketRow.M_NOTICE = textBox3.Text;

            marketTableAdapter1.Update(dataSet11.MARKET);


        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.productTableAdapter1.Update(this.dataSet11.PRODUCT);
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pRODUCTBindingSource.RemoveCurrent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.sTOCKLISTTableAdapter.Update(this.dataSet11.STOCKLIST);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sTOCKLISTBindingSource.RemoveCurrent();
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "상품코드->상품명";
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.label7, "Example");
        }

        private void dataGridView2_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int a = int.Parse(e.CellValue1.ToString()), b = int.Parse(e.CellValue2.ToString());
            e.SortResult = a.CompareTo(b);
            e.Handled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            productTypeTableadapter1.Fill(dataSet11.PRODUCT_TYPE);
            stocklistTableAdapter1.Fill(dataSet11.STOCKLIST);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("로그아웃하시겠습니까? ", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(connectedId);
                marketuserRow.U_CONNECTION = 0;           //로그아웃 시 connection 0으로 초기화
                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);
                Adminpanel.Visible = false;
                Loginpanel.Visible = true;
            }
            else
            {

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            productTableAdapter1.Fill(dataSet11.PRODUCT);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("로그아웃하시겠습니까? 구매요청을 안하신 경우 초기화 됩니다.", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(connectedId);
                marketuserRow.U_CONNECTION = 0;           //로그아웃 시 connection 0으로 초기화
                
                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);

                DataSet1.VASKETRow vASKETRows = dataSet11.VASKET.FindByV_ID(vasketNum);
                vASKETRows.V_AVAIL = 1;
                if (dataGridView7.RowCount > 0)
                {
                    vASKET_LISTTableAdapter.DeleteQuery(vasketNum);     //장바구니 내용 삭제
                }
                vasketTableAdapter1.Update(dataSet11.VASKET);
                Customerpanel.Visible = false;
                Loginpanel.Visible = true;
            }
            
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("로그아웃하시겠습니까? ", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(connectedId);
                marketuserRow.U_CONNECTION = 0;           //로그아웃 시 connection 0으로 초기화
                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);

                Sellerpanel.Visible = false;
                Loginpanel.Visible = true;
                dataGridView10.Visible = false;
            }
            else
            {

            }
        }

        private void inbt_Click(object sender, EventArgs e)         //담기버튼
        {
            DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
            string pID = selectedRow.Cells[0].Value.ToString();
            
            string Lid = stocklistTableAdapter1.ScalarQuery1(decimal.Parse(pID), selectMarket).ToString();
            DataSet1.STOCKLISTRow stocklistRow = dataSet11.STOCKLIST.FindByL_ID(decimal.Parse(Lid));
            string pNAME = stocklistRow.L_STOCKNAME;
            string pSTOCK = stocklistRow.L_AVAILSTOCK.ToString();
            string pDPRICE = stocklistRow.L_DISCOUNT.ToString();
            string pPRICE = stocklistRow.L_PRICE.ToString();

            if (int.Parse(pSTOCK) > 0)          //남은 재고가 있는 경우
            {
                stocklistRow.L_AVAILSTOCK = int.Parse(pSTOCK) - 1;    //    담는 경우 갯수 -1
                stocklistTableAdapter1.Update(dataSet11.STOCKLIST);
                
                DataTable vasketListtbl = dataSet11.Tables["VASKET_LIST"];
                DataRow vlist_dataRow = vasketListtbl.NewRow();

                vlist_dataRow["VLIST_V_ID"] = vasketNum;
                vlist_dataRow["VLIST_P_ID"] = pID;
                vlist_dataRow["VLIST_P_NAME"] = pNAME;
                vlist_dataRow["VLIST_NUM"] = 1;
                vlist_dataRow["VLIST_M_ID"] = selectMarket;
                if (pDPRICE == "0") vlist_dataRow["VLIST_P_PRICE"] = pPRICE;
                else vlist_dataRow["VLIST_P_PRICE"] = pDPRICE;
                vasketListtbl.Rows.Add(vlist_dataRow);
                vasketListTableAdapter1.Update(dataSet11.VASKET_LIST);

                if (pDPRICE == "0") vaskettotal = vaskettotal + int.Parse(pPRICE);  //총 가격 갱신
                else vaskettotal = vaskettotal + int.Parse(pDPRICE);  //총 가격 갱신
                textBox4.Text = vaskettotal.ToString();

                
            }
            else           //남은 재고가 없는 경우  
            {
                MessageBox.Show("수량이 부족합니다.");
            }
        }

        private void outbt_Click(object sender, EventArgs e)                //빼기버튼
        {
            if(dataGridView7.RowCount > 0)
            {
                DataGridViewRow selectedRow = dataGridView7.SelectedRows[0];
                string pID = selectedRow.Cells[5].Value.ToString();
                string pPRICE = selectedRow.Cells[2].Value.ToString();

                string Lid = stocklistTableAdapter1.ScalarQuery1(int.Parse(pID), selectMarket).ToString();
                DataSet1.STOCKLISTRow stocklistRow = dataSet11.STOCKLIST.FindByL_ID(decimal.Parse(Lid));
                string pSTOCK = stocklistRow.L_AVAILSTOCK.ToString();


                if (dataGridView7.SelectedRows.Count != 0)
                {
                    dataGridView7.Rows.Remove(dataGridView7.SelectedRows[0]);          //장바구니 선택된 항목 제거

                    stocklistRow.L_AVAILSTOCK = int.Parse(pSTOCK) + 1;    //    뺀 경우  재고+1
                    stocklistTableAdapter1.Update(dataSet11.STOCKLIST);

                    vasketListTableAdapter1.Update(dataSet11.VASKET_LIST);

                    vaskettotal = vaskettotal - int.Parse(pPRICE);  //총 가격 갱신
                    textBox4.Text = vaskettotal.ToString();
                }
                else
                {
                    MessageBox.Show("장바구니에서 뺄 상품을 선택하세요.");
                }
            }
            else
            {
                MessageBox.Show("장바구니에 남아있는 상품이 없습니다.");
            }
                
        }

        private void Requestbt_Click(object sender, EventArgs e)
        {
            stocklistTableAdapter1.Update(dataSet11.STOCKLIST);
            if (dataGridView7.RowCount > 0)
            {
                vasketListTableAdapter1.Update(dataSet11.VASKET_LIST);
                DataRow tmpRow;
                DataTable purchasettbl = dataSet11.Tables["PURCHASE"];
                DataRow pca_dataRow = purchasettbl.NewRow();
                pca_dataRow["PCA_ACCESS"] = "구매승인대기";
                pca_dataRow["PCA_M_ID"] = selectMarket;
                pca_dataRow["PCA_TOTALPRICE"] = textBox4.Text;
                pca_dataRow["PCA_C_ID"] = connectedId;
                pca_dataRow["PCA_S_ID"] = 0;
                pca_dataRow["PCA_DATE"] = DateTime.Now.ToString("d");
                purchasettbl.Rows.Add(pca_dataRow);
                tmpRow = pca_dataRow;
                purchaseTableAdapter1.Update(dataSet11.PURCHASE);
                
                string cnt = vasketListTableAdapter1.ScalarQuery().ToString();
                int cnt1 = int.Parse(cnt);
                DataTable purchaselisttbl = dataSet11.Tables["PURCHASE_LIST"];
                
                for (int i = 0; i < cnt1; i++)
                {

                    DataRow vasketlistRow = dataSet11.VASKET_LIST.Rows[i];
                    DataRow purchaselistRow = purchaselisttbl.NewRow();
                    purchaselistRow["PCAL_P_ID"] = vasketlistRow[2];
                    purchaselistRow["PCAL_P_NAME"] = vasketlistRow[3];
                    purchaselistRow["PCAL_P_NUM"] = vasketlistRow[4];
                    purchaselistRow["PCAL_P_PRICE"] = vasketlistRow[6];
                    purchaselistRow["PCAL_PCA_ID"] = tmpRow["PCA_ID"];
                    purchaselistRow["PCAL_STATE"] = "구매승인대기";

                    purchaselisttbl.Rows.Add(purchaselistRow);
                    
                    purchaseListTableAdapter1.Update(dataSet11.PURCHASE_LIST);
                }
                MessageBox.Show("상품 구매 요청 완료, 로그아웃 합니다.");

                DataSet1.MARKETUSERRow marketuserRow = dataSet11.MARKETUSER.FindByU_ID(connectedId);        //로그아웃 코드
                marketuserRow.U_CONNECTION = 0;           //로그아웃 시 connection 0으로 초기화

                marketuserTableAdapter1.Update(dataSet11.MARKETUSER);

                DataSet1.VASKETRow vASKETRows = dataSet11.VASKET.FindByV_ID(vasketNum);
                vASKETRows.V_AVAIL = 1;
                if (dataGridView7.RowCount > 0)
                {
                    vASKET_LISTTableAdapter.DeleteQuery(vasketNum);     //장바구니 내용 삭제
                }
                vasketTableAdapter1.Update(dataSet11.VASKET);
                vaskettotal = 0;
                textBox4.Text = "0";
                Customerpanel.Visible = false;          //화면전환코드
                Loginpanel.Visible = true;
            }
            else
            {
                MessageBox.Show("장바구니에 담겨있는 상품이 없습니다.");
            }
        }

        private void button24_Click(object sender, EventArgs e)         //판매자-구매요청승인
        {
            if(dataGridView9.SelectedRows != null)
            {
                DataGridViewRow selectedRow = dataGridView9.SelectedRows[0];
                string pcaID = selectedRow.Cells[0].Value.ToString();
                DataSet1.PURCHASERow purchaseRow = dataSet11.PURCHASE.FindByPCA_ID(int.Parse(pcaID));
                DataTable purchaselisttbl = dataSet11.Tables["PURCHASE_LIST"];


                if (purchaseRow.PCA_ACCESS.ToString() == "구매승인완료" || purchaseRow.PCA_ACCESS.ToString() == "환불승인대기" || purchaseRow.PCA_ACCESS.ToString() == "환불승인완료")
                {
                    MessageBox.Show("구매승인대기중인 상태가 아닙니다. 다시시도");
                }
                else
                {
                    purchaseRow.PCA_ACCESS = "구매승인완료";
                    purchaseRow.PCA_S_ID = connectedId;
                    if (dataGridView10.RowCount > 0)          //영수증에 제품이 있는경우
                    {
                        purchaseListTableAdapter1.FillBy(dataSet11.PURCHASE_LIST, int.Parse(pcaID));

                        for (int i = 0; i < purchaselisttbl.Rows.Count; i++)        //row의 특정 내용 수정
                        {
                            purchaselisttbl.Rows[i][6] = "구매승인완료";        //oracle내 데이터구조에 따라감                     

                            DataSet1.PRODUCTRow productRow = dataSet11.PRODUCT.FindByP_ID(int.Parse(purchaselisttbl.Rows[i][1].ToString())); //PRODUCT에 SOLD +1
                            productRow.P_SOLD = productRow.P_SOLD + 1;
                            productTableAdapter1.Update(dataSet11.PRODUCT);
                        }

                        purchaseListTableAdapter1.Update(dataSet11.PURCHASE_LIST);
                        purchaseTableAdapter1.Update(dataSet11.PURCHASE);
                    }
                    else           //영수증에 제품이 없는경우
                    {
                        MessageBox.Show("영수증에 상품내역이 없습니다.");
                    }
                }
                
            }
            else
            {
                MessageBox.Show("해당 주문번호를 선택하세요.");
            }
        }

        private void button12_Click(object sender, EventArgs e)             //판매자-영수증확인
        {
            
            if (dataGridView9.RowCount > 0 && dataGridView9.SelectedRows != null)          //영수증에 제품이 있는경우
            {
                DataGridViewRow selectedRow = dataGridView9.SelectedRows[0];
                string pcaID = selectedRow.Cells[0].Value.ToString();             
                string date = selectedRow.Cells[2].Value.ToString();
                DateTime dateTime = Convert.ToDateTime(date);
                string pcatotalPRICE = selectedRow.Cells[3].Value.ToString();

                label46.Text = dateTime.ToString("d");
                textBox6.Text = pcatotalPRICE;
                purchaseListTableAdapter1.FillBy(dataSet11.PURCHASE_LIST, int.Parse(pcaID));
                dataGridView10.Visible = true;
            }
            else           //영수증에 제품이 없는경우
            {
                MessageBox.Show("영수증에 상품내역이없습니다.");
            }
        }

        private void button15_Click(object sender, EventArgs e)         //고객-영수증확인
        {

            if (dataGridView13.RowCount > 0 && dataGridView13.SelectedRows != null)          //영수증에 제품이 있는경우
            {
                DataGridViewRow selectedRow = dataGridView13.SelectedRows[0];
                string pcaID = selectedRow.Cells[0].Value.ToString();
                string date = selectedRow.Cells[2].Value.ToString();
                DateTime dateTime = Convert.ToDateTime(date);
                string pcatotalPRICE = selectedRow.Cells[3].Value.ToString();

                label45.Text = dateTime.ToString("d");
                textBox7.Text = pcatotalPRICE;
                purchaseListTableAdapter1.FillBy(dataSet11.PURCHASE_LIST, int.Parse(pcaID));
                dataGridView12.Visible = true;
            }
            else           //영수증에 제품이 없는경우
            {
                MessageBox.Show("해당 주문번호를 선택하세요.");
            }
        }

        private void button13_Click(object sender, EventArgs e)     //고객-전체환불요청
        {
            if(dataGridView13.SelectedRows != null)
            {
                DataGridViewRow selectedRow = dataGridView13.SelectedRows[0];
                string pcaID = selectedRow.Cells[0].Value.ToString();
                DataSet1.PURCHASERow purchaseRow = dataSet11.PURCHASE.FindByPCA_ID(int.Parse(pcaID));
                DataTable purchaselisttbl = dataSet11.Tables["PURCHASE_LIST"];
                RFtotalprice = int.Parse(purchaseRow.PCA_TOTALPRICE.ToString());            //환불 후 총 가격

                if (purchaseRow.PCA_ACCESS == "구매승인대기" || purchaseRow.PCA_ACCESS == "환불승인대기" || purchaseRow.PCA_ACCESS == "환불승인완료")
                {
                    MessageBox.Show("구매완료상태가 아닙니다. 다시 시도.");
                }

                else
                {
                    purchaseRow.PCA_ACCESS = "환불승인대기";
                    textBox7.Text = purchaseRow.PCA_TOTALPRICE.ToString();

                    if (dataGridView12.RowCount > 0)          //영수증에 제품이 있는경우
                    {
                        purchaseListTableAdapter1.FillBy(dataSet11.PURCHASE_LIST, int.Parse(pcaID));

                        for (int i = 0; i < purchaselisttbl.Rows.Count; i++)        //row의 특정 내용 수정
                        {

                            purchaselisttbl.Rows[i][6] = "환불승인대기";        //oracle내 데이터구조에 따라감                     
                            RFtotalprice -= int.Parse(purchaselisttbl.Rows[i][4].ToString());       //환불 후 가격 => 총금액 - 각 상품당 금액

                            DataSet1.PRODUCTRow productRow = dataSet11.PRODUCT.FindByP_ID(int.Parse(purchaselisttbl.Rows[i][1].ToString())); //PRODUCT에 SOLD +1
                            productRow.P_REFUND = productRow.P_REFUND + 1;
                            productTableAdapter1.Update(dataSet11.PRODUCT);
                        }
                        textBox8.Text = RFtotalprice.ToString();
                        purchaseRow.PCA_TOTALPRICE = RFtotalprice;
                        purchaseListTableAdapter1.Update(dataSet11.PURCHASE_LIST);
                        purchaseTableAdapter1.Update(dataSet11.PURCHASE);
                    }
                    else           //영수증에 제품이 없는경우
                    {
                        MessageBox.Show("해당 주문번호를 선택하세요.");
                    }
                }
                
            }
            else
            {
                MessageBox.Show("환불요청이 올바르게 이루어지지않았습니다.");
            }
        }

        private void button22_Click(object sender, EventArgs e)     //판매자-환불요청승인
        {
            if (dataGridView9.SelectedRows != null)
            {
                DataGridViewRow selectedRow = dataGridView9.SelectedRows[0];
                string pcaID = selectedRow.Cells[0].Value.ToString();
                DataSet1.PURCHASERow purchaseRow = dataSet11.PURCHASE.FindByPCA_ID(int.Parse(pcaID));
                DataTable purchaselisttbl = dataSet11.Tables["PURCHASE_LIST"];


                if (purchaseRow.PCA_ACCESS.ToString() == "구매승인완료" || purchaseRow.PCA_ACCESS.ToString() == "구매승인대기" ||purchaseRow.PCA_ACCESS.ToString() == "환불승인완료")
                {
                    MessageBox.Show("환불요청대기중인 상태가 아닙니다. 다시시도");
                }

                purchaseRow.PCA_ACCESS = "환불승인완료";
                purchaseRow.PCA_S_ID = connectedId;
                if (dataGridView10.RowCount > 0)          //영수증에 제품이 있는경우
                {
                    purchaseListTableAdapter1.FillBy(dataSet11.PURCHASE_LIST, int.Parse(pcaID));

                    for (int i = 0; i < purchaselisttbl.Rows.Count; i++)        //row의 특정 내용 수정
                    {
                        if(purchaselisttbl.Rows[i][6].ToString() == "환불승인대기")
                        {
                            purchaselisttbl.Rows[i][6] = "환불승인완료";        //oracle내 데이터구조에 따라감                     
                            string pID = purchaselisttbl.Rows[i][1].ToString();     //productid + marketid를 통해 stocklistid를 찾고 refund를 +1
                            string mID = selectMarket;
                            string slistID = stocklistTableAdapter1.ScalarQuery1(decimal.Parse(pID), mID).ToString();
                            DataSet1.STOCKLISTRow stocklistRow = dataSet11.STOCKLIST.FindByL_ID(decimal.Parse(slistID));
                            //MessageBox.Show(stocklistRow.L_REFUNDSTOCK.ToString());
                            stocklistRow.L_REFUNDSTOCK += 1;
                            stocklistTableAdapter1.Update(dataSet11.STOCKLIST);
                        }
                        
                    }
                    
                    purchaseListTableAdapter1.Update(dataSet11.PURCHASE_LIST);
                    purchaseTableAdapter1.Update(dataSet11.PURCHASE);
                }
                else           //영수증에 제품이 없는경우
                {
                    MessageBox.Show("영수증에 상품내역이없습니다.");
                }
            }
            else
            {
                MessageBox.Show("해당 주문번호를 선택하세요.");
            }
        }

        private void button14_Click(object sender, EventArgs e)     //고객-부분환불요청
        {
            if(dataGridView13.SelectedRows != null)
            {
                DataGridViewRow selectedRow = dataGridView13.SelectedRows[0];
                string pcaID = selectedRow.Cells[0].Value.ToString();
                DataSet1.PURCHASERow purchaseRow = dataSet11.PURCHASE.FindByPCA_ID(int.Parse(pcaID));
                DataTable purchaselisttbl = dataSet11.Tables["PURCHASE_LIST"];
                RFtotalprice = int.Parse(purchaseRow.PCA_TOTALPRICE.ToString());            //환불 후 총 가격
                if (purchaseRow.PCA_ACCESS == "구매승인대기" || purchaseRow.PCA_ACCESS == "환불승인대기" || purchaseRow.PCA_ACCESS == "환불승인완료")
                {
                    MessageBox.Show("구매완료상태가 아닙니다. 다시 시도.");
                }

                else
                {
                    purchaseRow.PCA_ACCESS = "환불승인대기";
                    textBox7.Text = purchaseRow.PCA_TOTALPRICE.ToString();

                    var selectedRows = dataGridView12.SelectedRows.OfType<DataGridViewRow>().Where(row => !row.IsNewRow).ToArray();
                    //mulitselect시 선택한 row 들의 배열로 만들어주는 코드

                    int[] list = new int[selectedRows.Length];        //환불하기 위해 선택된 열의 갯수와 그에 맞는 크기의 배열
                    int k = 0;


                    if (dataGridView12.RowCount > 0)          //영수증에 제품이 있는경우
                    {
                        //purchaseListTableAdapter1.FillBy(dataSet11.PURCHASE_LIST, int.Parse(pcaID));
                        foreach (var row in selectedRows)
                        {
                            list[k] = row.Index;                    //여러개의 선택한 열에 대한 인덱스 추출
                            //MessageBox.Show(list[k].ToString());      
                            k++;

                        }
                        for (int i = 0; i < selectedRows.Length; i++)        //row의 특정 내용 수정
                        {
                            int tmp = list[i];
                            
                            purchaselisttbl.Rows[tmp][6] = "환불승인대기";        //oracle내 데이터구조에 따라감                     
                            
                            RFtotalprice -= int.Parse(purchaselisttbl.Rows[tmp][4].ToString());       //환불 후 가격 => 총금액 - 각 상품당 금액

                            DataSet1.PRODUCTRow productRow = dataSet11.PRODUCT.FindByP_ID(int.Parse(purchaselisttbl.Rows[tmp][1].ToString())); //PRODUCT에 REFUND +1
                            productRow.P_REFUND = productRow.P_REFUND + 1;
                            productTableAdapter1.Update(dataSet11.PRODUCT);
                        }
                        textBox8.Text = RFtotalprice.ToString();
                        purchaseRow.PCA_TOTALPRICE = RFtotalprice;

                        purchaseListTableAdapter1.Update(dataSet11.PURCHASE_LIST);
                        purchaseTableAdapter1.Update(dataSet11.PURCHASE);
                    }

                }
            }
            else
            {
                
                MessageBox.Show("해당 주문번호를 선택하세요.");
            }
            
        }

        private void button20_Click(object sender, EventArgs e)     //고객-리뷰확인
        {
            string review;
            
            if(dataGridView8.SelectedRows != null)
            {
                listBox1.Items.Clear();
                DataGridViewRow selectRow = dataGridView8.SelectedRows[0];
                string pID = selectRow.Cells[0].Value.ToString();
                
                DataTable reviewtbl = dataSet11.Tables["REVIEW"];
                for(int i = 0; i<reviewtbl.Rows.Count; i++)
                {
                    
                    if(reviewtbl.Rows[i][2].ToString() == pID)
                    {
                        review = string.Format("[{0}] {1}: {2}", "Review", "사용자", reviewtbl.Rows[i][3].ToString());
                        listBox1.Items.Add(review);
                        listBox1.Items.Add(reviewtbl.Rows[i][1].ToString());
                        listBox1.Items.Add("-----------------------------------------");
                        
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("해당 상품번호를 선택하세요.");
            }
        }

        private void button17_Click(object sender, EventArgs e)         //고객-리뷰작성
        {
            if(dataGridView8.SelectedRows != null)
            {
                DataGridViewRow selectRow = dataGridView8.SelectedRows[0];
                string pID = selectRow.Cells[0].Value.ToString();

                DataTable reviewtbl = dataSet11.Tables["REVIEW"];

                DataRow newRow = reviewtbl.NewRow();
                newRow["R_COMMENT"] = textBox9.Text;
                newRow["R_P_ID"] = int.Parse(pID);
                newRow["R_U_ID"] = connectedId;
                reviewtbl.Rows.Add(newRow);

                reviewTableAdapter1.Update(dataSet11.REVIEW);
                
            }
            else
            {
                MessageBox.Show("해당 상품번호를 선택하세요.");
            }
        }

        private void button11_Click(object sender, EventArgs e)     //관리자-매장필터
        {
            if (comboBox2.SelectedIndex != -1)
            {
                string mID = comboBox2.SelectedItem.ToString();

                pURCHASEBindingSource.Filter = "PCA_M_ID = '" + mID + "'";
                
            }
            else
            {
                MessageBox.Show("매장을 선택하세요.");
            }
        }

        private void button18_Click(object sender, EventArgs e)     //관리자-주문날짜필터
        {
            
            string time = dateTimePicker1.Value.ToString("d");
            

            pURCHASEBindingSource.Filter = "PCA_DATE = '" + time + "'";

        }

        //datarow 한줄 생성 후 데이터 추가하는 코드
        //DataTable producttbl = dataSet11.Tables["PRODUCT"];
        //DataRow p_dataRow = producttbl.NewRow();
        //p_dataRow["P_ID"] = productTableAdapter1.ScalarQuery();
        //    p_dataRow["P_TYPENAME"] = productTypeTableadapter1.ScalarQuery(comboBox3.SelectedItem.ToString());
        //    p_dataRow["P_NAME"] = textBox5.Text;
        //    p_dataRow["P_PRICE"] = textBox6.Text;
        //    p_dataRow["P_STOCK"] = textBox4.Text;
        //    p_dataRow["P_REFUNDCNT"] = "0";
        //    p_dataRow["P_PURCHASECNT"] = "0";
        //    producttbl.Rows.Add(p_dataRow);
        //    productTableAdapter1.Update(dataSet11.PRODUCT);

        //콤보박스에 데이터 추가하는 코드
        //DataTable markettbl = dataSet11.Tables["MARKET"];

        //    foreach (DataRow mydataRow in markettbl.Rows)       // 마트 combobox
        //    {
        //        comboBox2.Items.Add(mydataRow["M_NAME"].ToString());
        //    }
    }
}
