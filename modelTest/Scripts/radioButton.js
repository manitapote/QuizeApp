$(document).ready(function () {
    $("#Signinup_btn, #Next_btn").hide();
    $('#index_bt').click(function () {
        $('#index_bt').hide();
        var inputs = $("#index_fm :input").serializeArray(); //get input from form
        console.log(inputs[2].value);
        $.ajax({
            type: "POST",
            url: "/person/Index",  //action Index[HttpPost]
            data: ({ id: inputs[1].value, option: inputs[2].value }), //inputs[1] =hidden QsnID, inputs[2]=option
            datatype: "jason",
            success: function (data) {                
                var cmnt;                
                if (data[0] == true)  //data[0] bool value to check correct answer
                    cmnt = "Correct!!";
                else
                    cmnt = "Incorrect";
                $('#status').html(cmnt); 
                $('#correctAns').html('<p>The Correct Answer is '+data[3]+'</p>');  //data[3] correct Ans
                $('#details').html(data[2]); //data[2] Detail                              
                $("#Next_btn").show();
                if (Number(data[1]) >= 2) {    //data[1] Number of times the Question displayed
                    $("#Next_btn").hide();
                    $("#Signinup_btn").show();
                }
               
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("error");
            }

        });//
    });//
})