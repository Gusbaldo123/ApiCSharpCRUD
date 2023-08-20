function SelectAllPerson(callback)
{
  var personObj = {
    personAction: 0,
    id: 0,
    name: "",
    email: "",
    entity: 0,
    idNumber: "",
    zipCode: "",
    address: "",
    publicArea: "",
    district: "",
    city: "",
    state: ""
  };

  $.ajax({
    type: "POST",
    url: localhost,
    data: JSON.stringify(personObj),
    contentType: "application/json",
    success: function (result) {
      callback(result);
    }
  });
}
function SelectPerson(numberID,name,callback)
{
  try {
    var personObj = {
      personAction: 1,
      id: 0,
      name: name,
      email: "",
      entity: 0,
      idNumber: numberID,
      zipCode: "",
      address: "",
      publicArea: "",
      district: "",
      city: "",
      state: ""
    };
  } catch (error) {
    alert("Erro inesperado");
    return;
  }

  $.ajax({
    type: "POST",
    url: localhost,
    data: JSON.stringify(personObj),
    contentType: "application/json",
    success: function (result) {
      callback(result);
    }
  });
}
function InsertPerson() {
  var personObj = {
    personAction: 2,
    id: 0,
    name: $("#name").val(),
    email: $("#email").val(),
    entity: parseInt($("#typeEntity").val()),
    idNumber: $("#idNumber").val(),
    zipCode: $("#zipCode").val(),
    address: $("#address").val(),
    publicArea: $("#publicArea").val(),
    district: $("#district").val(),
    city: $("#city").val(),
    state: $("#state").val()
};

$.ajax({
    type: "POST",
    url: localhost,
    data: JSON.stringify(personObj),
    contentType: "application/json",
    success: function (result) {
      alert(result.data);
      GenerateRow();
    }
  });
}
function UpdatePerson(id)
{
  try {
    var personObj = {
      personAction: 3,
      id: parseInt(id),
      name: $("#name").val(),
      email: $("#email").val(),
      entity: parseInt($("#typeEntity").val()),
      idNumber: $("#idNumber").val(),
      zipCode: $("#zipCode").val(),
      address: $("#address").val(),
      publicArea: $("#publicArea").val(),
      district: $("#district").val(),
      city: $("#city").val(),
      state: $("#state").val()
  };
  } catch (error) {
    alert("Erro inesperado");
    return;
  }

$.ajax({
    type: "POST",
    url: localhost,
    data: JSON.stringify(personObj),
    contentType: "application/json",
    success: function (result) {
      alert(result.data);
      GenerateRow();
    }
  });
}
function DeletePerson(id)
{
  try {
    var personObj = {
      personAction: 4,
      id: parseInt(id),
      name: "",
      email: "",
      entity: 0,
      idNumber: "",
      zipCode: "",
      address: "",
      publicArea: "",
      district: "",
      city: "",
      state: ""
    };
  } catch (error) {
    alert("Erro inesperado");
    return;
  }

$.ajax({
    type: "POST",
    url: localhost,
    data: JSON.stringify(personObj),
    contentType: "application/json",
    success: function (result) {
      alert(result.data);
      GenerateRow();
    }
  });
}

function GenerateRow() {
  $('#divOption').remove();
  $("#customRow").remove();
  SelectAllPerson(function (result) {
      var arrTable = [];

      if(result.success)
      {
        var data = result.data;

        data.forEach(element => {
          var arrRow = [element.name, element.idNumber, element.email];
          arrTable.push(arrRow);

          SetupButtons(element, arrRow);
        });
      }
  });
}
function SetupButtons(element, arrRow)
{
  var row = $("<tr>");
  row.attr("id", "customRow").appendTo($("#tableListShownPerson"));

  $("<td>").text(arrRow[0]).appendTo(row);
  $("<td>").text(arrRow[1]).appendTo(row);
  $("<td>").text(arrRow[2]).appendTo(row);
  var td = $("<td>");
  td.appendTo(row);
  var bt = $("<button>");
  bt.attr("id", "btOption").appendTo(td);

  bt.on( "click", function(e) {
    $("#btSubmitFormPerson").show();
    $("#btSubmitEditFormPerson").hide();
    var div = $("<div>");
    div.attr("id", "divOption").appendTo("body");
    var editBt = $("<Button>");
    editBt.text("Editar").appendTo(div);
    editBt.on( "click", function() {
      EditarBt(element);
    } );
    
    var removeBt = $("<Button>");
    removeBt.text("Remover").appendTo(div);
    removeBt.on( "click", function() {
      RemoverBt(element);
    });
    div.css({top: e.pageY, left: e.pageX, position:'absolute'});
  });
}
function RemoverBt(element)
{
  DeletePerson(element.id)
  $('#divOption').remove();
  ClearAll();
}
function EditarBt(element)
{
  $("#btSubmitFormPerson").hide();
  $("#btSubmitEditFormPerson").show();
  $('#divOption').remove();

  $("#btSubmitEditFormPerson").unbind();
  $("#btSubmitEditFormPerson").on( "click", function() {

    $("#btSubmitFormPerson").show();
    $("#btSubmitEditFormPerson").hide();
    $('#divOption').remove();
    UpdatePerson(element.id);
    ClearAll();
  } );
}
function ClearAll()
{
  $("#btSubmitFormPerson").show();
  $("#btSubmitEditFormPerson").hide();
  $('#divOption').remove();
  $("#name").val("");
  $("#email").val("");
  $("#idNumber").val("");
  $("#zipCode").val("");
  $("#publicArea").val("");
  $("#district").val("");
  $("#city").val("");
  $("#state").val("");
  $("#address").val("");
}

$(document).ready(function(){
  $("#idNumber").mask('000.000.000-00', {reverse: true});
  $("#zipCode").mask('00000-000');

  $("#btSubmitEditFormPerson").hide();

  GenerateRow();
});
$(document).click(function(e){
  if($(e.target).is('#btOption,#divOption, #divOption *'))return;
  $('#divOption').remove();
});

$("#btSubmitFilter").on( "click", function() {
  $('#divOption').remove();
  $("#customRow").remove();

  SelectPerson($("#idNumberFilter").val(),$("#nameFilter").val(),function (result) {
    if(typeof(result.data) == String)
      alert(result.data);

      var arrTable = [];

      if(result.success)
      {
        var data = result.data;

        data.forEach(element => {
          var arrRow = [element.name, element.idNumber, element.email];
          arrTable.push(arrRow);

          SetupButtons(element, arrRow);
        });
      }
  });
  ClearAll();
});
$("#btSubmitFormPerson").on( "click", function() {
  InsertPerson();
  ClearAll();
} );
$("#btCancelFormPerson").on( "click", function() {
  ClearAll();
} );
$('#typeEntity').on('change', function() {
  $("#lblIdNumber").text(this.value==0?"CPF":"CNPJ")
  $("#idNumber").mask(this.value==0?'000.000.000-00':'00.000.000/0000-00', {reverse: true});
});