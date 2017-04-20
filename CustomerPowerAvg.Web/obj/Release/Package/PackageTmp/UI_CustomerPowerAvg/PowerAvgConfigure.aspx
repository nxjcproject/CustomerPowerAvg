<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerAvgConfigure.aspx.cs" Inherits="CustomerPowerAvg.Web.UI_CustomerPowerAvg.PowerAvgConfigure" %>
<%@ Register Src="~/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagPrefix="uc1" TagName="OrganisationTree" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>重点设备功率配置</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css"/>
	<link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css"/>

	<script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
	<script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script> 
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>
    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
    <script type="text/javascript" src="/js/common/PrintFile.js" charset="utf-8"></script> 

    <script type="text/javascript" src="js/page/PowerAvgConfigure.js" charset="utf-8"></script>
</head>
<body>        
    <div id="cc" class="easyui-layout"data-options="fit:true,border:false" >   
         <div data-options="region:'west'" style="width: 150px;">
            <uc1:OrganisationTree ID="OrganisationTree_ProductionLine" runat="server" />
        </div>
          <div id="toorBar" title="" style="height:30px;padding:10px;">
            <div>
                <table>
                    <tr>
                        <td>组织机构:</td>
                        <td >                               
                            <input id="organizationName" class="easyui-textbox" readonly="readonly"style="width:80px" />  
                            <input id="organizationId" readonly="readonly" style="display: none;" />             
                        </td>
                        <td>
                            <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="Query()">查询</a>
                        </td>
                             <td style="width:40px"></td>
                       <td>
                            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="addFun()">添加</a>
                        </td>
                         <td>
                            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="refresh()">刷新</a>
                        </td>              
                    </tr>
                </table>         
            </div>
	    </div> 
         <div data-options="region:'center'" style="padding:5px;background:#eee;">
             <table id="grid_Main"class="easyui-datagrid"></table>
         </div>
        <div id="AddandEditor" class="easyui-window" title="重点设备功率配置" data-options="modal:true,closed:true,iconCls:'icon-edit',minimizable:false,maximizable:false,collapsible:false,resizable:false" style="width:620px;height:auto;padding:10px 60px 20px 60px">
	    	    <table>
                    <tr>
	    			    <td>名称：</td> 
	    			    <td><input class="easyui-textbox" type="text" id="itemName" style="width:120px" />                            
	    			    </td>
	    		    </tr>
                    <tr>
	    			    <td>公式：</td> 
	    			    <td><input class="easyui-textbox" type="text" id="formula" style="width:120px" />                            
	    			    </td>
	    		    </tr>
	    		    <tr>
	    			<td>物料：</td>
	    			<td> 
                        <input class="easyui-combobox" type="text" id="material" style="width:200px" />
                    </td>
	    		    </tr>
                    <tr>
	    			    <td>显示顺序：</td> 
	    			    <td><input class="easyui-numberbox" type="text" id="displayIndex" style="width:60px" />
	    			    </td>
	    		    </tr>
                    <tr>
	    			    <td>是否可用：</td> 
	    			    <td>  
                        <select class="easyui-combobox" id="enabled" name="delay" style="width:60px" data-options="panelHeight: 'auto'">
                            <option value="1">启用</option>
                            <option value="0">停用</option>             
                        </select></td>
	    		    </tr>
	    	    </table>
	            <div style="text-align:center;padding:5px;margin-left:-18px;">
	    	        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="save()">保存</a>
	    	        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="$('#AddandEditor').window('close');">取消</a>
	            </div>
            </div>
    </div>
</body>
</html>
