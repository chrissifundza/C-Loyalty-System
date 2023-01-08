using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class UserHome : System.Web.UI.Page
{
    public static String CS = ConfigurationManager.ConnectionStrings["MyShoppingDB"].ConnectionString;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        BindCartNumber22();
        if (Session["Username"]!=null)
        {
            btnlogout.Visible = true;
            btnLogin.Visible = false;
            lblSuccess.Text = "Login Success, Welcome <b>" + Session["Username"].ToString()+"</b>";
           
            Button1.Text ="Welcome: " +  Session["Username"].ToString().ToUpper()  ;

            BindUserOints();
            BindUserSentOints();
            BindUserBankAmount();
        }
        else
        {
            btnlogout.Visible = false;
            btnLogin.Visible = true;
            Response.Redirect("SignIn.aspx");
        }
    }

    protected void btnlogout_Click(object sender, EventArgs e)
    {
        //Session.Abandon();
        Session["Username"] = null;
        Response.Redirect("~/Default.aspx");
        
    }

    protected void BtnsenPoints_Click(object sender, EventArgs e)
    {
        if (Session["Username"] != null)
        {
           

            string USERID = Session["USERID"].ToString();
          
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SP_sentPoints", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Points", span1ToalBalance.InnerText);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                Int64 pointsID = Convert.ToInt64(cmd.ExecuteScalar());

                EmptyUserOrders();
            }

        }
        else
        {
            Response.Redirect("SignIn.aspx?RtPP=yes");
        }
    }
    protected void BtnsendBankPoints_Click(object sender, EventArgs e)
    {
        if (Session["Username"] != null)
        {


            string EMAILID = Session["USEREMAIL"].ToString();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SP_BankAmount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", EMAILID);
                cmd.Parameters.AddWithValue("@Points", span1ToalBalance.InnerText);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                Int64 amountID = Convert.ToInt64(cmd.ExecuteScalar());

                BindUserBankAmount();
                EmptyUserOrders();
            }

        }
        else
        {
            Response.Redirect("SignIn.aspx?RtPP=yes");
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
            spanRecievedPoints.InnerText = (0).ToString();
            span1ToalBalance.InnerText = (0).ToString();


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
            spanRecievedPoints.InnerText = (0).ToString();
            span1ToalBalance.InnerText = (0).ToString();


        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/SignIn.aspx");
    }
    public void BindCartNumber()
    {
        if (Request.Cookies["CartPID"] != null)
        {
            string CookiePID = Request.Cookies["CartPID"].Value.Split('=')[1];
            string[] ProductArray = CookiePID.Split(',');
            int ProductCount = ProductArray.Length;
            pCount.InnerText = ProductCount.ToString();
        }
        else
        {
            pCount.InnerText = 0.ToString();
        }
    }
   

    public void BindCartNumber22()
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
                        pCount.InnerText = CartQuantity;
                    }
                    else
                    {
                        //_ = CartBadge.InnerText == 0.ToString();
                        pCount.InnerText ="0";

                    }
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
               
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                //Int64 PurchaseID = Convert.ToInt64(cmd.ExecuteScalar());
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
                    
                    spanPointsTotal.InnerText = string.Format("{0:#,###.##}", double.Parse(Total));
                    
                }
                else
                {

                }
            }

        }
    }

    protected void BtnPlaceNPay_Click(object sender, EventArgs e)
    {
        if (Session["Username"] != null)
        {
            
            Session["PayMethod"] = "Place n Pay";

            string USERID = Session["USERID"].ToString();
            string PaymentType = "PnP";
            string PaymentStatus = "NotPaid";
            string EMAILID = Session["USEREMAIL"].ToString();
            string OrderStatus = "Pending";
            string FullName = Session["getFullName"].ToString();
            using (SqlConnection con = new SqlConnection(CS))
            {
            


            }

        }
        else
        {
            Response.Redirect("SignIn.aspx?RtPP=yes");
        }
    }

    private void BindUserSentOints()
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

                    spanRecievedPoints.InnerText = string.Format("{0:#,###.##}", double.Parse(Total));

                    double total = double.Parse(spanRecievedPoints.InnerText) + double.Parse(spanPointsTotal.InnerText);
                    span1ToalBalance.InnerText = total.ToString();
                }
                else
                {
                    double total =  double.Parse(spanPointsTotal.InnerText);
                    span1ToalBalance.InnerText = total.ToString();
                }
            }

        }
    }

    private void BindUserBankAmount()
    {

        string EMAILID = Session["USEREMAIL"].ToString();
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("SP_BindBankAmount", con)
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
                    double cash = double.Parse(Total) / 5;
                    span1AmountBank.InnerText = string.Format("{0:#,###.##}", cash);

                }
                else
                {

                }
            }

        }
    }
}

