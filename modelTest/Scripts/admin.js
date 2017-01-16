$(document).ready(function () {
    
    //sets session[admin_choice] variable to GRE/SAT in which user is 
   function AjaxCall(vals) {                
        $.ajax({
            type: "POST",
            url: "/admin/Add_temp",
            data: { v: vals },           
            success: function (data) { console.log("from ajaxs" + data); },
            error: function (xhr, ajaxOptions, throwError) {
                //console.log("error");
            }
        });
    }//

    var vals = $("input[name='genre']:checked").val();
    console.log("the global vals" + vals);
    AjaxCall(vals);        //initial ajax call when page loads

    $(function defaultState() {
        $("#tbl_gre").show();
        $("#tbl_sat").hide();
        //console.log("the temp");
    });//

    $('input[type="radio"]').change(function () {
        vals = $("input[name='genre']:checked").val();
        AjaxCall(vals);             //sets session[admin_choice] when radio button change
        if ($("#genre_gre").is(":checked")) {
            $("#tbl_gre").show();
            $("#tbl_sat").hide();            
        }
        else {
            $("#tbl_gre").hide();
            $("#tbl_sat").show();            
        }        
        console.log("vals " + vals);
    }); //   


    $('.text_auto_resize').on('keyup', function () {
        $(this).css('height', 'auto');
    });// 


    $(document).on("click", ".deleteButton", function () {        
        $(this).parents("tr").hide();           //hide row when delete button clicked
        var k = $(this).attr("qid");
        $.ajax({
            type: "POST",
            url: "/admin/Delete",
            data: { id: k },
            success: function (data) {
                console.log("from ajaxs" + data);
                $('#comment').html('<p>Deleted.......</p>');
            },
            error: function (xhr, ajaxOptions, throwError) {
                console.log("error");
            }


        });//
    });//

    $('.edit_a').each(function () {
        var t = $(this).find('input').attr('qid');       
        $(this).wrapInner("<a href='/admin/Edit/" + t + " '/>");    // attach anchor tag with question id to each edit button
    });


    //js for modal box Admin login
    $("#AdminLogin").off("click").click(function () {
        var userName = $("#userName").val();
        var pswrd = $("#pswrd").val();
        console.log("admin " + userName + pswrd);
        $.ajax({
            type: "POST",
            url: "/admin/AdminLogin",
            data: { "usrNm": userName, "pswrd": pswrd },
            datatype: "json",
            success: function (data) {                
                console.log("from admin ajaxs" +data);
                if (Boolean(data.ad))
                    $("#myModal").css({ display: "none" });
                else
                    $("#IncorrectMsg").html("Incorrect password and user name");
            },
            error: function (xhr, ajaxOptions, throwError) {
                console.log("error");
            }
        });//
    });
    
    
    
})