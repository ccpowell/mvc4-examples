<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<div style='display:none'>
    <div id="dialog-create-project" class="dialog" title="Create New RTP Project">
        <h2>Create a new Project</h2>
        <div class="error" style="display:none;">
          <span></span>.
        </div>
        <form>
        <fieldset>
            <input type="hidden" id="year" value="<%=Model.RtpYear.ToString()%>" />
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
            <button type="submit" id="add-project" class="fg-button big ui-state-default ui-priority-primarystate-enabled ui-corner-all" >Create</button>
        </fieldset>
        </form>
    </div>
</div>

<script type="text/javascript">
    var createProjectUrl = "<%=Url.Action("CreateProject","RTP")%>";

    $(function() {
		$("#button-create-project").colorbox({ 
            width: "500px", 
            height: "370px", 
            inline: true, 
            href: "#dialog-create-project",
            onLoad:function() {
                $.getJSON('<%= Url.Action("GetSponsorOrganizations")%>/', null, function(data) {
                    $('#availableSponsors').fillSelect(data);
                });
            }
        });
        
    });
    
    $('#add-project').click(function() {
        var projectName = $("#projectName").val(),
            facilityName = $("#facilityName").val(),
			tipYear = $("#tipYear").val(),
			sponsorOrganizationId = $("#availableSponsors").val();
			
		//reset form values
		$('#projectName').val("");
		$('#facilityName').val("");
		
		$.ajax({
            type: "POST",
            url: createProjectUrl,
            data: "projectName=" + projectName + "&facilityName=" + facilityName + "&tipYear=" + tipYear + "&sponsorOrganizationId=" + sponsorOrganizationId,
            dataType: "json",
            success: function(response) {
                //Add into the DOM
                var projectversionid = response;
                //alert(projectversionid);
                //var redirectActionUrl = "/Project/" + tipYear + "/Info/" + projectversionid + "?message=Project created successfully.";
                
                var redirectActionUrl = "<%= Url.Action("Info","Project", new { @message = "Project created successfully.", @year = "timeperiod", @id = "projectversionid"  }) %>"
                redirectActionUrl = redirectActionUrl.replace("timeperiod", tipYear);
                redirectActionUrl = redirectActionUrl.replace("projectversionid", projectversionid);
                
                location = redirectActionUrl;
                autoHide();
            },
            error: function(response, error) {
                alert('error: '+error+' '+projectName+' '+tipYear);
            }
        });
        return false;
    });

</script>