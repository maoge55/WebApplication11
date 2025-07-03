<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="头程物流价格_运营.aspx.cs" Inherits="WebApplication11.cg.tb.头程物流价格_运营"  EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>头程物流价格_运营</title>
            <style>
            .ttt {
                width: 100%;
            }

            .ttt tr td {
                border: 1px solid #000000;
                padding: 5px;
            }

            .ttta {
                width: 100%;
            }

            .ttta tr td {
                border: 1px solid #37cbc5;
                padding: 5px;
            }

            .bbb {
                font-weight: bold;
            }

            .butt {
                padding: 0 50px;
            }

            .add-button {
                border: none;
                border-radius: 4px;
                cursor: pointer;
                font-size: 14px;
                height: 32px;
                transition: all 0.3s ease;
                background-color: #1890ff !important;
            }

            .add-button:hover {
                opacity: 0.9;
                transform: translateY(-1px);
                box-shadow: 0 2px 5px rgba(0,0,0,0.2);
            }

            .required {
                color: red;
                vertical-align: middle;
            }
        </style>
            <script>
            // 验证物流商品种编号必填
            function validateRequired(button) {
                var table = button.parentNode.parentNode.parentNode;
                var codeInput = table.querySelector("input[id*='txtLogisticsCode']");
                
                if (!codeInput) {
                    alert('找不到物流商品种编号输入框');
                    return false;
                }
                
                if (codeInput.value.trim() === "") {
                    alert('物流商品种编号为必填项');
                    codeInput.focus();
                    return false;
                }
                
                return confirm('确定保存？');
            }
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>当前页面【<span style="color: #37cbc5">头程物流价格_运营</span>】</h3>
            <h2 style="color: blue">
                <asp:Literal ID="lits" runat="server"></asp:Literal>
            </h2>
        </div>

        <table class="ttt">
            <asp:Repeater ID="rplb" runat="server">
                <HeaderTemplate>
                    <tr>
                        <td colspan="6">
                            <div style="padding: 10px;">
                                <asp:Button ID="btnAdd" runat="server"
                                    Text="新增"
                                    BackColor="#1890ff"
                                    ForeColor="White"
                                    OnClick="btnAdd_Click"
                                    CssClass="butt add-button" />
                            </div>
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%# Eval("id") %>' />
                        <asp:HiddenField ID="hdnIsNewRecord" runat="server" Value='<%# Eval("IsNewRecord") %>' />
                        <td style="width: 4%; text-align: center">
                            <%# Container.ItemIndex + 1 %><br />
                        </td>
                        <td>
                            <table class="ttta">
                                <tr>
                                    <td style="width: 30%" class="bbb"><span class="required">*</span>物流商品种编号</td>
                                    <td>
                                        <asp:TextBox ID="txtLogisticsCode" runat="server" 
                                            Text='<%# Eval("logistics_product_type_code") %>' 
                                            Width="400px" />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">头程物流价格</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstLogisticsPrice" runat="server" 
                                            Text='<%# Eval("first_logistics_price") %>' 
                                            Width="400px" />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">计价单位</td>
                                    <td>
                                        <asp:TextBox ID="txtPricingUnit" runat="server" 
                                            Text='<%# Eval("pricing_unit") %>' 
                                            Width="400px" />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 30%" class="bbb">产品种类</td>
                                    <td>
                                        <asp:TextBox ID="txtProductCategory" runat="server" 
                                            Text='<%# Eval("product_category") %>' 
                                            Width="800px"
                                            Height="100px"
                                            TextMode="MultiLine" 
                                            style="resize: none;" />
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnSave"
                                            OnClientClick="return validateRequired(this);"
                                            runat="server"
                                            Text="保存"
                                            ForeColor="White"
                                            BackColor="Green"
                                            CssClass="butt"
                                            OnClick="btnSave_Click" />
                                        <asp:HiddenField ID="hdnRowId" runat="server" Value='<%# Eval("id") %>' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </form>
</body>
</html> 