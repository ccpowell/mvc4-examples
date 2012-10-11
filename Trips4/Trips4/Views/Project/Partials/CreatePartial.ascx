<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.TIP.TipBaseViewModel>" %>

<div style='display:none'>
    <div id="dialog-create-project" class="dialog" title="Create New TIP Project">
        <h2>Create a new Project</h2>
        <div id="create-dialog-result" class="" style="display:none;">
          <span></span>.
        </div>
        <form>
        <fieldset>
            <input type="hidden" id="tipYear" value="<%=Model.TipSummary.TipYear.ToString()%>" />
            <p>
                <label for="facilityName">Facility Name:</label>
                <input type="text" name="facilityName" class="big" id="facilityName" />
            </p>
            <p>
                <label for="projectName">Project Name:</label>
                <input type="text" name="projectName" class="big" id="projectName" />
            </p>
            <p>
                <label for="CurrentSponsors">Sponsor:</label>
                <%= Html.DropDownListFor(x => x.CurrentSponsors, new SelectList(Model.CurrentSponsors, "Key", "Value"), new { @class = "mediumInputElement big" })%>
                <%--<select class="mediumInputElement big" id="availableSponsors" name="availableSponsors" size="1"></select>--%>
            </p>
            <p>
                <label for="AmendmentTypes">Amendment Type:</label>
                <%= Html.DropDownListFor(x => x.AmendmentTypes, new SelectList(Model.AmendmentTypes, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </p>
            <button type="submit" id="add-project" class="fg-button big ui-state-default ui-priority-primarystate-enabled ui-corner-all" >Create</button>
        </fieldset>
        </form>
    </div>
</div>

<script type="text/javascript">
    var createProjectUrl = "<%=Url.Action("CreateProject","TIP")%>";

    $(function() {
		$("#button-create-project").colorbox({ 
            width: "500px", 
            height: "370px", 
            inline: true, 
            href: "#dialog-create-project",
            onLoad:function() {
                $.getJSON('<%= Url.Action("GetSponsorOrganizations")%>/', null, function(data) {
                    $('#CurrentSponsors').fillSelect(data);
                });
            }
        });
        
    });
    
    $('#add-project').click(function() {
        var projectName = $("#projectName").val(),
            facilityName = $("#facilityName").val(),
			tipYear = $("#tipYear").val(),
			sponsorOrganizationId = $("#CurrentSponsors").val(),
			amendmentType = $("#AmendmentTypes").val();
		
		if (projectName === '' || facilityName === '' || sponsorOrganizationId === '' || tipYear === '' || amendmentType === '') {
		    $("div#create-dialog-result span").html("All fields are required");
            $("div#create-dialog-result").addClass("error").autoHide({ wait: 10000, removeClass: "error" });
		} else {
            //reset form values
		    $('#projectName').val("");
		    $('#facilityName').val("");
    		
		    $.ajax({
                type: "POST",
                url: createProjectUrl,
                data: "projectName=" + projectName 
                    + "&facilityName=" + facilityName 
                    + "&tipYear=" + tipYear 
                    + "&sponsorOrganizationId=" + sponsorOrganizationId
                    + "&amendmentTypeId=" + amendmentType,
                dataType: "json",
                success: function(response) {
                    //Add into the DOM
                    var projectversionid = response;
                    //var redirectActionUrl = '<%= Url.Action("Info","Project", new { @message = "Project created successfully.", @year = "' + tipYear + '", @id = "' + projectversionid + '"  }) %>' 
                    
                    var redirectActionUrl = "<%= Url.Action("Info","Project", new { @message = "Project created successfully.", @year = "timeperiod", @id = "projectversionid"  }) %>"
                    redirectActionUrl = redirectActionUrl.replace("timeperiod", tipYear);
                    redirectActionUrl = redirectActionUrl.replace("projectversionid", projectversionid);
                    
                    //"/trips/Project/" + tipYear + "/Info/" + projectversionid + "?message=Project created successfully.";
                    location = redirectActionUrl;
                    //autoHide();
                },
                error: function(response, error) {
                    alert('error: '+error+' '+projectName+' '+tipYear);
                }
            });
		}
		
        return false;
    });

</script>