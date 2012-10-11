<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<div style='display:none'>
    <div id="dialog-create-project" class="dialog" title="Create New RTP Project">
        <h2>Create a new Project</h2>
        <div class="error" style="display:none;">
          <span></span>.
        </div>
        <form>
        <fieldset>
            <input type="hidden" id="rtpYear" value="<%=Model.RtpYear.ToString() %>" />
            <p>
                <label for="facilityName">Facility Name:</label>
                <input type="text" name="facilityName" class="big" id="facilityName" />
            </p>
            <p>
                <label for="projectName">Project Name:</label>
                <input type="text" name="projectName" class="big" id="projectName" />
            </p>
            <p>
                <label for="availableSponsors">Sponsor:</label>
                <select class="mediumInputElement big" id="availableSponsors" name="availableSponsors" size="1"></select>
            </p>
            <%--<button type="submit" id="add-project" class="fg-button big ui-state-default ui-priority-primarystate-enabled ui-corner-all" >Create</button>--%>
        </fieldset>
        </form>
    </div>
</div>

<script type="text/javascript">
    var createProjectUrl = "<%=Url.Action("CreateProject","RTP")%>";
    

    $("#btn-newproject").colorbox({
        width: "500px",
        height: "125px",
        inline: true,
        href: "#dialog-create-project",
        onLoad: function() {
            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetSponsorOrganizations", "RTP")%>',
                data: "plan=<%=Model.RtpYear.ToString() %>",
                dataType: "json",
                success: function(response) {
                    //Add into the DOM
                    $('#availableSponsors').fillSelect(response);
                },
                error: function(response, error) {
                    alert('error: '+error);
                }
            });
        },
        onComplete: function() {
            var $buttonRestoreProjects = $('<span id="button-create-projects" class="cboxBtn">Create</span>').appendTo('#cboxContent');
            
            $('#button-create-projects').click(function() {
                var projectName = $("#projectName").val(),
                    facilityName = $("#facilityName").val(),
		            plan = "<%=Model.RtpYear.ToString() %>",
		            sponsorOrganizationId = $("#availableSponsors").val(),
                    cycleid = "<%= Model.Cycle.Id.ToString() %>";

        		if( projectName == "" || facilityName == "" || sponsorOrganizationId == 0 ) {
                    var msg = 'Please fill in:';
                    if (facilityName == '') msg = msg + ' Facility Name';
                    if (projectName == '') msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Project Name';
                    if (sponsorOrganizationId == 0) msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Sponsor';
                    alert(msg);
                    //$('#segment-details-error').addClass('error').html(msg).show();
                    return false;
                }
        		
	            //reset form values
	            $('#projectName').val("");
	            $('#facilityName').val("");
        		
	            $.ajax({
                    type: "POST",
                    url: createProjectUrl,
                    data: "projectName=" + projectName + "&facilityName=" + facilityName + "&plan=" + plan + "&sponsorOrganizationId=" + sponsorOrganizationId + "&cycleId=" + cycleid,
                    dataType: "json",
                    success: function(response) {
                        //Add into the DOM
                        var projectversionid = response;
                        //alert(projectversionid);
                        //var redirectActionUrl = '<%= Url.Action("Info","RtpProject", new { @message = "Project created successfully.", @year = "' + plan + '", @id = "' + projectversionid + '"  }) %>' 
                        
                        var redirectActionUrl = "<%= Url.Action("Info","RtpProject", new { @message = "Project created successfully.", @year = "timeperiod", @id = "projectversionid"  }) %>"
                        redirectActionUrl = redirectActionUrl.replace("timeperiod", plan);
                        redirectActionUrl = redirectActionUrl.replace("projectversionid", projectversionid);
                        //"/RtpProject/" + plan + "/Info/" + projectversionid + "?message=Project created successfully.";
                        location = redirectActionUrl;
                        //autoHide();
                    },
                    error: function(response, error) {
                        alert('error: '+error+' '+projectName+' '+plan);
                    }
                });
                return false;
            });
            
            $.fn.colorbox.resize();
        },
        onClosed: function() {
            $("#button-create-projects").remove();
        }
    });
    
    

</script>
