<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jj.aspx.cs" Inherits="WebApplication11.jj" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
fetch('https://www.baidu.com')
  .then(response => {
    if (response.ok) {
      return response.text();
    }
    throw new Error('Network response was not ok.');
  })
    .then(html => {
        document.getElementById("form1").innerHTML = html;
      
  })
  .catch(error => {
  
  });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
        </div>
    </form>
</body>
</html>
