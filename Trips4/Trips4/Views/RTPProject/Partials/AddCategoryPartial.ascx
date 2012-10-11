<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<div style='display:none;'>
    <div id="dialog-create-category" class="dialog" title="Create New Category">
        <h2>Create a new Category</h2>
        <div class="error" style="display:none;">
          <span></span>.
        </div>
        <form>
        <fieldset>
            <input type="hidden" id="rtpYear" value="<%=Model.RtpYear.ToString() %>" />
            <p>
                <label for="categoryName">Category Name:</label>
                <input type="text" name="categoryName" class="big w200" maxlength="75" id="categoryName" />
            </p>
            <p>
                <label for="shortName">Short Name:</label>
                <input type="text" name="shortName" class="big w200" maxlength="50" id="shortName" />
            </p>
            <p>
                <label for="description">Description:</label>
                <textarea name="description" class="big" id="description" cols="10" rows="4"></textarea>
                <input type="hidden" name="maxlength" value="255" />
                Characters left: <span class="charsLeft">255</span>
            </p>
        </fieldset>
        </form>
    </div>
</div>

<script type="text/javascript">
    var createCategoryUrl = "<%=Url.Action("CreateCategory","RTPProject")%>";
    $(document).ready(function() {
        $("textarea#description").maxlength({
            'feedback': '.charsLeft',
            'useInput': true
        });

        $("#btn-newcategory").colorbox({
            width: "500px",
            height: "350px",
            inline: true,
            href: "#dialog-create-category",
            onComplete: function() {
                var $buttonCreateCategory = $('<span id="button-create-category" class="cboxBtn">Create</span>').appendTo('#cboxContent');

                $('#button-create-category').click(function() {
                    var categoryName = $("#categoryName").val(),
                        shortName = $("#shortName").val(),
                        description = $("#description").val(),
                        plan = "<%=Model.RtpYear.ToString() %>";

                    if (categoryName == "") {// || shortName == "" || description == "" ) {
                        var msg = 'Please fill in:';
                        if (categoryName == '') msg = msg + ' Category Name';
                        //if (shortName == '') msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Short Name';
                        //if (description == 0) msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Description';
                        alert(msg);
                        //$('#segment-details-error').addClass('error').html(msg).show();
                        return false;
                    }

                    //reset form values
                    $('#categoryName').val("");
                    $('#shortName').val("");
                    $('#description').val("");

                    $.ajax({
                        type: "POST",
                        url: createCategoryUrl,
                        data: "categoryName=" + categoryName + "&shortName=" + shortName + "&description=" + description + "&plan=" + plan,
                        dataType: "json",
                        success: function(response, textStatus, XMLHttpRequest) {
                            if (response.error == "false") {
                                $("#Funding_ReportGroupingCategoryId").addOption(response.data, categoryName);
                                getCategoryDetails(response.data);
                                $.fn.colorbox.close();
                            } else {
                                //obj.countError++;
                                //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                                //$('.dialog-result').addClass('error');
                                //autoHide(10000);
                            }
                            window.onbeforeunload = null;
                        },
                        error: function(response, textStatus, AjaxException) {
                            alert(AjaxException);
                        }
                    });
                    
                    
                    return false;
                });
            },
            onClosed: function() {
                $("#button-create-category").remove();
            }
        });
    });
    
    

</script>