$(function () {
    LoadMainDataGrid("first");
    initPageAuthority();
});
function onOrganisationTreeClick(node) {
    $('#organizationName').textbox('setText', node.text);
    $('#organizationId').val(node.OrganizationId);
    mOrganizationId = node.OrganizationId;
    LoadMaterialList(mOrganizationId);
}
function LoadMaterialList(value) {
    $.ajax({
        type: "POST",
        url: "PowerAvgConfigure.aspx/GetMaterial",
        data: " {mOrganizationId:'" + value + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var myData = jQuery.parseJSON(msg.d);
            $('#material').combobox({
                valueField: 'MaterialId',
                textField:  'Name',
                panelHeight: '300',
                data: myData.rows,
                onSelect: function (record) {
                }
            });
        },
        error: function () {
            $.messager.alert('失败', '加载失败！');
        }
    });
}
//初始化页面的增删改查权限
function initPageAuthority() {
    $.ajax({
        type: "POST",
        url: "PowerAvgConfigure.aspx/AuthorityControl",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//同步执行
        success: function (msg) {
            PageOpPermission = msg.d;
            //增加
            if (PageOpPermission[1] == '0') {
                $("#add").linkbutton('disable');
            }
            //修改
            //if (authArray[2] == '0') {
            //    $("#edit").linkbutton('disable');
            //}
            //删除
            //if (PageOpPermission[3] == '0') {
            //    $("#id_deleteAll").linkbutton('disable');
            //}
        }
    });
}
function LoadMainDataGrid(type, myData) {
    if (type == "first") {
        $('#grid_Main').datagrid({
            columns: [[
                  { field: 'EquipmentGroupId', title: '标识列', width: 100, hidden: true },
                  { field: 'Name', title: '名称', width: 60, align: 'left' },
                  { field: 'Formula', title: '公式', width: 100, align: 'left' },
                  {
                      field: 'NewName', title: '物料', width: 200, align: 'left',
                  },
                  { field: 'DisplayIndex', title: '显示顺序', width: 80, align: 'left' },
                  {
                      field: 'edit', title: '编辑', width: 100, formatter: function (value, row, index) {
                          var str = "";
                          str = '<a href="#" onclick="editFun(true,\'' + row.EquipmentGroupId + '\')"><img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" style="border:none;" title="编辑页面" onclick="editFun(true,\'' + row.EquipmentGroupId + '\')"/>编辑</a>';
                          str = str + '<a href="#" onclick="deleteFun(\'' + row.EquipmentGroupId + '\')"><img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" style="border:none;" title="删除页面"  onclick="deleteFun(\'' + row.EquipmentGroupId + '\')"/>删除</a>';
                          return str;
                      }
                  }
            ]],
            fit: true,
            toolbar: "#toorBar",
            idField: 'EquipmentGroupId',
            rownumbers: true,
            singleSelect: true,
            striped: true,
            data: []
        });
    }
    else {
        $('#grid_Main').datagrid('loadData', myData);
    }
}
function Query() {
    $.ajax({
        type: "POST",
        url: "PowerAvgConfigure.aspx/GetPowerAvg",
        data: "{mOrganizationId:'" + mOrganizationId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            var myData = jQuery.parseJSON(msg.d);
            if (myData.total == 0) {
                LoadMainDataGrid("last", []);
                $.messager.alert('提示', '没有查询到记录！');
            } else {
                LoadMainDataGrid("last", myData);
            }
        },
        error: function () {
            $.messager.progress('close');
            $("#grid_Main").datagrid('loadData', []);
            $.messager.alert('失败', '加载失败！');
        }
    });
}
function refresh() {
    Query();
}
function addFun() {
    editFun(false);
}
var mMaterial = '';
var mEquipmentGroupId = '';
function editFun(IsEdit, editContrastId) {
    if (IsEdit) {
        IsAdd = false;
        $('#grid_Main').datagrid('selectRecord', editContrastId);
        var data = $('#grid_Main').datagrid('getSelected');
        $('#itemName').textbox('setText', data.Name);
        $('#formula').textbox('setText', data.Formula);
        $('#material').combobox('setText', data.NewName);
        $('#displayIndex').numberbox('setText', data.DisplayIndex);
        mEquipmentGroupId = data.EquipmentGroupId;
        mMaterial = data.MaterialId;
    }
    else {
        IsAdd = true;
        $('#itemName').textbox('clear');
        $('#formula').textbox('clear');
        $('#material').combobox('clear');
        $('#displayIndex').numberbox('clear');
        if (mOrganizationId == "" && mOrganizationId == undefined) {
            $.messager.alert('提示', '请选择组织机构！');
        }      
    }
    $('#AddandEditor').window('open');
}
function save() {
    var mItemName = $('#itemName').textbox('getText');
    var mFormula = $('#formula').textbox('getText');
    //var mMaterial = $('#material').combobox('getValue');
    var mDisplayIndex = $('#displayIndex').numberbox('getText');
    var mEnabled = $('#enabled').combobox('getValue');
    if (mItemName == "" || mFormula == "" || mDisplayIndex == "") {
        $.messager.alert('提示', '请填写未填项!');
    }
    else {
        var mUrl = "";
        var mdata = "";
        if (IsAdd) {
            mMaterial = $('#material').combobox('getValue');
            mUrl = "PowerAvgConfigure.aspx/AddPowerAvg";
            mdata = "{mOrganizationId:'" + mOrganizationId + "',mItemName:'" + mItemName + "',mFormula:'" + mFormula + "',mMaterial:'" + mMaterial + "',mDisplayIndex:'" + mDisplayIndex + "',mEnabled:'" + mEnabled + "'}";
        } else if (IsAdd == false) {
            mUrl = "PowerAvgConfigure.aspx/EditPowerAvg";
            mdata = "{mEquipmentGroupId:'" + mEquipmentGroupId + "',mOrganizationId:'" + mOrganizationId + "',mItemName:'" + mItemName + "',mFormula:'" + mFormula + "',mMaterial:'" + mMaterial + "',mDisplayIndex:'" + mDisplayIndex + "',mEnabled:'" + mEnabled + "'}";
        }
        $.ajax({
            type: "POST",
            url: mUrl,
            data: mdata,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var myData = msg.d;
                if (myData == 1) {
                    $.messager.alert('提示', '操作成功！');
                    $('#AddandEditor').window('close');
                    refresh();
                }
                else {
                    $.messager.alert('提示', '操作失败！');
                    refresh();
                }
            },
            error: function () {
                $.messager.alert('提示', '操作失败！');
                refresh();
            }
        });
    }
}
function deleteFun(deleteFunContrastId) {
    $.messager.confirm('提示', '确定要删除吗？', function (r) {
        if (r) {
            $('#grid_Main').datagrid('selectRecord', deleteFunContrastId);
            var data = $('#grid_Main').datagrid('getSelected');

            mEquipmentGroupId = data.EquipmentGroupId;

            $.ajax({
                type: "POST",
                url: "PowerAvgConfigure.aspx/DeletePowerAvg",
                data: "{mEquipmentGroupId:'" + mEquipmentGroupId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var myData = msg.d;
                    if (myData == 1) {
                        $.messager.alert('提示', '删除成功！');
                        $('#AddandEditor').window('close');
                        refresh();
                    }
                    else {
                        $.messager.alert('提示', '操作失败！');
                        refresh();
                    }
                },
                error: function () {
                    $.messager.alert('提示', '操作失败！');
                    $('#AddandEditor').window('close');
                    refresh();
                }
            });
        }
    })
}