<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserHome.aspx.cs" Inherits="UserHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Home Page</title>
    <script src="http://code.jquery.com/jquery-3.5.1.min.js" integrity="sha256-9/aliU8dGd2tb6OSsuzixeV4y/faTqgFtohetphbbj0=" crossorigin="anonymous">

    </script>
     <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
   <meta http-equiv="X-UA-Compatible" content="IE-edge">
    <link href="css/Custome.css" rel="stylesheet" />
   
   <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
   <style>
       	.border-start-primary {
				border-left-color: #0061f2 !important;
			}
			.border-start-secondary {
				border-left-color: #6900c7 !important;
			}
			.border-start-success {
				border-left-color: #00ac69 !important;
			}
			.border-start-lg {
				border-left-width: 0.25rem !important;
			}
			.h-100 {
				height: 100% !important;
			}
			.form-control {
				border-bottom: 2px solid #eee !important;
				border: none;
				font-weight: 600;
			}

			.form-control:focus {
				color: #495057;
				background-color: #fff;
				border-color: #8bbafe;
				outline: 0;
				box-shadow: none;
				border-radius: 0px;
				border-bottom: 2px solid blue !important;
			}

			.inputbox {
				position: relative;
				margin-bottom: 20px;
				width: 100%;
			}

			.inputbox span {
				position: absolute;
				top: 7px;
				left: 11px;
				transition: 0.5s;
			}

			.inputbox i {
				position: absolute;
				top: 13px;
				right: 8px;
				transition: 0.5s;
				color: #3f51b5;
			}

			input::-webkit-outer-spin-button,
			input::-webkit-inner-spin-button {
				-webkit-appearance: none;
				margin: 0;
			}

			.inputbox input:focus ~ span {
				transform: translateX(-0px) translateY(-15px);
				font-size: 12px;
			}

			.inputbox input:valid ~ span {
				transform: translateX(-0px) translateY(-15px);
				font-size: 12px;
			}

			.card-blue {
				background-color: #492bc4;
			}

			.hightlight {
				background-color: #5737d9;
				padding: 10px;
				border-radius: 10px;
				margin-top: 15px;
				font-size: 14px;
			}
			.fa-cc-visa {
				background-color: #fdcc49;
			}
			.yellow {
				color: #fdcc49;
			}
            table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}

