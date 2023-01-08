using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class Payment : System.Web.UI.Page
{
    public static String CS = ConfigurationManager.ConnectionStrings["MyShoppingDB"].ConnectionString;
    public static Int32 OrderNumber = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["USERNAME"] != null)
        {
            if (!IsPostBack)
            {
                BindPriceData2();
                genAutoNum();
                BindCartNumber();
                BindOrderProducts();
                BindUserOints();
            }            
        }
        else
        {
            Response.Redirect("SignIn.aspx");
        }
    }

    public void BindPriceData()
    {
        if (Request.Cookies["CartPID"] != null)
        {
            string CookieData = Request.Cookies["CartPID"].Value.Split('=')[1];
            string[] CookieDataArray = CookieData.Split(',');
            if (CookieDataArray.Length > 0)
            {
                DataTable dtBrands = new DataTable();
                Int64 CartTotal = 0;
                Int64 Total = 0;
                Int64 Points =0;
                for (int i = 0; i < CookieDataArray.Length; i++)
                {
                    string PID = CookieDataArray[i].ToString().Split('-')[0];
                    string SizeID = CookieDataArray[i].ToString().Split('-')[1];

                    if (hdPidSizeID.Value != null && hdPidSizeID.Value != "")
                    {
                        hdPidSizeID.Value += "," + PID + "-" + SizeID;
                    }
                    else
                    {
                        hdPidSizeID.Value = PID + "-" + SizeID;
                    }

                   
                    using (SqlConnection con = new SqlConnection(CS))
                    {
                        using (SqlCommand cmd = new SqlCommand("select A.*,dbo.getSizeName(" + SizeID + ") as SizeNamee,"
                            + SizeID + " as SizeIDD,SizeData.Name,SizeData.Extention from tblProducts A cross apply( select top 1 B.Name,Extention from tblProductImages B where B.PID=A.PID ) SizeData where A.PID="
                            + PID + "", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                sda.Fill(dtBrands);
                            }

                        }
                    }
                    CartTotal += Convert.ToInt64(dtBrands.Rows[i]["PPrice"]);
                    Total += Convert.ToInt64(dtBrands.Rows[i]["PSelPrice"]);
                }
                divPriceDetails.Visible = true;

                spanCartTotal.InnerText = CartTotal.ToString();
                spanTotal.InnerText = "R " + Total.ToString();
                spanDiscount.InnerText = "- " + (CartTotal - Total).ToString();
                Points = Total * (10 / 100);
                spanPoints.InnerText = Points.ToString() +" points";
                hdCartAmount.Value = CartTotal.ToString();
                hdCartDiscount.Value = (CartTotal - Total).ToString();
                hdTotalPayed.Value = Total.ToString();
                HiddenField1.Value = Total.ToString();

            }
            else
            {
                //TODO Show Empty Cart
                Response.Redirect("~/Products.aspx");
            }
        }
        else
        {
            //TODO Show Empty Cart
            Response.Redirect("~/Products.aspx");
        }
    }

    private void BindPriceData2()
    {
        string UserIDD = Session["USERID"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("SP_BindPriceData", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("UserID", UserIDD);
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    string Total = dt.Compute("Sum(SubSAmount)", "").ToString();
                    string CartTotal = dt.Compute("Sum(SubPAmount)", "").ToString();
                    string CartQuantity = dt.Compute("Sum(Qty)", "").ToString();
                    double Total1 = Convert.ToInt32(dt.Compute("Sum(SubSAmount)", ""));
                    int CartTotal1 = Convert.ToInt32(dt.Compute("Sum(SubPAmount)", ""));
                    spanTotal.InnerText = ": R " + string.Format("{0:#,###.##}", double.Parse(Total)) + ".00";
                    Session["myCartAmount"] = string.Format("{0:####}", double.Parse(Total));
                    spanCartTotal.InnerText = ": R " + string.Format("{0:#,###.##}", double.Parse(CartTotal)) + ".00";
                    spanDiscount.InnerText = ": R " + (CartTotal1 - Total1).ToString();
                    spanPoints.InnerText = ": "+ ((Total1)*5/100).ToString() + " Points";
                    Session["TotalAmount"] = spanTotal.InnerText;
                    hdCartAmount.Value = CartTotal.ToString();
                    HiddenPoints.Value = ((Total1) * 5 / 100).ToString();
                    hdCartDiscount.Value = (CartTotal1 - Total1).ToString() + ".00";
                    hdTotalPayed.Value = Total.ToString();
                }
                else
                {
                    Response.Redirect("Products.aspx");
                }
            }
        }
    }

    protected void btnPaytm_Click(object sender, EventArgs e)
    {
        if (Session["USERNAME"] != null)
        {
            string USERID = Session["USERID"].ToString();
            string PaymentType = "Paytm";
            string PaymentStatus = "NotPaid";
            string EMAILID = Session["USEREMAIL"].ToString();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("insert into tblPurchase values('" + USERID + "','"
                    + hdPidSizeID.Value + "','" + hdCartAmount.Value + "','" + hdCartDiscount.Value + "','"
                    + hdTotalPayed.Value + "','" + PaymentType + "','" + PaymentStatus + "',getdate(),'"
                    + txtName.Text + "','" + txtAddress.Text + "','" + txtPinCode.Text + "','" + txtMobileNumber.Text + "') select SCOPE_IDENTITY()", con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                Int64 PurchaseID = Convert.ToInt64(cmd.ExecuteScalar());
            }
        }
        else
        {
            Response.Redirect("SignIn.aspx");
        }
    }

    private void BindUserOints()
    {
        string UserIDD = Session["USERID"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("SP_BindUserOrder", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UserID", UserIDD);
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string Total = dt.Compute("Sum(Points)", "").ToString();

                   
                    BindUserSentOints(double.Parse(Total));
                }
                else
                {
                    double accumulated = 0;
                    BindUserSentOints(accumulated);
                }
            }

        }
    }
    private void BindUserSentOints(double accumulatedOrder)
    {

        string EMAILID = Session["USEREMAIL"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("SP_BindSentPoints", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", EMAILID);
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string Total = dt.Compute("Sum(Points)", "").ToString();

                  
                    spanPointsTotal.InnerText = string.Format("{0:#,###.##}", double.Parse(Total) + accumulatedOrder);
                   
                }
                else
                {
                    spanPointsTotal.InnerText = string.Format("{0:#,###.##}", accumulatedOrder) ;
                }
            }

        }
    }

    protected void btnRedeemPoints_Click(object sender, EventArgs e)
    {

            string UserIDD = Session["USERID"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("SP_BindPriceData", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("UserID", UserIDD);
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                   
                    double Total1 = Convert.ToInt32(dt.Compute("Sum(SubSAmount)", ""));
                    calcul(Total1);
                }
                else
                {
                    Response.Redirect("Products.aspx");
                }
            }
        }
    }
    public void calcul(double num)
    {
        if (Session["USERNAME"] != null)
        {


            double Total1 = double.Parse(spanPointsTotal.InnerText);
                        double redeemedValue = (Total1) / 5;

                        double disc = num - redeemedValue;
                        newPrice.InnerText = "R "+(disc).ToString() + " New Price from Points Redeemed!";
                        EmptyUserOrders();
                   

        }
        else
        {
            Response.Redirect("SignIn.aspx");
        }
    }

    private void EmptyUserOrders()
    {
        string UserIDD = Session["USERID"].ToString();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmdU = new SqlCommand("SP_EmptyUserOrder", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmdU.Parameters.AddWithValue("@UserID", UserIDD);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            cmdU.ExecuteNonQuery();
            EmptyUserSentPoints();
            spanPointsTotal.InnerText = (0).ToString();


        }
    }
    private void EmptyUserSentPoints()
    {
        string EMAILID = Session["USEREMAIL"].ToString();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmdU = new SqlCommand("SP_EmptySentPoints", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmdU.Parameters.AddWithValue("@Email", EMAILID);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            cmdU.ExecuteNonQuery();
            spanPointsTotal.InnerText = (0).ToString();


        }
    }
    public void BindCartNumber()
    {
        if (Session["USERID"] != null)
        {
            string UserIDD = Session["USERID"].ToString();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SP_BindCartNumberz", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserID", UserIDD);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string CartQuantity = dt.Compute("Sum(Qty)", "").ToString();
                        CartBadge.InnerText = CartQuantity;
                    }
                    else
                    {
                        //_ = CartBadge.InnerText == 0.ToString();
                    }
                }
            }
        }
    }

    private void genAutoNum()
    {
        Random r = new Random();
        int num = r.Next(Convert.ToInt32("231965"),
       Convert.ToInt32("987654"));
        string ChkOrderNum = num.ToString();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("SP_FindOrderNumber", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@FindOrderNumber", ChkOrderNum);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    genAutoNum();
                }
                else
                {
                    OrderNumber = Convert.ToInt32(num.ToString());
                }
            }
        }
    }

    private void BindOrderProducts()
    {
        string UserIDD = Session["USERID"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con0 = new SqlConnection(CS))
        {
            SqlCommand cmd0 = new SqlCommand("SP_BindCartProducts", con0)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd0.Parameters.AddWithValue("@UID", UserIDD);
            using (SqlDataAdapter sda0 = new SqlDataAdapter(cmd0))
            {
                sda0.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn PID in dt.Columns)
                    {
                        using (SqlConnection con = new SqlConnection(CS))
                        {
                            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblCart C WHERE C.PID=" + PID + " AND UID ='" + UserIDD + "'", con))
                            {
                                cmd.CommandType = CommandType.Text;
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    DataTable dtProducts = new DataTable();
                                    sda.Fill(dtProducts);
                                    gvProducts.DataSource = dtProducts;
                                    gvProducts.DataBind();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnCart2_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("Cart.aspx");
    }

    protected void BtnPlaceNPay_Click(object sender, EventArgs e)
    {
        if (Session["Username"] != null)
        {
            Session["Address"] = txtAddress.Text;
            Session["Mobile"] = txtMobileNumber.Text;
            Session["OrderNumber"] = OrderNumber.ToString();
            Session["PayMethod"] = "Place n Pay";

            string USERID = Session["USERID"].ToString();
            string PaymentType = "PnP";
            string PaymentStatus = "NotPaid";
            string EMAILID = Session["USEREMAIL"].ToString();
            string OrderStatus = "Pending";
            string FullName = Session["getFullName"].ToString();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", USERID);
                cmd.Parameters.AddWithValue("@Email", EMAILID);
                cmd.Parameters.AddWithValue("@CartAmount", hdCartAmount.Value);
                cmd.Parameters.AddWithValue("@CartDiscount", hdCartDiscount.Value);
                cmd.Parameters.AddWithValue("@TotalPaid", hdTotalPayed.Value);
                cmd.Parameters.AddWithValue("@PaymentType", PaymentType);
                cmd.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);
                cmd.Parameters.AddWithValue("@DateOfPurchase", DateTime.Now);
                cmd.Parameters.AddWithValue("@Name", FullName);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text);
                cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
                cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber.ToString());
                cmd.Parameters.AddWithValue("@Points", HiddenPoints.Value);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                Int64 OrderID = Convert.ToInt64(cmd.ExecuteScalar());
                InsertOrderProducts();
                


            }

        }
        else
        {
            Response.Redirect("SignIn.aspx?RtPP=yes");
        }
    }

    private void BindUserPoints()
    {
        string UserIDD = Session["USERID"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
          
            SqlCommand cmd = new SqlCommand("Insert into tblUserPoints(UserID,Points) Values('" + UserIDD + "', '" + HiddenPoints.Value + "'", con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            Int32 LoggedUserID = Convert.ToInt32(cmd.ExecuteScalar());
           
        }
            
       }
            private void InsertOrderProducts()
    {
        string USERID = Session["USERID"].ToString();
        using (SqlConnection con = new SqlConnection(CS))
        {
            foreach (GridViewRow gvr in gvProducts.Rows)
            {
                SqlCommand myCmd = new SqlCommand("SP_InsertOrderProducts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                myCmd.Parameters.AddWithValue("@OrderID", OrderNumber.ToString());
                myCmd.Parameters.AddWithValue("@UserID", USERID);
                myCmd.Parameters.AddWithValue("@PID", gvr.Cells[0].Text);
                myCmd.Parameters.AddWithValue("@Products", gvr.Cells[1].Text);
                myCmd.Parameters.AddWithValue("@Quantity", gvr.Cells[2].Text);
                myCmd.Parameters.AddWithValue("@OrderDate", DateTime.Now.ToString("yyyy-MM-dd"));
                myCmd.Parameters.AddWithValue("@Status", "Pending");
                if (con.State == ConnectionState.Closed) { con.Open(); }
                Int64 OrderProID = Convert.ToInt64(myCmd.ExecuteScalar());
                con.Close();
                EmptyCart();
                Response.Redirect("Success.aspx");
            }
        }
    }

    private void EmptyCart()
    {
        Int32 CartUIDD = Convert.ToInt32(Session["USERID"].ToString());
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmdU = new SqlCommand("SP_EmptyCart", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmdU.Parameters.AddWithValue("@UserID", CartUIDD);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            cmdU.ExecuteNonQuery();
            con.Close();
        }
    }
}