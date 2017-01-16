$(document).ready(function () {    
    $('input[name="NextBtn"]').hide();
    var data, ret_response = [];
    function AjaxQuestion() {
        $.ajax({                                //retrieve question
            type: "GET",
            url: "/person/WelcomeQuestion_Retrieve",
            success: function (response) {
                data = [];
                data = response;
                $("#QuestionDisplay").html("<p>" + data[0].Qsn + "</p>");
                var radioBtn = '<p><input type="radio" name="rbtn" checked="checked" value="'+data[0].OptionA+'" />' + '<label for="' + data[0].OptionA + '">' + data[0].OptionA + '</label>' + '</p>'
                + '<p><input type="radio" name="rbtn" value="'+data[0].OptionB+'" />' + '<label for="' + data[0].OptionB + '">' + data[0].OptionB + '</label>' + '</p>' +
                '<p><input type="radio" name="rbtn" value="'+data[0].OptionC+'" />' + '<label for="' + data[0].OptionC + '">' + data[0].OptionC + '</label>' + '</p>' +
                '<p><input type="radio" name="rbtn" value="'+data[0].OptionD+'" />' + '<label for="' + data[0].OptionD + '">' + data[0].OptionD + '</label>' + '</p>';
                $('#radioBtn').html(radioBtn);                
                if (data[1] == "SAT") {
                    OptionE_radioBtn = $('<p><input type="radio" name="rbtn" value="'+data[0].OptionE+'" />' + '<label for="' + data[0].OptionE + '">' + data[0].OptionE + '</label>' + '</p>');
                    OptionE_radioBtn.appendTo("#radioBtn");
                }
                $("input[name='Quize_btn']").show();
                $("input[name='Quize_btn']").off("click").click(function () {                    
                    var vals = $("input[name='rbtn']:checked").val();                                       
                    $.ajax({                                        //check answer and calculate points
                        type: "POST",
                        url: "/person/Check",
                        data: { value: vals, qid: data[0].QsnID },                  //sending user choosen option and question ID
                        success: function (ret_response) {
                            ret_data = [];
                            ret_data = ret_response;                                                       
                            $("input[name='Quize_btn']").hide();
                            var cmnt="";
                            if (ret_data[0] == true) {
                                
                                 cmnt='<p>The answer is Correct!! You have earned 10 extra points.</p>';                                
                            }
                            else {
                                cmnt='<p>The answer is Incorrect!!</p>';
                            }

                            //Display detail, correct answer, total score
                            $("#Correct_Incorrect, #detail_welcome, #totalPoint, #CorrectAns").show();
                            $("#Correct_Incorrect").html(cmnt);                            
                            $("#CorrectAns").html('<p>The correct answer is ' + data[0].CorrectAs + '</p>');
                            $("#detail_welcome").html('<p>' + data[0].Detail + '</p>');
                            $("#totalPoint").html('<p> Your total score is' + ret_data[1] + '</p>');
                            $("input[name='NextBtn']").show();
                            $("#TotalPoint").html("Your total score is " + ret_data[1]);
                        },
                        error: function (xhr, ajaxOptions, throwError) {
                            console.log("error");
                        }
                    });
                });//
            },
            error: function (xhr, ajaxOptions, throwError) {
                console.log("error");
            }
        });//
    }//
    AjaxQuestion();
    $("input[name='NextBtn']").click(function () {                          
        $("input[name='NextBtn'], #Correct_Incorrect, #detail_welcome, #totalPoint, #CorrectAns").hide();       
        AjaxQuestion();
    });//

})//