td, th {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

tr:nth-child(even) {
  background-color: #dddddd;
}
   </style>
    <script src="https://kit.fontawesome.com/8f7c71be78.js"
			crossorigin="anonymous"></script>
			
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script>

        $(document).ready(function myfunction() {
            $("#btnCart").click(function myfunction() {
                window.location.href = "Cart.aspx";
            });
        });
      

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class ="navbar navbar-default navbar-fixed-top" role ="navigation">
            <div class ="container ">
                <div class ="navbar-header">
                    <button type="button" class ="navbar-toggle " data-toggle="collapse" data-target=".navbar-collapse">
                        <span class ="sr-only">Toggle navigation</span>
                        <span class ="icon-bar"></span>
                        <span class ="icon-bar"></span>
                        <span class ="icon-bar"></span>

                    </button>
                    <a class ="navbar-brand" href ="UserHome.aspx" ><span ><img src="icons/safeIcons.png" alt="MyEShoppoing" height ="30" /></span>MyEShopping </a>

                </div>
                <div class ="navbar-collapse collapse">
                    <ul class ="nav navbar-nav navbar-right">
                        <li ><a href ="UserHome.aspx">Home</a> </li>
                         <li ><a href ="About.aspx">About</a> </li>
                        <%--<li ><a href ="#">Contact US</a> </li>--%>
                        <%--<li ><a href ="#">Blog-</a> </li>--%>
                        <li class ="drodown">
                            <a href ="#" class ="dropdown-toggle" data-toggle="dropdown">Products<b class ="caret"></b></a>
                            <ul class ="dropdown-menu ">
                                <li> <a href ="Products.aspx">All Products</a></li>
                                <li role="separator" class ="divider "></li> 
                                <li class ="dropdown-header ">Men</li>
                                <li role="separator" class ="divider "></li> 
                                <li> <a href ="ManShirt.aspx">Shirts</a></li>
                                <li> <a href="ManPants.aspx">Pants</a></li>
                                <li> <a href ="ManDenims.aspx">Denims</a></li>
                                <li role="separator" class ="divider "></li>
                                <li class ="dropdown-header ">Women</li>
                                <li role="separator" class ="divider "></li>
                                <li> <a href ="WomanTop.aspx">Top</a></li>
                                <li> <a href ="womanLegging.aspx">Leggings</a></li>
                                <li> <a href ="WomanSarees.aspx">Sarees</a></li>
                            </ul>

                        </li>
                        <li>
                            <button id="btnCart" class="btn btn-primary navbar-btn" type="button">
                                 <i class="fas fa-cart-arrow-down"></i> <span class="badge" id="pCount" runat="server"></span>

                            </button>
                        </li>
                        <li >

                            <asp:Button ID="btnLogin" CssClass ="btn btn-default navbar-btn " runat="server" Text="SignIn" OnClick="btnLogin_Click" />
                            <asp:Button ID="btnlogout" CssClass ="btn btn-default navbar-btn " runat="server" Text="Sign Out" OnClick="btnlogout_Click" />
                            
                        </li>
                        <li>
                        <asp:Button ID="Button1" CssClass ="btn btn-link navbar-btn " runat="server" Text=""  />
                        </li>
                        
                    </ul>
                </div>
            </div>
        </div>
     </div>

        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />

        <div class="container m-4 text-center border-dark" >
            <div class="text-left">

            
             <asp:Label ID="lblSuccess" runat="server" CssClass ="text-success "></asp:Label>
				 
         <div style="gap:20px; border: 1px solid #dddddd; margin-top:10px; padding:20px; border-radius:10px">
			 
			<table>
  <tr>
    <th>Points Accumulated</th>
    <th>Points Recieved from Users</th>
    <th>Total Points Available </th>
  </tr>
  <tr>
    <td>  <span class=" priceGreen" runat="server" id="spanPointsTotal">0</span> <span class="priceGreen">points</span> </td>
    <td>  <span class=" priceGreen" runat="server" id="spanRecievedPoints">0</span> <span class="priceGreen">points</span></td>
    <td>  <span class=" priceGreen" runat="server" id="span1ToalBalance">0</span> <span class="priceGreen">points</span> </td>
  </tr>
  
</table>
          <br /> <br />
			 <hr />   
             <label class="col-xs-11">Send Points to user:</label>
            <div class="col-xs-11 mb-4">
            <asp:TextBox ID="txtEmail" runat="server" Class="form-control" placeholder="Enter Your Email"></asp:TextBox>
            </div> <br /> <br/>
             <div class="container m-3">
                 <br/><br/>
                 
                    <asp:Button ID="BtnPlaceNPay" CssClass=" btn btn-success" Font-Size="Large" ValidationGroup="PaymentPage" runat="server" OnClick="BtnsenPoints_Click" Text="Send &raquo;" />
             </div>
            
          </div>
        </div>
       
            </div>
		<hr />
        <br /><br /><br />
          <div class="col-md-12">
			  <div class="container">
                  <h3>Change points into cash and send to Bank Account</h3>
            <hr />
            <ul class="nav nav-tabs">
                <li class="nav-item"><a class="nav-link active" data-toggle="tab" href="#wallets">WALLETS</a></li>
                <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#cards">CREDIT/DEBIT CARDS</a></li>
                <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#cod">My Bank Balance</a></li>
            </ul>
             <div class="tab-content">
                <div id="wallets" class="tab-pane fade show active">
                    <h3>Mobile Money</h3>
                    <p>Connected to my registered number</p>
                    <asp:Button ID="btnPaytm" OnClick="btnPaytm_Click" runat="server" Text="Pay with Paytm" />
                </div>
                <div id="cards" class="tab-pane fade">
                    <h3>Payment details</h3>
                    <div class="row">
							<div class="col-md-6 ml-4" style="border: 1px solid gray; padding:20px;border-radius:10px; margin-left: 50px">
								<div class="card p-3">
									
									<div class="inputbox mt-3">
										<input
											type="text"
											name="name"
											class="form-control"
											
											id="name-card"
										/>
										<span>Name on card</span>
									</div>
									<div class="row">
										<div class="col-md-6">
											<div class="inputbox mt-3 mr-2">
												<input
													type="text"
													name="name"
													class="form-control"
													
													id="holder-card"
												/>

												<span>Account Holder</span>
											</div>
										</div>
										<div class="col-md-6">
											<div class="inputbox mt-3 mr-2">
												<select
													name=""
													class="form-control"
													id="type-card"
												>
													<option value="">
														Type of Account
													</option>
													<option value="Cheque">
														Cheque
													</option>
													<option value="Savings">
														Savings
													</option>
												</select>
											</div>
										</div>
									</div>

									<div class="row">
										<div class="col-md-6">
											<div class="inputbox mt-3 mr-2">
												<input
													type="text"
													name="name"
													class="form-control"
													
													id="number-card"
													onkeypress="return formats(this,event)"
													onkeyup="return numberValidation(event)"
												/>
												<i
													class="fa fa-credit-card"
												></i>
												<span>Card Number</span>
											</div>
										</div>
										<div class="col-md-6">
											<div class="d-flex flex-row">
												<div class="inputbox mt-3 mr-2">
													<input
														type="text"
														name="name"
														class="form-control"
														
														id="date-expirey"
														maxlength="5"
														onkeyup="formatString(event);"
													/>
													<span>Expiry</span>
												</div>
												<div class="inputbox mt-3 mr-2">
													<input
														type="text"
														name="name"
														class="form-control"
														
														id="the-cvv"
														maxlength="3"
													/>
													<span>CVV</span>
												</div>
											</div>
										</div>
									</div>
									
								</div>
							</div>
						</div>
					<br />
					 <asp:Button ID="Button3" CssClass=" btn btn-info" Font-Size="Large" ValidationGroup="PaymentPage" runat="server" OnClick="BtnsendBankPoints_Click" Text="Send &raquo;" />
                </div>
                <div id="cod" class="tab-pane fade">
                    <h3>My Bank Balance</h3>
                     <label class=" ">Amount changed from points</label>
            <span>R </span> <strong><span class=" priceGreen" runat="server" id="span1AmountBank">0</span></strong>   <br/> <br/>
                </div>
            </div>

             <div class="tab-content">
                 <br />
         
                                <div id="PlaceNPay" class="tab-pane fade in active">
                                  
                                   
                               <br /><br /> <br />
									</div>

                                
                                <div id="EasyPaisa" class="tab-pane fade">
                                    <h3 class="center1">EasyPaisa Payment Gateway Coming Soon</h3>
                                    <h4 class="center1">Until that you can send amount @ 0311 0000193</h4>
                                    <!--  <asp:Button ID="btnEasyPaisa" CssClass=" btn btn-success" Font-Size="Large" runat="server" Text="Pay with EasyPaisa &raquo;" /> -->
                                </div>
                                <div id="JazzCash" class="tab-pane fade">
                                    <h3 class="center1">JazzCash Payment Gateway Coming Soon</h3>
                                    <h4 class="center1">Until that you can send amount @ 0300 1888193</h4>
                                    <!-- <asp:Button ID="btnJazzCash" CssClass="btn btn-danger" runat="server" Font-Size="Large" Text="Pay with JazzCash &raquo;" /> -->
                                </div>
                                <div id="Div1" class="tab-pane fade">
                                    <h3 class="center1">Cash on Delivery - Coming Soon</h3>
                                    <!-- <asp:Button ID="btnCOD" CssClass="btn btn-primary" runat="server" Text="CheckOut &raquo;" Font-Size="Large" /> -->
                                </div>
                       </div>
        </div>
			  </div>
  
         <!---Footer COntents Start here---->
        <hr />
        <footer>
            <div class ="container ">
                <p class ="pull-right "><a href ="#">Back to top</a></p>
                <p>&copy;2022 E-Shopping &middot; <a href ="Default.aspx">Home</a>&middot;<a href ="#">About</a>&middot;<a href ="#">Contact</a>&middot;<a href ="#">Products</a> </p>
            </div>

        </footer>

         <!---Footer COntents End---->
    </form>


</body>
</html>
